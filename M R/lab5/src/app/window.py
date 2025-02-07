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
    title = 'Моделирование работы информационного центра'
    width = 500
    height = 600


class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        
        self.setWindowTitle(Settings.title)
        self.setFixedSize(Settings.width, Settings.height)
        self.setStyleSheet(StyleSheet)
        
        page = Page()
        self.setCentralWidget(page)
