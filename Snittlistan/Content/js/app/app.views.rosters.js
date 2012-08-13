(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || {};

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

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
