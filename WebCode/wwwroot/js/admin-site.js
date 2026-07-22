// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(window).on('load', function () { $("#global-loader").fadeOut(); });
 
var showLoader = function (form) {
    if ($(form).length && !$(form).valid()) { return false; }
    $("#global-loader").fadeIn();
    return true;
};


var hideLoader = function () { $("#global-loader").fadeOut(); };
$(function () {
    $("#global-loader").hide();
    setTimeout(function () { $("#global-loader").fadeOut(); }, 5000);
});


function previewImage(event, previewId) {
    const reader = new FileReader();
    reader.onload = function (e) {
        document.getElementById(previewId).src = e.target.result;
    };
    reader.readAsDataURL(event.target.files[0]);
}


function showZoom(src) {
    var modal = document.getElementById("zoomModal");
    var zoomImg = document.getElementById("zoomedImage");
    zoomImg.src = src;
    modal.style.display = "flex";
}

document.getElementById("zoomModal").addEventListener("click", function () {
    this.style.display = "none";
});



 
const onlyInteger = e => (e.which || e.keyCode) > 47 && (e.which || e.keyCode) < 58;
const onlyDecimal = e => (e.which || e.keyCode) === 46 ? !e.target.value.includes('.') : (e.which || e.keyCode) > 47 && (e.which || e.keyCode) < 58;
const formatINR = v => isFinite(v = Number(v)) ? v.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '';
const parseDate = d => d ? d.split("-").reverse().join("/") : null;