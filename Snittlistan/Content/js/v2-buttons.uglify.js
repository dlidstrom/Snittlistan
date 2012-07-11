$(function ($) {
    "use strict";

    $("button").click(function () {
        if ($(this).hasClass("active")) {
            // active when clicked
            $(this).removeClass("btn-warning");
            $(this).addClass("btn-inverse");
        } else {
            // inactive when clicked
            $(this).removeClass("btn-inverse");
            $(this).addClass("btn-warning");
        }
    });

    $("[data-content]").popover({
        delay: { hide: 5000 },
        placement: 'left'
    });
}(jQuery));
