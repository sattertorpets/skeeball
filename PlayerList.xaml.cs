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
using System.Collections.ObjectModel;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for PlayerList.xaml
    /// </summary>
    
    public partial class PlayerList : Window
    {
        public ObservableCollection<string> PlayerNames { get; set; }
        public string SelectedPlayer { get; set; }
        public PlayerList()
        {
            InitializeComponent();
            PlayerNames = new ObservableCollection<string>();
            PlayerNames = Util.LoadNames();
            nameList.DataContext = this;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedPlayer = ((Button)sender).Content.ToString();
            if (SelectedPlayer.ToLower() == "create new")
            {
                NewPlayer newPlayerWindow = new NewPlayer();
                newPlayerWindow.Owner = this;
                newPlayerWindow.ShowDialog();
                if (newPlayerWindow.EnteredName != "")
                {
                    SelectedPlayer = newPlayerWindow.EnteredName;
                    Util.SaveNames(PlayerNames, SelectedPlayer);
                    this.Close();
                }
            }
            else
            {
                Util.SaveNames(PlayerNames, SelectedPlayer);
                this.Close();
            }
            
        }


        
    }
}
