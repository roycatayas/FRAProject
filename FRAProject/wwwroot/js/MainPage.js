$(document).ready(function () {
    
    var sidebar = $("#sidebar");
    var sidebarbtn = $("#show-sidebar");
    

    $(window).scroll(function() {
        var windowpos = $(window).scrollTop();
  
        if (windowpos >= 10) {
            sidebar.css("top", 0);
            sidebar.css("height", "calc(100%)");
            sidebarbtn.css("top", 10);
        } else {
            sidebar.css("top", 81);
            sidebar.css("height", "calc(100% - 81px)");
            sidebarbtn.css("top", 90);
        }
    });


});