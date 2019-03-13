$(() => {
    let toolbarOptions = [
        ['bold', 'italic', 'underline'],        // toggled buttons
        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        ['clean']                                         // remove formatting button
    ];
    $('form[data-quill-data]').each((_, e) => {
        let $e = $(e);
        let quillData = $e.data('quill-data');
        let options = {
            debug: 'warn',
            modules: {
                toolbar: toolbarOptions
            },
            placeholder: quillData.Placeholder,
            theme: 'snow'
        };
        let $quillElement = $e.find('[data-quill]');
        let quill = new Quill($quillElement[0], options);

        $e.on('submit', onSubmit);

        $e.css({
            visibility: 'visible'
        });

        function onSubmit() {
            let commentaryHtml = document.querySelector(`input[name="${quillData.HtmlFieldName}"]`);
            commentaryHtml.value = quill.root.innerHTML;

            let commentary = document.querySelector(`input[name="${quillData.StringFieldName}"]`);
            commentary.value = quill.getText();
        }
    });
});