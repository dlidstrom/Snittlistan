$.validator.addMethod('requiredifexists',
    function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];

        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue =
          (targetvalue == null ? '' : targetvalue).toString();

        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue =
            controltype === 'checkbox' ?
            control.attr('checked').toString() :
            control.val();

        // if the condition is true, reuse the existing 
        // required field validator functionality
        if (targetvalue === actualvalue)
            return $.validator.methods.required.call(
              this, value, element, parameters);

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'requiredifexists',
    ['dependentproperty'],
    function (options) {
        options.rules['requiredifexists'] = {
            dependentproperty: options.params['dependentproperty']
        };
        options.messages['requiredifexists'] = options.message;
    });
if (typeof (jQuery) !== "undefined" && typeof (jQuery.validator) !== "undefined") {

    (function ($) {
        $.validator.addMethod('requiredifexists', function (value, element, parameters) {
            var id = '#' + parameters['dependentProperty'];

            // get the target value (as a string, as that's what actual value will be)
            var targetvalue = parameters['targetValue'];
            targetvalue = (targetvalue == null ? '' : targetvalue).toString();
            
            // get the actual value of the target control
            var actualvalue = $(id).val();

            // if the condition is true, reuse the existing required field validator functionality
            if (actualvalue !== null)
                return $.validator.methods.required.call(this, value, element, parameters);

            return true;
        });

    })(jQuery);

}
