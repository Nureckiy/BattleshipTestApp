namespace BattleshipGame.ConsoleRunner
{
    public class ConsoleOutput
    {
        public void WriteLine(string value) => Console.WriteLine(value);
        public void Write(string value) => Console.Write(value);
        public int CursorTop => Console.CursorTop;

        public void MoveToRow(int top)
        {
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, top);
        }
    }
}
