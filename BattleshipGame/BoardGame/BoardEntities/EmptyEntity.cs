namespace BattleshipGame.BoardGame.BoardEntities;

public class EmptyEntity : BoardEntity
{
    protected override ShotResult HandleShot()
    {
        return ShotResult.Missed;
    }
}