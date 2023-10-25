#include "client.hpp"

class NonEditableStringListModel : public QStringListModel {
public:
    NonEditableStringListModel(QObject* parent = nullptr) : QStringListModel(parent) {}

    Qt::ItemFlags flags(const QModelIndex& index) const override {
        return Qt::ItemIsEnabled | Qt::ItemIsSelectable;
    }
    bool setData(const QModelIndex& index, const QVariant& value, int role = Qt::EditRole) override {
        if (role == Qt::EditRole) {
            return false;
        }
        return QStringListModel::setData(index, value, role);
    }
};

Client::Client(QWidget *parent)
    : QMainWindow(parent), apiReader()
{
    ui.setupUi(this);
    resultsModel = new NonEditableStringListModel(this);
    detailsModel = new NonEditableStringListModel(this);
    ui.detailsView->setModel(detailsModel);
    ui.resultsView->setModel(resultsModel);
    ui.apiUrlLineEdit->setText("http://localhost:5000/uni-br/search");
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
    connect(ui.resultsView, &QListView::clicked, this, &Client::onResultsItemClicked);
    connect(ui.pushButton, &QPushButton::clicked, this, &Client::onPushButtonClicked);
    connect(ui.nextPageButton, &QPushButton::clicked, this, &Client::onNextButtonClicked);
    connect(ui.previousPageButton, &QPushButton::clicked, this, &Client::onPreviousButtonClicked);
}

void Client::onResultsItemClicked(const QModelIndex& index) {
    if (resultsModel && index.isValid()) {
        QVariant data = resultsModel->data(index, Qt::DisplayRole);
        QString qstr = data.toString();
        std::string str = qstr.toStdString();
        selectedItems.clear();
        std::regex pattern("^(\\d+)");
        std::smatch match;
        std::map<std::string, std::string> dataMap;
        if (std::regex_search(str, match, pattern)) {
            std::string numberStr = match[1];
            int pk = std::stoi(numberStr);
            QString newStr = QString::fromStdString(std::to_string(pk));
            QString apiUrl = QString::fromStdString(ui.apiUrlLineEdit->text().trimmed().toStdString());
            if (!apiUrl.isEmpty()) {
                dataMap = apiReader.getApiResponseByPK(apiUrl.toStdString(), pk);
            }
        }
        else {
            std::cout << "ID not found" << std::endl;
        }
        if (detailsModel) {
            for (auto const& p : dataMap) {
                std::string output = p.first + ": " + p.second;
                QString qstr = QString::fromStdString(output);
                selectedItems << qstr;
            }
            detailsModel->setStringList(selectedItems);
        }
    }
}

void Client::onPushButtonClicked() {
    QString apiUrl = QString::fromStdString(ui.apiUrlLineEdit->text().trimmed().toStdString());
    std::map<int, std::string> dataMap;
    if (!apiUrl.isEmpty()) {
        dataMap = apiReader.getApiResponseAll(apiUrl.toStdString());
    }
    ui.textDisplay->setText(dataMap.empty() ? "Empty Response from API" : "Successfully connected to API");
    QStringList dataList;
    for (auto const& p : dataMap) {
        std::string str = std::to_string(p.first) + " - " + p.second;
        QString qstr = QString::fromStdString(str);
        dataList << qstr;
    }
    resultsModel->setStringList(dataList);
}

void Client::onNextButtonClicked() {
    QString labelText = ui.pageLabel->text();
    int currentValue = labelText.toInt();
    int totalPages = apiReader.getTotalPages();
    if (totalPages <= currentValue) {
        currentValue = totalPages;
        return;
    }
    QString newText = QString::number(currentValue + 1);
    ui.pageLabel->setText(newText);
}

void Client::onPreviousButtonClicked() {
    QString labelText = ui.pageLabel->text();
    int currentValue = labelText.toInt();
    if (currentValue <= 1) {
        currentValue = 1;
        return;
    }
    QString newText = QString::number(currentValue - 1);
    ui.pageLabel->setText(newText);
}

Client::~Client()
{}
