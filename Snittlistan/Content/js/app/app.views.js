(function ($, backbone, app, undefined) {
    "use strict";
    var views = { };

    // turn view
    views.Turn = backbone.View.extend({
        template: window.JST['turn-template'],
        initialize: function (options) {
            this.container = $('#main');
            options.model.bind('change', this.render, this);
        },
        render: function () {
            $(this.el).html(this.template.render(this.model.toJSON()));
            this.container.append($(this.el));
            return this;
        }
    });

    // turns view
    views.Turns = backbone.View.extend({
        render: function () {
            this.collection.each(function (turn) {
                var view = new app.Views.Turn({ model: turn });
                view.render();
            });
        }
    });
    
    // header view
    views.Header = backbone.View.extend({
        template: window.JST['header-template'],
        render: function () {
            this.$el.html(this.template.render({ title: "Home" }));
            return this.el;
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
