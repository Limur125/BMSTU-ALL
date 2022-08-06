QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

CONFIG += c++11

# You can make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    canvas_funcs.cpp \
    err_message.cpp \
    link.cpp \
    main.cpp \
    mainwindow.cpp \
    menu.cpp \
    picture.cpp \
    point.cpp \
    render_funcs.cpp \
    transform_funcs.cpp

HEADERS += \
    canvas_funcs.h \
    err_message.h \
    link.h \
    mainwindow.h \
    menu.h \
    picture.h \
    point.h \
    render_funcs.h \
    return_codes.h \
    transform_funcs.h

FORMS += \
    mainwindow.ui

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target
