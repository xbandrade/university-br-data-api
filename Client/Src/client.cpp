#include "client.hpp"

Client::Client(QWidget *parent)
    : QMainWindow(parent)
{
    ui.setupUi(this);
    ui.apiUrlLineEdit->setText("http://localhost:5000");
    ui.pushButton->setStyleSheet(
        "QPushButton {"
        "   background-color: gray;"
        "   color: white;"
        "}"
        "QPushButton:hover {"
        "   background-color: lightgray;"
        "}"
        "QPushButton:pressed {"
        "   background-color: lightblue;"
        "}"
    );
}

Client::~Client()
{}
