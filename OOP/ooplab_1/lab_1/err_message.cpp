#include "err_message.h"

void err_message(const ret_code error)
{
    switch (error)
    {
        case FILE_OPEN_ERROR:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Ошибка открытия файла.");
            break;
        case FILE_FORMAT_ERROR:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Неверный формат файла.");
            break;
        case MEMORY_ERROR:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Ошибка работы с памятью.");
            break;
        case NO_POINTS:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Файл пустой, нет точек для постоения.");
            break;
        case NO_LINKS:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Отсутствуют линии, проверьте файл.");
            break;
        case UNKNOWN_COMMAND:
            QMessageBox::critical(NULL, "Внимание ошибка!",
                                  "Неизвестная команда в файле.");
            break;
        default:
            QMessageBox::critical(NULL, "Внимание ошибка",
                                  "Неизвестная ошибка.");
    }
}


