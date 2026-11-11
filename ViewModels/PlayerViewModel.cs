using Labb_3_Mio.Command;
using Labb_3_Mio.Models;
using Labb_3_Mio.Views;
using System.Windows;
using System.Windows.Threading;

namespace Labb_3_Mio.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private DispatcherTimer _timer;
    private DispatcherTimer _IconTimer;

    public RelayCommand ChangePackNameCommand { get; }
    public RelayCommand AnswerCommand { get; }

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    private int _playerScore = 0;
    public int PlayerScore
    {
        get => _playerScore;
        private set
        {
            _playerScore = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ScoreDisplay));
        }
    }

    public string ScoreDisplay => $"Score: {PlayerScore}";

    private int _currentQuestionIndex = 0;

    private Question? _currentQuestion;

    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        private set
        {
            _currentQuestion = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(CurrentQuestionText));
            RaisePropertyChanged(nameof(QuestionProgress));
        }
    }
    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));

    public string CurrentQuestionText => CurrentQuestion?.Query ?? "No question loaded";

    public string QuestionProgress
    {
        get
        {
            if (ActivePack?.Questions == null || ActivePack.Questions.Count == 0)
                return "No questions available";

            return $"Question {_currentQuestionIndex + 1} of {ActivePack.Questions.Count}";
        }
    }

    private Visibility _answer1IconVisibility = Visibility.Collapsed;
    public Visibility Answer1IconVisibility
    {
        get => _answer1IconVisibility;
        set
        {
            _answer1IconVisibility = value;
            RaisePropertyChanged();
        }
    }
    private Visibility _answer2IconVisibility = Visibility.Collapsed;
    public Visibility Answer2IconVisibility
    {
        get => _answer2IconVisibility;
        set
        {
            _answer2IconVisibility = value;
            RaisePropertyChanged();
        }
    }
    private Visibility _answer3IconVisibility = Visibility.Collapsed;
    public Visibility Answer3IconVisibility
    {
        get => _answer3IconVisibility;
        set
        {
            _answer3IconVisibility = value;
            RaisePropertyChanged();
        }
    }
    private Visibility _answer4IconVisibility = Visibility.Collapsed;
    public Visibility Answer4IconVisibility
    {
        get => _answer4IconVisibility;
        set
        {
            _answer4IconVisibility = value;
            RaisePropertyChanged();
        }
    }
    private string _rightOrWrongIcon = "";
    public string RightOrWrongIcon
    {
        get => _rightOrWrongIcon;
        set
        {
            _rightOrWrongIcon = value;
            RaisePropertyChanged();
        }
    }
    private string _answer1 = "";
    public string Answer1
    {
        get => _answer1;
        private set
        {
            _answer1 = value;
            RaisePropertyChanged();
        }
    }
    private string _answer2 = "";
    public string Answer2
    {
        get => _answer2;
        private set
        {
            _answer2 = value;
            RaisePropertyChanged();
        }
    }
    private string _answer3 = "";
    public string Answer3
    {
        get => _answer3;
        private set
        {
            _answer3 = value;
            RaisePropertyChanged();
        }
    }
    private string _answer4 = "";
    public string Answer4
    {
        get => _answer4;
        private set
        {
            _answer4 = value;
            RaisePropertyChanged();
        }
    }
    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        ChangePackNameCommand = new RelayCommand(_ => ChangePackName(), _ => DemoText != string.Empty);
        AnswerCommand = new RelayCommand(OnAnswerSelected);

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1.0);
        _timer.Tick += Timer_Tick;

        _IconTimer = new DispatcherTimer();
        _IconTimer.Interval = TimeSpan.FromSeconds(1.5);
        _IconTimer.Tick += IconTimer_Tick;
    }
    public void StartGame()
    {
        _currentQuestionIndex = 0;
        PlayerScore = 0;
        HideAnswerIcons();
        LoadQuestion(0);
        _timer.Start();
    }

    private void HideAnswerIcons()
    {
        Answer1IconVisibility = Visibility.Collapsed;
        Answer2IconVisibility = Visibility.Collapsed;
        Answer3IconVisibility = Visibility.Collapsed;
        Answer4IconVisibility = Visibility.Collapsed;
    }

    private void LoadQuestion(int index)
    {
        if (ActivePack?.Questions != null &&
            index >= 0 &&
            index < ActivePack.Questions.Count)
        {
            _currentQuestionIndex = index;
            CurrentQuestion = ActivePack.Questions[index];
            ShuffleAnswers(CurrentQuestion);
            ResetTimer();

            HideAnswerIcons();
        }
    }
    private void ResetTimer()
    {
        TimeRemaining = ActivePack?.TimeLimitInSeconds ?? 30;
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
        if (TimeRemaining > 0)
        {
            TimeRemaining--;
        }
        else
        {
            MoveToNextQuestion();
        }
    }

    private void IconTimer_Tick(object? sender, EventArgs e)
    {

        _IconTimer.Stop();


        HideAnswerIcons();

        MoveToNextQuestion();
    }

    private void OnAnswerSelected(object? selectedAnswer)
    {
        if (selectedAnswer is string answer && CurrentQuestion != null)
        {
            _timer.Stop();

            bool isCorrect = answer == CurrentQuestion.CorrectAnswer;

            if (isCorrect)
            {
                RightOrWrongIcon = "/Images/correctanswer.png";
            }
            else
            {
                RightOrWrongIcon = "/Images/wronganswer.png";
            }

            HideAnswerIcons();

            if (answer == Answer1)
                Answer1IconVisibility = Visibility.Visible;
            else if (answer == Answer2)
                Answer2IconVisibility = Visibility.Visible;
            else if (answer == Answer3)
                Answer3IconVisibility = Visibility.Visible;
            else if (answer == Answer4)
                Answer4IconVisibility = Visibility.Visible;

            if (isCorrect) PlayerScore++;


            _IconTimer.Start();
        }
    }

    private void MoveToNextQuestion()
    {
        int nextIndex = _currentQuestionIndex + 1;

        if (ActivePack?.Questions != null && nextIndex < ActivePack.Questions.Count)
        {
            LoadQuestion(nextIndex);
            _timer.Start();
        }
        else
        {
            _timer.Stop();
            CurrentQuestion = null;
            Answer1 = "Quiz";
            Answer2 = "Finished!";
            Answer3 = $"Score: {PlayerScore}/{ActivePack?.Questions.Count ?? 0}";
            Answer4 = "!!!";

            HideAnswerIcons();
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
