namespace BattleshipGame.BoardGame.BoardEntities;

public delegate void SectionSunk();

public class ShipSection : BoardEntity
{
    public event SectionSunk OnSunk;

    public bool Sunk { get; private set; }

    protected override ShotResult HandleShot()
    {
        Sunk = true;
        OnSunk?.Invoke();
        return ShotResult.Hit;
    }
}