$(() => {
    let options = {
        debug: 'warn',
        modules: {
        },
        placeholder: 'Skriv ett matchreferat...',
        theme: 'snow'
    };
    let quill = new Quill('[data-quill]', options);

    $('form').on('submit', onSubmit);

    function onSubmit() {
        let commentaryHtml = document.querySelector('input[name="Model.CommentaryHtml"]');
        commentaryHtml.value = quill.root.innerHTML;

        let commentary = document.querySelector('input[name="Model.Commentary"]');
        commentary.value = quill.getText();
    }
});