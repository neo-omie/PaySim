// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showLoader() {
    document.getElementById('loader').style.display = 'block';
    document.getElementById('overlay').style.display = 'block';
    document.getElementById('loadButton').disabled = true;

    setTimeout(function () {
        document.getElementById('loader').style.display = 'none';
        document.getElementById('overlay').style.display = 'none';
        document.getElementById('loadButton').disabled = false;
    }, 6000);
}