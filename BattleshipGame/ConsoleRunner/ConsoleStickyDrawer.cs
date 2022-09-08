namespace BattleshipGame.ConsoleRunner;

public class ConsoleStickyDrawer : IDisposable
{
    private readonly ConsoleOutput _output;
    private readonly int _stickToTop;
    private readonly int _currentConsoleY;

    public ConsoleStickyDrawer(ConsoleOutput output, int top)
    {
        _output = output;
        _stickToTop = top;
        _currentConsoleY = _output.CursorTop;
        _output.MoveToRow(top);
    }

    public void Dispose()
    {
        var newConsolePosition = _stickToTop == _currentConsoleY ? Console.CursorTop : _currentConsoleY;
        _output.MoveToRow(newConsolePosition);
    }
}