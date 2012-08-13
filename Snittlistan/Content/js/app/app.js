// @reference app.models.js
// @reference app.views.header.js
// @reference app.views.turns.js
(function ($, backbone, app, undefined) {
    "use strict";

    //var turns = new Turns();
    //turns.fetch();

    // application router, create as instantiated singleton
    app.router = new (backbone.Router.extend({
        initialize: function () {
            _.bindAll(this);
            // application state keeps track of current page
            app.app_state = new app.Models.AppState();
            // place a header first in the body element
            this.header = new app.Views.Header({ model: app.app_state });
            this.header.bind('players', this.menuOnPlayers);
            this.header.bind('turns', this.menuOnTurns);
            this.header.bind('completed', this.menuOnCompleted);
            $("body").prepend(this.header.render().el);

            // initialize turns from server
            var turns = new app.Collections.Turns(app.initial_data);
            app.turns = turns;
        },
        routes: {
            'v2/players': 'players',
            'v2/turns': 'turns',
            'v2/completed': 'completed',
            '*other': 'main'
        },
        // main entry point
        main: function () {
            this.turns();
        },
        menuOnPlayers: function () {
            this.navigate('v2/players');
        },
        menuOnTurns: function () {
            this.navigate('v2/turns');
        },
        menuOnCompleted: function () {
            this.navigate('v2/completed');
        },
        // show players
        players: function () {
            app.app_state.playersMenu();
        },
        // show coming turns
        turns: function () {
            var turns_view = new app.Views.Turns({ collection: app.turns });
            $("#main").html(turns_view.render().el);
            app.app_state.turnsMenu();
        },
        // show completed turns
        completed: function () {
            app.app_state.completedMenu();
        }
    }))();

    $(function () {
        // Because hash-based history in Internet Explorer
        // relies on an <iframe>, be sure to only call start()
        // after the DOM is ready.
        backbone.history.start({ pushState: true });
    });
}($, Backbone, window.App = window.App || { }));
