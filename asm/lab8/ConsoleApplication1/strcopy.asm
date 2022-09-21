.686
.MODEL FLAT, C
.STACK
.CODE
strcopy PROC
    mov esi, ecx
    mov edi, edx
    mov ecx, eax
    cmp edi, esi
    jl simple_copy
        std
        add esi, ecx
        add edi, ecx
        dec esi
        dec edi
    simple_copy:
    rep movsb
    cld
    mov eax, edi
    ret
strcopy ENDP

END
