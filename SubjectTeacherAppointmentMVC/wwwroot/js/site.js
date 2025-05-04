// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/** Подтверждение действия */
function ActionConfirmation(text) {
    if (confirm(text) == false) {
        event.preventDefault();
    }
}



/** Увеличение номера в именах элементов */
function incrementTagName(tag, inc) {
    let tagNameRegex = new RegExp("\\[[0-9]*\\]");
    let matches = tag.name?.match(tagNameRegex);
    if (matches) {
        matches.forEach(match => {
            let number = Number(match.substring(1, match.length - 1));
            number += inc;
            tag.name = tag.name.replace(match, `[${number}]`);
        });
    }
    tag.childNodes?.forEach(n => incrementTagName(n, inc));
}