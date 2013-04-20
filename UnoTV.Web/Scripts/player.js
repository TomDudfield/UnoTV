﻿
var gameHub = $.connection.gameHub;
$.connection.hub.start()
.done(function () {

})
.fail(function () { console.log("Could not Connect!"); });

//methods for server to call
gameHub.client.gameStarted = function (value) {
    playerVM.gameOver(false);
    playerVM.gameActive(true);
};
gameHub.client.deal = function (hand) {
    updateVM(hand);
};
gameHub.client.turn = function (hand, current) {
    playerVM.playerActive(true);
    updateVM(hand, current);
};
gameHub.client.gameOver = function (winner) {
    playerVM.winner(winner.Name);
    playerVM.gameActive(false);
    playerVM.gameOver(true);
}

var playerVM = {
    gameReady: ko.observable(),
    gameActive: ko.observable(),
    gameOver: ko.observable(),
    playerActive: ko.observable(),
    playerName: ko.observable(),
    points: ko.observable(),
    winner: ko.observable(),
    currentCard: ko.observable(),
    cards: ko.observableArray(),
    playCard: function(item) {
        var card = ko.toJS(item);
        playerVM.cards.remove(item);
        playerVM.playerActive(false);
        gameHub.server.playCard(card)
            .fail(function (error) {
                alert(error);
            });
    },
    joinGame: function () {
        gameHub.server.join(playerVM.playerName())
            .done(function (result) {
                playerVM.gameReady(true);
            })
            .fail(function (error) {
                alert("A game is alreay in progress!");
            });
    },
    startGame: function () {
        gameHub.server.startGame()
             .fail(function (error) {
                 alert("A game is alreay in progress!");
             });
    }
};

function updateVM(data, current) {
    var pointsTally = 0;

    if (current) {
        playerVM.currentCard(current);
    }
    playerVM.cards(data.PlayableCards);

    var unplayableCardCount = 0;

    ko.utils.arrayForEach(playerVM.cards(), function (item) {
        pointsTally += item.Value;

        if (item.Playable === false) {
            unplayableCardCount++;
        }
    });
    playerVM.points(pointsTally); //calculate from sum of all values

    if (unplayableCardCount === playerVM.cards().length) {
        playerVM.playerActive(false);
    }
};


ko.applyBindings(playerVM);