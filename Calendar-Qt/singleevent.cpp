#include "singleevent.h"
#include "ui_singleevent.h"

SingleEvent::SingleEvent(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::SingleEvent)
{
    ui->setupUi(this);
}

SingleEvent::~SingleEvent()
{
    delete ui;
}

void SingleEvent::on_buttonBox_accepted()
{
    accept();
    result = true;
}

void SingleEvent::on_buttonBox_rejected()
{
    reject();
    result = false;
}

bool SingleEvent::getResult() {
    return result;
}

QString SingleEvent::getDescription() {
    return ui->description->text();
}

QString SingleEvent::getTimeOfEvent() {
    return ui->timeOfEvent->text();
}

void SingleEvent::setDescription(QString des) {
    //ui->description->text() = des;
    ui->description->setText(des);
}

void SingleEvent::setTimeOfEvent(QTime tim) {
    ui->timeOfEvent->setTime(tim);
}
