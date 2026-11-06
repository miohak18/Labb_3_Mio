using Labb_3_Mio.Command;
using System.Windows.Threading;

namespace Labb_3_Mio.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public RelayCommand ChangePackNameCommand { get; }

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));
    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        ChangePackNameCommand = new RelayCommand(_ => ChangePackName(), _ => DemoText != string.Empty);

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(2.0);
        timer.Tick += Timer_Tick;
        timer.Start();
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        ActivePack.Name += "+";
    }
    private string _demoText = string.Empty;
    public string DemoText
    {
        get => _demoText;
        set
        {
            if (SetProperty(ref _demoText, value))
            {
                ChangePackNameCommand.RaiseCanExecuteChanged();
            }
        }
    }
    private bool CanChangePackName(object? arg)
    {
        return DemoText != string.Empty;
    }

    private void ChangePackName()
    {
        ActivePack?.Name = DemoText;
    }
}
