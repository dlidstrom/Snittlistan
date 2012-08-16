(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // player model
    models.Player = backbone.Model.extend({
        url: '/players'
    });

    app.Models = models;

    var collections = app.Collections || { };

    // playerss collection
    collections.Players = backbone.Collection.extend({
        model: app.Models.Player,
        url: '/players'
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
