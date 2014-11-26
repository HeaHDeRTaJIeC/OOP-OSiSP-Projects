using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace _5Sem_Kursa4
{
    public enum CardOwner
    {
        First,
        Second,
        None
    }

    enum CardStatus
    {
        NotInGame,
        Player,
        Table,
        OutOfGame
    }

    class CardClass
    {
    
        public CardClass(string name, string nation, int numberToLoad, int value)
        {
            this.name = name;
            Value = value;
            Nation = nation;
            this.numberToLoad = numberToLoad;
            Status = CardStatus.NotInGame;
            PlayerOwner = CardOwner.None;
            ImageBox = new PictureBox();
            ImageBox.Width = 122;
            ImageBox.Height = 176;
            ImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ImageBox.MouseClick += ImageMouseClick;
        }

        public void SetImagePosition(int x, int y)
        {
            ImageBox.Top = y;
            ImageBox.Left = x;
        }

        public void SetVisible(bool b)
        {
            Visible = b;
            if (Visible)
                LoadFrontImage();
            else
                LoadBackImage();
        }

        public static void SetLocalPath(string path)
        {
            LocalPath = path + "\\Resources\\";
        }

        public delegate void MoveCard(CardClass item);

        public event MoveCard OnCardClick;

        public PictureBox ImageBox;
        public CardStatus Status;
        public int Value { get; private set; }
        public string Nation { get; private set; }
        public CardOwner PlayerOwner { get; set; }
        public static string LocalPath { get; private set; }

        private Bitmap MakeFrontImage()
        {
            Bitmap bitmapPlayer = new Bitmap(LocalPath + "Players\\" + numberToLoad + ".png");
            Bitmap bitmapFlag = new Bitmap(LocalPath + "Flags\\" + Nation + ".png");
            Bitmap finalBitmap = new Bitmap(LocalPath + "CardFon.png");
            Font font = new Font("Arial", 22);

            Graphics graph = Graphics.FromImage(finalBitmap);

            graph.DrawImage(bitmapPlayer, new Rectangle(7, 40, 120, 120));
            graph.DrawImage(bitmapFlag, new Rectangle(70, 5, 60, 37));
            graph.DrawString(Value.ToString(CultureInfo.InvariantCulture), font, Brushes.Gold, new Point(5, 5));
            graph.DrawString(name, new Font("Arial", 10), Brushes.Gold, new Rectangle(8, 162, 100, 25));

            return finalBitmap;
        }

        private void LoadFrontImage()
        {
            ImageBox.Image = MakeFrontImage();
        }

        private void LoadBackImage()
        {
            ImageBox.Load(LocalPath + "CardBack.png");
            Color temp = ColorTranslator.FromOle(0x54FF70);
            ImageBox.BackColor = Color.FromArgb(250, temp);
        }

        private bool Visible { get; set; }

        private void ImageMouseClick(object sender, EventArgs e)
        {
            CardClicked();
        }

        public void CardClicked()
        {
            switch (Status)
            {
                case CardStatus.Player:
                    Debug.Assert(OnCardClick != null, "OnCardClick != null");
                    OnCardClick(this);
                    break;
            }
        }

        private readonly int numberToLoad;
        private readonly string name;
    }
}
