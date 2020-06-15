#ifndef MAINWINDOW_H
#define MAINWINDOW_H
#include <QCalendarWidget>
#include <QPushButton>
#include <QPen>
#include <QMap>
#include "singleday.h"
#include "element.h"
#include <QMainWindow>

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

signals:
    void clickedCalendarDay(const QDate &date);

private slots:
    void showDialog();

private:
    //Ui::MainWindow *ui;
    QCalendarWidget *calendar;
    SingleDay *sd;
    QMap <QString, QVector<Element>> mapOfDays;
    QVector <Element> *dayVec;
};
#endif // MAINWINDOW_H
