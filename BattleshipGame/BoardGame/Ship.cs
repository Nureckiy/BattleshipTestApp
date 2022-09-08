using BattleshipGame.BoardGame.BoardEntities;

namespace BattleshipGame.BoardGame;

public class Ship
{
    private readonly List<ShipSection> _sections = new();

    public bool Sunk { get; private set; }

    public void OnShipSectionSunk()
    {
        if (!Sunk)
        {
            Sunk = _sections.All(s => s.Sunk);
        }
    }

    public void AddSection(ShipSection section)
    {
        _sections.Add(section);
        section.OnSunk += OnShipSectionSunk;
    }

    public bool ContainsSection(ShipSection section) => _sections.Any(s => ReferenceEquals(s, section));
}