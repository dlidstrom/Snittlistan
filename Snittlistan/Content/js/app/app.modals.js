// Modal views
$(function (app) {
    var modals = {
        $el: $('#modal-root'),
        show: function (modal) {
            // hide any open collapse menus
            $('.nav-collapse').collapse('hide');
            // render modal
            var el = modal.render().el;
            this.$el.html(el);
            modal.$el.modal('show');
        }
    };
    app.Modals = modals;
}(window.App));
