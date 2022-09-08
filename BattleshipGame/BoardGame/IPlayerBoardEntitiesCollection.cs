using BattleshipGame.BoardGame.BoardEntities;

namespace BattleshipGame.BoardGame;

public interface IPlayerBoardEntitiesCollection
{
    void Add(int x, int y, BoardEntity entity);

    BoardEntity Get(int x, int y);
}