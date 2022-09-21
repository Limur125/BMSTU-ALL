extrn number: word
extrn output10_msg: byte
extrn print_newline: near
public output_dec

output10_data segment para 'data'
    numb10_str db 6 dup (0), '$'
output10_data ends

code_seg segment para public 'code'
assume cs:code_seg, ds:output10_data

word_to_udec_str proc near
    xor cx, cx
    mov bx, 10
    loop1:
        xor dx, dx
        div bx
        add dl, '0'
        push dx
        inc cx
        cmp ax, 0
    jnz loop1
 
    loop2:
        pop dx
        mov numb10_str[di], dl
        inc di
    loop loop2
    retn
word_to_udec_str endp

word_to_sdec_str proc near
    test ax, ax
    jns wtsds_no_sign
        mov numb10_str[di], '-'
        inc di
        neg ax
    wtsds_no_sign:
    call word_to_udec_str
    retn
word_to_sdec_str endp

output_dec proc near
    call print_newline
    mov dx, offset output10_msg
    mov ah, 9
    int 21h
    mov ax, ds
    mov es, ax
    mov ax, output10_data
    mov ds, ax
    mov cx, 5
    clear:
        mov di, cx
        mov numb10_str[di], 0
    loop clear
    mov ax, es:number
    xor di, di
    call word_to_sdec_str
    mov dx, offset numb10_str
    mov ah, 9
    int 21h
    mov ax, es
    mov ds, ax
    retn
output_dec endp

code_seg ends
end