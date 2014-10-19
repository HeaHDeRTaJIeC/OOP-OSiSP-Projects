using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class ELLIPSES : SHAPES
{
    public ELLIPSES(Point first, Point second)
    {
        Rect.Location = first;
        Rect.Width = second.X - first.X;
        Rect.Height = second.Y - first.Y;
    }

    public override void Draw(Graphics e)
    {
        e.DrawEllipse(new Pen(Color.Black), Rect);
    }

    private Rectangle Rect;

}

