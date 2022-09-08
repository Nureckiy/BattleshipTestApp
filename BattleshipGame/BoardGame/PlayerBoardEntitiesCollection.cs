using BattleshipGame.BoardGame.BoardEntities;

namespace BattleshipGame.BoardGame
{
    public class PlayerBoardEntitiesCollection : IPlayerBoardEntitiesCollection
    {
        private readonly IDictionary<(int x, int y), BoardEntity> _entities = new Dictionary<(int x, int y), BoardEntity>();

        public void Add(int x, int y, BoardEntity entity) => _entities.Add((x, y), entity);

        public BoardEntity Get(int x, int y) => _entities.TryGetValue((x, y), out var entity)
            ? entity
            : null;
    }
}
