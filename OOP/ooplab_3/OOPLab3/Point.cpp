#include "Point.h"

istream& operator>>(istream& is, Point& point)
{
	double x, y, z;
	is >> x >> y >> z;
	point = Point(x, y, z);
	return is;
}

Point Point::move(const MoveT& mt)
{
	double nx = x + mt.getdx();
	double ny = y + mt.getdy();
	double nz = z + mt.getdz();
	return Point(nx, ny, nz);
}

Point Point::scale(const ScaleT& st, const Point& center)
{
	double nx = x * st.getkx() + (1 - st.getkx()) * center.x;
	double ny = y * st.getky() + (1 - st.getky()) * center.y;
	double nz = z * st.getkz() + (1 - st.getkz()) * center.z;

	return Point(nx, ny, nz);
}

Point Point::rotate(const RotateT rt, const Point& center)
{
	Point point_c(*this);
	MoveT m_neg(-center.x, -center.y, -center.z);
	point_c = point_c.move(m_neg);

	_rotate(point_c.y, point_c.z, rt.getax());
	_rotate(point_c.x, point_c.z, rt.getay());
	_rotate(point_c.x, point_c.y, rt.getaz());

	MoveT m_pos(center.x, center.y, center.z);
	point_c = point_c.move(m_pos);

	return point_c;
}

void Point::_rotate(double& x, double& y, double a)
{
	double tmp_x = x;
	double tmp_y = y;
	tmp_x = x * cos(a) + y * sin(a);
	tmp_y = -x * sin(a) + y * cos(a);
	x = tmp_x;
	y = tmp_y;
}

Point Point::transform(const MoveT& mt, const ScaleT& st, const RotateT& rt, const Point& center)
{
	Point new_p = move(mt);
    new_p = new_p.rotate(rt, center);
    new_p = new_p.scale(st, center);
	return new_p;
}
