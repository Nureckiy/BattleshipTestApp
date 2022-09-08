namespace BattleshipGame.BoardGame;

public enum ShotResult
{
    Missed = 0,
    Hit = 1,
    DestroyedShip = 2,
    Win = 3,
    AlreadyTouched = 4,
    OutOfBounds = 5
}