(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // application state
    models.AppState = backbone.Model.extend({
        url: '/appstate',
        defaults: {
            page: 'turns',
            title: 'Snittlistan V2',
            menuPlayersState: '',
            menuTurnsState: 'active',
            menuCompletedState: ''
        },
        playersMenu: function () {
            this.set({
                menuPlayersState: 'active',
                menuTurnsState: '',
                menuCompletedState: ''
            });
            this.trigger('players');
        },
        turnsMenu: function () {
            this.set({
                menuPlayersState: '',
                menuTurnsState: 'active',
                menuCompletedState: ''
            });
            this.trigger('turns');
        },
        completedMenu: function () {
            this.set({
                menuPlayersState: '',
                menuTurnsState: '',
                menuCompletedState: 'active'
            });
            this.trigger('completed');
        }
    });

    app.Models = models;
}($, Backbone, window.App = window.App || { }));
