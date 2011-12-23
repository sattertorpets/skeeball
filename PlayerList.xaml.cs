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
        public PlayerList()
        {
            InitializeComponent();
            PlayerNames = new ObservableCollection<string>();
            PlayerNames.Add("Ben");
            PlayerNames.Add("Nick");
            PlayerNames.Add("Kat");
            nameList.DataContext = this;
        }
    }
}
