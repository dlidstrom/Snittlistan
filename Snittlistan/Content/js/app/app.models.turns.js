(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // turn model
    models.Turn = backbone.Model.extend({
        url: '/turns'
    });

    app.Models = models;

    var collections = app.Collections || { };

    // turns collection
    collections.Turns = backbone.Collection.extend({
        model: app.Models.Turn,
        url: '/turns'
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
