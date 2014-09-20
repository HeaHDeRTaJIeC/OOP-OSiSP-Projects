// ELLIPSES.h
#include "..\SHAPES\SHAPES.h"

#ifndef ELLIPSES_H
#define ELLIPSES_H

class _declspec(dllexport) Ellipses : public Shapes
{
public:
    Ellipses(int = 0, int = 0, int = 0, int = 0);
	~Ellipses();
	virtual void Draw(HDC);
private:
    POINT *ellips;
};

#endif