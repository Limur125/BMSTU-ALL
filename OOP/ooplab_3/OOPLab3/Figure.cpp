#include "Figure.h"

void Figure::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt)
{
	fd->transformation(mt, st, rt);
}

void Figure::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center)
{
	fd->transformation(mt, st, rt, center);
}

void FigureData::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center)
{
	this->center.transform(mt, st, rt, center);
	for (auto& point : points)
        point = point.transform(mt, st, rt, center);
}

void FigureData::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt)
{
	this->center.transform(mt, st, rt, center);
	for (auto& point : points)
		point = point.transform(mt, st, rt, center);
}
