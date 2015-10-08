$(document).ready(function () {
    $('.btnUser, .btnComputer').mouseenter(function () {
        $(this).animate({
            height: '+=10px'
        });
    });
    $('.btnUser, .btnComputer').mouseleave(function () {
        $(this).animate({
            height: '-=10px'
        });
    });
    $('.btnUser, .btnComputer').click(function () {
        $('.btnUser').slideUp("slow");
        $('.btnComputer').slideUp("slow");
    });
});