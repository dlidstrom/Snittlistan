// @reference app.models.rosters.js
(function ($, backbone, app, undefined) {
    "use strict";
    var models = app.Models || { };

    // turn model
    models.Turn = backbone.Model.extend({
        url: '/api/turns',
        set: function(attributes, options) {
            /*if (attributes.rosters !== undefined
                && !(attributes.rosters instanceof app.Collections.Rosters)) {
                attributes.rosters = new JobSummaryList(attributes.rosters);
            }*/
            console.log('Turn.set: ' + JSON.stringify(attributes) + '\n' + JSON.stringify(options));
            attributes.rosters = new app.Collections.Rosters(attributes.rosters);
            return Backbone.Model.prototype.set.call(this, attributes, options);
        },
        parse: function (response) {
            // response: {"number":"1",
            //  "team":"Fredrikshof A",
            //  "opponent":"AIK C",
            //  "place":"Bowl-O-Rama",
            //  "date":"2012-09-08",
            //  "time":"10:00"}
            /*
            console.log('Turn.parse: ' + JSON.stringify(response));
            var roster = new app.Models.Roster({
                team: response.team,
                location: response.location,
                opponent: response.opponent,
                date: response.date
            });
            var rosters = new app.Collections.Rosters([roster]);
            return {
                id: response.id,
                turn: response.number,
                startDate: response.date,
                endDate: response.date,
                rosters: rosters
            };
            */
            return response;
        }
    });

    app.Models = models;

    var collections = app.Collections || { };

    // turns collection
    collections.Turns = backbone.Collection.extend({
        model: app.Models.Turn,
        url: '/api/turns',
        comparator: function (turn) {
            return turn.get('turn');
        },
        set: function(attributes, options) {
            /*if (attributes.rosters !== undefined
                && !(attributes.rosters instanceof app.Collections.Rosters)) {
                attributes.rosters = new JobSummaryList(attributes.rosters);
            }*/
            console.log('Turns.set: ' + JSON.stringify(attributes) + '\n' + JSON.stringify(options));
            return Backbone.Model.prototype.set.call(this, attributes, options);
        }
    });

    app.Collections = collections;
}($, Backbone, window.App = window.App || { }));
