#include "stdafx.h"
#include "TRIANGLES.h"

Triangles :: Triangles(int x0, int y0, int x1, int y1, int x2, int y2)
{
   tri = new POINT[3];

   tri[0].x = x0;
   tri[0].y = y0;

   tri[1].x = x1;
   tri[1].y = y1;

   tri[2].x = x2;
   tri[2].y = y2;

}

Triangles :: ~Triangles()
{
   delete [] tri;
}

void Triangles :: Draw(HDC hdc)
{
   Polygon(hdc, tri, 3);
}
