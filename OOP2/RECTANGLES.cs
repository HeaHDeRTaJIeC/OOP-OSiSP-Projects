using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class RECTANGLES : SHAPES
{
    public RECTANGLES()
    {
        Number = 0;
    }

    public override void Add(Point current, Graphics e)
    {
        if (Number == 0)
        {
            First = current;
            ++Number;
        }
        else
            if (Number == 1)
            {
                Second = current;
                Draw(e);
                ++Number;
            }
    }

    public override void Draw(Graphics e)
    {
        e.DrawRectangle(new Pen(Color.Black), First.X, First.Y, Second.X - First.X, Second.Y - First.Y);
    }

    private Point First;
    private Point Second;
    private int Number;
}

