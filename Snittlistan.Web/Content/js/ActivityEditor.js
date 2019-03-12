$(() => {
    let options = {
        debug: 'warn',
        modules: {
        },
        placeholder: 'Beskriv aktiviteten...',
        theme: 'snow'
    };
    let quill = new Quill('[data-quill]', options);

    $('form').on('submit', onSubmit);

    function onSubmit() {
        let message = document.querySelector('input[name=Message]');
        message.value = quill.root.innerHTML;
    }
});