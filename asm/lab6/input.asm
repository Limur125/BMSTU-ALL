extrn number: word
extrn input_msg: byte
extrn print_newline: near

input_data segment para 'data'
    ascii_table db 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0, 0, 0, 10, 11, 12, 13, 14, 15
input_data ends

code_seg segment para public 'code'
assume cs:code_seg, ds:input_data

input_hexdec proc near
    call print_newline
    mov dx, offset input_msg
    mov ah, 9
    int 21h
    mov ax, ds
    mov es, ax
    mov ax, input_data
    mov ds, ax
    mov cl, 4
    mov es:number, 0
    loop1:
        mov ah, 1h
        int 21h
        cmp al, 13
        jz exit_input
        sub al, '0'
        jb exit_input
        cmp al, 10
        jb let
            sub al, 7
            cmp al, 16
            jb l_let
                sub al, 32
            ja exit_input
            l_let:
        let:
        ; mov bx, offset ascii_table
        ; xlat 
        xor ah, ah
        mov ch, cl
        mov cl, 4
        sal es:number, cl
        mov cl, ch
        xor ch, ch
        add es:number, ax
    loop loop1
    exit_input:
    mov ax, es
    mov ds, ax
    retn
input_hexdec endp

code_seg ends
end