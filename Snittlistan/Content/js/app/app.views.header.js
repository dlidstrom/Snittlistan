// @reference app.session.js
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
            'click li#menu-turns a': 'turns',
            'click button#menu-logout': 'logOut'
        },
        initialize: function (options) {
            _.bindAll(this);
            this.model.on('change', this.render);
            this.router = options.router;
            app.session.on('all', this.render);
        },
        beforeClose: function () {
            this.model.off('change', this.render);
            app.auth.off('change', this.render);
        },
        render: function () {
            var combined_model = this.model.toJSON();
            combined_model.session = {
                isAuthenticated: app.session.isAuthenticated,
                email: app.session.email
            };
            this.$el.html(this.template.render(combined_model));
            return this;
        },
        players: function () {
            this.router.navigate('players', { trigger: true });
            return false;
        },
        turns: function () {
            this.router.navigate('turns', { trigger: true });
            return false;
        },
        logOut: function () {
            app.session.logOut();
            return false;
        }
    });

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
