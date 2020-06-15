#include "singleday.h"
#include "ui_singleday.h"
#include "singleevent.h"
#include "mainwindow.h"
#include "element.h"
#include <QMap>
#include <QPushButton>

SingleDay::SingleDay(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::SingleDay)
{
    ui->setupUi(this);
    numberOfEvents = 0;
}

SingleDay::~SingleDay()
{
    delete ui;
}

void SingleDay::on_AddNewButton_clicked()
{
    QString str, description, timeOfEvent;
    SingleEvent *eve = new SingleEvent;
    eve->setModal(true);
    eve->exec();
    int col;
    bool res = eve->getResult();
    if(!res) {
        return;
    } else {
        description = eve->getDescription();
        timeOfEvent = eve->getTimeOfEvent();
        Element add;
        add.setDescriptionOfElement(eve->getDescription());
        add.setTimeOfElement(eve->getTimeOfEvent());
        eventsVector.push_back(add);
        ui->TableWidget->insertRow(ui->TableWidget->rowCount());
        col = ui->TableWidget->rowCount() - 1;
        ui->TableWidget->setItem(col, TIME, new QTableWidgetItem(timeOfEvent));
        ui->TableWidget->setItem(col, DESCRIPTION, new QTableWidgetItem(description));
        ui->TableWidget->sortItems(0, Qt::AscendingOrder);
        numberOfEvents++;
    }

}

int SingleDay::getNumberOfEvents() {
    return numberOfEvents;
}

void SingleDay::insertTimeAndDescription(const QString &d, const QMap<QString,QVector<Element>> &m) {
    int col;
    for(QMap<QString, QVector<Element>>::const_iterator it = m.begin(); it != m.end(); ++it) {
        if(d == it.key()) {
            for(int i = 0; i<it.value().size(); i++) {
                ui->TableWidget->insertRow(ui->TableWidget->rowCount());
                col = ui->TableWidget->rowCount() - 1;
                Element *add = new Element();
                QString t = it.value().at(i).getTimeOfElement();
                QString des = it.value().at(i).getDescriptionOfElement();
                add->setTimeOfElement(t);
                add->setDescriptionOfElement(des);
                eventsVector.push_back(*add);
                ui->TableWidget->setItem(col, TIME, new QTableWidgetItem(t));
                ui->TableWidget->setItem(col, DESCRIPTION, new QTableWidgetItem(des));
            }
        }
    }
    ui->TableWidget->sortItems(0, Qt::AscendingOrder);
    numberOfEvents = ui->TableWidget->rowCount();
}

 void SingleDay::setDateOfEvent(QString date) {
     dateOfEvent = date;
 }


void SingleDay::on_deleteButton_clicked()
{
    QItemSelectionModel *select = ui->TableWidget->selectionModel();

    if(select->hasSelection()) {
        int len = select->selectedRows().size();
        for(int i=0; i<len; i++) {
            int row = ui->TableWidget->currentRow();
            QString tim = ui->TableWidget->item(row, 0)->text();
            QString des = ui->TableWidget->item(row, 1)->text();
            for(int i = 0; i<eventsVector.size(); i++) {
                if(eventsVector.at(i).getTimeOfElement() == tim && eventsVector.at(i).getDescriptionOfElement() == des) {
                    eventsVector.erase(eventsVector.begin()+i);
                }
            }
            ui->TableWidget->removeRow(ui->TableWidget->currentRow());
            numberOfEvents--;
        }
    }
}



void SingleDay::on_editButton_clicked()
{
    QItemSelectionModel *select = ui->TableWidget->selectionModel();
    if(select->hasSelection() && select->selectedRows().size()==1) {
        int row = ui->TableWidget->currentRow();
        SingleEvent *eve = new SingleEvent();
        eve->setModal(true);
        QString tim = ui->TableWidget->item(row, 0)->text();
        QString des = ui->TableWidget->item(row, 1)->text();
        QTime ti;
        int h = ti.fromString(tim, "hh:mm").hour();
        int m = ti.fromString(tim, "hh:mm").minute();
        QTime n(h,m);
        eve->setTimeOfEvent(n);
        eve->setDescription(des);
        eve->exec();
        Element *change = new Element();
        QString description = eve->getDescription();
        QString timeOfEvent = eve->getTimeOfEvent();
        change->setTimeOfElement(timeOfEvent);
        change->setDescriptionOfElement(description);
        for(int i = 0; i<eventsVector.size(); i++) {
            if(eventsVector.at(i).getDescriptionOfElement() == des && eventsVector.at(i).getTimeOfElement() == tim) {
                eventsVector.replace(i, *change);
            }
        }
        ui->TableWidget->setItem(row, TIME, new QTableWidgetItem(timeOfEvent));
        ui->TableWidget->setItem(row, DESCRIPTION, new QTableWidgetItem(description));
        ui->TableWidget->sortItems(0, Qt::AscendingOrder);
    }
}

QVector<Element> SingleDay::getEventsVector() {
    return eventsVector;
}

void SingleDay::on_closeButton_clicked()
{
    this->close();
}
