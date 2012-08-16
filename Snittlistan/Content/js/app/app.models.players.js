(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // players-list model
    models.PlayersList = backbone.Model.extend({
        url: '/players'
    });

    // player model
    models.Player = backbone.Model.extend({
        url: '/players'
    });

    app.Models = models;

    var collections = app.Collections || { };

    // players collection
    collections.Players = backbone.Collection.extend({
        model: app.Models.Player,
        url: '/players'
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
