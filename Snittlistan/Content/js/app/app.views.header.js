(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || {};

    // header view
    // handle active page using an application-state model,
    // as suggested here:
    // http://stackoverflow.com/questions/10926071/highlighting-menu-submenu-in-accordance-with-the-rendered-view
    views.Header = backbone.View.extend({
        tagName: 'section',
        className: 'navbar navbar-fixed-top',
        template: window.JST['header-template'],
        initialize: function () {
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
            'click li#menu-players a': 'players',
            'click li#menu-turns a': 'turns',
            'click li#menu-completed a': 'completed'
        },
        players: function () {
            this.model.playersMenu();
        },
        turns: function () {
            this.model.turnsMenu();
        },
        completed: function() {
            this.model.completedMenu();
        }
    });

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
