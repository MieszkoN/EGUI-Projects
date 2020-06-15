#ifndef ELEMENT_H
#define ELEMENT_H
#include <QString>

class Element
{
private:
    QString timeOfElement;
    QString descriptionOfElement;
public:
    Element();
    QString getTimeOfElement() const;
    void setTimeOfElement(QString eventTime);
    QString getDescriptionOfElement() const;
    void setDescriptionOfElement(QString des);
};

#endif // ELEMENT_H
