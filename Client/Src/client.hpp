#ifndef CLIENT_HPP
#define CLIENT_HPP

#include <iostream>
#include <string>
#include <algorithm>
#include <map>
#include <regex>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QListView>
#include <QAbstractListModel>
#include <QStringListModel>
#include "apiReader.hpp"
#include "ui_client.h"

class Client : public QMainWindow
{
    Q_OBJECT

public:
    Client(QWidget *parent = nullptr);
    ~Client();

public slots:
    void onResultsItemClicked(const QModelIndex& index);
    void onPushButtonClicked();
    void onNextButtonClicked();
    void onPreviousButtonClicked();

private:
    ApiReader apiReader;
    Ui::ClientClass ui;
    QStringListModel* resultsModel;
    QStringListModel* detailsModel;
    QStringList selectedItems;
};

#endif  // CLIENT_HPP
