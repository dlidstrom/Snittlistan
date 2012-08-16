(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // roster model
    models.Roster = backbone.Model.extend({
        url: '/rosters'
    });

    app.Models = models;

    var collections = app.Collections || { };

    // rosters collection
    collections.Rosters = backbone.Collection.extend({
        model: app.Models.Roster,
        url: '/rosters'
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
