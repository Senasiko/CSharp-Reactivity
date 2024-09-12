using R3;

namespace Reactivity;

    
public interface IReactive
{
    EffectManager EffectManager { get; }
    
}

public class EffectScope: IDisposable
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

public abstract class Effect: IDisposable
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

public class EffectManager
{
    private readonly HashSet<Effect> _effects = new();
    
    public int Count => _effects.Count;
    
    private readonly IReactive reactive;

    public EffectManager(IReactive reactive)
    {
        this.reactive = reactive;
    }

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

public struct ReactiveChangeEvent<T>
{
    public T Value;
    public T? OldValue;
}


