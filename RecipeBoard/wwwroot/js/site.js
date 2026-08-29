// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function updateCharCount() {
    var textarea = document.getElementById("instructionsField");
    var counter = document.getElementById("charCount");
    if (textarea == null || counter == null) {
        return;
    }
    counter.innerHTML = textarea.value.length + " characters";
}

function confirmDelete() {
    return confirm("This will permanently delete the recipe. Continue?");
}
