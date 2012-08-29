// @reference app.views.rosters.js
// @reference app.views.turns.new.js
(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // turn view
    views.Turn = backbone.View.extend({
        template: window.JST['turn-template'],
        className: 'turn',
        initialize: function () {
            _.bindAll(this);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            var rosters_collection = this.model.get('rosters');
            var rosters_view = new app.Views.Rosters({ collection: rosters_collection });
            this.$el.append(rosters_view.render().el);
            return this;
        }
    });

    // turns view
    views.Turns = backbone.View.extend({
        template: window.JST['turns-template'],
        admin_template: window.JST['turn-admin-template'],
        className: 'row-fluid',
        events: {
            'click button#add-turn-btn': 'newTurn'
        },
        initialize: function () {
            _.bindAll(this);
            this.collection.on('add', this.addTurn);
        },
        beforeClose: function () {
            this.collection.off('add', this.addTurn);
        },
        render: function () {
            var json = $.extend(this.options, {
                isAuthenticated: app.session.isAuthenticated
            });
            this.$el.html(this.template.render(json));
            this.collection.each(this.addTurn);
            return this;
        },
        addTurn: function (turn, options, context) {
            var view = new app.Views.Turn({ model: turn });
            var $view_el = view.render().$el;
            // if creating, place at correct index
            if (context && context.index !== undefined) {
                var selector = 'div.turn:eq(' + context.index + ')';
                $(selector, this.$el).before($view_el);
            } else {
                // not creating, simply append
                this.$el.append($view_el);
            }
            turn.on('remove', view.remove, view);
        },
        newTurn: function () {
            // available turns are turns that don't already exist
            var turn_pluck = this.collection.pluck('turn');
            turn_pluck.splice(0, 0, _.range(1, 23));
            var available_turns = _.without.apply(null, turn_pluck);
            var modal = new app.Views.NewTurn({
                // TODO: Current season
                seasonStart: 2012,
                seasonEnd: 2013,
                availableTurns: available_turns
            });
            modal.on('save', this.createRoster);
            app.Modals.show(modal);
            return false;
        },
        createRoster: function (data) {
            /*
            data: {"number":"1",
            "team":"Fredrikshof A",
            "opponent":"AIK C",
            "place":"Bowl-O-Rama",
            "date":"2012-09-08",
            "time":"10:00"}
            */
            var turn = new app.Models.Turn({
                turn: Number(data.turn),
                startDate: data.date,
                endDate: data.date
            });
            this.collection.add(turn);
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
