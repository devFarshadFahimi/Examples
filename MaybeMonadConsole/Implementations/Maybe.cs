namespace MaybeMonadConsole.Implementations;

public readonly struct Maybe<T>
{
    private readonly T _value;

    public T Value => _value;
    public bool HasValue { get; }

    private Maybe(T value)
    {
        _value = value;
        HasValue = true;
    }

    public static Maybe<T> Some(T value) => new(value);

    public static Maybe<T> None() => new();

    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        => HasValue ? Maybe<TResult>.Some(selector(_value)) : Maybe<TResult>.None();

    public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> selector)
        => HasValue ? selector(_value) : Maybe<TResult>.None();
}
