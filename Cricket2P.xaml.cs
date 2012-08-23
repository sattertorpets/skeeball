using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
// for Bindings Class
using SkeeBall.Models;
using System.Collections.Specialized;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Game2P.xaml
    /// </summary>
    public partial class Cricket2P : Window
    {
        int BallTimestamp = 0;
        public BaseGame Model { get; set; }

        public Cricket2P()
        {
            InitializeComponent();
            Model = new BaseGame("Cricket");
            Model.HighScores = Util.LoadScores("GameNames", "GameScores");
            Model.HighestScore = Model.HighScores.First<Score>().Value;
            this.DataContext = Model;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
        }

    }
}
