#include "util.h"
#include <linux/kernel.h>
#include <linux/string.h>
#include <linux/module.h>
static char g_debug = 0;
module_param_named(debug, g_debug, byte, 0644);

struct parser_context {
    unsigned char id;   
    unsigned int offset; 
};

#define NUM_USAGES 32
#define NUM_CONTEXTS 32
#define SET_ENTRY(entry, _id, _offset, _size, _sign) \
    entry.id = _id;                                 \
    entry.offset = _offset;                         \
    entry.size = _size;                             \
    entry.sgn = _sign;

int parse_report_desc(unsigned char *buffer, int buffer_len, struct report_positions *pos)
{
    int r_count = 0, r_size = 0, r_sgn = 0, len = 0;
    int r_usage[NUM_USAGES];
    unsigned char ctl, button = 0;
    unsigned char *data;
    unsigned int n, i = 0;
    int context_found;
    struct parser_context contexts[NUM_CONTEXTS]; 
    struct parser_context *c = contexts;
    for(n = 0; n < NUM_USAGES; n++){
        r_usage[n] = 0;
    }
    pos->report_id_tagged = 0;
    for(n = 0; n < NUM_CONTEXTS; n++){
        contexts[n].id = 0;
        contexts[n].offset = 0;
    }

    while(i < buffer_len){
        ctl = buffer[i] & 0xFC;  
        len = buffer[i] & 0x03;
        if(i < buffer_len) data = buffer + i + 1;
        if(ctl == D_REPORT_SIZE)  r_size = (int) data[0];
        if(ctl == D_REPORT_COUNT){
            r_count = (int) data[0];
            if (r_count > NUM_USAGES){
                printk("MOUSE_ACCELERATION: parse_report_desc r_count > NUM_USAGES (%d). Should only happen on first probe.", NUM_USAGES);
                return -1;
            }
        }
        if(ctl == D_REPORT_ID){
            pos->report_id_tagged = 1;
            context_found = 0;
            for(n = 0; n < NUM_CONTEXTS; n++){
                if(contexts[n].id == data[0]){
                    c = contexts + n;
                    c->id = data[0];
                    context_found = 1;
                    break;
                }
            }
            if(!context_found){
                for(n = 0; n < NUM_CONTEXTS; n++){
                    if(contexts[n].id == 0){
                        c = contexts + n;
                        c->id = data[0];
                        c->offset = 8; 
                        break;
                    }
                }
            }
        }
        if(ctl == D_LOGICAL_MINIMUM){
            switch(len){
            case 1:
                r_sgn = ((int) *((__s8*) data)) < 0;
                break;
            case 2:
                r_sgn = (__s16) le16_to_cpu(*((__s16*) data)) < 0;
                break;
            }
        }
        if((ctl == D_USAGE_PAGE || ctl == D_USAGE) && len == 1){
            if(
                data[0] == D_USAGE_BUTTON ||
                data[0] == D_USAGE_WHEEL ||
                data[0] == D_USAGE_X ||
                data[0] == D_USAGE_Y
            ) {
                for(n = 0; n < NUM_USAGES; n++){
                    if(!r_usage[n]){
                        r_usage[n] = (int) data[0];
                        break;
                    }
                }
            }
        }
        if(ctl == D_INPUT || ctl == D_FEATURE){
            if(!button && r_usage[0] == D_USAGE_BUTTON){
                SET_ENTRY(pos->button, c->id, c->offset, r_size*r_count, r_sgn);
                button = 1;
            } else {
                for(n = 0; n < r_count; n++){
                    switch(r_usage[n]){
                    case D_USAGE_X:
                        SET_ENTRY(pos->x, c->id, c->offset + r_size*n, r_size, r_sgn);
                        break;
                    case D_USAGE_Y:
                        SET_ENTRY(pos->y, c->id, c->offset + r_size*n, r_size, r_sgn);
                        break;
                    case D_USAGE_WHEEL:
                        SET_ENTRY(pos->wheel, c->id, c->offset + r_size*n, r_size, r_sgn);
                        break;
                    }

                }
            }
            for(n = 0; n < NUM_USAGES; n++){
                r_usage[n] = 0;
            }
            //Increment offset
            c->offset += r_size*r_count;
        }
        i += len + 1;
    }
    
    if(g_debug){
        printk("BTN\t(%d): Offset %u\tSize %u\t Sign %u",   pos->button.id ,    (unsigned int) pos->button.offset,  pos->button.size,   pos->button.sgn);
        printk("X\t(%d): Offset %u\tSize %u\t Sign %u",     pos->x.id,          (unsigned int) pos->x.offset,       pos->x.size,        pos->x.sgn);
        printk("Y\t(%d): Offset %u\tSize %u\t Sign %u",     pos->x.id,          (unsigned int) pos->y.offset,       pos->y.size,        pos->x.sgn);
        printk("WHL\t(%d): Offset %u\tSize %u\t Sign %u",   pos->wheel.id,      (unsigned int) pos->wheel.offset,   pos->wheel.size,    pos->wheel.sgn);
    }

    return 0;
}

inline void array_shift_le(unsigned char *data, int data_len, bool right, int num){
    int i;
    if(num == 0) return;
    if(right){
        for(i = 0; i < data_len; i++){
            data[i] >>= num;
            if(i + 1 < data_len){
                data[i] |= data[i+1] << (8 - num);
            }
        }
    } else {
        for(i = data_len - 1; i >= 0; i--){
            data[i] <<= num;
            if(i){
                data[i] |= data[i-1] >> (8 - num);
            }
        }
    }
}

inline int extract_at(unsigned char *data, int data_len, struct report_entry *entry)
{
    int size = entry->size/8;
    int i = entry->offset/8;
    char shift = entry->offset % 8;
    union {
        __u8 raw[4];
        __u32 init;
        __s8 s8;  
        __s16 s16;
        __u8 u8;
        __u16 u16;
    } buffer;

    if(entry->size % 8) size += 1;
    if(shift) size += 1;
    if(size > sizeof(buffer.init)) return 0;
    buffer.init = 0;
    if(i + size > data_len) return 0;
    memcpy(buffer.raw, data + i, size);
    if(shift)
        array_shift_le(buffer.raw,size,true,shift);  

    if(entry->size <= 8){
        buffer.raw[0] &= 0xFF >> (8 - entry->size); 
        if(entry->sgn)  
            buffer.raw[0] = buffer.raw[0] >> (8 - entry->size - 1) == 0 ? buffer.raw[0] : (0xFF ^ (0xFF >> (8 - entry->size))) | buffer.raw[0];
        
        return (int) (entry->sgn ? buffer.s8 : buffer.u8);
    }
    if(entry->size <= 16){
        buffer.raw[1] &= 0xFF >> (16 - entry->size);  
        if(entry->sgn)
            buffer.raw[1] = buffer.raw[1] >> (16 - entry->size - 1) == 0 ? buffer.raw[1] : (0xFF ^ (0xFF >> (16 - entry->size))) | buffer.raw[1];
        buffer.u16 = le16_to_cpu(buffer.u16); 
        return (int) (entry->sgn ? buffer.s16 : buffer.u16);
    }

    return 0; 
}

int extract_mouse_events(unsigned char *buffer, int buffer_len, struct report_positions *pos, int *btn, int *x, int *y, int *wheel)
{
    unsigned char id = 0;

    if(g_debug){
        int i;
        printk(KERN_CONT "Raw: ");
        for(i = 0; i<buffer_len;i++){
            printk(KERN_CONT "0x%02x ", (int) buffer[i]);
        }
        printk(KERN_CONT "\n");
    }

    if(pos->report_id_tagged)
        id = buffer[0];

    *btn = 0; *x = 0; *y = 0; *wheel = 0;
    if(pos->button.id == id)
        *btn =      extract_at(buffer, buffer_len, &pos->button);
    if(pos->x.id == id)
        *x =        extract_at(buffer, buffer_len, &pos->x);
    if(pos->y.id == id)
        *y =        extract_at(buffer, buffer_len, &pos->y);
    if(pos->wheel.id == id)
        *wheel =    extract_at(buffer, buffer_len, &pos->wheel);

    return 0;
}
