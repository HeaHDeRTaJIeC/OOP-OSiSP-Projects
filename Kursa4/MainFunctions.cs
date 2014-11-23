using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainClass;

namespace _5Sem_Kursa4
{
    public partial class MainForm
    {
        Players ChangeTurn(Players Item)
        {
            foreach (var card in FirstPlayerGameCards)
                card.setVisible(false);
            foreach (var card in SecondPlayerGameCards)
                card.setVisible(false);
            skip1.Visible = false;
            skip2.Visible = false;

            if (Item == Players.First)
            {
                MessageBox.Show("Player 2 turn", "Football");
                foreach (var card in SecondPlayerGameCards)
                    card.setVisible(true);
                skip2.Visible = true;
                Item = Players.Second;
            }
            else
            {
                MessageBox.Show("Player 1 turn", "Football");
                foreach (var card in FirstPlayerGameCards)
                    card.setVisible(true);
                skip1.Visible = true;
                Item = Players.First;

            }
            return Item;
        }

        void MoveCurrentCard(CardClass Item)
        {
            if ((Turn == Players.First && Item.PlayerOwner == CardOwner.First) ||
                (Turn == Players.Second && Item.PlayerOwner == CardOwner.Second))
            {
                if ((Item.PlayerOwner == CardOwner.First && !FirstPlayerBlocked) ||
                    (Item.PlayerOwner == CardOwner.Second && !SecondPlayerBlocked))
                {
                    int TableIndex;
                    if (Item.PlayerOwner == CardOwner.First)
                        if (CardsOnTable[1] == null)
                            TableIndex = 1;
                        else
                        {
                            TableIndex = 0;
                            FirstPlayerBlocked = true;
                        }
                    else
                        if (CardsOnTable[2] == null)
                            TableIndex = 2;
                        else
                        {
                            TableIndex = 3;
                            SecondPlayerBlocked = true;
                        }
                    ++CardsChosen;
                    CardsOnTable[TableIndex] = Item;
                    Item.setImagePosition(244 + TableIndex * 128, 268);
                    Item.ImageBox.BringToFront();
                    Item.Status = CardStatus.Table;
                    Turn = ChangeTurn(Turn);
                    if (CardsChosen > 1)
                        CompareCards();
                }
            }
        }

        void GetNewCard(CardClass Item)
        {
            if ((Turn == Players.First && Item.PlayerOwner == CardOwner.First) ||
                (Turn == Players.Second && Item.PlayerOwner == CardOwner.Second))
            {
                if ((Item.PlayerOwner == CardOwner.First && !FirstPlayerBlocked) ||
                    (Item.PlayerOwner == CardOwner.Second && !SecondPlayerBlocked))
                {
                    int index;
                    if (Item.PlayerOwner == CardOwner.First)
                        index = FirstPlayerGameCards.IndexOf(Item);
                    else
                        index = SecondPlayerGameCards.IndexOf(Item);

                    if (Item.PlayerOwner == CardOwner.Second)
                    {
                        Cards[Counter].setImagePosition(182 + index * 128, 32);
                        Cards[Counter].PlayerOwner = CardOwner.Second;
                        SecondPlayerGameCards[index] = Cards[Counter];
                    }
                    else
                    {
                        Cards[Counter].setImagePosition(182 + index * 128, 504);
                        Cards[Counter].PlayerOwner = CardOwner.First;
                        FirstPlayerGameCards[index] = Cards[Counter];
                    }

                    Cards[Counter].setVisible(true);
                    Controls.Add(Cards[Counter].ImageBox);
                    Cards[Counter].ImageBox.BringToFront();
                    Cards[Counter].Status = CardStatus.Player;
                    ++Counter;
                }
            }
        }

        void CompareCards()
        {
            switch (CardsChosen)
            {
                case 2:
                    {
                        if (CardsOnTable[1].Nation == CardsOnTable[2].Nation ||
                            CardsOnTable[1].Value == CardsOnTable[2].Value)                   
                        {
                            RandomCards();
                            StartNewRound();
                        }
                        else
                            if (CardsOnTable[1].Value > CardsOnTable[2].Value)
                                if (Turn == Players.Second && CheckOtherPlayerCards(CardsOnTable[2].Nation, Players.Second) == true)
                                    skip2.Enabled = true;
                                else
                                {
                                    MessageBox.Show("First win", "Football");
                                    RoundWinner = Players.First;
                                    StartNewRound();
                                }
                            else
                                if (Turn == Players.First && CheckOtherPlayerCards(CardsOnTable[1].Nation, Players.First))
                                    skip1.Enabled = true;
                                else
                                {
                                    MessageBox.Show("Second win", "Football");
                                    RoundWinner = Players.Second;
                                    StartNewRound();
                                }
                        break;
                    }
                case 3:
                    {
                        if (CompareCardsOnTable() == Players.First)
                            if (Turn == Players.Second && skip2.Enabled == false &&
                                CheckOtherPlayerCards(CardsOnTable[2].Nation, Turn) == true)
                                skip2.Enabled = true;
                            else
                            {
                                MessageBox.Show("First win", "Football");
                                RoundWinner = Players.First;
                                StartNewRound();
                            }
                        else
                            if (CompareCardsOnTable() == Players.Second)
                                if (Turn == Players.First && skip1.Enabled == false &&
                                    CheckOtherPlayerCards(CardsOnTable[1].Nation, Turn) == true)
                                    skip1.Enabled = true;
                                else
                                {
                                    MessageBox.Show("Second win", "Football");
                                    RoundWinner = Players.Second;
                                    StartNewRound();
                                }
                        break;
                    }
                case 4:
                    {
                        if (CompareCardsOnTable() == Players.First)
                        {
                            MessageBox.Show("First win", "Football");
                            RoundWinner = Players.First;
                            StartNewRound();
                        }
                        else
                            if (CompareCardsOnTable() == Players.Second)
                            {
                                MessageBox.Show("Second win", "Football");
                                RoundWinner = Players.Second;
                                StartNewRound();
                            }
                            else
                            {
                                RandomCards();
                                StartNewRound();
                            }
                        break;
                    }
            }
        }

        void RandomCards()
        {
            MessageBox.Show("Penalties", "Football");

            int FirstPlayerSumma = 0;
            int SecondPlayerSumma = 0;

            foreach (CardClass card in FirstPlayerGameCards)
                card.ImageBox.Visible = false;
            foreach (CardClass card in SecondPlayerGameCards)
                card.ImageBox.Visible = false;
            for (int i = 0; i < 4; i++)
                if (CardsOnTable[i] != null)
                    CardsOnTable[i].ImageBox.Visible = false;
            FirstPlayerBlocked = true;
            SecondPlayerBlocked = true;
            skip1.Visible = false;
            skip2.Visible = false;

            for (int i = 0; i < 6; i++)
            {
                Cards[Counter].setVisible(true);
                Controls.Add(Cards[Counter].ImageBox);
                Cards[Counter].ImageBox.BringToFront();
                Cards[Counter].Status = CardStatus.Table;
                if (i % 2 == 0)
                {
                    Cards[Counter].setImagePosition(240 + i * 100, 420);
                    RandomGameCards.Add(Cards[Counter]);
                    FirstPlayerSumma += Cards[Counter].Value;
                }
                else
                {
                    Cards[Counter].setImagePosition(240 + (i - 1) * 100, 120);
                    RandomGameCards.Add(Cards[Counter]);
                    SecondPlayerSumma += Cards[Counter].Value;
                }
                ++Counter;
                Form.ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
            }

            MessageBox.Show("First " + FirstPlayerSumma.ToString() + "\nSecond " + SecondPlayerSumma.ToString(), "Football");

            if (FirstPlayerSumma > SecondPlayerSumma)
            {
                MessageBox.Show("First win", "Football");
                foreach (CardClass card in RandomGameCards)
                {
                    card.setVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    Controls.Remove(card.ImageBox);
                    card.setImagePosition(830, 504);
                    FirstPlayerOutCards.Add(card);
                }
                RandomGameCards.Clear();
                RoundWinner = Players.First;
            }
            else if (SecondPlayerSumma > FirstPlayerSumma)
            {
                MessageBox.Show("Second win", "Football");
                foreach (CardClass card in RandomGameCards)
                {
                    card.setVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    Controls.Remove(card.ImageBox);
                    card.setImagePosition(830, 32);
                    SecondPlayerOutCards.Add(card);
                }
                RandomGameCards.Clear();
                RoundWinner = Players.Second;
            }
            else
            {
                MessageBox.Show("Round draw", "Football");
                foreach(CardClass card in RandomGameCards)
                {
                    card.setVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    Controls.Remove(card.ImageBox);
                    card.setImagePosition(0, 0);
                }
                RandomGameCards.Clear();
                RoundWinner = Players.None;
            }

            foreach (CardClass card in FirstPlayerGameCards)
                card.ImageBox.Visible = true;
            foreach (CardClass card in SecondPlayerGameCards)
                card.ImageBox.Visible = true;
            for (int i = 0; i < 4; i++)
                if (CardsOnTable[i] != null)
                    CardsOnTable[i].ImageBox.Visible = true;
        }

        bool CheckOtherPlayerCards(string nation, Players turn)
        {
            bool Result = false;
            if (turn == Players.First)
                foreach (var card in FirstPlayerGameCards)
                {
                    if (nation == card.Nation)
                        Result = true;
                }
            else
                foreach (var card in SecondPlayerGameCards)
                {
                    if (nation == card.Nation)
                        Result = true;
                }

            return Result;
        }

        Players CompareCardsOnTable()
        {
            Players Result;
            int Player1 = CardsOnTable[1].Value;
            int Player2 = CardsOnTable[2].Value;

            if (CardsOnTable[0] != null)
                Player1 += CardsOnTable[0].Value;
            if (CardsOnTable[3] != null)
                Player2 += CardsOnTable[3].Value;

            if (Player1 > Player2)
                Result = Players.First;
            else
                if (Player2 > Player1)
                    Result = Players.Second;
                else
                    Result = Players.None;

            return Result;
        }

        void StartNewRound()
        {
            for (int i = 0; i < 4; i++)
                if (CardsOnTable[i] != null)
                {
                    CardsOnTable[i].setVisible(false);
                    CardsOnTable[i].Status = CardStatus.OutOfGame;
                    if (RoundWinner == Players.First)
                    {
                        FirstPlayerOutCards.Add(CardsOnTable[i]);
                        CardsOnTable[i].setImagePosition(855, 424);
                    }
                    else if (RoundWinner == Players.Second)
                    {
                        SecondPlayerOutCards.Add(CardsOnTable[i]);
                        CardsOnTable[i].setImagePosition(855, 112);
                    }
                    else
                    {
                        Controls.Remove(CardsOnTable[i].ImageBox);
                        CardsOnTable[i].setImagePosition(0, 0);
                    }
                    CardsOnTable[i] = null;
                }
            CardsChosen = 0;
            FirstPlayerBlocked = false;
            SecondPlayerBlocked = false;
            skip1.Enabled = false;
            skip2.Enabled = false;
            if (RoundWinner != Turn)
                Turn = ChangeTurn(Turn);
            ++Round;
            RoundLabel.Text = "Round " + (Round + 1).ToString();
            TimeLabel.Text = "Time\n" + (Round * 15).ToString() + ":00";
            NumberOutCards1Label.Text = FirstPlayerOutCards.Count.ToString();
            NumberOutCards2Label.Text = SecondPlayerOutCards.Count.ToString();
            if (Round == 7)
            {
                MessageBox.Show("Final round", "Football"); //end routine
                FinalGame();
            }
        }

        void FinalGame()
        {
            foreach (CardClass item in FirstPlayerGameCards)
            {
                Controls.Remove(item.ImageBox);
            }
            foreach (CardClass item in SecondPlayerGameCards)
            {
                Controls.Remove(item.ImageBox);
            }
            FirstPlayerOutCards.Sort(compare);
            SecondPlayerOutCards.Sort(compare);

            int FirstIndex = 11;
            if (FirstPlayerOutCards.Count < 11)
                FirstIndex = FirstPlayerOutCards.Count;
            int SecondIndex = 11;
            if (SecondPlayerOutCards.Count < 11)
                SecondIndex = SecondPlayerOutCards.Count;

            int FirstSumma = 0;
            int SecondSumma = 0;

            for (int i = 0; i < FirstIndex; i++)
            {
                FirstPlayerOutCards[i].setVisible(true);
                if (i < 6)
                    FirstPlayerOutCards[i].setImagePosition(120 + i * 128, 360);
                    
                else
                    FirstPlayerOutCards[i].setImagePosition(182 + (i - 6) * 128, 504);
                FirstSumma += FirstPlayerOutCards[i].Value;
                FirstPlayerOutCards[i].ImageBox.BringToFront();
                Form.ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
                
            }
            Form.ActiveForm.Refresh();

            for (int i = 0; i < SecondIndex; i++)
            {
                SecondPlayerOutCards[i].setVisible(true);
                if (i < 5)
                    SecondPlayerOutCards[i].setImagePosition(182 + i * 128, 32);
                else
                    SecondPlayerOutCards[i].setImagePosition(120 + (i - 5) * 128, 180);
                SecondSumma += SecondPlayerOutCards[i].Value;
                SecondPlayerOutCards[i].ImageBox.BringToFront();
                Form.ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
            }
            Form.ActiveForm.Refresh();

            MessageBox.Show("First " + FirstSumma.ToString() + "\nSecond" + SecondSumma.ToString());

            if (FirstSumma > SecondSumma)
                MessageBox.Show("First win game", "Football");
            else if (SecondSumma > FirstSumma)
                MessageBox.Show("Second win game", "Football");

        }

        int compare(CardClass x, CardClass y)
        {
            return x.Value.CompareTo(y.Value);
        }

        void MakeRandom()
        { 
        }

    }
}
