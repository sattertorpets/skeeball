using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SkeeBall.Models
{
    public class Player : DependencyObject
    {
        public int ThisThrow { get; set; }
        public List<int> BallHistory { get; set; }
        public int TargetsComplete
        {
            get { return Num100s + Num50s + Num40s + Num30s + Num20s + Num10s; }
        }
        
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Score.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int BallsPlayed      //Keeps track of total balls played by a player within a game
        {
            get { return (int)GetValue(BallsPlayedProperty); }
            set { SetValue(BallsPlayedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BallsPlayed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BallsPlayedProperty =
            DependencyProperty.Register("BallsPlayed", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int BallsLeft    //Keeps track of how many balls remain in a turn
        {
            get { return (int)GetValue(BallsLeftProperty); }
            set { SetValue(BallsLeftProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BallsLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BallsLeftProperty =
            DependencyProperty.Register("BallsLeft", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int LastThrow
        {
            get { return (int)GetValue(LastThrowProperty); }
            set { SetValue(LastThrowProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LastThrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastThrowProperty =
            DependencyProperty.Register("LastThrow", typeof(int), typeof(Player), new UIPropertyMetadata(-1));

        public int Multiplier
        {
            get { return (int)GetValue(MultiplierProperty); }
            set { SetValue(MultiplierProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Multiplier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultiplierProperty =
            DependencyProperty.Register("Multiplier", typeof(int), typeof(Player), new UIPropertyMetadata(1));
        
        public int MultiTarget
        {
            get { return (int)GetValue(MultiTargetProperty); }
            set { SetValue(MultiTargetProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MultiTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultiTargetProperty =
            DependencyProperty.Register("MultiTarget", typeof(int), typeof(Player), new UIPropertyMetadata(-1));

        public int Num100s
        {
            get { return (int)GetValue(num100sProperty); }
            set { SetValue(num100sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num100s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num100sProperty =
            DependencyProperty.Register("Num100s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Num50s
        {
            get { return (int)GetValue(num50sProperty); }
            set { SetValue(num50sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num50s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num50sProperty =
            DependencyProperty.Register("Num50s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Num40s
        {
            get { return (int)GetValue(num40sProperty); }
            set { SetValue(num40sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num40s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num40sProperty =
            DependencyProperty.Register("Num40s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Num30s
        {
            get { return (int)GetValue(num30sProperty); }
            set { SetValue(num30sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num30s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num30sProperty =
            DependencyProperty.Register("Num30s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Num20s
        {
            get { return (int)GetValue(num20sProperty); }
            set { SetValue(num20sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num20s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num20sProperty =
            DependencyProperty.Register("Num20s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Num10s
        {
            get { return (int)GetValue(num10sProperty); }
            set { SetValue(num10sProperty, value); }
        }
        // Using a DependencyProperty as the backing store for num10s.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty num10sProperty =
            DependencyProperty.Register("Num10s", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public int Throw1
        {
            get { return (int)GetValue(Throw1Property); }
            set { SetValue(Throw1Property, value); }
        }
        // Using a DependencyProperty as the backing store for Throw1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Throw1Property =
            DependencyProperty.Register("Throw1", typeof(int), typeof(Player), new UIPropertyMetadata(-1));

        public int Throw2
        {
            get { return (int)GetValue(Throw2Property); }
            set { SetValue(Throw2Property, value); }
        }
        // Using a DependencyProperty as the backing store for Throw2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Throw2Property =
            DependencyProperty.Register("Throw2", typeof(int), typeof(Player), new UIPropertyMetadata(-1));

    }
}
