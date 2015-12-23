$(document).ready(function () {
    setup();
    init();
});

function setup() {
    
    $('#index-product td span').unbind('click');
    $('#index-product td span').click(function () {
        var val = $(this).text();
        $(this).text('');
        $(this).parent().append('<textarea>{0}</textarea>'.f(val));
        $(this).parent().children('textarea').focus();
        setup();
    });

    $('#index-product td textarea').unbind('change');
    $('#index-product td textarea').change(function () {
        var val = $(this).val();
        var td = $(this).parent();
        $(this).remove();
        td.children('span').text(val);

        update(td.parent());
    });

    $('#index-product td textarea').unbind('blur');
    $('#index-product td textarea').blur(function () {
        var val = $(this).val();
        var td = $(this).parent();
        $(this).remove();
        td.children('span').text(val);
    });

    $('#index-product td img').unbind('click');
    $('#index-product td img').click(function () {
        var file = $('#index-product input:file');
        file.attr('data-rowid', $(this).parent().parent().index());
        file.click();
    });

    $('#index-product input:file').unbind('change');
    $('#index-product input:file').change(function (ev) {
        var f = ev.target.files[0];
        var fr = new FileReader();
        var file = $(this);
        fr.onload = function (ev2) {
            var rowid = file.attr('data-rowid');
            $('#index-product tr:nth-child({0}) td img'.f(parseInt(rowid)+1)).attr('src', ev2.target.result);
        };
        fr.readAsDataURL(f);
        uploadImage(file.attr('data-rowid'));
    });

    $('#add-product input:file').unbind('change');
    $('#add-product input:file').change(function (ev) {
        var f = ev.target.files[0];
        var fr = new FileReader();
        var file = $(this);
        fr.onload = function (ev2) {
            var rowid = file.attr('data-rowid');
            $('#add-product img').attr('src', ev2.target.result);
        };
        fr.readAsDataURL(f);
    });

    $('div.btn-group ul li').unbind('click');
    $('div.btn-group ul li').click(function () {
        var val = $(this).text();
        var key = $(this).attr('data-id');
        var btn = $(this).parent().parent();
        btn.children('button').html('{0} <span class="caret"></span>'.f(val));
        $('#add-product input[name={0}]'.f(btn.attr('data-inputname'))).val(key);    
    });
}
function uploadImage(rowid) {
    var tr = $('#index-product tr:nth-child({0})'.f(parseInt(rowid) + 1));
    console.log($('#index-product input:file'));
    var data = new FormData();
    data.append('Image', $('#index-product input:file').get(0).files[0]);
    console.log($('#index-product input:file').get(0).files[0]);
    data.append('Id', tr.attr('data-id'));
    $.ajax('/Admin/Product/UploadImage', {
        data: data,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (res) {
            $('#index-product div.alert-info').text(res.error);
        },
        error: function (jqXHR, status, error) {
            $('#index-product div.alert-info').text(error);

        }
    });
    console.log('ajax');
}

function update(tr)
{
    $.ajax('/Admin/Product/Edit', {
        type: 'POST',
        datatype: 'json',
        data: {
            Id: tr.attr('data-id'),
            Name: tr.children('td:nth-child(1)').text(),
            Cost: tr.children('td:nth-child(2)').text(),
            Detail: tr.children('td:nth-child(3)').text(),
        },
        success: function (res) {
            $('#index-product div.alert-info').text(res.error);
        },
        error: function (jqXHR, status, error) {
        $('#index-product div.alert-info').text(error);
    }
    });
}

function init() {
}