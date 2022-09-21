SD SEGMENT para 'DATA'
	symb db 0
SD ENDS

SC1 SEGMENT para public 'CODE'
	assume CS:SC1, DS:SD
main:
    mov ax, SD
    mov ds, ax

    mov ah, 01
    int 21h
    mov symb, al

	call print

    mov ax, 4c00h
	int 21h
SC1 ENDS

SC2 SEGMENT para public 'CODE'
	assume CS:SC2
print:
    mov ah, 2
    mov dl, 0Dh
	int 21h	

    mov dl, 0Ah
	int 21h	

    mov dl, symb
    add dl, 32
	int 21h	
    retf
	
SC2 ENDS

END main