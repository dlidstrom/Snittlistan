// @reference app.views.rosters.js
(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // turn view
    views.Turn = backbone.View.extend({
        className: 'row-fluid',
        template: window.JST['turn-template'],
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            var rosters_collection = new app.Collections.Rosters(this.model.get('rosters'));
            var rosters_view = new app.Views.Rosters({ collection: rosters_collection });
            this.$el.append(rosters_view.render().el);
            return this;
        }
    });

    // turns view
    views.Turns = backbone.View.extend({
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.collection.each(this.addTurn);
            return this;
        },
        addTurn: function (turn) {
            var view = new app.Views.Turn({ model: turn });
            this.$el.append(view.render().el);
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
