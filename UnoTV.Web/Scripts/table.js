var tableData = {
    "table": {
        "players": [{
            "name": "Luke",
            "active": true
        },
        {
            "name": "Tim",
            "active": false
        },
        {
            "name": "Pete",
            "active": false
        },
        {
            "name": "Tom",
            "active": false
        }],
        "gameActive": true,
        "card": {
            "value": 1,
            "colour": "red"
        }
    }
};

var tableData2 = {
    "table": {
        "players": [{
            "name": "Luke",
            "active": false
        },
        {
            "name": "Tim",
            "active": true
        },
        {
            "name": "Pete",
            "active": false
        },
        {
            "name": "Tom",
            "active": false
        }],
        "gameActive": true,
        "card": {
            "value": 1,
            "colour": "red"
        }
    }
};

//setup view model
var tableVM = {
    gameActive: ko.observable(),
    card: ko.observable(),
    players: ko.observableArray()
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

$.connection.hub.start()
.done(function () {
    console.log("Now connected!");
    gameHub.server.startGame()
    .done(function (result) {
        console.log('game started', result);
        //updateVM(result.table);
        updateVM(tableData.table);
        document.querySelector('.card').className += ' slide';
    })
    .fail(function (error) {
        console.log(error);
    });

    gameHub.client.playerJoined = function (value) {
        console.log('Server called player joined, name is (' + value + ')');
        //updateVM(value.table);
    };

    gameHub.client.gameStarted = function (value) {
        console.log('Server called gameStarted(' + value + ')');
        //updateVM(value.table);
    };

    //fired on each turn
    gameHub.client.turn = function (value) {
        //updateVM(value.table);
    };

    //fired when a card is played
    gameHub.client.cardPlayed = function (value) {
        //show newly active card
        //updateVM(value.table);
    };

    gameHub.client.gameOver = function (value) {
        //updateVM(value.table);
        alert('Game Over!');
        console.log('Server called gameStarted(' + value + ')');
    };
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

