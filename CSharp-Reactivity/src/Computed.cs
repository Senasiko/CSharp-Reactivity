using R3;

namespace Reactivity;


struct ComputedChangeEvent<T>
{
    public T? OldValue;
}


class Computed<T> : Effect, IReactive
{
    private Func<T> getter;

    private bool _isDirty = true;

    private T? _currentValue;

    public EffectManager EffectManager { get; }

    public Subject<ComputedChangeEvent<T>> OnChange = new();

    public Computed(Func<T> fn)
    {
        getter = fn;
        EffectManager = new EffectManager(this);
    }

    public T Value
    {
        get
        {
            if (!EffectScope.isEmpty)
            {
                EffectManager.AddEffect(EffectScope.CurrentEffect);
            }
            using (new EffectScope(this))
            {
                if (_isDirty)
                {
                    _currentValue = getter();
                }

                _isDirty = false;
            }
            return _currentValue!;
        }
    }

    public override void Trigger()
    {
        _isDirty = true;
        EffectManager.Trigger();
        OnChange.OnNext(new ComputedChangeEvent<T>()
        {
            OldValue = _currentValue,
        });
    }
}