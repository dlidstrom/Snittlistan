$(function ($, backbone, app, undefined) {
    "use strict";
    var collections = { };

    // turns collection
    collections.Turns = backbone.Collection.extend({
        model: app.Models.Turn,
        url: '/turns'
    });

    app.Collections = collections;
}($, Backbone, window.App || { }));
