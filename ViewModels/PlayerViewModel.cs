using Labb_3_Mio.Command;
using Labb_3_Mio.Models;
using Labb_3_Mio.Views;
using System.Windows.Threading;

namespace Labb_3_Mio.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public RelayCommand ChangePackNameCommand { get; }

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));
    public string Answer1 { get; private set; }
    public string Answer2 { get; private set; }
    public string Answer3 { get; private set; }
    public string Answer4 { get; private set; }
    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        ChangePackNameCommand = new RelayCommand(_ => ChangePackName(), _ => DemoText != string.Empty);

        TimeRemaining = 30;
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(2.0);
        timer.Tick += Timer_Tick;
        timer.Start();
    }
    private int _timeRemaining;
    public int TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            _timeRemaining = value;
            RaisePropertyChanged();
        }
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        {
            if (TimeRemaining > 0)
                TimeRemaining--;
        }

    }


    private void ShuffleAnswers(Question q)
    {
        var answersList = new List<string>
    {
        q.CorrectAnswer,
        q.IncorrectAnswers[0],
        q.IncorrectAnswers[1],
        q.IncorrectAnswers[2]
    };

        var shuffleAnswers = new Random();
        answersList = answersList.OrderBy(_ => shuffleAnswers.Next()).ToList();

        Answer1 = answersList[0];
        Answer2 = answersList[1];
        Answer3 = answersList[2];
        Answer4 = answersList[3];

        RaisePropertyChanged(nameof(Answer1));
        RaisePropertyChanged(nameof(Answer2));
        RaisePropertyChanged(nameof(Answer3));
        RaisePropertyChanged(nameof(Answer4));
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
