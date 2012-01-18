/// <reference path="jquery-1.6.4-vsdoc.js" />

$(function () {
    var options = { cancelSelection: true };
    $('table[data-sortable="true"]').each(function () {
        $(this).tablesorter(options);
    });
});