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
            "active": true
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
    
    tableVM.card().value = ko.observable(data.card.value);
    tableVM.card().colour = ko.observable(data.card.colour);
}

updateVM(tableData.table);
ko.applyBindings(tableVM);
