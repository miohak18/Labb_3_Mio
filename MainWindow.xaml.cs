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

            var pack = new QuestionPack("Mina Frågor");

            DataContext = new QuestionPackViewModel(pack);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    (DataContext as QuestionPackViewModel).Difficulty = Difficulty.Easy;
        //    (DataContext as QuestionPackViewModel).TimeLimitInSeconds = 999;
        //}

    }
}