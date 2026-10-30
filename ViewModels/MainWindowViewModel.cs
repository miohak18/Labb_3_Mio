using Labb_3_Mio.Models;
using System.Collections.ObjectModel;

namespace Labb_3_Mio.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; }

	private QuestionPackViewModel _activePack;

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set => SetProperty(ref _activePack, value);
    }

    public PlayerViewModel? PlayerViewModel { get; }
	public ConfigurationViewModel? ConfigurationViewModel { get; }

	public MainWindowViewModel()
    {
		PlayerViewModel = new PlayerViewModel(this);
		ConfigurationViewModel = new ConfigurationViewModel(this);

        var questionPack = new Models.QuestionPack("Mina Frågor!", Difficulty.Easy);
        questionPack.Questions.Add(new Models.Question("Vad heter sveriges huvudstad?", "Stockholm", "Oslo", "Göteborg", "London"));
        questionPack.Questions.Add(new Models.Question("Vad heter sveriges största sjö?", "Vänern", "Vättern", "Mälaren", "Storsjön"));

        ActivePack = new QuestionPackViewModel(questionPack);
    }

}
