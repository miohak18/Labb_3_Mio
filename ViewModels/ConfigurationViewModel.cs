using Labb_3_Mio.Command;

namespace Labb_3_Mio.ViewModels;

internal class ConfigurationViewModel :ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

    }

}
