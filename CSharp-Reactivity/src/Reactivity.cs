using R3;

namespace Reactivity;

    
interface IReactive
{
    EffectManager EffectManager { get; }
    
}

class EffectScope: IDisposable
{
    private static readonly Stack<Effect> Stack = new();
    
    public static Effect CurrentEffect => Stack.First();

    public static bool isEmpty => Stack.Count == 0;

    
    public EffectScope(Effect effect)
    {
        Stack.Push(effect);
    }

    public void Dispose()
    {
        Stack.Pop();
    }
}

abstract class Effect: IDisposable
{

    private HashSet<IReactive> deps = new();

    public void AddDep(IReactive dep)
    {
        deps.Add(dep);
    }

    public abstract void Trigger();

    public void Dispose()
    {
        foreach (var reactive in deps)
        {
            reactive.EffectManager.RemoveEffect(this);
        }
    }
}

class EffectManager(IReactive reactive)
{
    private readonly HashSet<Effect> _effects = new();
    
    public int Count => _effects.Count;

    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
        effect.AddDep(reactive);
    }

    public void RemoveEffect(Effect effect)
    {
        _effects.Remove(effect);
    }

    public void Trigger()
    {
        foreach (var dep in _effects)
        {
            dep.Trigger();
        }
    }
}

struct ReactiveChangeEvent<T>
{
    public T Value;
    public T? OldValue;
}


