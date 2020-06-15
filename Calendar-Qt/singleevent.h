#ifndef SINGLEEVENT_H
#define SINGLEEVENT_H

#include <QDialog>

namespace Ui {
class SingleEvent;
}

class SingleEvent : public QDialog
{
    Q_OBJECT

public:
    explicit SingleEvent(QWidget *parent = nullptr);
    ~SingleEvent();


    QString getTimeOfEvent();
    QString getDescription();
    void setTimeOfEvent(QTime tim);
    void setDescription(QString des);
    bool getResult();


private slots:
    void on_buttonBox_accepted();
    void on_buttonBox_rejected();

private:
    Ui::SingleEvent *ui;
    bool result;
};

#endif // SINGLEEVENT_H
