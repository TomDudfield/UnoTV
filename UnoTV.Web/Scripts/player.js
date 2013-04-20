var hand = {
    "hand": {
        "cards": [{
            "value": 3,
            "colour": "blue",
            "playable": true,
            "pickedUp": false
        },
        {
            "value": 1,
            "colour": "red",
            "playable": false,
            "pickedUp": false
        },
        {
            "value": 7,
            "colour": "red",
            "playable": false,
            "pickedUp": false
        },
        {
            "value": 5,
            "colour": "yellow",
            "playable": true,
            "pickedUp": false
        },
        {
            "value": 3,
            "colour": "red",
            "playable": false,
            "pickedUp": false
        },
        {
            "value": 8,
            "colour": "green",
            "playable": false,
            "pickedUp": false
        },
        {
            "value": 9,
            "colour": "green",
            "playable": false,
            "pickedUp": true
        }],
        "playerActive": true,
        "playerName": "",
        "points": 20,
        "currentCard": {
            "value": 5,
            "colour": "blue"
        }
    }
};


var gameHub = $.connection.gameHub;
$.connection.hub.start()
.done(function () {
    console.log("Now connected!");
})
.fail(function () { console.log("Could not Connect!"); });

//methods for server to call
gameHub.client.gameStarted = function (value) {
    playerVM.gameActive(true);
};
gameHub.client.deal = function (hand) {
    //updateVM(value);
    console.log(hand);
    updateVM(hand);
};
gameHub.client.turn = function (hand, current) {
    //updateVM(value);
    console.log(hand);
    updateVM(hand, current);
    playerVM.playerActive(true);
};

var playerVM = {
    gameReady: ko.observable(),
    gameActive: ko.observable(),
    playerActive: ko.observable(),
    playerName: ko.observable(),
    points: ko.observable(),
    currentCard: ko.observable(),
    cards: ko.observableArray(),
    playCard: function(item) {
        var card = ko.toJS(item);
        gameHub.server.playCard(card)
            .done(function (result) {
                console.log(card);
                console.log('card played ' + result);
                playerVM.cards.remove(item);
                playerVM.playerActive(false);
            })
            .fail(function (error) {
                console.log('card not played ' + error);
            });
    },
    joinGame: function () {
        gameHub.server.join(playerVM.playerName())
            .done(function (result) {
                console.log('joined ' + result);
                playerVM.gameReady(true);
            })
            .fail(function (error) {
                console.log('not joined ' + error);
            });
    },
    startGame: function () {
        gameHub.server.startGame()
             .done(function (result) {
                 console.log('started ' + result);
             })
             .fail(function (error) {
                 console.log('not started ' + error);
             });
    }
};

function updateVM(data, current) {
    //playerVM.playerActive(data.playerActive);
    //playerVM.playerName(data.playerName);
    var pointsTally = 0;
    console.log(current);
    if (current) {
        playerVM.currentCard(current);
    }
    playerVM.cards(data.PlayableCards);
    console.log(data.PlayableCards);
    ko.utils.arrayForEach(playerVM.cards(), function (item) {
        pointsTally += item.Value;
    });
    playerVM.points(pointsTally); //calculate from sum of all values
};


ko.applyBindings(playerVM);