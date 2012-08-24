// Modal views with jQuery manual event handling
$(function (app) {
    $('#login-form').submit(function (event) {
        // stop form from submitting normally
        event.preventDefault();
        // get form values
        var form = $('#login-form');
        var e = form.find('input[name=email]').val();
        var p = form.find('input[name=password]').val();
        var r = form.find('input[name=remember]').val();
        app.session.logIn({
            email: e,
            password: p,
            remember: r
        });
    });
    
    // close login-modal when session has been successfully created
    app.session.on('session:logInSuccess', function () {
        var login_modal = $('#login-modal');
        login_modal.modal('hide');
    });
}(window.App));
