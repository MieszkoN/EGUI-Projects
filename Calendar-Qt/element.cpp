#include "element.h"

Element::Element()
{

}

QString Element::getTimeOfElement() const {
    return timeOfElement;
}

void Element::setTimeOfElement(QString eventTime) {
    timeOfElement = eventTime;
}

QString Element::getDescriptionOfElement() const {
    return descriptionOfElement;
}

void Element::setDescriptionOfElement(QString des) {
    descriptionOfElement = des;
}
