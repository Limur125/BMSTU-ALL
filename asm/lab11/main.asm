.686
.model flat, stdcall
option casemap:none
main_win proto :DWORD,:DWORD,:DWORD,:DWORD 

include \masm32\include\windows.inc
include \masm32\include\user32.inc
include \masm32\include\kernel32.inc
includelib \masm32\lib\user32.lib
includelib \masm32\lib\kernel32.lib

.data
    class_name    db "win_class",0
    win_name      db "sum of digits",0
    button_name   db "button",0
    button_text   db "add",0
    edit_name     db "edit",0
    format_result db "%d",0

.data?
    hinstance    HINSTANCE ?
    command_line LPSTR ?
    hwnd_button  HWND ?
    hwnd_edit1   HWND ?
    hwnd_edit2   HWND ?
    buf          db 2 dup(?)

.const
    button_id equ 1
    edit_id   equ 2

.code
start:
    invoke GetModuleHandle, NULL
    mov hinstance, eax 
    invoke GetCommandLine
    mov command_line, eax
    invoke main_win, hinstance, NULL, command_line, SW_SHOWDEFAULT
    invoke ExitProcess, eax

main_win proc hinst:HINSTANCE, hPrevInst:HINSTANCE, CmdLine:DWORD, CmdShow:DWORD
    LOCAL wc:WNDCLASSEX
    LOCAL msg:MSG
    LOCAL hwnd:HWND

    mov wc.cbSize, SIZEOF WNDCLASSEX
    mov wc.style, CS_HREDRAW or CS_VREDRAW
    mov wc.lpfnWndProc, offset sum_digits
    mov wc.cbClsExtra, NULL
    mov wc.cbWndExtra, NULL
    push hinst
    pop wc.hInstance
    mov wc.hbrBackground, COLOR_BTNFACE+1
    mov wc.lpszMenuName, NULL
    mov wc.lpszClassName, offset class_name
    invoke LoadIcon, NULL, IDI_APPLICATION
    mov wc.hIcon, eax
    mov wc.hIconSm, eax
    invoke LoadCursor, NULL, IDC_ARROW
    mov wc.hCursor, eax
    invoke RegisterClassEx, addr wc
     
    invoke CreateWindowEx, WS_EX_CLIENTEDGE, addr class_name, addr win_name, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, 300, 200, NULL, NULL, hinst, NULL
    mov hwnd, eax

    invoke ShowWindow, hwnd, SW_SHOWNORMAL
    invoke UpdateWindow, hwnd

    .WHILE TRUE
        invoke GetMessage, addr msg, NULL, 0, 0
    .BREAK .IF(!eax)
        invoke TranslateMessage, addr msg
        invoke DispatchMessage, addr msg
    .ENDW
        mov eax, msg.wParam
        ret
main_win endp

sum_digits proc hwnd:HWND, uMsg:UINT, wParam:WPARAM, lParam:LPARAM
    .IF uMsg == WM_DESTROY
        invoke PostQuitMessage, NULL
    .ELSEIF uMsg == WM_CREATE
        invoke CreateWindowEx ,WS_EX_CLIENTEDGE, addr edit_name, NULL, WS_CHILD or WS_VISIBLE or WS_BORDER or ES_LEFT or ES_AUTOHSCROLL, 50, 35, 200, 25, hwnd, 8, hinstance, NULL
        mov hwnd_edit1, eax
        invoke CreateWindowEx, WS_EX_CLIENTEDGE, addr edit_name, NULL, WS_CHILD or WS_VISIBLE or WS_BORDER or ES_LEFT or ES_AUTOHSCROLL, 50, 70, 200, 25, hwnd, 8, hinstance, NULL
        mov hwnd_edit2, eax

        invoke SetFocus, hwnd_edit1
        invoke CreateWindowEx, NULL, addr button_name, addr button_text, WS_CHILD or WS_VISIBLE or BS_DEFPUSHBUTTON, 75, 105, 140, 25, hwnd, button_id, hinstance, NULL
        mov hwnd_button, eax


    .ELSEIF uMsg == WM_COMMAND
        mov eax, wParam
        .IF ax == button_id
            push edi
            push ebx

            invoke GetWindowText, hwnd_edit1, addr buf, 2
            xor edi, edi
            xor eax, eax
            xor ebx, ebx
            mov cx, 10
            mov bl, byte ptr buf[edi]
            sub bl, '0'
            push ebx

            invoke GetWindowText, hwnd_edit2, addr buf, 2
            xor edi, edi
            xor eax, eax
            xor ebx, ebx
            mov bl, byte ptr buf[edi]
            sub bl, '0'
            pop eax
            add eax, ebx

            invoke wsprintf, addr buf, addr format_result, eax
            invoke MessageBox, hwnd, addr buf, addr win_name, MB_OK

            pop ebx
            pop edi
        .ENDIF
    .ELSE
        invoke DefWindowProc,hwnd,uMsg,wParam,lParam
        ret
    .ENDIF
    xor eax, eax
    ret
sum_digits endp
end start