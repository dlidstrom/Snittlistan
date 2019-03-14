$(() => {
    var toolbarOptions = [
        ['bold', 'italic', 'underline'],        // toggled buttons
        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        ['clean']                                         // remove formatting button
    ];
    $('form[data-quill-data]').each(function (i, e) {
        var $e = $(e);
        var quillData = $e.data('quill-data');
        var options = {
            debug: 'warn',
            modules: {
                toolbar: toolbarOptions
            },
            placeholder: quillData.Placeholder,
            theme: 'snow'
        };
        var $quillElement = $e.find('[data-quill]');
        var quill = new Quill($quillElement[0], options);

        $e.on('submit', onSubmit);

        $e.css({
            visibility: 'visible'
        });

        function onSubmit() {
            var commentaryHtml = document.querySelector('input[name="' + quillData.HtmlFieldName + '"]');
            commentaryHtml.value = quill.root.innerHTML;

            var commentary = document.querySelector('input[name="' + quillData.StringFieldName + '"]');
            commentary.value = quill.getText();
        }
    });
});