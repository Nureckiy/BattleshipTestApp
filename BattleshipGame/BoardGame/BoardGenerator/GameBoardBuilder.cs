using BattleshipGame.BoardGame.BoardEntities;

namespace BattleshipGame.BoardGame.BoardGenerator;

public class GameBoardBuilder
{
    private readonly int _xAxis;
    private readonly int _yAxis;
    private readonly byte[,] _matrix;
    private readonly List<Ship> _ships = new();
    private readonly IPlayerBoardEntitiesCollection _boardEntities = new PlayerBoardEntitiesCollection();

    public GameBoardBuilder(int xAxis, int yAxis)
    {
        _xAxis = xAxis;
        _yAxis = yAxis;
        _matrix = new byte[xAxis, yAxis];
    }

    public PlayerBoard Build()
    {
        FillEmptyEntities();
        return new PlayerBoard(_boardEntities, _ships);
    }

    private void FillEmptyEntities()
    {
        for (var i = 0; i < _xAxis; i++)
        {
            for (var j = 0; j < _yAxis; j++)
            {
                if (_boardEntities.Get(i, j) == null)
                {
                    _boardEntities.Add(i, j, new EmptyEntity());
                }
            }
        }
    }

    public void PlaceShip(ShipPosition position)
    {
        var ship = new Ship();
        _ships.Add(ship);

        if (position.Horizontal)
        {
            PlaceShipOnMapHorizontally(position.X, position.Y, position.Size, ship);
            FillNeighborhoodFlagAroundHorizontalShip(position.X, position.Y, position.Size);
        }
        else
        {
            PlaceShipOnMapVertically(position.X, position.Y, position.Size, ship);
            FillNeighborhoodFlagAroundVerticalShip(position.X, position.Y, position.Size);
        }
    }

    // We try to find the nearest free places to reduce the number of random attempts
    public ShipPosition FindNearestEmptyPosition(CreateShipModel shiftToPlace, int x, int y, bool horizontal)
    {
        var headX = x;
        var headY = y;

        if (horizontal && !TryFindVacantHeadForHorizontalShip(shiftToPlace, x, y, out headX))
        {
            return null;
        }

        if (!horizontal && !TryFindVacantHeadForVerticalShip(shiftToPlace, x, y, out headY))
        {
            return null;
        }

        return new ShipPosition
        {
            Horizontal = horizontal,
            Size = shiftToPlace.Size,
            X = headX,
            Y = headY
        };
    }

    private void PlaceShipOnMapHorizontally(int shipHeadX, int y, int length, Ship ship)
    {
        for (var x = shipHeadX; x < shipHeadX + length; x++)
        {
            PlaceShipSectionOnMap(x, y, ship);
        }
    }

    private void PlaceShipOnMapVertically(int x, int shipHeadY, int length, Ship ship)
    {
        for (var y = shipHeadY; y < shipHeadY + length; y++)
        {
            PlaceShipSectionOnMap(x, y, ship);
        }
    }

    private void PlaceShipSectionOnMap(int x, int y, Ship ship)
    {
        var section = new ShipSection();
        ship.AddSection(section);
        _matrix[x, y] = 2;
        _boardEntities.Add(x, y, section);
    }

    private void FillNeighborhoodFlagAroundHorizontalShip(int shipHeadX, int y, int length)
    {
        var xStart = shipHeadX;
        if (shipHeadX > 0)
        {
            xStart = shipHeadX - 1;
            _matrix[xStart, y] = 1;
        }

        var xEnd = shipHeadX + length;
        if (xEnd < _xAxis - 1)
        {
            xEnd = shipHeadX + 1;
            _matrix[xEnd, y] = 1;
        }

        for (var x = xStart; x < xEnd; x++)
        {
            if (y > 0)
            {
                _matrix[x, y - 1] = 1;
            }

            if (y < _yAxis - 1)
            {
                _matrix[x, y + 1] = 1;
            }
        }
    }

    private void FillNeighborhoodFlagAroundVerticalShip(int x, int shipHeadY, int length)
    {
        var yStart = shipHeadY;
        if (shipHeadY > 0)
        {
            yStart = shipHeadY - 1;
            _matrix[x, yStart] = 1;
        }

        var yEnd = shipHeadY + length;
        if (yEnd < _yAxis - 1)
        {
            yEnd = shipHeadY + 1;
            _matrix[x, yEnd] = 1;
        }

        for (var y = yStart; y < yEnd; y++)
        {
            if (x > 0)
            {
                _matrix[x - 1, y] = 1;
            }

            if (x < _xAxis - 1)
            {
                _matrix[x + 1, y] = 1;
            }
        }
    }
    
    private bool IsVacantCell(int x, int y) =>
        x < _xAxis && x >= 0 &&
        y < _yAxis && y >= 0 &&
        _matrix[x, y] == 0;

    private bool TryFindVacantHeadForHorizontalShip(CreateShipModel shiftToPlace, int x, int y, out int newHead)
    {
        var goingBack = false;
        var goingBackPosition = 0;

        for (var nextX = x; nextX < shiftToPlace.Size + x; nextX++)
        {
            var currentX = nextX;
            if (goingBack)
            {
                goingBackPosition--;
                currentX = goingBackPosition;
            }

            if (IsVacantCell(currentX, y))
            {
                continue;
            }

            if (goingBack)
            {
                newHead = default;
                return false;
            }

            goingBack = true;
            goingBackPosition = x;
            nextX--;
        }

        newHead = goingBack ? goingBackPosition : x;
        return true;
    }

    private bool TryFindVacantHeadForVerticalShip(CreateShipModel shiftToPlace, int x, int y, out int newHead)
    {
        var goingBack = false;
        var goingBackPosition = 0;

        for (var nextY = y; nextY < shiftToPlace.Size + y; nextY++)
        {
            var currentY = nextY;

            if (goingBack)
            {
                goingBackPosition--;
                currentY = goingBackPosition;
            }

            if (IsVacantCell(x, currentY))
            {
                continue;
            }

            if (goingBack)
            {
                newHead = default;
                return false;
            }

            goingBack = true;
            goingBackPosition = y;
            nextY--;
        }

        newHead = goingBack ? goingBackPosition : y;
        return true;
    }
}