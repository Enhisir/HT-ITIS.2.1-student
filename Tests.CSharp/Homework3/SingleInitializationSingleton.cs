namespace Tests.CSharp.Homework3;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;

    private static volatile bool _isInitialized = false;
    private static Lazy<SingleInitializationSingleton> _instance = new(isThreadSafe:true);

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    public int Delay { get; }

    public static SingleInitializationSingleton Instance => _instance.Value;

    internal static void Reset()
    {
        // check for avoiding redundant initialization and closure
        var delay = _isInitialized ? Instance.Delay : DefaultDelay;
        
        _instance = new Lazy<SingleInitializationSingleton>(
            () => new SingleInitializationSingleton(delay),
            isThreadSafe: true);
        _isInitialized = false;
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized) throw new InvalidOperationException();

        // I don't implement DCL by myself
        // because of guarantee of thread-safety
        // when using Lazy<>
        _instance = new Lazy<SingleInitializationSingleton>(
            () => new SingleInitializationSingleton(delay),
            isThreadSafe: true);
        _isInitialized = true;
    }
}