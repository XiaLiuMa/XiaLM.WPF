
$(function () {

    $('.container').show();

    $('body').on('click', '.city-list p', function () {
        parent.nowcity=$(this).text();
        parent.setting.SetCity();
    });

    $('body').on('click', '.letter a', function () {
        var s = $(this).html();
        $(window).scrollTop($('#' + s + '1').offset().top);
        $("#showLetter span").html(s);
        $("#showLetter").show().delay(800).hide(0);
    });

     $('body').on('onMouse', '.showLetter span', function () {
        $("#showLetter").show().delay(800).hide(0);
    });

})