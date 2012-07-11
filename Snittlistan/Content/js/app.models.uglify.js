$(function ($, backbone, _, handlebars, app, undefined) {
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
}($, Backbone, _, Handlebars, window.App = window.App || { }));
