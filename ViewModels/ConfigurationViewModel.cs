using Labb_3_Mio.Command;
using Labb_3_Mio.Views;
using System.Windows;

namespace Labb_3_Mio.ViewModels;

internal class ConfigurationViewModel :ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public RelayCommand AddQuestionCommand { get; private set; }
    public RelayCommand PackOptionsCommand { get; private set; }
    public RelayCommand RemoveQuestionCommand { get; private set; }
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        AddQuestionCommand = new RelayCommand(AddQuestion);

        PackOptionsCommand = new RelayCommand(PackOptions);

        RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
    }
    private void AddQuestion(object? obj)
    {
        throw new NotImplementedException();
    }
    private void PackOptions(object? obj)
    {
        var dialog = new PackOptions
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = Application.Current.MainWindow
        };
        dialog.ShowDialog();
    }
    private void RemoveQuestion(object? obj)
    {
        throw new NotImplementedException();
    }

}
