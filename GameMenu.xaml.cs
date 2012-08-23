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
using System.Windows.Shapes;
using SkeeBall.Models;
using System.Collections.ObjectModel;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        
        public GameMenu()
        {
            InitializeComponent();
            //btnClassic.Focus();
        }

        private void btnClassic_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Classic", false);
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnClassic_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Classic Skeeball, 9 balls, no 100s.";
        }

        private void btnCricket_Click(object sender, RoutedEventArgs e)
        {
            CricketWindow game = new CricketWindow("Cricket");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnCricket_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Solo or Multiplayer Cricket, like in darts.";
        }

        private void btnModern_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Modern", true);
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnModern_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Modern Skeeball, 9 balls, with 100s.";
        }

        private void btn310_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Get to exactly 310 in the fewest throws possible.";
        }

        private void btnMultiBall_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Build up a multiplier for huge scores.";
        }

        private void btn310_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("ThreeTen", true);
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnMultiBall_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Multi", true);
            game.Owner = this;
            game.ShowDialog();
        }

        private void btn510_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("FiveTen", true);
            game.Owner = this;
            game.ShowDialog();
        }

        private void btn510_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Get to exactly 510 in the fewest throws possible.";
        }

    }
}
