/// <reference path="jquery-1.6.4-vsdoc.js" />
/// <reference path="jquery-ui.js" />

$(function () {
    $(":input[data-autocomplete]").each(function () {
        $(this).autocomplete({ source: $(this).attr("data-autocomplete") });
    });
})