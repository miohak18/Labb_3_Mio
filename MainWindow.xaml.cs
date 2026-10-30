using Labb_3_Mio.Models;
using Labb_3_Mio.ViewModels;
using System.Windows;

namespace Labb_3_Mio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

    }
}