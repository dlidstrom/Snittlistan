(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // roster model
    models.Roster = backbone.Model.extend({
        url: '/rosters',
        set: function(attributes, options) {
            /*if (attributes.rosters !== undefined
                && !(attributes.rosters instanceof app.Collections.Rosters)) {
                attributes.rosters = new JobSummaryList(attributes.rosters);
            }*/
            console.log('Roster.set: ' + JSON.stringify(attributes) + '\n' + JSON.stringify(options));
            return Backbone.Model.prototype.set.call(this, attributes, options);
        },
    });

    app.Models = models;

    var collections = app.Collections || { };

    // rosters collection
    collections.Rosters = backbone.Collection.extend({
        model: app.Models.Roster,
        url: '/rosters',
        set: function(attributes, options) {
            /*if (attributes.rosters !== undefined
                && !(attributes.rosters instanceof app.Collections.Rosters)) {
                attributes.rosters = new JobSummaryList(attributes.rosters);
            }*/
            console.log('Rosters.set: ' + JSON.stringify(attributes) + '\n' + JSON.stringify(options));
            return Backbone.Model.prototype.set.call(this, attributes, options);
        },
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
