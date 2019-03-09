$.fn.datepicker.language['sv'] = {
    days: ['Söndag', 'Månday', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag'],
    daysShort: ['Sön', 'Mån', 'Tis', 'Ons', 'Tor', 'Fre', 'Lör'],
    daysMin: ['Sö', 'Må', 'Ti', 'On', 'To', 'Fr', 'Lö'],
    months: ['Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'Oktober', 'November', 'December'],
    monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dec'],
    today: 'Idag',
    clear: 'Rensa',
    dateFormat: 'yyyy-mm-dd',
    timeFormat: 'hh:ii',
    firstDay: 1
};
$(function () {
    window.Waypoints.debug(true).intercept('a');
    $('[data-auth-menu-toggle]').on('click', function () {
        setTimeout(function () {
            $('[data-nav-collapse]').css({
                height: 'auto'
            });
        });
    });
    $('[data-datepicker]').datepicker({
        language: 'sv',
        minutesStep: 10
    });
});
