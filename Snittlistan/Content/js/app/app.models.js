(function ($, backbone, app, undefined) {
    "use strict";
    var models = { };

    // turn model
    models.Turn = backbone.Model.extend({
        initialize: function (options) {
            this.number = this.get('number');
        },
        url: '/turns'
    });

    app.Models = models;

    var collections = { };

    // turns collection
    collections.Turns = backbone.Collection.extend({
        model: app.Models.Turn,
        url: '/turns'
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
