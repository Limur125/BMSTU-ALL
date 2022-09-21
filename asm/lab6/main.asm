public number
public input_msg
public output2_msg
public output10_msg
public print_newline

extrn input_hexdec: near
extrn output_bin: near
extrn output_dec: near
stack_seg segment para stack 'stack'
    db 100h dup (?)
stack_seg ends

data_seg segment para 'data'
    menu_msg db 'Menu:', 13, 10 
    db '1. Input number in Hex format.', 13, 10
    db '2. Output number in Bin format.', 13, 10
    db '3. Output number in Dec format.', 13, 10
    db '0. Exit.'
    db '$'
    choice_msg db 'Your choice: $'
    input_msg db 'Input hexdecimal number: $'
    output2_msg db 'Binary number $'
    output10_msg db 'Signed decimal number $'
    funcs_array dw exit, input_hexdec, output_bin, output_dec
    number dw 0
    db '$'
data_seg ends

code_seg segment para public 'code'
assume ss:stack_seg, ds:data_seg, cs:code_seg

print_msg:
	mov ah, 9
	int 21h
	ret

print_newline:
	mov ah, 2
	mov dl, 13
	int 21h
	mov dl, 10
	int 21h
	ret

scan_command proc near
	mov ah, 1h
	int 21h
    sub al, '0'
	ret
scan_command endp

exit proc near
    mov ax, 4c00h
    int 21h
exit endp

main:
    mov ax, data_seg
    mov ds, ax

    mov dx, offset menu_msg
    call print_msg

    menu:
        call print_newline
        mov dx, offset choice_msg
        call print_msg

        call scan_command

        cmp al, 0
        jb menu
        cmp al, 3
        ja menu

        mov ah, 2
        mul ah
        mov si, ax
        call funcs_array[si]
    jmp menu
code_seg ends
end main
