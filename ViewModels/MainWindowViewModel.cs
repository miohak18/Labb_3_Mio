using Labb_3_Mio.Command;
using Labb_3_Mio.Models;
using Labb_3_Mio.Views;
using Labb_3_Mio.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb_3_Mio.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly QuestionPackService _questionPackService;
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();

	private QuestionPackViewModel _activePack;

    private Visibility _visibilityConfigurationView;
    private Visibility _visiblePlayerView;
    private Visibility _visibilityQuizCompleteView;

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value))
            {
                PlayerViewModel?.RaiseActivePackChanged();
            }
        }
    }
    public RelayCommand ShowPlayerViewCommand { get; }
    public RelayCommand ShowConfigurationViewCommand { get; }
    public RelayCommand NewQuestionPackCommand { get; }
    public RelayCommand SelectPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }
    public RelayCommand ToggleFullscreenCommand { get; }
    public RelayCommand ExitCommand { get; }
    public RelayCommand ShowQuizCompleteViewCommand { get; }

    public Visibility VisiblePlayerView
    {
        get => _visiblePlayerView;
        set
        {
            _visiblePlayerView = value;
            RaisePropertyChanged();
        }
    }
    public Visibility VisibilityConfigurationView
    {
        get => _visibilityConfigurationView;
        set
        {
            _visibilityConfigurationView = value;
            RaisePropertyChanged();
        }
    }
    public Visibility VisibilityQuizCompleteView
    {
        get => _visibilityQuizCompleteView;
        set
        {
            _visibilityQuizCompleteView = value;
            RaisePropertyChanged();
        }
    }
    public PlayerViewModel? PlayerViewModel { get; }
	public ConfigurationViewModel? ConfigurationViewModel { get; }

	public MainWindowViewModel()
    {
        _questionPackService = new QuestionPackService();

        PlayerViewModel = new PlayerViewModel(this);
		ConfigurationViewModel = new ConfigurationViewModel(this);

        VisibilityConfigurationView = Visibility.Visible;
        VisiblePlayerView = Visibility.Hidden;
        VisibilityQuizCompleteView = Visibility.Hidden;

        ShowPlayerViewCommand = new RelayCommand(ShowPlayerView);
        ShowConfigurationViewCommand = new RelayCommand(ShowConfigurationView);
        NewQuestionPackCommand = new RelayCommand(async _ => await CreateNewQuestionPack());
        SelectPackCommand = new RelayCommand(SelectPack);
        DeletePackCommand = new RelayCommand(async parameter => await DeletePack(parameter));
        ToggleFullscreenCommand = new RelayCommand(ToggleFullscreen);
        ExitCommand = new RelayCommand(_ => Application.Current.MainWindow?.Close());
        ShowQuizCompleteViewCommand = new RelayCommand(ShowQuizCompleteView);

        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        await LoadAllPacks();
    }
    public async Task SaveAllPacksAsync()
    {
        foreach (var packViewModel in Packs)
        {
            await _questionPackService.SaveQuestionPackAsync(packViewModel.GetModel());
        }
    }
    private void SelectPack(object? parameter)
    {
        if (parameter is QuestionPackViewModel selectedPack)
        {
            ActivePack = selectedPack;
            ShowConfigurationView(null);
        }
    }
    private async Task DeletePack(object? parameter)
    {
        if (parameter is QuestionPackViewModel packToDelete)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete '{packToDelete.Name}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _questionPackService.DeleteQuestionPackAsync(packToDelete.Name);

                Packs.Remove(packToDelete);

                if (ActivePack == packToDelete)
                {
                    ActivePack = Packs.FirstOrDefault();
                }
            }
        }
    }
    private async Task CreateNewQuestionPack()
    {
        var dialog = new NewQuestionPack
        {
            Owner = Application.Current.MainWindow,
            PackName = "New Question Pack",
            Difficulty = Difficulty.Medium,
            TimeLimitInSeconds = 30
        };

        if (dialog.ShowDialog() == true)
        {
            var newPack = new QuestionPack(
                dialog.PackName,
                dialog.Difficulty,
                dialog.TimeLimitInSeconds
            );

            var packViewModel = new QuestionPackViewModel(newPack);
            Packs.Add(packViewModel);
            ActivePack = packViewModel;

            await _questionPackService.SaveQuestionPackAsync(newPack);

            ShowConfigurationView(null);
        }
    }
    private async Task LoadAllPacks()
    {
        var loadedPacks = await _questionPackService.LoadAllQuestionPacksAsync();

        foreach (var pack in loadedPacks)
        {
            if (!Packs.Any(p => p.Name == pack.PackName))
            {
                var packViewModel = new QuestionPackViewModel(pack);
                Packs.Add(packViewModel);
            }
        }
        if (Packs.Any() && ActivePack == null)
        {
            ActivePack = Packs.First();
        }
    }
    private void ShowQuizCompleteView(object? obj)
    {
        VisibilityQuizCompleteView = Visibility.Visible;
        VisiblePlayerView = Visibility.Hidden;
        VisibilityConfigurationView = Visibility.Hidden;
    }
    private void ShowPlayerView(object? obj)
    {
        VisiblePlayerView = Visibility.Visible;
        VisibilityConfigurationView = Visibility.Hidden;
        VisibilityQuizCompleteView = Visibility.Hidden;

        PlayerViewModel?.StartGame();
    }
    private void ShowConfigurationView(object? obj)
    {
        VisibilityConfigurationView = Visibility.Visible;
        VisiblePlayerView = Visibility.Hidden;
        VisibilityQuizCompleteView = Visibility.Hidden;
    }
    private void ToggleFullscreen(object? obj)
    {
        var window = Application.Current.MainWindow;

        if (window != null)
        {
            if (window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
                window.WindowStyle = WindowStyle.None;
            }
            else
            {
                window.WindowState = WindowState.Normal;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }
}
