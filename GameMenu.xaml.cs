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

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        public Classic MainGame { get; set; }

        public GameMenu()
        {
            InitializeComponent();
            MainGame = new Classic();
        }

        private void label1_Click(object sender, RoutedEventArgs e)
        {
            Classic2P game = new Classic2P();
            game.Owner = this;
            game.Show();
        }
    }
}
