(function ($, backbone, app, undefined) {
    "use strict";
    var views = {};

    // roster view
    views.Roster = backbone.View.extend({
        className: 'span4',
        template: window.JST['roster-template'],
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            return this;
        }
    });

    // rosters view
    views.Rosters = backbone.View.extend({
        className: 'row-fluid',
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.collection.each(this.addRoster);
            return this;
        },
        addRoster: function (roster) {
            var view = new app.Views.Roster({ model: roster });
            this.$el.append(view.render().el);
        }
    });

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

    // header view
    // handle active page using an application-state model,
    // as suggested here:
    // http://stackoverflow.com/questions/10926071/highlighting-menu-submenu-in-accordance-with-the-rendered-view
    views.Header = backbone.View.extend({
        tagName: 'section',
        className: 'navbar navbar-fixed-top',
        template: window.JST['header-template'],
        initialize: function (options) {
            _.bindAll(this);
        },
        render: function () {
            this.$el.html(this.template.render({ title: "Home" }));
            return this.el;
        }
    });

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
