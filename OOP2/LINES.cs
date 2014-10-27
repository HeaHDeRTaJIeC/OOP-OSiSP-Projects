using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class LINES : SHAPES
{
    public LINES()
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
        e.DrawLine(new Pen(Color.Black), First, Second);
    }

    private Point First;
    private Point Second;
    private int Number;
}

