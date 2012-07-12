(function ($, handlebars, app, undefined) {
    "use strict";

    app.TemplateCache = {
        get: function (selector) {
            this.templates = this.templates || { };

            var template = this.templates[selector];
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
}($, Handlebars, window.App = window.App || { }));
