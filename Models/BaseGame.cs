using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Media;

namespace SkeeBall.Models
{
    public class BaseGame : DependencyObject
    {
        public ObservableCollection<Score> HighScores { get; set; }
        public int Last10 { get; set; }
        public System.Windows.Input.Key LastKey { get; set; }
        public bool IsOver { get; set; }
        public int BallTimestamp { get; set; }
        public bool Count100s { get; set; }
        public Player OtherPlayer { get; set; }

        private SoundPlayer wav50 = new SoundPlayer(@"Sounds\Sound50.wav");
        private SoundPlayer wav40 = new SoundPlayer(@"Sounds\Sound40.wav");
        private SoundPlayer wav30 = new SoundPlayer(@"Sounds\Sound30.wav");
        private SoundPlayer wav20 = new SoundPlayer(@"Sounds\Sound20.wav");
        private SoundPlayer wav10 = new SoundPlayer(@"Sounds\Sound10.wav");
        //Used only for tic tac toe, initializes to 0
        public int[,] TTT3Grid = new int[3,3];
        public int[,] TTT5Grid = new int[5, 5];


        public string GameName
        {
            get { return (string)GetValue(GameNameProperty); }
            set { SetValue(GameNameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for GameName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameNameProperty =
            DependencyProperty.Register("GameName", typeof(string), typeof(BaseGame), new UIPropertyMetadata(null));

        public int HighestScore
        {
            get { return (int)GetValue(HighestScoreProperty); }
            set { SetValue(HighestScoreProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HighestScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighestScoreProperty =
            DependencyProperty.Register("HighestScore", typeof(int), typeof(BaseGame), new UIPropertyMetadata(0));

        public Player Player1
        {
            get { return (Player)GetValue(Player1Property); }
            set { SetValue(Player1Property, value); }
        }
        // Using a DependencyProperty as the backing store for Player1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1Property =
            DependencyProperty.Register("Player1", typeof(Player), typeof(BaseGame), new UIPropertyMetadata(null));  

        public Player Player2
        {
            get { return (Player)GetValue(Player2Property); }
            set { SetValue(Player2Property, value); }
        }
        // Using a DependencyProperty as the backing store for Player2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2Property =
            DependencyProperty.Register("Player2", typeof(Player), typeof(BaseGame), new UIPropertyMetadata(null));

        public Player ActivePlayer
        {
            get { return (Player)GetValue(ActivePlayerProperty); }
            set { SetValue(ActivePlayerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ActivePlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivePlayerProperty =
            DependencyProperty.Register("ActivePlayer", typeof(Player), typeof(BaseGame), new UIPropertyMetadata(null));

        public bool TwoPlayer
        {
            get { return (bool)GetValue(TwoPlayerProperty); }
            set { SetValue(TwoPlayerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TwoPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TwoPlayerProperty =
            DependencyProperty.Register("TwoPlayer", typeof(bool), typeof(BaseGame), new UIPropertyMetadata(false));

        public bool HasMultiplier
        {
            get { return (bool)GetValue(HasMultiplierProperty); }
            set { SetValue(HasMultiplierProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HasMultiplier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasMultiplierProperty =
            DependencyProperty.Register("HasMultiplier", typeof(bool), typeof(BaseGame), new UIPropertyMetadata(false));

        public int ActvPlyrNum
        {
            get { return (int)GetValue(ActvPlyrNumProperty); }
            set { SetValue(ActvPlyrNumProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ActvPlyrNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActvPlyrNumProperty =
            DependencyProperty.Register("ActvPlyrNum", typeof(int), typeof(BaseGame), new UIPropertyMetadata(1));

        /// <summary>
        /// Do NOT call this outside a Try/Catch block, it does not handle its KeyNotFoundExceptions.  Returns true if the key was a valid scoring key.
        /// </summary>
        public bool ScoringKeyHandler(KeyEventArgs e)
        {
            if (BallTimestamp == 0)     //Set the comparison timestamps in the past for the first ball
            {
                BallTimestamp = e.Timestamp - 3000;
                Last10 = e.Timestamp - 3000;
            }
            //If its a repeat key, or is a switch bounce, ignore it and return
            if (e.IsRepeat || (e.Key == LastKey && BallTimestamp + 100 > e.Timestamp))
                return false;
            LastKey = e.Key;
            /* 
            Enum Hole10, Hole20, Hole30, Hole40, Hole50, HoleL100, HoleR100, Gutter, UpKey, DownKey, SelectKey, BackKey;
              A = 10, S = 20, Q = 30, W = 40, I = 50, K = L100, J = R100, L = Gutter, V = BackButton
              UpArrow = UpArrow, UpArrow = DownArrow, SpaceBar = Select
            */

            switch (Util.KeyMap[e.Key])
            {
                case Util.KeyName.Hole10:
                    Last10 = e.Timestamp;
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    ActivePlayer.ThisThrow = 10;
                    wav10.Play();
                    break;
                case Util.KeyName.Hole20:
                    Last10 = e.Timestamp;
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    ActivePlayer.ThisThrow = 20;
                    wav20.Play();
                    break;
                case Util.KeyName.Hole30:
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    ActivePlayer.ThisThrow = 30;
                    wav30.Play();
                    break;
                case Util.KeyName.Hole40:
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    ActivePlayer.ThisThrow = 40;
                    wav40.Play();
                    break;
                case Util.KeyName.Hole50:
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    ActivePlayer.ThisThrow = 50;
                    wav50.Play();
                    break;
                case Util.KeyName.HoleL100: // Left 100
                case Util.KeyName.HoleR100: // Right 100
                    if (IsDuplicateBall(e.Timestamp)) return false;
                    if (Count100s)
                        ActivePlayer.ThisThrow = 100;
                    else
                        ActivePlayer.ThisThrow = 10;
                    break;
                case Util.KeyName.Gutter:
                    if (e.Timestamp > (Last10 + 2600))   //If its been more than 2.6s since the 10 switch was tripped, this was a gutter
                        ActivePlayer.ThisThrow = 0;
                    else
                        return false;        //If it wasn't, then ignore it
                    break;
                default:
                    return false;
            }
            BallTimestamp = e.Timestamp;
            return true;
        }

        private bool IsDuplicateBall(int time)
        {
            if ((time - BallTimestamp) <= 450)
            {
                BallTimestamp = time;
                return true;
            }
            return false;
        }

        public BaseGame(string gameName)
        {
            Player1 = new Player();
            Player2 = new Player();
            ActivePlayer = Player1;
            OtherPlayer = Player2;
            Last10 = 0;
            IsOver = false;
            if (gameName.ToLower().Equals("multi"))
            {
                HasMultiplier = true;
                ActivePlayer.MultiTarget = 30;
            }
            if (gameName.ToLower().Equals("classic"))
                Count100s = false;
            else
                Count100s = true;
            HighScores = Util.LoadScores(gameName + "Names", gameName + "Scores");
            HighestScore = HighScores.First<Score>().Value;
            BallTimestamp = 0;
            ActivePlayer.LastThrow = -1;
            ActivePlayer.ThisThrow = -1;
            GameName = gameName;
        }
        public void IncrementMultiTarget()
        {
            ActivePlayer.MultiTarget += 10;
            if (ActivePlayer.MultiTarget == 60) ActivePlayer.MultiTarget = 100;
        }

        public void GameOver(int score, Window gameWindow)
        {
            IsOver = true;
            int numHighScores = HighScores.Count - 1;
            if (HighScores.ElementAt(numHighScores).Value < score)  //if higher than the last item in the high score list
            {
                HighScores.RemoveAt(numHighScores);  //remove last score
                Score newHighScore = new Score();
                PlayerList playerListWindow = new PlayerList();
                playerListWindow.Owner = gameWindow;
                playerListWindow.ShowDialog();
                newHighScore.Name = playerListWindow.SelectedPlayer;
                newHighScore.Value = score;
                HighScores.Add(newHighScore);     //add new score at the bottom of the list
                HighScores.ReverseBubbleSort();

                Util.WriteScores(HighScores, GameName + "Names", GameName + "Scores");
            }
        }

        public void GameOverGolf(int balls, Window gameWindow)
        {
            IsOver = true;
            int numHighScores = HighScores.Count - 1;
            if (HighScores.ElementAt(numHighScores).Value > balls)  //if smaller than the last item in the high score list
            {
                HighScores.RemoveAt(numHighScores);  //remove last score
                Score newHighScore = new Score();
                PlayerList playerListWindow = new PlayerList();
                playerListWindow.Owner = gameWindow;
                playerListWindow.ShowDialog();
                newHighScore.Name = playerListWindow.SelectedPlayer;
                newHighScore.Value = balls;
                HighScores.Add(newHighScore);     //add new score at the bottom of the list
                HighScores.ReverseBubbleGolfSort();

                Util.WriteScores(HighScores, GameName + "Names", GameName + "Scores");
            }
        }

        public void SwitchPlayer()
        {
            if (ActvPlyrNum == 1)
            {
                ActivePlayer = Player2;
                OtherPlayer = Player1;
                ActvPlyrNum = 2;
            }
            else
            {
                ActivePlayer = Player1;
                OtherPlayer = Player2;
                ActvPlyrNum = 1;
            }
        }
        //public BaseGame()
        //{
        //    Player1 = new Player();
        //    Player2 = new Player();
        //    ActivePlayer = Player1;
        //    Last10 = 0;
        //}
    }
}
