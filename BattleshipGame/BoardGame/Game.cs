using BattleshipGame.BoardGame.BoardGenerator;

namespace BattleshipGame.BoardGame
{
    public class Game
    {
        private readonly int _xAxis;
        private readonly int _yAxis;
        private PlayerBoard _board;

        public Game(int xAxis, int yAxis)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
        }

        public void Init()
        {
            var generator = new RandomBoardGenerator(_xAxis, _yAxis);
            var ships = new List<CreateShipModel>
            {
                new(5),
                new(4),
                new(4)
            };

            _board = generator.Generate(ships);
        }

        public ShotResult Shot(int x, int y)
        {
            return _board.Shot(x, y);
        }
    }
}
