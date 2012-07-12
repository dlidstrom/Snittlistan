$(function () {
    var options = { cancelSelection: true };
    $('table[data-sortable="true"]').each(function () {
        $(this).tablesorter(options);
    });
});