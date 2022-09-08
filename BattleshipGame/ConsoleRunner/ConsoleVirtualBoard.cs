using BattleshipGame.BoardGame;
using System.Text;

namespace BattleshipGame.ConsoleRunner
{
    public class ConsoleVirtualBoard
    {
        private readonly int _xAxis;
        private readonly int _yAxis;
        private readonly ShotResult?[,] _virtualBoard;
        private int _cursorPositionY;
        private ConsoleOutput _output;
        private const string DefaultCellSymbol = "~";

        private readonly IDictionary<ShotResult?, string> _symbolsMap = new Dictionary<ShotResult?, string>()
        {
            { ShotResult.Missed, " " },
            { ShotResult.Hit, "#" },
            { ShotResult.DestroyedShip, "#" }
        };

        public ConsoleVirtualBoard(int xAxis, int yAxis)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
            _virtualBoard = new ShotResult?[xAxis, yAxis];
        }

        public void ApplyShotResult(int x, int y, ShotResult shotResult)
        {
            if (_symbolsMap.ContainsKey(shotResult))
            {
                _virtualBoard[x, y] = shotResult;
            }
        }

        public void Draw()
        {
            var sb = new StringBuilder();
            AppendLettersAxis(sb);

            for (var y = 0; y < _yAxis; y++)
            {
                sb.AppendLine();
                AppendRowNumber(sb, y);

                for (var x = 0; x < _xAxis; x++)
                {
                    AppendCell(sb, x, y);
                }
            }

            sb.AppendLine();

            using (new ConsoleStickyDrawer(_output, _cursorPositionY))
            {
                _output.Write(sb.ToString());
            }
        }

        private void AppendLettersAxis(StringBuilder sb)
        {
            sb.Append("  ");
            for (var c = 'A'; c < _xAxis + 'A'; c++)
            {
                sb.Append($" {c} ");
            }
        }

        private static void AppendRowNumber(StringBuilder sb, int rowNumber)
        {
            sb.Append($"{rowNumber + 1}");
            if (rowNumber < 9)
            {
                sb.Append(' ');
            }
        }

        private void AppendCell(StringBuilder sb, int x, int y)
        {
            var symbol = DefaultCellSymbol;

            if (_virtualBoard[x, y] != null)
            {
                _symbolsMap.TryGetValue(_virtualBoard[x, y], out symbol);
            }

            sb.Append($" {symbol} ");
        }

        public void StickToConsole(ConsoleOutput output)
        {
            _output = output;
            _cursorPositionY = output.CursorTop;
        }
    }
}
