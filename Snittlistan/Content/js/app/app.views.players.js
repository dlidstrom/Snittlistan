(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // players-list view
    views.PlayersList = backbone.View.extend({
        className: 'row-fluid',
        template: window.JST['player-list-template'],
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function() {
            var json = this.model.toJSON();
            var tmpl = this.template.render(json);
            this.$el.html(tmpl);
            return this;
        }
    });

    // player view
    views.Player = backbone.View.extend({
        tagName: 'tr',
        template: window.JST['player-template'],
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            return this;
        }
    });

    // players view
    views.Players = backbone.View.extend({
        tagName: 'table',
        className: 'table table-condensed',
        initialize: function () {
            _.bindAll(this);
        },
        render: function () {
            this.collection.each(this.addPlayer);
            return this;
        },
        addPlayer: function (player) {
            var view = new app.Views.Player({ model: player });
            this.$el.append(view.render().el);
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
