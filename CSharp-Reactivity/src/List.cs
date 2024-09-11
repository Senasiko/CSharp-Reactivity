using R3;

namespace Reactivity;

enum ReactiveListChangeType
{
    Add,
    Remove,
    Clear
}

struct ReactiveListChangeEvent<T>
{
    public T Item;
    public ReactiveListChangeType Type;
}

class ReactiveList<T>: IReactive
{
    public EffectManager EffectManager { get; }
    
    private List<T> Value { get; }
    
    public Subject<ReactiveListChangeEvent<T>> OnChange = new();
    
    public int Count {
        get
        {
            Track();
            return Value.Count;
        }
    }
    
    public T this[int index]
    {
        get
        {
            Track();
            return Value[index];
        }
    }

    public ReactiveList()
    {
        Value = new List<T>();
        EffectManager = new EffectManager(this);
    }
    
    public ReactiveList(List<T> value)
    {
        Value = value;
        EffectManager = new EffectManager(this);
    }
    
    public void Add(T item)
    {
        Value.Add(item);
        EffectManager.Trigger();
        
        OnChange.OnNext(new ReactiveListChangeEvent<T>()
        {
            Item = item,
            Type = ReactiveListChangeType.Add
        });
    }
    
    public void Remove(T item)
    {
        Value.Remove(item);
        EffectManager.Trigger();
        
        OnChange.OnNext(new ReactiveListChangeEvent<T>()
        {
            Item = item,
            Type = ReactiveListChangeType.Remove
        });
    }
    
    public void Clear()
    {
        Value.Clear();
        EffectManager.Trigger();
        
        OnChange.OnNext(new ReactiveListChangeEvent<T>()
        {
            Type = ReactiveListChangeType.Clear
        });
    }

    private void Track()
    {
        if (!EffectScope.isEmpty)
        {
            EffectManager.AddEffect(EffectScope.CurrentEffect);
        }
    }
}



