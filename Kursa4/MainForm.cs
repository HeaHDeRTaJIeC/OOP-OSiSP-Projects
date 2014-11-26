using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace _5Sem_Kursa4
{
    public partial class MainForm : Form
    {
        public enum Opponents
        {
            Bot,
            Player
        }

        public enum Players
        {
            First,
            Second,
            None
        }

        private Opponents opponent;
        private const int NumberOfRounds = 9;
        private int cardsChosen;
        private int counter;
        private int round;
        private readonly Random rand = new Random();
        private Players roundWinner;
        private bool firstPlayerBlocked;
        private bool secondPlayerBlocked;
        private Players turn = Players.First;
        short[] indexArray = new short[150];
        private readonly CardClass[] cards = new CardClass[150];
        private readonly CardClass[] cardsOnTable = new CardClass[4];
        private readonly List<CardClass> firstPlayerGameCards = new List<CardClass>();
        private readonly List<CardClass> firstPlayerOutCards = new List<CardClass>();
        private readonly List<CardClass> randomGameCards = new List<CardClass>();
        private readonly List<CardClass> secondPlayerGameCards = new List<CardClass>();
        private readonly List<CardClass> secondPlayerOutCards = new List<CardClass>();

        public MainForm()
        {
            InitializeComponent();
            CardClass.SetLocalPath(Path.GetDirectoryName(Application.ExecutablePath));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var str = new StreamReader(CardClass.LocalPath + "info.txt");
            for (var i = 0; i < 150; i++)
            {
                var line = str.ReadLine();
                Debug.Assert(line != null, "line != null");
                var data = line.Split(';');
                var value = 0;
                var name = data[0];
                try
                {
                    value = int.Parse(data[1]);
                }
                catch
                {
                    MessageBox.Show("Error parse");
                }
                var nation = data[2];
                cards[i] = new CardClass(name, nation, i + 1, value);
                cards[i].OnCardClick += GetNewCard;
                cards[i].OnCardClick += MoveCurrentCard;
                cards[i].OnCardClick += ActivateBot;
                Controls.Add(cards[i].ImageBox);
            }
            skip1.Enabled = false;
            skip2.Enabled = false;
            skip1.Visible = false;
            skip2.Visible = false;
            RoundLabel.Visible = false;
            TimeLabel.Visible = false;
            NumberOutCards1Label.Visible = false;
            NumberOutCards2Label.Visible = false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opponent = MessageBox.Show("Play PvP?", "Footbal", MessageBoxButtons.YesNo) == DialogResult.Yes ? Opponents.Player : Opponents.Bot;
            firstPlayerGameCards.Clear();
            secondPlayerGameCards.Clear();
            firstPlayerOutCards.Clear();
            secondPlayerOutCards.Clear();
            indexArray = MakeRandom();
            counter = 0;
            round = 0;
            for (int i = 0; i < 150; i++)
                cards[i].SetImagePosition(-200, -200);
            for (int i = 0; i < 10; i++)
            {
                var cardItem = cards[indexArray[counter]];
                if (i%2 == 0)
                    firstPlayerGameCards.Add(cardItem);
                else
                    secondPlayerGameCards.Add(cardItem);
                ++counter;
            }
            var count = 0;
            foreach (var item in firstPlayerGameCards)
            {
                item.SetImagePosition(182 + count*128, 504);
                item.SetVisible(false);
                item.ImageBox.BringToFront();
                item.Status = CardStatus.Player;
                item.PlayerOwner = CardOwner.First;
                ++count;
            }

            count = 0;
            foreach (var item in secondPlayerGameCards)
            {
                item.SetImagePosition(182 + count*128, 32);
                item.SetVisible(false);
                item.ImageBox.BringToFront();
                item.Status = CardStatus.Player;
                item.PlayerOwner = CardOwner.Second;
                ++count;
            }

            skip1.Enabled = false;
            skip2.Enabled = false;
            skip2.Visible = false;
            skip1.Visible = true;
            turn = Players.Second;
            turn = ChangeTurn(turn);
            RoundLabel.Visible = true;
            TimeLabel.Visible = true;
            NumberOutCards1Label.Visible = true;
            NumberOutCards2Label.Visible = true;
            RoundLabel.Text = "Round " + (round + 1);
            TimeLabel.Text = "Time\n" + (round*5) + ":00";
            NumberOutCards1Label.Text = firstPlayerOutCards.Count.ToString(CultureInfo.InvariantCulture);
            NumberOutCards2Label.Text = secondPlayerOutCards.Count.ToString(CultureInfo.InvariantCulture);
            if (opponent == Opponents.Bot)
            {
                foreach (var card in firstPlayerGameCards)
                    card.SetVisible(true);
                secondPlayerBlocked = true;
            }
        }

        private void skip1_Click(object sender, EventArgs e)
        {
            turn = ChangeTurn(turn);
            CompareCards();
        }

        private void skip2_Click(object sender, EventArgs e)
        {
            turn = ChangeTurn(turn);
            CompareCards();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}