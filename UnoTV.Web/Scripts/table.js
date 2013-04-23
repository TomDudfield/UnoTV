$(function() {
    function tableViewModel() {
        var self = this;
        
        self.gameHub = $.connection.gameHub;

        self.gameId = ko.observable($('#gameId').val());
        self.players = ko.observableArray();
        self.gameStarted = ko.observable(false);
        self.winner = ko.observable();
        self.card = {
            Colour: ko.observable(),
            Value: ko.observable(),
            Label: ko.observable(),
        };

        self.init = function () {
            self.gameHub.server.newTable(self.gameId())
                .fail(function (error) {
                    alert("Error! " + error);
                    console.log(error);
                });
        };
        
        self.gameHub.client.playerJoined = function (player, id, total, cardCount) {
            self.players.push({ name: ko.observable(player), id: ko.observable(id), total: ko.observable(total), cardCount: ko.observable(cardCount), active: ko.observable(false) });
        };

        self.gameHub.client.gameStarted = function () {
            self.players.removeAll();
            self.winner(null);
            self.gameStarted(true);
        };

        self.gameHub.client.playerTurn = function (player, id, total, cardCount) {
            ko.utils.arrayForEach(self.players(), function (p) {
                if (p.id() == id) {
                    p.total(total);
                    p.cardCount(cardCount);
                    p.active(true);
                }
            });
        };

        self.gameHub.client.cardPlayed = function (card) {
            if (card != null) {
                ko.utils.arrayForEach(self.players(), function (p) {
                    if (p.active()) {
                        p.total(p.total() - card.Value);
                        p.cardCount(p.cardCount() - 1);
                        p.active(false);
                    }
                });
                self.card.Colour(card.Colour);
                self.card.Value(card.Value);
                self.card.Label(card.Label);
            }
        };

        self.gameHub.client.cardPickup = function (player, id) {
            ko.utils.arrayForEach(self.players(), function (p) {
                if (p.active() && p.id() == id) {
                    p.active(false);
                }
            });
        };

        self.gameHub.client.gameReset = function () {
            self.gameStarted(false);
            self.players.removeAll();
            self.card.Colour(null);
            self.card.Value(null);
            self.card.Label(null);
        };

        self.gameHub.client.gameOver = function (winner) {
            self.winner(winner.Name);
            self.gameStarted(false);
        };
    };

    var vm = new tableViewModel();
    ko.applyBindings(vm);
    $.connection.hub.start()
        .done(function () {
            vm.init();
        })
        .fail(function (error) {
            alert("Error! " + error);
            console.log(error);
        });

    $('.card').drags();
});

//draggable jquery plugin without jquery ui
(function($) {
    $.fn.drags = function(opt) {

        opt = $.extend({ handle: "", cursor: "move" }, opt);

        if (opt.handle === "") {
            var $el = this;
        } else {
            var $el = this.find(opt.handle);
        }

        return $el.css('cursor', opt.cursor).on("mousedown", function(e) {
            if (opt.handle === "") {
                var $drag = $(this).addClass('draggable');
            } else {
                var $drag = $(this).addClass('active-handle').parent().addClass('draggable');
            }
            var z_idx = $drag.css('z-index'),
                drg_h = $drag.outerHeight(),
                drg_w = $drag.outerWidth(),
                pos_y = $drag.offset().top + drg_h - e.pageY,
                pos_x = $drag.offset().left + drg_w - e.pageX;
            $drag.css('z-index', 1000).parents().on("mousemove", function(e) {
                $('.draggable').offset({
                    top: e.pageY + pos_y - drg_h,
                    left: e.pageX + pos_x - drg_w
                }).on("mouseup", function() {
                    $(this).removeClass('draggable').css('z-index', z_idx);
                    if (e.pageY + pos_y - drg_h < 50) {
                        //$('.card').remove();
                        $('.card').addClass('slide-up');
                        $('.card').on('oanimationend animationend webkitAnimationEnd', function() {
                            $('.card').remove();
                        });
                    }
                });
            });
            e.preventDefault(); // disable selection
        }).on("mouseup", function() {
            if (opt.handle === "") {
                $(this).removeClass('draggable');
            } else {
                $(this).removeClass('active-handle').parent().removeClass('draggable');
            }
        });

    };
})(jQuery);