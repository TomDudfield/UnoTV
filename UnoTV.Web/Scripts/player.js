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
        "playerName": "{}",
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
    gameHub.server.startGame()
         .done(function (result) {
             console.log(result);
         })
         .fail(function (error) {
             console.log(error);
         });

})
.fail(function () { console.log("Could not Connect!"); });


//method for server to call
gameHub.client.gameStarted = function (value) {
    console.log('Server called gameStarted(' + value + ')');
};

var playerVM = {
    gameActive: ko.observable(),
    playerActive: ko.observable(),
    playerName: ko.observable(),
    points: ko.observable(),
    currentCard: ko.observable(),
    cards: ko.observableArray(),
    playCard: function(item) {
        var card = ko.toJS(item);
        console.log(card);
    }
};

function updateVM(data) {
    playerVM.playerActive(data.playerActive);
    playerVM.playerName(data.playerName);
    playerVM.points(data.points);
    playerVM.currentCard(data.currentCard);
    playerVM.cards(data.cards);
    ko.utils.arrayForEach(playerVM.cards(), function (item) {
        item.value = ko.observable(item.value);
        item.colour = ko.observable(item.colour);
        item.playable = ko.observable(item.playable);
        item.pickedUp = ko.observable(item.pickedUp);
    });
    playerVM.currentCard().value = ko.observable(data.currentCard.value);
    playerVM.currentCard().colour = ko.observable(data.currentCard.colour);
};

updateVM(hand.hand);

ko.applyBindings(playerVM);