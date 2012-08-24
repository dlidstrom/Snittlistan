// @reference app.session.js
(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // login view
    views.Login = backbone.View.extend({
        className: 'modal hide fade',
        template: window.JST['login-modal-template'],
        initialize: function () {
            _.bindAll(this);
            app.session.on('session:logInSuccess', this.hide);
        },
        beforeClose: function () {
            app.session.off('session:logInSuccess', this.hide);
        },
        events: {
            'submit form': 'submit',
            'click button[data-type=hide]': 'hide',
            'hidden': 'hidden'
        },
        render: function () {
            this.$el.html(this.template.render());
            return this;
        },
        submit: function (event) {
            // get form values
            var form = $(event.target);
            var e = form.find('input[name=email]').val();
            var p = form.find('input[name=password]').val();
            var r = form.find('input[name=remember]').val();
            app.session.logIn({
                email: e,
                password: p,
                remember: r
            });
            return false;
        },
        // start hiding process
        hide: function () {
            this.$el.modal('hide');
            return false;
        },
        // called when modal has been faded out completely
        hidden: function () {
            this.close();
            return false;
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
