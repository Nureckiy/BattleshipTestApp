using BattleshipGame.BoardGame;

namespace BattleshipGame.ConsoleRunner
{
    public class ConsoleGameRunner
    {
        private readonly ConsoleOutput _output;
        private readonly Game _game;
        private readonly ConsoleVirtualBoard _boardDrawer;
        private readonly CoordinatesParser _coordinatesParser;

        public ConsoleGameRunner(ConsoleOutput output, int xAxis, int yAxis)
        {
            _output = output;
            _game = new Game(xAxis, yAxis);
            _game.Init();
            _boardDrawer = new ConsoleVirtualBoard(xAxis, yAxis);
            _coordinatesParser = new CoordinatesParser(xAxis, yAxis);
        }

        public void Run()
        {
            PrintIntro();
            _boardDrawer.StickToConsole(_output);
            _boardDrawer.Draw();

            do
            {
                _boardDrawer.Draw();
            } while (ProcessInput() == 0);

            _output.WriteLine("The end!");
        }

        private int ProcessInput()
        {
            var inputString = Console.ReadLine()?.Replace(" ", "");

            if (IsExitRequested(inputString))
            {
                return 1;
            }
            
            if (!_coordinatesParser.TryParseCoordinates(inputString, out var coordinates))
            {
                _output.WriteLine("Invalid input");
                return 0;
            }

            return ProcessShot(inputString, coordinates.x, coordinates.y);
        }

        private int ProcessShot(string inputString, int x, int y)
        {
            var shotResult = _game.Shot(x, y);

            _boardDrawer.ApplyShotResult(x, y, shotResult);
            _output.MoveToRow(_output.CursorTop - 1);
            _output.WriteLine($"{inputString} -> {shotResult}");

            return shotResult == ShotResult.Win ? 1 : 0;
        }

        private void PrintIntro()
        {
            _output.WriteLine("Welcome to the Battleship Game!\n\nPlease, enter a guess (e.g. A5) or Q to exit\n");
        }

        private static bool IsExitRequested(string inputString) =>
            string.Equals(inputString, "Q", StringComparison.OrdinalIgnoreCase);
    }
}
