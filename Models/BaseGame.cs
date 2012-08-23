using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace SkeeBall.Models
{
    public class BaseGame : DependencyObject
    {
        public ObservableCollection<Score> HighScores { get; set; }
        public int Last10 { get; set; }
        public System.Windows.Input.Key LastKey { get; set; }
        public bool GameIsOver { get; set; }
        
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
            DependencyProperty.Register("Player1", typeof(Player), typeof(BaseGame), new UIPropertyMetadata(null));  //using null doesn't work right

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

        public Player OtherPlayer
        {
            get { return (Player)GetValue(OtherPlayerProperty); }
            set { SetValue(OtherPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OtherPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OtherPlayerProperty =
            DependencyProperty.Register("OtherPlayer", typeof(Player), typeof(BaseGame), new UIPropertyMetadata(null));

        public bool TwoPlayer
        {
            get { return (bool)GetValue(TwoPlayerProperty); }
            set { SetValue(TwoPlayerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TwoPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TwoPlayerProperty =
            DependencyProperty.Register("TwoPlayer", typeof(bool), typeof(BaseGame), new UIPropertyMetadata(false));

        
        public BaseGame(string gameName)
        {
            Player1 = new Player();
            Player2 = new Player();
            ActivePlayer = Player1;
            OtherPlayer = Player2;
            Last10 = 0;
            GameIsOver = false;
            HighScores = Util.LoadScores(gameName+"Names", gameName+"Scores");
            HighestScore = HighScores.First<Score>().Value;
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
