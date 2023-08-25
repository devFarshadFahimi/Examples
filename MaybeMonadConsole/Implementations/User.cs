namespace MaybeMonadConsole.Implementations;

public class User
{
    public string Name { get; set; } = null!;
    public Maybe<int> Age { get; set; }
    public Maybe<string> Email { get; set; }
}
