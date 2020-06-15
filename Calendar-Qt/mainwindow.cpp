#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QCalendarWidget>
#include <QAbstractButton>
#include <QLayout>
#include <QPushButton>
#include <QTextCharFormat>
#include <QCoreApplication>
#include "singleday.h"
#include <QtGui>
#include <QMap>
#include <QColor>
#include <QMessageBox>
using namespace std;

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
{
    calendar = new QCalendarWidget();

    //Reading from a file
    QFile inputFile("myFile.txt");
    if (inputFile.open(QIODevice::ReadOnly))
    {
       QTextStream in(&inputFile);
       while (!in.atEnd())
       {
          QString line = in.readLine();
          QStringList lst = line.split("||");
          QString key = lst.at(0);
          QDate d = QDate::fromString(key, "d/M/yyyy");
          dayVec = new QVector<Element>;
          for(int i = 1; i<lst.size();i+=2) {
              Element *add = new Element;
              add->setTimeOfElement(lst.at(i));
              add->setDescriptionOfElement(lst.at(i+1));
              dayVec->push_back(*add);
          }
          mapOfDays.insert(key, *dayVec);
          QTextCharFormat blueDate;
          QColor c(3, 138, 255, 255);
          blueDate.setBackground(c);
          calendar->setDateTextFormat(QDate(d.year(), d.month(), d.day()), blueDate);
       }
       inputFile.close();
    }


    resize(800, 600);
    calendar->setFirstDayOfWeek(Qt::Sunday);
    calendar->setHorizontalHeaderFormat(QCalendarWidget::SingleLetterDayNames);                           //Name of days are displayed as one letters
    calendar->setVerticalHeaderFormat(QCalendarWidget::NoVerticalHeader);                                 //lack of number of weeks
    calendar->setLocale(QLocale::English);                                                                //default language - English
    QTextCharFormat format;
    format.setFontWeight(81);                                                                     //81 - very bold
    calendar->setHeaderTextFormat(format);
    QWidget * wdg = new QWidget(this);
    QGridLayout *vlay = new QGridLayout(wdg);
    vlay->addWidget(calendar);
    vlay->setContentsMargins(0,0,0,0);
    wdg->setLayout(vlay);
    setCentralWidget(wdg);
    setWindowTitle("Calendar");
    connect(calendar, SIGNAL(clicked(QDate)), this, SLOT (showDialog()));
}

MainWindow::~MainWindow()
{
    //saving to file
    QFile file("myFile.txt");
    if(!file.open(QFile::WriteOnly | QFile::Text)) {
        QMessageBox::warning(this, "title", "file not open");
    }
    QTextStream out(&file);
    for(QMap<QString, QVector<Element>>::const_iterator it = mapOfDays.begin(); it != mapOfDays.end(); ++it) {
        QString text = it.key();
        out << text;
        for(int i = 0; i<it.value().size(); i++) {
            out << "||" << it.value().at(i).getTimeOfElement();
            out << "||" << it.value().at(i).getDescriptionOfElement();
        }
        out << endl;
    }

    file.flush();
    file.close();
    delete calendar;
    mapOfDays.clear();
}

void MainWindow::showDialog() {
    int day = calendar->selectedDate().day();
    int month = calendar->selectedDate().month();
    int year = calendar->selectedDate().year();
    dayVec = new QVector<Element>();
    QString key = QString::number(day)+ "/" + QString::number(month) + "/" + QString::number(year);
    sd = new SingleDay();
    sd->setDateOfEvent(key);
    sd->insertTimeAndDescription(key, mapOfDays);
    sd->setModal(true);
    sd->exec();
    if(sd->getNumberOfEvents() != 0) {

        QTextCharFormat blueDate;
        QColor c(3, 138, 255, 255);
        blueDate.setBackground(c);
        calendar->setDateTextFormat(QDate(year, month, day), blueDate);

        for(int i=0;i<sd->getEventsVector().size(); i++) {
            Element *add = new Element();
            add->setTimeOfElement(sd->getEventsVector().at(i).getTimeOfElement());
            add->setDescriptionOfElement(sd->getEventsVector().at(i).getDescriptionOfElement());
            dayVec->push_back(*add);
        }
        mapOfDays.insert(key, *dayVec);
    } else {
        QTextCharFormat whiteDate;
        whiteDate.setBackground(Qt::white);
        calendar->setDateTextFormat(QDate(year, month, day), whiteDate);
        if(mapOfDays.contains(key)) {
            mapOfDays.remove(key);
        }
    }
}
