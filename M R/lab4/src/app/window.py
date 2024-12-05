from PyQt6.QtWidgets import (
    QMainWindow,
    QToolBar,
    QMessageBox
)
from PyQt6.QtGui import QAction
from dataclasses import dataclass

from app.style import StyleSheet
from app.page import Page


@dataclass
class Settings:
    title = 'Моделирование работы Q-системы'
    width = 500
    height = 600

    info_title = 'О программе'

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        
        self.setWindowTitle(Settings.title)
        self.setFixedSize(Settings.width, Settings.height)
        self.setStyleSheet(StyleSheet)
        
        self.__createToolbar()
        page = Page()
        self.setCentralWidget(page)
    
    def __createToolbar(self):
        toolbar = QToolBar()
        
        self.addToolBar(toolbar)

    def __get_info(self):
        info = QMessageBox()

        info.setText(Settings.info_text)
        info.setWindowTitle(Settings.info_title)

        info.exec()
