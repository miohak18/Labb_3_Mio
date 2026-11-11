using Labb_3_Mio.Command;
using Labb_3_Mio.Models;
using Labb_3_Mio.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb_3_Mio.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; }

	private QuestionPackViewModel _activePack;

    private Visibility _visibilityConfigurationView;
    private Visibility _visiblePlayerView;


    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value)) ;
            {
                PlayerViewModel?.RaiseActivePackChanged();
            }
        }
    }
    public RelayCommand ShowPlayerViewCommand { get; }
    public RelayCommand ShowConfigurationViewCommand { get; }

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

    public PlayerViewModel? PlayerViewModel { get; }
	public ConfigurationViewModel? ConfigurationViewModel { get; }

	public MainWindowViewModel()
    {
		PlayerViewModel = new PlayerViewModel(this);
		ConfigurationViewModel = new ConfigurationViewModel(this);

        VisibilityConfigurationView = Visibility.Hidden;
        VisiblePlayerView = Visibility.Hidden;

        ShowPlayerViewCommand = new RelayCommand(ShowPlayerView);
        ShowConfigurationViewCommand = new RelayCommand(ShowConfigurationView);

        var questionPack = new Models.QuestionPack("Mina Frågor!", Difficulty.Easy);
        questionPack.Questions.Add(new Models.Question("Vad heter sveriges huvudstad?", "Stockholm", "Oslo", "Göteborg", "London"));
        questionPack.Questions.Add(new Models.Question("Vad är 2 + 3?", "4", "5", "6", "7"));

        ActivePack = new QuestionPackViewModel(questionPack);
    }
    private void ShowPlayerView(object? obj)
    {
        VisiblePlayerView = Visibility.Visible;
        VisibilityConfigurationView = Visibility.Hidden;

        PlayerViewModel?.StartGame();
    }
    private void ShowConfigurationView(object? obj)
    {
        VisibilityConfigurationView = Visibility.Visible;
        VisiblePlayerView = Visibility.Hidden;
    }
}
