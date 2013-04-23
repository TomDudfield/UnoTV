$(function() {
    function playerViewModel() {
        var self = this;
        
        self.gameHub = $.connection.gameHub;

        self.gameId = ko.observable($('#gameId').val());
        self.currentPlayer = ko.observable();
        self.currentCard = ko.observable();
        self.gameReady = ko.observable();
        self.gameActive = ko.observable();
        self.gameOver = ko.observable();
        self.playerActive = ko.observable();
        self.playerName = ko.observable();
        self.points = ko.observable();
        self.winner = ko.observable();
        self.cards = ko.observableArray();

        self.init = function () {
            if (self.gameId() == "") {
                alert("Game Id missing!");
            }
        };

        self.playCard = function (item) {
            var card = ko.toJS(item);
            self.points(self.points() - card.Value);
            self.cards.remove(item);
            self.playerActive(false);
            self.gameHub.server.playCard(card)
                .fail(function (error) {
                    alert("Error! " + error);
                    console.log(error);
                });
        };
        
        self.joinGame = function () {
            if (self.playerName() == null) {
                alert("Enter you name!");
            } else {
                self.gameHub.server.join(self.playerName())
                    .done(function () {
                        self.gameReady(true);
                    })
                    .fail(function (error) {
                        alert("Error! " + error);
                        console.log(error);
                    });
            }
        };
        
        self.startGame = function () {
            self.gameHub.server.startGame()
                .fail(function (error) {
                    alert("Error! " + error);
                    console.log(error);
                });
        };
        
        self.gameHub.client.gameStarted = function () {
            self.gameOver(false);
            self.gameActive(true);
        };
        
        self.gameHub.client.deal = function (hand) {
            updateViewModel(hand);
        };
        
        self.gameHub.client.turn = function (hand) {
            self.playerActive(true);
            updateViewModel(hand);
        };
        
        self.gameHub.client.gameOver = function (winner) {
            self.winner(winner.Name);
            self.gameActive(false);
            self.gameOver(true);
        };

        self.gameHub.client.playerTurn = function (player) {
            self.currentPlayer(player);
        };

        self.gameHub.client.cardPlayed = function (card) {
            self.currentCard(card);
        };

        self.gameHub.client.gameReset = function () {
            if (self.gameActive()) {
                alert("Game has been closed!");
            }
            
            self.gameActive(false);
            self.gameOver(false);
            self.joinGame();
        };

        self.gameHub.client.error = function (error) {
            alert(error.Message);
        };
        
        function updateViewModel(hand) {
            self.points(hand.Total);
            self.cards(hand.PlayableCards);

            if (hand.PlayableCardCount === 0) {
                self.playerActive(false);
            }
        };
    }

    var vm = new playerViewModel();
    ko.applyBindings(vm);
    $.connection.hub.start()
        .done(function () {
            vm.init();
        })
        .fail(function (error) {
            alert("Error! " + error);
            console.log(error);
        });
});