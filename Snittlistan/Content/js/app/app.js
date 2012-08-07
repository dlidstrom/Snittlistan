// @reference app.models.js
// @reference app.views.js
(function ($, backbone, app, undefined) {
    "use strict";

    //var turns = new Turns();
    //turns.fetch();
    var turns = new app.Collections.Turns();
    turns.reset(app.initial_data);
    app.turns = turns;

    // render
    var turns_view = new app.Views.Turns({ collection: turns });
    turns_view.render();

    // application router
    var router = backbone.Router.extend({
        initialize: function () {
            $("#header").html(new app.Views.Header().render());
        },
        routes: {
            "": "home"
        },
        home: function () {
        }
    });

    app.Router = router;
    app.router = new app.Router();
    backbone.history.start();
}($, Backbone, window.App = window.App || { }));
