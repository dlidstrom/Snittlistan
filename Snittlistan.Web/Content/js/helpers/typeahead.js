$(function () {
    'use strict';

    $(':input[data-enabletypeahead]').each(function () {
        var $this = $(this);
        var url = $this.data('url');
        var options = {
            source: function (query, process) {
                return $.get(url, { q: query }, function(data) {
                    return process(data.options);
                });
            }
        };

        $this.typeahead(options);
    });
});