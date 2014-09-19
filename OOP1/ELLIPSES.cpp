#include "stdafx.h"
#include "ELLIPSES.h"

Ellipses :: Ellipses(int xLeftUp, int yLeftUp, int xRightDown, int yRightDown)
{
    ellips = new POINT[2];

	ellips[0].x = xLeftUp;
	ellips[0].y = yLeftUp;

	ellips[1].x = xRightDown;
	ellips[1].y = yRightDown;
}

Ellipses :: ~Ellipses()
{
	delete [] ellips;
}

void Ellipses :: Draw(HDC hdc)
{
	Ellipse(hdc, ellips[0].x, ellips[0].y, ellips[1].x, ellips[1].y);
}