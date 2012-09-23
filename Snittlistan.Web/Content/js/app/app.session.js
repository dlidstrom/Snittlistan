(function ($, backbone, app, undefined) {
    "use strict";

    // TODO: hide internals using private variables
    app.session = _.extend({
        isAuthenticated: app.initial_data.session.isAuthenticated,
        email: app.initial_data.session.email,
        // logging in, creates a new session
        logIn: function (options) {
            var self = this;
            $.post('/api/session',
                {
                    email: options.email,
                    password: options.password,
                    remember: options.remember
                }
            ).success(function (data) {
                self.email = data.email;
                self.isAuthenticated = data.isAuthenticated;
                self.trigger('session:logInSuccess');
                console.log(JSON.stringify(data));
            }).error(function (data) {
                self.trigger('session:logInError');
                console.log(JSON.stringify(data));
            });
        },
        // logging out, destroys current session
        logOut: function () {
            var self = this;
            $.ajax({
                url: '/api/session',
                type: 'delete'
            }).success(function (data) {
                self.isAuthenticated = data.isAuthenticated;
                self.trigger('session:loggedOut');
                console.log(JSON.stringify(data));
            }).error(function (data) {
                self.trigger('session:loggedOutError');
                console.log(JSON.stringify(data));
            });
        }
    }, backbone.Events);
}($, Backbone, window.App = window.App || { }));
