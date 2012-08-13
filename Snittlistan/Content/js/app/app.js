// @reference app.models.js
// @reference app.views.header.js
// @reference app.views.turns.js
(function ($, backbone, app, undefined) {
    "use strict";

    //var turns = new Turns();
    //turns.fetch();

    // application router
    var router = backbone.Router.extend({
        initialize: function () {
            $("body").prepend(new app.Views.Header().render());
            var turns = new app.Collections.Turns(app.initial_data);
            app.turns = turns;
        },
        routes: {
            "turns": "turns",
            "*other": "main"
        },
        main: function () {
            this.turns();
            alert("Main");
        },
        turns: function () {
            var turns_view = new app.Views.Turns({ collection: app.turns, el: $("#main") });
            turns_view.render();
        }
    });

    app.Router = router;
    app.router = new app.Router();
    $(function () {
        // Because hash-based history in Internet Explorer
        // relies on an <iframe>, be sure to only call start()
        // after the DOM is ready.
        backbone.history.start({ pushState: true });
    });
}($, Backbone, window.App = window.App || { }));
