using Labb_3_Mio.Models;
using Labb_3_Mio.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Labb_3_Mio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
        protected override async void OnClosing(CancelEventArgs e)
        {
            if (_isClosing)
            {
                base.OnClosing(e);
                return;
            }

            e.Cancel = true;
            _isClosing = true;

            if (DataContext is MainWindowViewModel viewModel)
                await viewModel.SaveAllPacksAsync();

            Close();
        }
    }
}