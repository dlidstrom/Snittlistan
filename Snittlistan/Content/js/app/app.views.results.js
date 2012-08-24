(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // results view
    views.Results = backbone.View.extend({
        className: 'row-fluid',
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.$el.append('<h3>Resultat</h3>');
            return this;
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
