// @reference app.session.js
// @reference app.views.login.js
(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // header view
    // handle active page using an application-state model,
    // as suggested here:
    // http://stackoverflow.com/questions/10926071/highlighting-menu-submenu-in-accordance-with-the-rendered-view
    views.Header = backbone.View.extend({
        tagName: 'section',
        className: 'navbar navbar-inverse',
        template: window.JST['header-template'],
        events: {
            'click li a': 'clicked',
            'click li a i': 'clickedImage',
            'click button#menu-login': 'login',
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
            app.session.off('all', this.render);
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
        clicked: function (event) {
            var href = $(event.target).attr('href');
            this.router.navigate(href, { trigger: true });
            return false;
        },
        clickedImage: function (event) {
            var href = $(event.target).parent().attr('href');
            this.router.navigate(href, { trigger: true });
            return false;
        },
        login: function () {
            app.Modals.show(new app.Views.Login());
        },
        logOut: function () {
            app.session.logOut();
            return false;
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
