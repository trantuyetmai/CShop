$(document).ready(function () {
    setup();
    init();
});

function setup() {
    $('#index-publisher td span').unbind('click');
    $('#index-publisher td span').click(function () {
        var val = $(this).text();
        $(this).text('');
        $(this).parent().append('<textarea>{0}</textarea>'.f(val));
        $(this).siblings('textarea').focus();
        setup();
    });

    $('#index-publisher td textarea ').unbind('change');
    $('#index-publisher td textarea').change(function () {
        var val = $(this).val();
        var tr = $(this).parent().parent();
        $(this).parent().children('span').text(val);
        $(this).parent().children('textarea').remove();
        
        update(tr);
    });

    $('#index-publisher td textarea').unbind('blur');
    $('#index-publisher td textarea').blur(function () {
        var val = $(this).val();
        var tr = $(this).parent().parent();
        $(this).parent().children('span').text(val);
        $(this).parent().children('textarea').remove();
        
    });
}

function update(tr) {
    console.log(tr);
    $.ajax('/Admin/Publisher/Edit', {
        type: 'POST',
        datatype: 'json',
        data: {
            Id: tr.attr('data-id'),
            Name: tr.children('td:nth-child(1)').text(),
            Detail: tr.children('td:nth-child(2)').text()
        },
        success: function (res) {
            $('#index-publisher').children('.alert-info').text(res.error);
        },
        error: function (jqXHR, status, error) {
            $('#index-publisher').children('.alert-info').text(error);
        }
    });
}
function init() {

}