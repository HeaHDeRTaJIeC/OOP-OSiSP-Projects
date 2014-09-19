#include "stdafx.h"
#include "LINES.h"

Lines :: Lines(int x0, int y0, int x1, int y1)
{
   lin = new POINT[2];

   lin[0].x = x0;
   lin[0].y = y0;

   lin[1].x = x1;
   lin[1].y = y1;
}

Lines :: ~Lines()
{
   delete [] lin;
}

void Lines :: Draw(HDC hdc)
{
	MoveToEx(hdc, lin[0].x, lin[0].y, NULL);
	LineTo(hdc, lin[1].x, lin[1].y);
}
