using BattleshipGame.BoardGame;
using BattleshipGame.BoardGame.BoardEntities;
using Moq;
using NUnit.Framework;

namespace BattleshipGame.UnitTests
{
    public class PlayerBoardUnitTests
    {
        private PlayerBoard _sut;
        private Mock<IPlayerBoardEntitiesCollection> _boardEntitiesCollection;
        private List<Ship> _ships;

        [SetUp]
        public void SetUp()
        {
            _boardEntitiesCollection = new Mock<IPlayerBoardEntitiesCollection>();
            _ships = new List<Ship>();
            _sut = new PlayerBoard(_boardEntitiesCollection.Object, _ships);
        }

        [Test]
        public void Shot_NothingFound_ShouldReturnOutOfBounds()
        {
            // Act
            var result = _sut.Shot(1, 2);

            // Assert
            Assert.That(result, Is.EqualTo(ShotResult.OutOfBounds));
        }

        [Test]
        public void Shot_EmptySection_ShouldReturnMissedState()
        {
            // Arrange
            _boardEntitiesCollection.Setup(x => x.Get(1, 2)).Returns(new EmptyEntity());

            // Act
            var result = _sut.Shot(1, 2);

            // Assert
            Assert.That(result, Is.EqualTo(ShotResult.Missed));
        }

        [Test]
        public void Shot_Hit_ShouldReturnHitState()
        {
            // Arrange
            var section = new ShipSection();
            var ship = new Ship();
            ship.AddSection(section);
            ship.AddSection(new ShipSection());
            _ships.Add(ship);

            _boardEntitiesCollection.Setup(x => x.Get(1, 2)).Returns(section);

            // Act
            var result = _sut.Shot(1, 2);

            // Assert
            Assert.That(result, Is.EqualTo(ShotResult.Hit));
        }

        [Test]
        public void Shot_HitSingleSection_ShouldReturnFinishedGame()
        {
            // Arrange
            var section = new ShipSection();
            var ship = new Ship();
            ship.AddSection(section);
            _ships.Add(ship);

            _boardEntitiesCollection.Setup(x => x.Get(1, 2)).Returns(section);

            // Act
            var result = _sut.Shot(1, 2);

            // Assert
            Assert.That(result, Is.EqualTo(ShotResult.Win));
        }

        [Test]
        public void Shot_HitAllShipSections_ShouldReturnDestroyedShip()
        {
            // Arrange
            var section1 = new ShipSection();
            var section2 = new ShipSection();
            var ship = new Ship();
            ship.AddSection(section1);
            ship.AddSection(section2);
            
            _ships.Add(ship);
            _ships.Add(new Ship());

            _boardEntitiesCollection.Setup(x => x.Get(1, 2)).Returns(section1);
            _boardEntitiesCollection.Setup(x => x.Get(1, 3)).Returns(section2);

            // Act
            _sut.Shot(1, 2);
            var result = _sut.Shot(1, 3);

            // Assert
            Assert.That(result, Is.EqualTo(ShotResult.DestroyedShip));
        }
    }
}
