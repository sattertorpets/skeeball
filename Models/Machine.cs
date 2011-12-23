using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SkeeBall.Models
{
    public class Machine : DependencyObject
    {


        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Score.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(Machine), new UIPropertyMetadata(0));



        public int BallsPlayed
        {
            get { return (int)GetValue(BallsPlayedProperty); }
            set { SetValue(BallsPlayedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BallsPlayed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BallsPlayedProperty =
            DependencyProperty.Register("BallsPlayed", typeof(int), typeof(Machine), new UIPropertyMetadata(0));

        
    }
}
