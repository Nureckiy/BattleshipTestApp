namespace BattleshipGame.BoardGame.BoardGenerator
{
    public class RandomBoardGenerator
    {
        private readonly int _xAxis;
        private readonly int _yAxis;
        private readonly Random _random;

        public RandomBoardGenerator(int xAxis, int yAxis)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
            _random = new Random();
        }

        public PlayerBoard Generate(List<CreateShipModel> ships)
        {
            var builder = new GameBoardBuilder(_xAxis, _yAxis);

            foreach (var ship in ships.OrderByDescending(s => s.Size))
            {
                var position = FindNextPosition(builder, ship);
                builder.PlaceShip(position);
            }

            return builder.Build();
        }

        private ShipPosition FindNextPosition(GameBoardBuilder gameBoardBuilder, CreateShipModel shiftToPlace)
        {
            var attempts = 0;
            while (attempts++ < 10)
            {
                var x = _random.Next(_xAxis);
                var y = _random.Next(_yAxis);
                var horizontal = _random.NextDouble() < 0.5;
                var position = gameBoardBuilder.FindNearestEmptyPosition(shiftToPlace, x, y, horizontal);

                if (position != null)
                {
                    return position;
                }
            }

            throw new ApplicationException("Could not find a free space for placing a ship");
        }
    }
}
