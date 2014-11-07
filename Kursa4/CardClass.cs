using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MainClass
{
    class CardClass
    {
        public const string LocalPath = @"D:\Completed Programs\5 Semestr Kursa4\5Sem_Kursa4\5Sem_Kursa4\Resources\";

        public CardClass(string name, string nation, int numberToLoad, int value, bool flag = false)
        {
            Name = name;
            Value = value;
            Nation = nation;
            NumberToLoad = numberToLoad;
            ImageBox = new PictureBox();
            ImageBox.Width = 122;
            ImageBox.Height = 176;
            ImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ImageBox.MouseClick += ImageMouseClick;
        }

        public int getValue()
        {
            return Value;
        }

        public string getNation()
        {
            return Nation;
        }

        public void setImagePosition(int x, int y)
        {
            ImageBox.Top = y;
            ImageBox.Left = x;
        }

        public void setVisible(bool b)
        {
            Visible = b;
            if (Visible)
                loadFrontImage();
            else
                loadBackImage();
        }

        public PictureBox ImageBox;

        private Bitmap makeFrontImage()
        {
            Bitmap bitmapPlayer = new Bitmap(LocalPath + "Players\\" + NumberToLoad.ToString() + ".png");
            Bitmap bitmapFlag = new Bitmap(LocalPath + "Flags\\" + Nation + ".png");
            Bitmap finalBitmap = new Bitmap(LocalPath + "CardFon.png");
            Font font = new Font("Arial", 22);

            Graphics graph = Graphics.FromImage(finalBitmap);

            graph.DrawImage(bitmapPlayer, new Rectangle(10, 40, 120, 120));
            graph.DrawImage(bitmapFlag, new Rectangle(70, 5, 60, 37));
            graph.DrawString(Value.ToString(), font, Brushes.Gold, new Point(5, 5));
            graph.DrawString(Name, new Font("Arial", 10), Brushes.Gold, new Rectangle(8, 162, 100, 25));

            return finalBitmap;
        }

        private void loadFrontImage()
        {
            ImageBox.Image = makeFrontImage();
        }

        private void loadBackImage()
        {
            ImageBox.Load(LocalPath + "CardBack.jpg");
        }

        private bool Visible { get; set; }

        private void ImageMouseClick(object sender, EventArgs e)
        {
            setImagePosition(10, 10);
        }
        private int NumberToLoad;
        private int Value;
        private string Nation;
        private string Name;
        
    }
}
