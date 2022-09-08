using BattleshipGame.BoardGame;
using BattleshipGame.BoardGame.BoardGenerator;
using NUnit.Framework;

namespace BattleshipGame.UnitTests
{
    public class GameBoardBuilderUnitTests
    {
        private GameBoardBuilder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new GameBoardBuilder(10, 10);
        }

        [TestCase(4, 2, true, 4, 2)]
        [TestCase(8, 2, true, 6, 2)] // slide back horizontally to fit
        [TestCase(4, 2, false, 4, 2)]
        [TestCase(4, 8, false, 4, 6)] // slide back vertically to fit
        public void FindNearestNextPosition_EmptyDesk_ShouldReturnCorrectPosition(int x, int y, bool horizontal, int expectedXPosition, int expectedYPosition)
        {
            // Arrange
            var ship = new CreateShipModel(4);

            // Act
            var result = _sut.FindNearestEmptyPosition(ship, x, y, horizontal);

            // Assert
            Assert.That(horizontal, Is.EqualTo(result.Horizontal));
            Assert.That(4, Is.EqualTo(result.Size));
            Assert.That(expectedXPosition, Is.EqualTo(result.X));
            Assert.That(expectedYPosition, Is.EqualTo(result.Y));
        }

        [TestCase(0, 5, true, 0, 5)]
        [TestCase(1, 5, true, 0, 5)] // slide back horizontally to fit
        [TestCase(5, 0, false, 5, 0)]
        [TestCase(5, 1, false, 5, 0)] // slide back vertically to fit
        public void FindNearestNextPosition_DeskHasPlacedShips_ShouldReturnCorrectPosition(int x, int y, bool horizontal, int expectedXPosition, int expectedYPosition)
        {
            // Arrange
            var ship = new CreateShipModel(4);

            var existing = new ShipPosition
            {
                Horizontal = horizontal,
                Size = 4,
                X = 5,
                Y = 5
            };

            _sut.PlaceShip(existing);
            
            // Act
            var result = _sut.FindNearestEmptyPosition(ship, x, y, horizontal);

            // Assert
            Assert.That(result.Horizontal, Is.EqualTo(horizontal));
            Assert.That(result.Size, Is.EqualTo(4));
            Assert.That(result.X, Is.EqualTo(expectedXPosition));
            Assert.That(result.Y, Is.EqualTo(expectedYPosition));
        }

        [TestCase(5, 5, true)]
        [TestCase(5, 6, false)]
        public void FindNearestNextPosition_IntersectWithExistingShips_ShouldReturnNull(int x, int y, bool horizontal)
        {
            // Arrange
            var ship = new CreateShipModel(4);
            var existing = new ShipPosition
            {
                Horizontal = horizontal,
                Size = 4,
                X = 5,
                Y = 5
            };

            _sut.PlaceShip(existing);

            // Act
            var result = _sut.FindNearestEmptyPosition(ship, x, y, horizontal);

            // Assert
            Assert.IsNull(result);
        }

        [TestCase(7, 5, true)]
        [TestCase(5, 9, false)]
        public void FindNearestNextPosition_NoFreeSpaceAvailable_ShouldReturnNull(int x, int y, bool horizontal)
        {
            // Arrange
            var ship = new CreateShipModel(4);
            var existing = new ShipPosition
            {
                Horizontal = horizontal,
                Size = 4,
                X = 5,
                Y = 5
            };

            _sut.PlaceShip(existing);

            // Act
            var result = _sut.FindNearestEmptyPosition(ship, x, y, horizontal);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Build_ShouldInitializeBoardCorrectly()
        {
            // Arrange
            var existing = new ShipPosition
            {
                Horizontal = true,
                Size = 4,
                X = 5,
                Y = 5
            };

            _sut.PlaceShip(existing);

            // Act
            var result = _sut.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Shot(4, 5), Is.EqualTo(ShotResult.Missed));
            Assert.That(result.Shot(6, 5), Is.EqualTo(ShotResult.Hit));
            result.Shot(5, 5);
            result.Shot(7, 5);
            Assert.That(result.Shot(8, 5), Is.EqualTo(ShotResult.Win));
        }
    }
}