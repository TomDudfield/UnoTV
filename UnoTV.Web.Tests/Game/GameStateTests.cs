
using System;
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

        [Test]
        public void Start_LessTwoPlayersThrowsException()
        {
            Assert.Throws<Exception>(_gameState.Start);
        }

        [Test]
        public void Start_SetsInitialValues()
        {
            _gameState.AddPlayer(new Player("{E6AB01BE-E623-492C-8390-01786604DD14}", "Bob"));
            _gameState.AddPlayer(new Player("{18DFB92D-7EF8-45F2-87AD-72FBC9ABE683}", "Tim"));

            _gameState.Start();

            Assert.AreEqual(_gameState.Players.First(), _gameState.CurrentPlayer);
            Assert.AreEqual(_gameState.PlayedCards.First(), _gameState.CurrentCard);



            Assert.IsFalse(_gameState.Cards.Contains(_gameState.CurrentCard));
        }

        [Test]
        public void PlayCard_SetsCurrentCardAndPlayedCard()
        {
            _gameState.AddPlayer(new Player("{E6AB01BE-E623-492C-8390-01786604DD14}", "Bob"));
            _gameState.AddPlayer(new Player("{18DFB92D-7EF8-45F2-87AD-72FBC9ABE683}", "Tim"));
            _gameState.Start();
            var card = _gameState.Cards.First();

            _gameState.PlayCard(card);

            Assert.AreEqual(card, _gameState.CurrentCard);
            Assert.IsTrue(_gameState.PlayedCards.Contains(card));
        }
    }
}
