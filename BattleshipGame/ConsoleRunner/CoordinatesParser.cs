namespace BattleshipGame.ConsoleRunner
{
    public class CoordinatesParser
    {
        private readonly int _xAxis;
        private readonly int _yAxis;

        public CoordinatesParser(int xAxis, int yAxis)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
        }

        public bool TryParseCoordinates(string inputString, out (int x, int y) result)
        {
            if (!string.IsNullOrWhiteSpace(inputString) &&
                TryParseCol(inputString, out var col) &&
                TryParseRow(inputString, out var row))
            {
                result = (col, row);
                return true;
            }

            result = default;
            return false;
        }

        private bool TryParseCol(string inputString, out int result)
        {
            var columnLetter = char.ToLower(inputString.First());

            var columnNumber = columnLetter - 'a';
            if (columnNumber >= 0 && columnNumber < _xAxis)
            {
                result = columnNumber;
                return true;
            }

            result = default;
            return false;
        }

        private bool TryParseRow(string inputString, out int result)
        {
            var rowString = inputString[1..];

            if (int.TryParse(rowString, out var rowNumber) && rowNumber > 0 && rowNumber <= _yAxis)
            {
                result = rowNumber - 1;
                return true;
            }

            result = default;
            return false;
        }
    }
}
