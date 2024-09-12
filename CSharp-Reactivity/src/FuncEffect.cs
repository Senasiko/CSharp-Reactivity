namespace Reactivity;

public class FuncEffect<T>: Effect
{
    private Func<T> _fn;

    public FuncEffect(Func<T> fn)
    {
        _fn = fn;
    }

    public void Run()
    {
        using (new EffectScope(this))
        {
            _fn();
        }
    }
    
    public override void Trigger()
    {
        Run();
    }
}