$(function ($, backbone, _, handlebars, app_global, undefined) {
    "use strict";

    app_global.Models = app_global.Models || { };
    app_global.Models.Turn = backbone.Model.extend({
        initialize: function (options) {
            this.number = this.get('number');
        },
        url: '/turns'
    });

    app_global.Collections = app_global.Collections || { };
    app_global.Collections.Turns = backbone.Collection.extend({
        model: app_global.Models.Turn,
        url: '/turns'
    });

    // turn view
    app_global.Views = app_global.Views || { };
    app_global.Views.Turn = backbone.View.extend({
        template: handlebars.compile('<h2>{{number}}</h2>'),
        initialize: function (options) {
            this.container = $('#main');
            options.model.bind('change', this.render, this);
        },
        render: function () {
            $(this.el).html(this.template(this.model.toJSON()));
            this.container.append($(this.el));
            return this;
        }
    });

    app_global.Views.Turns = backbone.View.extend({
        render: function () {
            this.collection.each(function (turn) {
                var view = new app_global.Views.Turn({ model: turn });
                view.render();
            });
        }
    });

    //var turns = new Turns();
    //turns.fetch();
    var turns = new app_global.Collections.Turns();
    turns.reset(
        [
            { number: 1 },
            { number: 2 }
        ]);
    app_global.turns = turns;
    
    // render
    var turns_view = new app_global.Views.Turns({ collection: turns });
    turns_view.render();
}($, Backbone, _, Handlebars, window.App = window.App || { }));
