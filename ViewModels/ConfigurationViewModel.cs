using Labb_3_Mio.Command;
using Labb_3_Mio.Models;
using Labb_3_Mio.Views;
using System.ComponentModel;
using System.Windows;

namespace Labb_3_Mio.ViewModels;

internal class ConfigurationViewModel :ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public RelayCommand AddQuestionCommand { get; private set; }
    public RelayCommand PackOptionsCommand { get; private set; }
    public RelayCommand RemoveQuestionCommand { get; private set; }
    public RelayCommand SaveQuestionCommand { get; private set; }

    private Question? _selectedQuestion;
    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
            LoadQuestionToEditor();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            SaveQuestionCommand.RaiseCanExecuteChanged();
        }
    }

    private string _editQuery = string.Empty;
    public string EditQuery
    {
        get => _editQuery;
        set
        {
            _editQuery = value;
            RaisePropertyChanged();
        }
    }

    private string _editCorrectAnswer = string.Empty;
    public string EditCorrectAnswer
    {
        get => _editCorrectAnswer;
        set
        {
            _editCorrectAnswer = value;
            RaisePropertyChanged();
        }
    }

    private string _editIncorrectAnswer1 = string.Empty;
    public string EditIncorrectAnswer1
    {
        get => _editIncorrectAnswer1;
        set
        {
            _editIncorrectAnswer1 = value;
            RaisePropertyChanged();
        }
    }

    private string _editIncorrectAnswer2 = string.Empty;
    public string EditIncorrectAnswer2
    {
        get => _editIncorrectAnswer2;
        set
        {
            _editIncorrectAnswer2 = value;
            RaisePropertyChanged();
        }
    }

    private string _editIncorrectAnswer3 = string.Empty;
    public string EditIncorrectAnswer3
    {
        get => _editIncorrectAnswer3;
        set
        {
            _editIncorrectAnswer3 = value;
            RaisePropertyChanged();
        }
    }
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        AddQuestionCommand = new RelayCommand(AddQuestion);

        PackOptionsCommand = new RelayCommand(PackOptions);

        RemoveQuestionCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);

        SaveQuestionCommand = new RelayCommand(SaveQuestion, CanSaveQuestion);

        if (mainWindowViewModel != null)
        {
            mainWindowViewModel.PropertyChanged += OnMainWindowPropertyChanged;
        }
    }

    private void OnMainWindowPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.ActivePack))
        {
            RaisePropertyChanged(nameof(ActivePack));
            AddQuestionCommand.RaiseCanExecuteChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            SaveQuestionCommand.RaiseCanExecuteChanged();
        }
    }
    private void LoadQuestionToEditor()
    {
        if (SelectedQuestion != null)
        {
            EditQuery = SelectedQuestion.Query;
            EditCorrectAnswer = SelectedQuestion.CorrectAnswer;
            EditIncorrectAnswer1 = SelectedQuestion.IncorrectAnswers.Length > 0 ? SelectedQuestion.IncorrectAnswers[0] : string.Empty;
            EditIncorrectAnswer2 = SelectedQuestion.IncorrectAnswers.Length > 1 ? SelectedQuestion.IncorrectAnswers[1] : string.Empty;
            EditIncorrectAnswer3 = SelectedQuestion.IncorrectAnswers.Length > 2 ? SelectedQuestion.IncorrectAnswers[2] : string.Empty;
        }
        else
        {
            EditQuery = string.Empty;
            EditCorrectAnswer = string.Empty;
            EditIncorrectAnswer1 = string.Empty;
            EditIncorrectAnswer2 = string.Empty;
            EditIncorrectAnswer3 = string.Empty;
        }
    }
    private bool CanSaveQuestion(object? obj)
    {
        return SelectedQuestion != null && _mainWindowViewModel?.ActivePack != null;
    }

    private void SaveQuestion(object? obj)
    {
        if (string.IsNullOrWhiteSpace(EditQuery))
        {
            MessageBox.Show("Question text cannot be empty.", "Empty text box error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(EditCorrectAnswer))
        {
            MessageBox.Show("Please write a correct answer.", "Empty text box error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(EditIncorrectAnswer1))
        {
            MessageBox.Show("First incorrect answer box is still empty.", "Empty text box error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(EditIncorrectAnswer2))
        {
            MessageBox.Show("Second incorrect answer box is still empty.", "Empty text box error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(EditIncorrectAnswer3))
        {
            MessageBox.Show("Third incorrect answer box is still empty.", "Empty text box error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SelectedQuestion!.Query = EditQuery;
        SelectedQuestion.CorrectAnswer = EditCorrectAnswer;
        SelectedQuestion.IncorrectAnswers[0] = EditIncorrectAnswer1;
        SelectedQuestion.IncorrectAnswers[1] = EditIncorrectAnswer2;
        SelectedQuestion.IncorrectAnswers[2] = EditIncorrectAnswer3;

    }

    private bool CanRemoveQuestion(object? obj)
    {
        return SelectedQuestion != null && _mainWindowViewModel?.ActivePack != null;
    }
    private void AddQuestion(object? obj)
    {
        var newQuestion = new Question(
            query: "Write a new question here",
            correctAnswer: "Correct Answer goes here",
            incorrectAnswer1: "Wrong Answer 1",
            incorrectAnswer2: "Wrong Answer 2",
            incorrectAnswer3: "Wrong Answer 3"
        );

        _mainWindowViewModel!.ActivePack.Questions.Add(newQuestion);
        SelectedQuestion = newQuestion;
    }
    private void PackOptions(object? obj)
    {
        if (_mainWindowViewModel?.ActivePack != null)
        {
            var dialog = new PackOptions
            {
                DataContext = _mainWindowViewModel.ActivePack,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
    private void RemoveQuestion(object? obj)
    {
        _mainWindowViewModel!.ActivePack.Questions.Remove(SelectedQuestion!);
        SelectedQuestion = null;
    }

}
