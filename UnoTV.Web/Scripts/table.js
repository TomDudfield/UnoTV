$(function() {
    function tableViewModel() {
        var self = this;
        
        self.gameHub = $.connection.gameHub;

        self.players = ko.observableArray();
        self.gameStarted = ko.observable(false);
        self.card = {
            Colour: ko.observable(),
            Value: ko.observable(),
            Label: ko.observable(),
        };

        self.init = function () {
            // todo some checks
        };
        
        self.resetGame = function() {
            self.gameHub.server.resetGame()
                .fail(function (error) {
                    alert("Error! " + error);
                    console.log(error);
                });
        };
        
        self.gameHub.client.playerJoined = function (player, id, total) {
            self.players.push({ name: ko.observable(player), id: ko.observable(id), total: ko.observable(total), active: ko.observable(false) });
        };

        self.gameHub.client.gameStarted = function () {
            self.gameStarted(true);
        };

        self.gameHub.client.playerTurn = function (player, id, total) {
            self.players.remove(function (item) { return item.id() == id; });
            ko.utils.arrayForEach(self.players(), function (p) {
                p.active(false);
            });
            self.players.push({ name: ko.observable(player), id: ko.observable(id), total: ko.observable(total), active: ko.observable(true) });
        };

        self.gameHub.client.cardPlayed = function (card) {
            if (card != null) {
                self.card.Colour(card.Colour);
                self.card.Value(card.Value);
                self.card.Label(card.Label);
            }
        };

        self.gameHub.client.gameReset = function () {
            self.gameStarted(false);
            self.players.removeAll();
            self.card.Colour(null);
            self.card.Value(null);
            self.card.Label(null);
        };

        self.gameHub.client.gameOver = function (winner) {
            ko.utils.arrayForEach(self.players(), function (p) {
                p.active(false);
            });
            alert('Game Over! ' + winner.Name + ' won the round.');
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