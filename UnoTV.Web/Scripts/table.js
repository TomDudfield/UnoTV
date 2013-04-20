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
            "name": "Ash",
            "active": false
        },
        {
            "name": "Brock",
            "active": false
        },
        {
            "name": "Misty",
            "active": false
        },
        {
            "name": "Pikachu",
            "active": false
        }],
        "gameActive": true,
        "card": {
            "value": 1,
            "colour": "red"
        }
    }
};

var tableVM = {
    gameActive: ko.observable(),
    card: ko.observable(),
    players: ko.observableArray()
};

function updateVM(data) {
    tableVM.gameActive(data.gameActive);
    tableVM.card(data.card);
    tableVM.players(data.players);
    
    ko.utils.arrayForEach(tableVM.players(), function (item) {
        item.name = ko.observable(item.name);
        item.active = ko.observable(item.active);
    });

    tableVM.card().value = ko.observable(data.card.value);
    tableVM.card().colour = ko.observable(data.card.colour);
}

updateVM(tableData.table);
ko.applyBindings(tableVM);

function makeRequest(request) {
    /// <summary></summary>
    /// <param name="request" type="Object">method, success, url</param>

    request.method = request.method || 'GET';

    var xhr = new XMLHttpRequest();
    xhr.open(request.method, request.url);

    xhr.onload = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                request.success();
            }
        }
    };

    xhr.onerror = request.error(xhr.statusText);

    xhr.send(null);
}

makeRequest({
    url: '',
    success: function () {

    },
    error: function () {

    }
});