QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

CONFIG += c++17

# You can make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    AbstractFactory.cpp \
    Actions.cpp \
    Camera.cpp \
    Composite.cpp \
    DrawManager.cpp \
    Drawer.cpp \
    DrawerDirector.cpp \
    Exceptions.cpp \
    Facade.cpp \
    Figure.cpp \
    FigureBuilder.cpp \
    FigureDirector.cpp \
    FileLoader.cpp \
    Link.cpp \
    LoadManager.cpp \
    Point.cpp \
    Scene.cpp \
    SceneManager.cpp \
    Visitor.cpp \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    AbstractFactory.h \
    Actions.h \
    BaseFigureBuilder.h \
    BaseFigureDirector.h \
    BaseManager.h \
    BaseObject.h \
    BaseVisitor.h \
    Camera.h \
    Composite.h \
    DrawManager.h \
    Drawer.h \
    DrawerDirector.h \
    Exceptions.h \
    Facade.h \
    Figure.h \
    FigureBuilder.h \
    FigureDirector.h \
    FileLoader.h \
    Link.h \
    LoadManager.h \
    Point.h \
    Scene.h \
    SceneManager.h \
    SourceLoader.h \
    Transformations.h \
    Visitor.h \
    mainwindow.h

FORMS += \
    mainwindow.ui

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target
