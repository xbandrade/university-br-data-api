#ifndef CLIENT_HPP
#define CLIENT_HPP

#include <QtWidgets/QMainWindow>
#include "ui_client.h"

class Client : public QMainWindow
{
    Q_OBJECT

public:
    Client(QWidget *parent = nullptr);
    ~Client();

private:
    Ui::ClientClass ui;
};

#endif  // CLIENT_HPP
