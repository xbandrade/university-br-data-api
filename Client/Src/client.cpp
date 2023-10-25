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
    QStringList dataList;
    dataList << "Item 1" << "Item 2" << "Item 3";
    resultsModel->setStringList(dataList);
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
        selectedItems.clear();
        selectedItems << data.toString();
        if (detailsModel) {
            detailsModel->setStringList(selectedItems);
        }
    }
}

void Client::onPushButtonClicked() {
    QString apiUrl = QString::fromStdString(ui.apiUrlLineEdit->text().trimmed().toStdString());
    std::map<int, std::string> dataMap;
    if (!apiUrl.isEmpty()) {
        dataMap = apiReader.getApiResponse(apiUrl.toStdString());
    }
    ui.textDisplay->setText(dataMap.empty() ? "Empty Response" : QString::fromStdString(std::to_string(dataMap.size())));
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
