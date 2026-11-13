using Labb_3_Mio.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Labb_3_Mio.ViewModels;

internal class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack _model;

    public QuestionPackViewModel(QuestionPack model)
    {
        _model = model;
        Questions = new ObservableCollection<Question>(_model.Questions);
        Questions.CollectionChanged += Questions_CollectionChanged;
    }
    public string Name
    {
        get => _model.PackName;
        set {
            if (_model.PackName == value)
                return;

            _model.PackName = value;
            RaisePropertyChanged();
        }
    }
    public Difficulty Difficulty
    {
        get => _model.Difficulty;
        set
        {
            if (_model.Difficulty == value)
                return;

            _model.Difficulty = value;
            RaisePropertyChanged();
        }
    }
    public int TimeLimitInSeconds
    {
        get => _model.TimeLimitInSeconds;
        set
        {
            if (_model.TimeLimitInSeconds == value)
                return;

            _model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }  
    public ObservableCollection<Question> Questions { get; }
    public QuestionPack GetModel() => _model;

    private void Questions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not null)
                {
                    int index = e.NewStartingIndex;
                    foreach (Question question in e.NewItems)
                        _model.Questions.Insert(index++, question);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not null)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                        _model.Questions.RemoveAt(e.OldStartingIndex);
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.NewItems is not null)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                        _model.Questions[e.NewStartingIndex + i] = (Question)e.NewItems[i]!;
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (e.OldItems is not null)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        Question question = _model.Questions[e.OldStartingIndex];
                        _model.Questions.RemoveAt(e.OldStartingIndex);
                        _model.Questions.Insert(e.NewStartingIndex + i, question);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                _model.Questions.Clear();
                break;
        }
    }
}
