using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SkeeBall.Models
{
    public class Score : DependencyObject, IComparable
    {

        public Score()
        {

        }
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(Score), new UIPropertyMetadata(0));



        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Score), new UIPropertyMetadata(""));

        public override string ToString()
        {
            return Name + "   " + Value.ToString();
        }
    }
}
