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
            completedMenu: ''
        },
        playersMenu: function () {
            this.set({
                playersMenu: 'active',
                turnsMenu: '',
                completedMenu: ''
            });
        },
        turnsMenu: function () {
            this.set({
                playersMenu: '',
                turnsMenu: 'active',
                completedMenu: ''
            });
        },
        completedMenu: function () {
            this.set({
                playersMenu: '',
                turnsMenu: '',
                completedMenu: 'active'
            });
        }
    });

    app.Models = models;
}($, Backbone, window.App = window.App || { }));
