
using System.Linq;
using NUnit.Framework;
using UnoTV.Web.Domain;
using UnoTV.Web.Game;
namespace UnoTV.Web.Tests.Game
{
    public class GameStateTests
    {
        private GameState _gameState;

        [SetUp]
        public void SetUp()
        {
            _gameState = new GameState();
        }

        [Test]
        public void AddPlayer_AddsNewPlayerToTheGame()
        {
            var id = "{E6AB01BE-E623-492C-8390-01786604DD14}";
            var name = "Bob";
            var newPlayer = new Player(id, name);
            _gameState.AddPlayer(newPlayer);
            Assert.That(_gameState.Players.Contains(newPlayer), Is.True);
            Assert.AreEqual(_gameState.Players.First().Id, id);
            Assert.AreEqual(_gameState.Players.First().Name, name);
        }
    }
}
