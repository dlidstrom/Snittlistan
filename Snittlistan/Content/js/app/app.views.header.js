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
        events: {
            'click li#menu-players a': 'players',
            'click li#menu-turns a': 'turns'
        },
        initialize: function (options) {
            _.bindAll(this);
            this.model.on('change', this.render);
            this.router = options.router;
        },
        beforeClose: function () {
            this.model.off('change', this.render);
        },
        render: function () {
            this.$el.html(this.template.render(this.model.toJSON()));
            return this;
        },
        players: function () {
            this.router.navigate('players', { trigger: true });
            return false;
        },
        turns: function () {
            this.router.navigate('turns', { trigger: true });
            return false;
        }
    });

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
