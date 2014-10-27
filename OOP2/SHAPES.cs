using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


public abstract class SHAPES
{
    public abstract void Draw(Graphics e);
    public abstract void Add(Point current, Graphics e);
}

