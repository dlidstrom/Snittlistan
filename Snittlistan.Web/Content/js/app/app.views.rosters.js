(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // roster view
    views.Roster = backbone.View.extend({
        className: 'span4',
        template: window.JST['roster-template'],
        initialize: function (options) {
            _.bindAll(this);
            this.model.on('change', this.render);
        },
        beforeClose: function () {
            this.model.off('change', this.render);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            return this;
        },
        events: {
            'click .can-play': 'canPlay',
            'click .cannot-play': 'cannotPlay'
        },
        canPlay: function () {
            var declined_names = this.model.get('declinedNames');
            var declined_new = declined_names == '' ? [] : declined_names.split(', ');
            declined_new.pop();
            this.model.set({
                declinedCount: declined_new.length,
                declinedNames: declined_new.join(', '),
                declinedClass: declined_new.length ? 'warning' : 'success'
            });
        },
        cannotPlay: function () {
            var declined_names = this.model.get('declinedNames');
            var declined_new = declined_names == '' ? [] : declined_names.split(', ');
            declined_new.push('Daniel Lidström');
            this.model.set({
                declinedCount: declined_new.length,
                declinedNames: declined_new.join(', '),
                declinedClass: declined_new.length ? 'warning' : 'success'
            });
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
}($, Backbone, window.App = window.App || { }));
