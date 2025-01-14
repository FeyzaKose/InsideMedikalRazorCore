$(document).ready(function () {

    "use strict";
    /*==============================================================
  // back to top js
  ==============================================================*/

    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 600) {
            $('#top').addClass('show');
        } else {
            $('#top').removeClass('show');
        }
    });


    $('#top').on('click', function () {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });
    /*==============================================================
   // toggler js
   ==============================================================*/

    $("button.navbar-toggler").on('click', function () {
        $(".main-menu-area").addClass("active");
        $(".mm-fullscreen-bg").addClass("active");
        $("body").addClass("hidden");
    });

    $(".close-box").on('click', function () {
        $(".main-menu-area").removeClass("active");
        $(".mm-fullscreen-bg").removeClass("active");
        $("body").removeClass("hidden");
    });

    $(".mm-fullscreen-bg").on('click', function () {
        $(".main-menu-area").removeClass("active");
        $(".mm-fullscreen-bg").removeClass("active");
        $("body").removeClass("hidden");
    });

  
  

    /*==============================================================
    // header sticky
    ==============================================================*/
    $(window).scroll(function () {
        var sticky = $('.header-main-area'),
            scroll = $(window).scrollTop();
        if (scroll >= 150) {
            sticky.addClass('is-sticky');
        }
        else {
            sticky.removeClass('is-sticky');
        }
    });

});

/*==============================================================
// signalR hub connection
==============================================================*/
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/adaKurumsalHub")
    .build();

connection.on("CacheUpdated", function (message) {
    console.log("Cache updated:", message);
    if (message.ModelName === "Category") {
        // Category modeli için güncelleme iþlemleri
        console.log("Category cache updated");
        // Kategori güncelleme iþlemleri
    } else if (message.ModelName === "Product") {
        // Product modeli için güncelleme iþlemleri
        console.log("Product cache updated");
        // Ürün güncelleme iþlemleri
    }
    // Diðer modeller için ek iþlemler
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

// Dile göre gruba katýlma
function joinLanguageGroup(language) {
    connection.invoke("JoinLanguageGroup", language).catch(function (err) {
        return console.error(err.toString());
    });
}

// Grubu terk etme
function leaveLanguageGroup(language) {
    connection.invoke("LeaveLanguageGroup", language).catch(function (err) {
        return console.error(err.toString());
    });
}