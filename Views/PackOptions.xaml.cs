using System.Windows;
using Labb_3_Mio.Models;

namespace Labb_3_Mio.Views
{
    /// <summary>
    /// Interaction logic for PackOptions.xaml
    /// </summary>
    public partial class PackOptions : Window
    {
        public PackOptions()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private string _difficulty;
        public string Difficulty
        {
            get => _difficulty;
            set => _difficulty = value;
        }

        private int _timeLimitInSeconds;
        public int TimeLimitInSeconds
        {
            get => _timeLimitInSeconds;
            set => _timeLimitInSeconds = value;
        }

        private string _packName;
        public string PackName
        {
            get => _packName;
            set => _packName = value;
        }

    }
}
