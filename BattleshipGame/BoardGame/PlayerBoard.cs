using BattleshipGame.BoardGame.BoardEntities;

namespace BattleshipGame.BoardGame
{
    public class PlayerBoard
    {
        private readonly IPlayerBoardEntitiesCollection _boardEntities;
        private readonly List<Ship> _ships;

        public PlayerBoard(IPlayerBoardEntitiesCollection boardEntities, List<Ship> ships)
        {
            _boardEntities = boardEntities;
            _ships = ships;
        }

        public ShotResult Shot(int x, int y)
        {
            var entity = _boardEntities.Get(x, y);

            if (entity == null)
            {
                return ShotResult.OutOfBounds;
            }

            var result = entity.Shot();

            if (result != ShotResult.Hit)
            {
                return result;
            }

            var ship = _ships.Find(s => s.ContainsSection(entity as ShipSection));

            if (ship == null || !ship.Sunk)
            {
                return ShotResult.Hit;
            }

            if (_ships.All(s => s.Sunk))
            {
                return ShotResult.Win;
            }

            return ShotResult.DestroyedShip;
        }
    }
}
