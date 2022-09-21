.MODEL TINY

CODES SEGMENT
    ASSUME CS:CODES, DS:CODES
    ORG 100H                    ; Там хранится PSP (делаем сдвиг)

MAIN:
    JMP INSTALL_BREAKING
    OLD_8H DD ? 
    FLAG DB 'E'
    CUR_T DB 0
    BUF DB 0

MY_NEW_8H PROC
    PUSH AX
    PUSH BX
    PUSH CX
    PUSH DX
    PUSH DI
    PUSH SI

    PUSH ES
    PUSH DS

    PUSHF
    

    CALL CS:OLD_8H

    MOV AH, 02H
    INT 1AH

    CMP DH, CUR_T
    JE EXIT_BREAKING
    CMP BUF, 63
    JE BUF_AGAIN
    INC BUF
    JMP NEXT_STEP
    BUF_AGAIN:
        MOV BUF, 0

    NEXT_STEP:
        MOV AL, 0F3h
        OUT 60H, AL
        MOV AL, 96
        ADD AL, BUF
        OUT 60H, AX


    EXIT_BREAKING:
        MOV CUR_T, DH
        POP DS
        POP ES

        POP SI
        POP DI
        POP DX
        POP CX
        POP BX
        POP AX

        IRET

MY_NEW_8H ENDP

INSTALL_BREAKING:
    MOV AX, 3508H  
    INT 21H

    CMP ES:FLAG, 'E'
    JE UNINSTALL_BREAKING

    MOV WORD PTR OLD_8H, BX
    MOV WORD PTR OLD_8H + 2, ES

    MOV AX, 2508H               
    MOV DX, OFFSET MY_NEW_8H         
    INT 21H

    MOV DX, OFFSET INSTALL_MSG
    MOV AH, 9
    INT 21H

    MOV DX, OFFSET INSTALL_BREAKING
    INT 27H                 

UNINSTALL_BREAKING:
    PUSH ES
    PUSH DS

    MOV DX, WORD PTR ES:OLD_8H 
    MOV DS, WORD PTR ES:OLD_8H + 2 
    MOV AX, 2508H
    INT 21H

    POP DS
    POP ES

    MOV AH, 49H                         
    INT 21H

    MOV DX, OFFSET UNINSTALL_MSG
    MOV AH, 9H
    INT 21H

    MOV AX, 4C00H
    INT 21H

    INSTALL_MSG   DB 'Breaking my_new_8h!$'
    UNINSTALL_MSG DB 'Breaking old_8h!$'
    


CODES ENDS
END MAIN