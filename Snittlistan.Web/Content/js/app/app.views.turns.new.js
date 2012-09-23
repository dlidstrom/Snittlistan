(function ($, backbone, app, undefined) {
    "use strict";

    var views = app.Views || { };

    // new turn view
    views.NewTurn = backbone.View.extend({
        className: 'modal hide fade',
        template: window.JST['modal-template'],
        initialize: function (options) {
            _.bindAll(this);
            this.options = {
                title: options.seasonStart + '&minus;' + options.seasonEnd,
                paragraph: [
                    'Skapa en ny omgång genom att fylla i detaljerna för en match nedan.',
                    ' Du kan senare lägga till flera matcher till samma omgång.'].join(),
                controls: [
                    {
                        select: true,
                        name: 'turn',
                        label: 'Omgång',
                        autofocus: true,
                        options: options.availableTurns
                    },
                    {
                        input: true,
                        type: 'text',
                        name: 'team',
                        label: 'Lag',
                        required: true
                    },
                    {
                        input: true,
                        type: 'text',
                        name: 'opponent',
                        label: 'Motståndare',
                        required: true
                    },
                    {
                        input: true,
                        type: 'text',
                        name: 'location',
                        label: 'Plats',
                        required: true
                    },
                    {
                        input: true,
                        type: 'date',
                        name: 'date',
                        label: 'Datum',
                        required: true
                    },
                    {
                        input: true,
                        type: 'time',
                        name: 'time',
                        label: 'Klockslag',
                        required: true
                    }
                ]
            };
        },
        events: {
            'submit form': 'submit',
            'click button[data-type=hide]': 'hide',
            'shown': 'shown',
            'hidden': 'hidden'
        },
        render: function () {
            this.$el.html(this.template.render(this.options));
            return this;
        },
        submit: function (event) {
            // get form values
            var form = $(event.target);
            this.trigger('save', {
                turn: form.find('[name=turn]').val(),
                team: form.find('[name=team]').val(),
                opponent: form.find('[name=opponent]').val(),
                location: form.find('[name=location]').val(),
                date: form.find('[name=date]').val(),
                time: form.find('[name=time]').val()
            });
            this.hide();
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
        },
        shown: function () {
            var el = this.$el.find('select[autofocus]');
            el.focus();
            return false;
        }
    });

    app.Views = views;
}($, Backbone, window.App = window.App || { }));
