$(document).ready(function () {
    setup();
    init();
});

function setup()
{
    $('#index-account td span').unbind('click');
    $('#index-account td span').click(function () {
        if ($(this).parent().index() < 1 || $(this).parent().index() > 4) return;
        var val = $(this).text();
        $(this).text('');
        $(this).parent().append('<textarea>{0}</textarea>'.f(val));
        $(this).parent().children('textarea').focus();
        setup();
    });

    $('#index-account td textarea').unbind('change');
    $('#index-account td textarea').change(function () {
        var val = $(this).val();
        var td = $(this).parent();
        td.children('textarea').remove();
        td.children('span').text(val);
        update(td.parent());
    });

    $('#index-account td textarea').unbind('blur');
    $('#index-account td textarea').blur(function () {
        var val = $(this).val();
        var td = $(this).parent();
        td.children('textarea').remove();
        td.children('span').text(val);
    });
}

function update(tr) {
    $.ajax('/Admin/Account/Edit', {
        type: 'POST',
        datatype: 'json',
        data: {
                UserName: tr.children('td:nth-child(1)').text(),
                Name: tr.children('td:nth-child(2)').text(),
                Email:tr.children('td:nth-child(3)').text(),
                Phone :tr.children('td:nth-child(4)').text(),
                Address:tr.children('td:nth-child(5)').text()},
        success: function (res) {
            if (res.error != "") $('div.alert-info').text(res.error);       
        },
        error: function (jqXHR, status, error) {
            $('div.alert-info').text(error);
        }
    });
}

function init() { }