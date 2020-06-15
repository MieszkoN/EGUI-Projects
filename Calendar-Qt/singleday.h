#ifndef SINGLEDAY_H
#define SINGLEDAY_H

#include <QDialog>
#include <QVector>
#include <QMap>
#include "element.h"

namespace Ui {
class SingleDay;
}

class SingleDay : public QDialog
{
    Q_OBJECT
private:
    Ui::SingleDay *ui;
    int numberOfEvents;
    QString dateOfEvent;
    enum Column {
        TIME, DESCRIPTION
    };
    QVector <Element> eventsVector;

public:
    explicit SingleDay(QWidget *parent = nullptr);
    ~SingleDay();
    int getNumberOfEvents();
    void setDateOfEvent(QString date);
    QVector <Element> getEventsVector();
    void insertTimeAndDescription(const QString &d,const QMap<QString, QVector<Element>> &m);
private slots:
    void on_AddNewButton_clicked();
    void on_deleteButton_clicked();
    void on_editButton_clicked();
    void on_closeButton_clicked();
};

#endif // SINGLEDAY_H
