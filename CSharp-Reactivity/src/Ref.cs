using R3;

namespace Reactivity;


public class Ref<T>: IReactive
{
    private EffectManager _effectManager;
    
    private T _value;
    
    private T? _oldValue = default;

    public Subject<ReactiveChangeEvent<T>> OnChange = new();

    public EffectManager EffectManager => _effectManager;
    public Ref(T value)
    {
        _value = value;
        _effectManager = new EffectManager(this);
    }
    public T Value
    {
        get
        {
            if (!EffectScope.isEmpty)
            {
                _effectManager.AddEffect(EffectScope.CurrentEffect);
            }
            return _value;
        }
        set
        {
            _oldValue = _value;
            _value = value;
            _effectManager.Trigger();
            OnChange.OnNext(new ReactiveChangeEvent<T>()
            {
                Value = _value,
                OldValue = _oldValue,
            });
            _oldValue = default;
        }
    }

}
