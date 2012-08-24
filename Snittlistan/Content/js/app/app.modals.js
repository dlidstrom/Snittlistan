// Modal views
$(function (app) {
    var modals = {
        $el: $('#modal-root'),
        show: function (modal) {
            var el = modal.render().el;
            this.$el.html(el);
            modal.$el.modal('show');
        }
    };
    app.Modals = modals;
}(window.App));
