
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
            var newPlayer = new Player();
            _gameState.AddPlayer(newPlayer);
            Assert.That(_gameState.Players.Contains(newPlayer), Is.True);
        }
    }
}
