#include "Drawer.h"

void QtDrawer::clear()
{
	this->scene->clear();
}

void QtDrawer::create_line(const Point& point1, const Point& point2)
{
	this->scene->addLine(point1.getx(), point1.gety(), point2.getx(), point2.gety());
}
