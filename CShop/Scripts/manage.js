

$(document).ready(function (e) {
    $('#changePass-form').fadeOut(0);
    $('#userinfo-form').fadeOut(0);
    $('#changeinfo-form').fadeOut(0);
})
$("#userinfo-btn").click(function () {
    $('#userinfo-form').fadeIn(0);
    $('#changePass-form').fadeOut(0);
    $('#changeinfo-form').fadeOut(0);
})
$("#changePass-btn").click(function () {
    $('#changePass-form').fadeIn(0);
    $('#userinfo-form').fadeOut(0);
    $('#changeinfo-form').fadeOut(0);
})
$("#changeinfo-btn").click(function () {
    $('#changePass-form').fadeOut(0);
    $('#userinfo-form').fadeOut(0);
    $('#changeinfo-form').fadeIn(0);
})