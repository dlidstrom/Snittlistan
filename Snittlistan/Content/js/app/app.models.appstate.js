(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // application state
    models.AppState = backbone.Model.extend({
        url: '/appstate',
        defaults: {
            page: 'turns',
            title: 'Snittlistan V2',
            playersMenu: '',
            turnsMenu: '',
            resultsMenu: '',
            addTurnMenu: ''
        },
        addTurnMenu: function () {
            this.turnsMenu();
        },
        playersMenu: function () {
            this.set({
                playersMenu: 'active',
                turnsMenu: '',
                resultsMenu: ''
            });
        },
        turnsMenu: function () {
            this.set({
                playersMenu: '',
                turnsMenu: 'active',
                resultsMenu: ''
            });
        },
        resultsMenu: function () {
            this.set({
                playersMenu: '',
                turnsMenu: '',
                resultsMenu: 'active'
            });
        }
    });

    app.Models = models;
}($, Backbone, window.App = window.App || { }));
