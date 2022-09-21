stks segment para stack 'stack'
    db 100h dup (?)
stks ends

datas segment para 'data'
    rowsmsg db 'Input the number of rows: $'
    colmsg db 'Input the number of columns: $'
    mtrmsg db 'Input matrix: $'
    resmsg db 'Result matrix: $'
    errmsg db 'Error!!! $'
    rows db 0
    cols db 0
    max_cols db 9
    max_rows db 9
    matrix db 9 * 9 dup(?)
datas ends

codes segment para 'code'
    assume ss:stks, ds:datas, cs:codes
scan_sym:
	mov ah, 1
	int 21h
	ret
	
; Перевод на новую строку
print_newline:
	mov ah, 2
	mov dl, 13
	int 21h
	mov DL, 10
	int 21h
	ret

; Печать пробела
print_space:
	mov AH, 2
	mov DL, ' '
	int 21h
	ret
	
; Вывод символа
print_sym:
	mov AH, 2
	int 21h
	ret

; Вывод сообщения
print_msg:
	mov AH, 9
	int 21h
	ret

delete_col:
    mov bh, cl
    dec cols
    xor ch, ch
    del_col_i_loop:
        cmp ch, rows
        jz end_del_col_i_loop
        mov cl, bh
        del_col_j_loop:
            cmp cl, cols
            jz end_del_col_j_loop
            mov al, ch
            mul max_cols
            add al, cl
            mov si, ax
            inc si
            mov dl, matrix[si]
            dec si
            mov matrix[si], dl
            inc cl
            jmp del_col_j_loop
        end_del_col_j_loop:
        inc ch
        jmp del_col_i_loop
    end_del_col_i_loop:
    mov cl, bh
    dec cl
    ret

main:
    mov ax, datas
    mov ds, ax

    mov dx, offset rowsmsg
    call print_msg

    call scan_sym
    sub al, '0'
    jz exit_failure
    cmp al, max_rows
    ja exit_failure  
    mov rows, al
    call print_newline

    mov dx, offset colmsg
    call print_msg
    
    call scan_sym
    sub al, '0'
    jz exit_failure
    cmp al, max_cols
    ja exit_failure  
    mov cols, al
    call print_newline

    mov dx, offset mtrmsg
    call print_msg

    
    xor si, si
    xor cx, cx
    i_loop1:
        cmp ch, rows
        jz end_i_loop1
        call print_newline
        xor cl, cl
        j_loop1: 
            cmp cl, cols
            jz end_j_loop1
            call scan_sym
            mov dl, al
            mov al, ch
            mul max_cols
            add al, cl
            mov si, ax
            mov matrix[si], dl
            call print_space
            inc cl
            jmp j_loop1
        end_j_loop1:
        inc ch
        jmp i_loop1
    end_i_loop1:

   

    xor cx, cx
    xor bx, bx
    i_loop2:
        cmp cl, cols
        jz end_i_loop2
        xor ch, ch
        xor bl, bl
        j_loop2: 
            cmp ch, rows
            jz end_j_loop2
            mov al, ch
            mul max_rows
            add al, cl
            mov si, ax
            ;mov dh, 
            cmp matrix[si], 'X';88; dh
            jnz end_if1
                inc bl
            end_if1:
            inc ch
            jmp j_loop2
        end_j_loop2:
        xor ch, ch
        cmp bl, rows
        jnz end_if2
            call delete_col
        end_if2:
        inc cl
        jmp i_loop2
    end_i_loop2:

    call print_newline
    mov dx, offset resmsg
    call print_msg

    xor si, si
    xor cx, cx
    i_loop3:
        cmp ch, rows
        jz end_i_loop3
        call print_newline
        xor cl, cl
        j_loop3: 
            cmp cl, cols
            jz end_j_loop3
            mov al, ch
            mul max_cols
            add al, cl
            mov si, ax
            mov dl, matrix[si]
            call print_sym
            call print_space
            inc cl
            jmp j_loop3
        end_j_loop3:
        inc ch
        jmp i_loop3
    end_i_loop3:

    mov ax, 4c00h
    int 21h

    exit_failure:
        call print_newline
        mov dx, offset errmsg
        call print_msg
        mov ax, 4c01h
        int 21h
codes ends

end main