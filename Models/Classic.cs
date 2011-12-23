using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace SkeeBall.Models
{
    public class Classic : DependencyObject
    {
        public ObservableCollection<Score> HighScores { get; set; }

        public int HighestScore
        {
            get { return (int)GetValue(HighestScoreProperty); }
            set { SetValue(HighestScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighestScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighestScoreProperty =
            DependencyProperty.Register("HighestScore", typeof(int), typeof(Classic), new UIPropertyMetadata(0));



        public Machine Machine1
        {
            get { return (Machine)GetValue(Machine1Property); }
            set { SetValue(Machine1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Machine1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Machine1Property =
            DependencyProperty.Register("Machine1", typeof(Machine), typeof(Classic), new UIPropertyMetadata(new Machine()));



        public Machine Machine2
        {
            get { return (Machine)GetValue(Machine2Property); }
            set { SetValue(Machine2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Machine2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Machine2Property =
            DependencyProperty.Register("Machine2", typeof(Machine), typeof(Classic), new UIPropertyMetadata(new Machine()));

        

    }
}
