 
$(function () {
    // Dropdown-submenu toggle for mobile: clicking parent toggles its submenu
    //$(document).on('click', '.dropdown-submenu > a', function (e) {
    //    const $submenu = $(this).next('.dropdown-menu');
    //    if (window.innerWidth < 992) {
    //        e.preventDefault();
    //        e.stopPropagation();
    //        $('.dropdown-submenu > .dropdown-menu').not($submenu).hide();
    //        $submenu.toggle();
    //    }
    //});
    updateMstCartUI();
});


//$(window).on('load', function () { $("#global-loader").fadeOut(); });


//var showLoader = function (form) {
//    if ($(form).length && !$(form).valid()) { return false; }
//    $("#global-loader").fadeIn();
//    return true;
//};


//var hideLoader = function () { $("#global-loader").fadeOut(); };
//$(function () {
//    $("#global-loader").hide();
//    setTimeout(function () { $("#global-loader").fadeOut(); }, 5000);
//});

//function showRightSideCart() {
//    const off = document.getElementById('cartOffcanvas');
//    const bsOff = bootstrap.Offcanvas.getOrCreateInstance(off);
//    bsOff.show();
//}

$(document).ready(function () {
    var url = window.location.pathname.toLowerCase();
    $('.navbar-nav a').each(function () {
        var link = $(this).attr('href');
        if (link && url === link.toLowerCase()) {
            $('.navbar-nav a').removeClass('active');
            $(this).addClass('active');
            $(this).closest('.dropdown').find('.nav-link').addClass('active');
        }
    });

    $('.dropdown-item').each(function () {
        var link = $(this).attr('href').toLowerCase();
        if (url === link) {
            $(this).addClass('active');
            $(this).closest('.dropdown').find('.nav-link').addClass('active');
        }
    });
});


/* NAVBAR SHADOW */
window.addEventListener("scroll", function () {
    let nav = document.querySelector(".navbar");
    nav.classList.toggle("scrolled", window.scrollY > 50);
});

/* THEME SWITCH */
function setTheme(color) {
    document.getElementById("theme-style").href = "/css/theme-" + color + ".css";
}

/* SCROLL TOP BUTTON */
let scrollBtn = document.getElementById("scrollTop");
window.onscroll = function () {
    if (document.body.scrollTop > 300 || document.documentElement.scrollTop > 300)
        scrollBtn.style.display = "block";
    else
        scrollBtn.style.display = "none";
};
scrollBtn.onclick = function () {
    window.scrollTo({ top: 0, behavior: "smooth" });
};

$(window).on('load', function () { $("#global-loader").fadeOut(); });

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
 

$(document).on('input', '#mst_login_id, #mst_password, #user_id, #password, #tran_password, #mobile, #email_id, #account_no, #ifsc, #pan_no, #aadhar_no, #gst_no, #pin_code', function () {
    this.value = this.value.replace(/\s/g, '');
});
 

$(document).ready(function () {
    // If current page is /cart/view-cart
    var currentPath = window.location.pathname.toLowerCase();
    if (currentPath === '/cart/view-cart') {
        $('#openCartBtn').removeAttr('data-bs-toggle').removeAttr('data-bs-target').removeAttr('aria-controls').css('pointer-events', 'none').css('opacity', '0.6');   // optional faded look
    }
});


function mst_changeQty(cid, delta) {
    const input = document.getElementById(`mst-qty-${cid}`);
    let currentQty = parseInt(input.value) || 1;
    let newQty = currentQty + delta;
    newQty = Math.min(Math.max(newQty, 1), 999999);
    input.value = newQty;
    AddToCart(cid, 0, newQty, updateMstCartUI);
}
 
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



const formatINR = v => isFinite(v = Number(v)) ? v.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '';
const parseDate = d => d ? d.split("-").reverse().join("/") : null;
