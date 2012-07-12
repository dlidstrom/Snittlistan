(function ($, window, app, undefined) {
    "use strict";

    app.TemplateCache = {
        get: function (name) {
            var template = this.templates[name];
            if (!template) {
                var tmpl = $(selector).html();
                if (typeof tmpl === undefined || tmpl === null) {
                    throw new Error("TemplateCache: " + selector + " not found");
                }
                template = handlebars.compile(tmpl);
                this.templates[selector] = template;
            }

            return template;
        }
    };
}($, window, window.App = window.App || { }));
