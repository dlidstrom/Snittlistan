(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // helper for serializing animations
    // see: http://ricostacruz.com/backbone-patterns/#animation_buffer
    views.AnimationBuffer = {
        commands: [],
        add: function (fn) {
            // adds a command to the buffer, and executes it
            // if it's the only command to be run
            var commands = this.commands;
            commands.push(fn);
            if (this.commands.length == 1)
                fn(next);

            function next () {
                // moves onto the next command in the buffer
                commands.shift();
                if (commands.length) commands[0](next);
            }
        }
    };

    app.Views = views;
} ($, Backbone, window.App = window.App || {}));
