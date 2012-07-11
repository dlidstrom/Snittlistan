$(function ($, backbone, _, handlebars, app, undefined) {
    "use strict";
    var collections = { };

    // turns collection
    collections.Turns = backbone.Collection.extend({
        model: app.Models.Turn,
        url: '/turns'
    });

    app.Collections = collections;
}($, Backbone, _, Handlebars, window.App || { }));
