// @reference app.models.appstate.js
// @reference app.models.players.js
// @reference app.models.turns.js
// @reference app.views.header.js
// @reference app.views.turns.js
// @reference app.views.players.js
(function ($, backbone, app, undefined) {
    "use strict";

    //var turns = new Turns();
    //turns.fetch();

    // define a custom close method
    // if special clean-up is necessary,
    // create a beforeClose function
    // see http://lostechies.com/derickbailey/2011/09/15/zombies-run-managing-page-transitions-in-backbone-apps/
    backbone.View.prototype.close = function () {
        if (this.beforeClose)
            this.beforeClose();
        this.remove();
        this.unbind();
    };

    // application router, create as instantiated singleton
    app.router = new (backbone.Router.extend({
        routes: {
            'v2/turns': 'turns',
            'v2/completed': 'completed',
            'v2/players': 'players',
            '*other': 'turns'
        },
        initialize: function (options) {
            _.bindAll(this);
            // save references to models
            this.appState = options.app_state;
            this.turns = options.turns;
            this.players = options.players;
            // place a header first in the body element
            this.header = new app.Views.Header({ model: options.app_state });
            $("body").prepend(this.header.render().el);
        },
        // handles views
        showView: function (selector, view) {
            if (this.currentView)
                this.currentView.close();
            $(selector).html(view.render().el);
            this.currentView = view;
            return view;
        },
        // routes
        // show coming turns
        turns: function () {
            // adjust appState
            this.appState.turnsMenu();
            var turns_view = new app.Views.Turns({ collection: this.turns });
            this.showView("#main", turns_view);
        },
        // show completed turns
        completed: function () {
            // adjust appState
            this.appState.completedMenu();
        },
        // show players
        players: function () {
            // adjust appState
            this.appState.playersMenu();
            var players_view = new app.Views.PlayersList({ model: this.players });
            this.showView("#main", players_view);
        }
    }))({
        // initialize data here, and keep inside the router
        // application state keeps track of current page
        app_state: new app.Models.AppState(),
        turns: new app.Collections.Turns(app.initial_data.turns),
        players: new app.Models.PlayersList({ players: app.initial_data.players })
    });

    $(function () {
        // Because hash-based history in Internet Explorer
        // relies on an <iframe>, be sure to only call start()
        // after the DOM is ready.
        backbone.history.start({ pushState: true });
    });
}($, Backbone, window.App = window.App || { }));
