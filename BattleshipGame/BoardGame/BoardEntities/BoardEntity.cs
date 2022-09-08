namespace BattleshipGame.BoardGame.BoardEntities;

public abstract class BoardEntity
{
    private bool _touched;

    public ShotResult Shot()
    {
        if (_touched)
        {
            return ShotResult.AlreadyTouched;
        }

        _touched = true;
        return HandleShot();
    }

    protected abstract ShotResult HandleShot();
}