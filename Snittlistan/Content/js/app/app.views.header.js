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
