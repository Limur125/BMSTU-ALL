extrn number: word
extrn output2_msg: byte
extrn print_newline: near
public output_bin

output2_data segment para 'data'
    numb2_str db 10h dup (0), '$'
output2_data ends

code_seg segment para public 'code'
assume cs:code_seg, ds:output2_data

byte_to_bin_str proc near
    mov cx, 8
    loop1:
        rol al, 1
        jc sym_if
        mov numb2_str[di],'0'
        jmp sym_endif
        sym_if:
            mov numb2_str[di],'1'
        sym_endif:
        inc di
    loop loop1
    retn
byte_to_bin_str endp

output_bin proc near
    call print_newline
    mov dx, offset output2_msg
    mov ah, 9
    int 21h
    mov ax, ds
    mov es, ax
    mov ax, output2_data
    mov ds, ax
    mov ax, es:number
    xor di, di
    xchg ah, al
    call byte_to_bin_str
    xchg ah, al
    call byte_to_bin_str 
    mov dx, offset numb2_str
    mov ah, 9
    int 21h
    mov ax, es
    mov ds, ax
    retn
output_bin endp
code_seg ends
end