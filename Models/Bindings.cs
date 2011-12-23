using System.Windows; // for DependencyObject

namespace SkeeBall.Models
{
    // make sure to make this public so binding can find it
    public class Bindings : DependencyObject // could use INotifyPropertyChanged (System.ComponentModel) instead
    {
        // Use the 'propdp' snippet, set the ownerType to the class type (Class1 in this case) and typeMetadata to the default (starting) value.

        public int Score1
        {
            get { return (int)GetValue(Score1Property); }
            set { SetValue(Score1Property, value); }
        }
        public static readonly DependencyProperty Score1Property =
            DependencyProperty.Register("Score1", typeof(int), typeof(Bindings), new UIPropertyMetadata(0));

        public int Score2
        {
            get { return (int)GetValue(Score2Property); }
            set { SetValue(Score2Property, value); }
        }
        public static readonly DependencyProperty Score2Property =
            DependencyProperty.Register("Score2", typeof(int), typeof(Bindings), new UIPropertyMetadata(0));

        public int BallsPlayed1
        {
            get { return (int)GetValue(BallsPlayed1Property); }
            set { SetValue(BallsPlayed1Property, value); }
        }

        // Using a DependencyProperty as the backing store for BallsPlayed1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BallsPlayed1Property =
            DependencyProperty.Register("BallsPlayed1", typeof(int), typeof(Bindings), new UIPropertyMetadata(0));

        public int BallsPlayed2
        {
            get { return (int)GetValue(BallsPlayed2Property); }
            set { SetValue(BallsPlayed2Property, value); }
        }

        // Using a DependencyProperty as the backing store for BallsPlayed2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BallsPlayed2Property =
            DependencyProperty.Register("BallsPlayed2", typeof(int), typeof(Bindings), new UIPropertyMetadata(0));
    }
}
