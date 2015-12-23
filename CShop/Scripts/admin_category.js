$(document).ready(function () {
    setup();
    init();
});

function setup() {
    $('#index-category td span').unbind('click');
    $('#index-category td span').click(function () {
        var val = $(this).text();
        $(this).text('');
        $(this).parent().append('<textarea>{0}</textarea>'.f(val));
        $(this).siblings('textarea').focus();
        setup();
    });

    $('#index-category td textarea ').unbind('change');
    $('#index-category td textarea').change(function () {
        var val = $(this).val();
        var tr = $(this).parent().parent();
        $(this).parent().children('span').text(val);
        $(this).parent().children('textarea').remove();
        
        update(tr);
    });

    $('#index-category td textarea').unbind('blur');
    $('#index-category td textarea').blur(function () {
        var val = $(this).val();
        var tr = $(this).parent().parent();
        $(this).parent().children('span').text(val);
        $(this).parent().children('textarea').remove();
        
    });
}

function update(tr) {
    console.log(tr);
    $.ajax('/Admin/Category/Edit', {
        type: 'POST',
        datatype: 'json',
        data: {
            Id: tr.attr('data-id'),
            Name: tr.children('td:nth-child(1)').text()
        },
        success: function (res) {
            $('#index-category').children('.alert-info').text(res.error);
        },
        error: function (jqXHR, status, error) {
            $('#index-category').children('.alert-info').text(error);
        }
    });
}
function init() {

}