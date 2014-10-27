using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class ELLIPSES : SHAPES
{
    public ELLIPSES()
    {
        Number = 0;
    }

    public override void Add(Point current, Graphics e)
    {
        if (Number == 0)
        {
            Rect.Location = current;
            ++Number;
        }
        else
            if (Number == 1)
            {
                Rect.Width = current.X - Rect.Location.X;
                Rect.Height = current.Y - Rect.Location.Y;
                Draw(e);
                ++Number;
            }
    }

    public override void Draw(Graphics e)
    {
        e.DrawEllipse(new Pen(Color.Black), Rect);
    }

    private Rectangle Rect;
    private int Number;

}

