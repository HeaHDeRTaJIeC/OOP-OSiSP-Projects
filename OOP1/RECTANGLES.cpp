#include "stdafx.h"
#include "RECTANGLES.h"

Rectangles :: Rectangles(int xLeftUp, int yLeftUp, int xDownRight, int yDownRight)
{
   Rect = new POINT[4];

   Rect[0].x = xLeftUp;
   Rect[0].y = yLeftUp;

   Rect[1].x = xDownRight;
   Rect[1].y = yLeftUp;

   Rect[2].x = xDownRight;
   Rect[2].y = yDownRight;

   Rect[3].x = xLeftUp;
   Rect[3].y = yDownRight;
}

Rectangles :: ~Rectangles()
{
   delete [] Rect;
}

void Rectangles :: Draw(HDC hdc)
{
	Polygon(hdc, Rect, 4);
}