//setup view model
var tableVM = {
    gameActive: ko.observable(),
    card: {
        Colour: ko.observable(),
        Value: ko.observable()
    },

    players: ko.observableArray(),
    restart: ko.observable(false)
};

function updateVM(data) {
    tableVM.gameActive(data.gameActive);
    tableVM.card(data.card);
    tableVM.players(data.players);
    
    tableVM.card().value = ko.observable(data.card.value);
    tableVM.card().colour = ko.observable(data.card.colour);
}

//updateVM(tableData.table);
ko.applyBindings(tableVM);

//setup server connection
var gameHub = $.connection.gameHub;
gameHub.client.playerJoined = function (value) {
    console.log('Server called player joined, name is (' + value + ')');
    //updateVM(value.table);
    tableVM.players.push({ name: value });
};

gameHub.client.gameStarted = function (value) {
    console.log('Server called gameStarted(' + value + ')');
    //updateVM(value.table);
};

//fired on each turn
gameHub.client.turn = function (value) {
    //updateVM(value.table);
    //look for this to determine which player to animate
    console.log('turn', value);
};

//fired when a card is played
gameHub.client.cardPlayed = function (card) {
    //show newly active card
    if (card !== null) {
        tableVM.card.Colour(card.Colour);
        tableVM.card.Value(card.Value);
    }
};

gameHub.client.gameOver = function (winner) {
    //updateVM(value.table);
    alert('Game Over! ' + winner.Name + ' won the round.');
    console.log('Server called gameOver(' + value + ')');
    tableVM.restart(true);
};

$.connection.hub.start()
.done(function () {
    console.log("Now connected!");
})
.fail(function () { console.log("Could not connect!"); });

//draggable jquery plugin without jquery ui
(function ($) {
    $.fn.drags = function (opt) {

        opt = $.extend({ handle: "", cursor: "move" }, opt);

        if (opt.handle === "") {
            var $el = this;
        } else {
            var $el = this.find(opt.handle);
        }

        return $el.css('cursor', opt.cursor).on("mousedown", function (e) {
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
            $drag.css('z-index', 1000).parents().on("mousemove", function (e) {
                $('.draggable').offset({
                    top: e.pageY + pos_y - drg_h,
                    left: e.pageX + pos_x - drg_w
                }).on("mouseup", function () {
                    $(this).removeClass('draggable').css('z-index', z_idx);
                    if (e.pageY + pos_y - drg_h < 50) {
                        //$('.card').remove();
                        $('.card').addClass('slide-up');
                        $('.card').on('oanimationend animationend webkitAnimationEnd', function () {
                            $('.card').remove();
                        });
                    }
                });
            });
            e.preventDefault(); // disable selection
        }).on("mouseup", function () {
            if (opt.handle === "") {
                $(this).removeClass('draggable');
            } else {
                $(this).removeClass('active-handle').parent().removeClass('draggable');
            }
        });

    }
})(jQuery);

$('.card').drags();

