using System.Globalization;
using System.Windows.Forms;

namespace _5Sem_Kursa4
{
    public partial class MainForm
    {
        Players ChangeTurn(Players item)
        {
            foreach (var card in firstPlayerGameCards)
                card.SetVisible(false);
            foreach (var card in secondPlayerGameCards)
                card.SetVisible(false);
            skip1.Visible = false;
            skip2.Visible = false;

            if (item == Players.First)
            {
                MessageBox.Show("Player 2 turn", "Football");
                foreach (var card in secondPlayerGameCards)
                    card.SetVisible(true);
                skip2.Visible = true;
                item = Players.Second;
            }
            else
            {
                MessageBox.Show("Player 1 turn", "Football");
                foreach (var card in firstPlayerGameCards)
                    card.SetVisible(true);
                skip1.Visible = true;
                item = Players.First;

            }
            return item;
        }

        void MoveCurrentCard(CardClass item)
        {
            if ((turn == Players.First && item.PlayerOwner == CardOwner.First) ||
                (turn == Players.Second && item.PlayerOwner == CardOwner.Second))
            {
                if ((item.PlayerOwner == CardOwner.First && !firstPlayerBlocked) ||
                    (item.PlayerOwner == CardOwner.Second && !secondPlayerBlocked))
                {
                    int tableIndex;
                    if (item.PlayerOwner == CardOwner.First)
                        if (cardsOnTable[1] == null)
                            tableIndex = 1;
                        else
                        {
                            tableIndex = 0;
                            firstPlayerBlocked = true;
                        }
                    else
                        if (cardsOnTable[2] == null)
                            tableIndex = 2;
                        else
                        {
                            tableIndex = 3;
                            secondPlayerBlocked = true;
                        }
                    ++cardsChosen;
                    cardsOnTable[tableIndex] = item;
                    item.SetImagePosition(244 + tableIndex * 128, 268);
                    item.ImageBox.BringToFront();
                    item.Status = CardStatus.Table;
                    turn = ChangeTurn(turn);
                    if (cardsChosen > 1)
                        CompareCards();
                }
            }
        }

        void GetNewCard(CardClass item)
        {
            if ((turn == Players.First && item.PlayerOwner == CardOwner.First) ||
                (turn == Players.Second && item.PlayerOwner == CardOwner.Second))
            {
                if ((item.PlayerOwner == CardOwner.First && !firstPlayerBlocked) ||
                    (item.PlayerOwner == CardOwner.Second && !secondPlayerBlocked))
                {
                    var index = item.PlayerOwner == CardOwner.First ? firstPlayerGameCards.IndexOf(item) : secondPlayerGameCards.IndexOf(item);

                    if (item.PlayerOwner == CardOwner.Second)
                    {
                        cards[indexArray[counter]].SetImagePosition(182 + index * 128, 32);
                        cards[indexArray[counter]].PlayerOwner = CardOwner.Second;
                        secondPlayerGameCards[index] = cards[indexArray[counter]];
                    }
                    else
                    {
                        cards[indexArray[counter]].SetImagePosition(182 + index * 128, 504);
                        cards[indexArray[counter]].PlayerOwner = CardOwner.First;
                        firstPlayerGameCards[index] = cards[indexArray[counter]];
                    }

                    cards[indexArray[counter]].SetVisible(true);
                    cards[indexArray[counter]].ImageBox.BringToFront();
                    cards[indexArray[counter]].Status = CardStatus.Player;
                    ++counter;
                }
            }
        }

        void CompareCards()
        {
            switch (cardsChosen)
            {
                case 2:
                    {
                        if (cardsOnTable[1].Nation == cardsOnTable[2].Nation ||
                            cardsOnTable[1].Value == cardsOnTable[2].Value)                   
                        {
                            RandomCards();
                            StartNewRound();
                        }
                        else
                            if (cardsOnTable[1].Value > cardsOnTable[2].Value)
                                if (turn == Players.Second && CheckOtherPlayerCards(cardsOnTable[2].Nation, Players.Second))
                                    skip2.Enabled = true;
                                else
                                {
                                    MessageBox.Show("First win", "Football");
                                    roundWinner = Players.First;
                                    StartNewRound();
                                }
                            else
                                if (turn == Players.First && CheckOtherPlayerCards(cardsOnTable[1].Nation, Players.First))
                                    skip1.Enabled = true;
                                else
                                {
                                    MessageBox.Show("Second win", "Football");
                                    roundWinner = Players.Second;
                                    StartNewRound();
                                }
                        break;
                    }
                case 3:
                    {
                        if (CompareCardsOnTable() == Players.First)
                            if (turn == Players.Second && skip2.Enabled == false &&
                                CheckOtherPlayerCards(cardsOnTable[2].Nation, turn))
                                skip2.Enabled = true;
                            else
                            {
                                MessageBox.Show("First win", "Football");
                                roundWinner = Players.First;
                                StartNewRound();
                            }
                        else
                            if (CompareCardsOnTable() == Players.Second)
                                if (turn == Players.First && skip1.Enabled == false &&
                                    CheckOtherPlayerCards(cardsOnTable[1].Nation, turn))
                                    skip1.Enabled = true;
                                else
                                {
                                    MessageBox.Show("Second win", "Football");
                                    roundWinner = Players.Second;
                                    StartNewRound();
                                }
                        break;
                    }
                case 4:
                    {
                        if (CompareCardsOnTable() == Players.First)
                        {
                            MessageBox.Show("First win", "Football");
                            roundWinner = Players.First;
                            StartNewRound();
                        }
                        else
                            if (CompareCardsOnTable() == Players.Second)
                            {
                                MessageBox.Show("Second win", "Football");
                                roundWinner = Players.Second;
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

            int firstPlayerSumma = 0;
            int secondPlayerSumma = 0;

            foreach (CardClass card in firstPlayerGameCards)
                card.ImageBox.Visible = false;
            foreach (CardClass card in secondPlayerGameCards)
                card.ImageBox.Visible = false;
            for (int i = 0; i < 4; i++)
                if (cardsOnTable[i] != null)
                    cardsOnTable[i].ImageBox.Visible = false;
            firstPlayerBlocked = true;
            secondPlayerBlocked = true;
            skip1.Visible = false;
            skip2.Visible = false;

            for (int i = 0; i < 6; i++)
            {
                cards[indexArray[counter]].SetVisible(true);
                cards[indexArray[counter]].ImageBox.BringToFront();
                cards[indexArray[counter]].Status = CardStatus.Table;
                if (i % 2 == 0)
                {
                    cards[indexArray[counter]].SetImagePosition(240 + i * 100, 420);
                    randomGameCards.Add(cards[indexArray[counter]]);
                    firstPlayerSumma += cards[indexArray[counter]].Value;
                }
                else
                {
                    cards[indexArray[counter]].SetImagePosition(240 + (i - 1) * 100, 120);
                    randomGameCards.Add(cards[indexArray[counter]]);
                    secondPlayerSumma += cards[indexArray[counter]].Value;
                }
                ++counter;
                if (ActiveForm != null) 
                    ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
            }

            MessageBox.Show("First " + firstPlayerSumma.ToString(CultureInfo.InvariantCulture) +
                "\nSecond " + secondPlayerSumma.ToString(CultureInfo.InvariantCulture), "Football");

            if (firstPlayerSumma > secondPlayerSumma)
            {
                MessageBox.Show("First win", "Football");
                foreach (CardClass card in randomGameCards)
                {
                    card.SetVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    card.SetImagePosition(855, 424);
                    firstPlayerOutCards.Add(card);
                }
                randomGameCards.Clear();
                roundWinner = Players.First;
            }
            else if (secondPlayerSumma > firstPlayerSumma)
            {
                MessageBox.Show("Second win", "Football");
                foreach (CardClass card in randomGameCards)
                {
                    card.SetVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    card.SetImagePosition(855, 112);
                    secondPlayerOutCards.Add(card);
                }
                randomGameCards.Clear();
                roundWinner = Players.Second;
            }
            else
            {
                MessageBox.Show("Round draw", "Football");
                foreach(CardClass card in randomGameCards)
                {
                    card.SetVisible(false);
                    card.Status = CardStatus.OutOfGame;
                    card.SetImagePosition(-200, -200);
                }
                randomGameCards.Clear();
                roundWinner = Players.None;
            }

            foreach (CardClass card in firstPlayerGameCards)
                card.ImageBox.Visible = true;
            foreach (CardClass card in secondPlayerGameCards)
                card.ImageBox.Visible = true;
            for (int i = 0; i < 4; i++)
                if (cardsOnTable[i] != null)
                    cardsOnTable[i].ImageBox.Visible = true;
        }

        bool CheckOtherPlayerCards(string nation, Players turn)
        {
            bool result = false;
            if (turn == Players.First)
                foreach (var card in firstPlayerGameCards)
                {
                    if (nation == card.Nation)
                        result = true;
                }
            else
                foreach (var card in secondPlayerGameCards)
                {
                    if (nation == card.Nation)
                        result = true;
                }

            return result;
        }

        Players CompareCardsOnTable()
        {
            Players result;
            int player1 = cardsOnTable[1].Value;
            int player2 = cardsOnTable[2].Value;

            if (cardsOnTable[0] != null)
                player1 += cardsOnTable[0].Value;
            if (cardsOnTable[3] != null)
                player2 += cardsOnTable[3].Value;

            if (player1 > player2)
                result = Players.First;
            else
                if (player2 > player1)
                    result = Players.Second;
                else
                    result = Players.None;

            return result;
        }

        void StartNewRound()
        {
            for (int i = 0; i < 4; i++)
                if (cardsOnTable[i] != null)
                {
                    cardsOnTable[i].SetVisible(false);
                    cardsOnTable[i].Status = CardStatus.OutOfGame;
                    if (roundWinner == Players.First)
                    {
                        firstPlayerOutCards.Add(cardsOnTable[i]);
                        cardsOnTable[i].SetImagePosition(855, 424);
                    }
                    else if (roundWinner == Players.Second)
                    {
                        secondPlayerOutCards.Add(cardsOnTable[i]);
                        cardsOnTable[i].SetImagePosition(855, 112);
                    }
                    else
                    {
                        cardsOnTable[i].SetImagePosition(-200, -200);
                    }
                    cardsOnTable[i] = null;
                }
            cardsChosen = 0;
            firstPlayerBlocked = false;
            secondPlayerBlocked = false;
            skip1.Enabled = false;
            skip2.Enabled = false;
            if (roundWinner != turn)
                turn = ChangeTurn(turn);
            ++round;
            RoundLabel.Text = string.Format("Round {0}", (round + 1));
            TimeLabel.Text = string.Format("Time\n{0}:00", (round * 5));
            NumberOutCards1Label.Text = firstPlayerOutCards.Count.ToString(CultureInfo.InvariantCulture);
            NumberOutCards2Label.Text = secondPlayerOutCards.Count.ToString(CultureInfo.InvariantCulture);
            if (round == 9)
            {
                MessageBox.Show("Final round", "Football"); //end routine
                FinalGame();
            }
        }

        void FinalGame()
        {
            skip1.Visible = false;
            skip2.Visible = false;
            foreach(CardClass card in firstPlayerGameCards)
                card.SetImagePosition(-200, -200);
            foreach(CardClass card in secondPlayerGameCards)
                card.SetImagePosition(-200, -200);
            firstPlayerOutCards.Sort(Compare);
            secondPlayerOutCards.Sort(Compare);

            int firstIndex = 11;
            if (firstPlayerOutCards.Count < 11)
                firstIndex = firstPlayerOutCards.Count;
            int secondIndex = 11;
            if (secondPlayerOutCards.Count < 11)
                secondIndex = secondPlayerOutCards.Count;

            int firstSumma = 0;
            int secondSumma = 0;

            for (int i = 0; i < firstIndex; i++)
            {
                firstPlayerOutCards[i].SetVisible(true);
                if (i < 6)
                    firstPlayerOutCards[i].SetImagePosition(120 + i * 128, 360);
                    
                else
                    firstPlayerOutCards[i].SetImagePosition(182 + (i - 6) * 128, 504);
                firstSumma += firstPlayerOutCards[i].Value;
                firstPlayerOutCards[i].ImageBox.BringToFront();
                if (ActiveForm != null) 
                    ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
                
            }
            if (ActiveForm != null)
                ActiveForm.Refresh();

            for (int i = 0; i < secondIndex; i++)
            {
                secondPlayerOutCards[i].SetVisible(true);
                if (i < 5)
                    secondPlayerOutCards[i].SetImagePosition(182 + i * 128, 32);
                else
                    secondPlayerOutCards[i].SetImagePosition(120 + (i - 5) * 128, 180);
                secondSumma += secondPlayerOutCards[i].Value;
                secondPlayerOutCards[i].ImageBox.BringToFront();
                if (ActiveForm != null) 
                    ActiveForm.Refresh();
                System.Threading.Thread.Sleep(1000);
            }
            if (ActiveForm != null) 
                ActiveForm.Refresh();

            MessageBox.Show("First " + firstSumma + "\nSecond " + secondSumma);

            if (firstSumma > secondSumma)
                MessageBox.Show("First win game", "Football");
            else if (secondSumma > firstSumma)
                MessageBox.Show("Second win game", "Football");
            if (MessageBox.Show("Start a new game?", "Football", MessageBoxButtons.YesNo) == DialogResult.Yes)
                newToolStripMenuItem_Click(null, null);
            else
            {
                if (MessageBox.Show("Exit game?", "Football", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Application.Exit(null);
            }
            


        }

        int Compare(CardClass x, CardClass y)
        {
            return x.Value.CompareTo(y.Value);
        }

        short[] MakeRandom()
        { 
            short[] s = new short[150];
            const int number = 5;
            int[] u = new int[number];
            for (short i = 0; i < 150; i++)
                s[i] = i;
            for (int i = 0; i < number; i++)
                u[i] = rand.Next();
            int j = 0;
            for (int i = 0; i < 150; i++)
            {
                j = (j + s[i] + u[i % number]) % 150;
                short c = s[i];
                s[i] = s[j];
                s[j] = c;
            }
                 
            int k = 0;
            j = 0;
            for (int i = 0; i < 150; i++)
            {
                k = (k + 1) % 150;
                j = (j + s[k]) % 150;
                short c = s[k];
                s[k] = s[j];
                s[j] = c;
            }

            return s;
        }

    }
}
