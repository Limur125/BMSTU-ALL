#ifndef _UTIL_H
#define _UTIL_H

#define INLINE __attribute__((always_inline)) inline

enum D_hid_descriptor{
    D_END_COLLECTION = 0xC0,

    D_REPORT_ID = 0x84,
    D_INPUT = 0x80,
    D_FEATURE = 0xB0,
    D_REPORT_SIZE = 0x74,
    D_REPORT_COUNT = 0x94,
    D_LOGICAL_MINIMUM = 0x14,
    D_LOGICAL_MAXIMUM = 0x24,
    D_USAGE = 0x08,
    D_USAGE_PAGE = 0x04,
};

enum hid_data{
    D_USAGE_BUTTON = 0x09,
    D_USAGE_WHEEL = 0x38,
    D_USAGE_X = 0x30,
    D_USAGE_Y = 0x31
};

struct report_entry {
    unsigned char id;
	unsigned char offset;
	unsigned char size;
    unsigned char sgn; 
};

struct report_positions {
    int report_id_tagged; 
	struct report_entry button;
	struct report_entry x;
	struct report_entry y;
	struct report_entry wheel;
};

int parse_report_desc(unsigned char *data, int data_len, struct report_positions *data_pos);
int extract_mouse_events(unsigned char *data, int data_len, struct report_positions *data_pos, int *btn, int *x, int *y, int *wheel);

#endif  //_UTIL_H
