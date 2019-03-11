$(() => {
    let toolbarOptions = [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'font': [] }],
        [{ 'align': [] }],

        ['clean']                                         // remove formatting button
    ];
    let options = {
        debug: 'warn',
        modules: {
            //toolbar: toolbarOptions
        },
        placeholder: 'Compose an epic...',
        theme: 'snow'
    };
    let quill = new Quill('[data-quill]', options);

    $('form').on('submit', onSubmit);

    function onSubmit() {
        let message = document.querySelector('input[name=Message]');
        message.value = quill.root.innerHTML;
    }
});