/// <reference path="product-detail.js" />
/// <reference path="product-detail.js" />
// JavaScript Document
var AjaxUrl;
var erroMess;



function setup()
{
    $('#1').on('click', function () {
       

        $.ajax({
            url: Product/Details,
            type: 'POST',
            datatype: 'Json',
            data: { page: '1'},
            success: function (data) {
                

            },
            error: function (data) {
                erroMess.text('Da xay ra loi');
            }
        });

       
        if (nameUser == "") {
            erroMess.text("Vui lòng nhập tên của bạn!");
        }

        if (textCm == "") {
            erroMess.text("Vui lòng nhập bình luận của bạn!");
        }
    });

    $('.edit-function').unbind('click');
    $('.edit-function').click(function () {
        var postId = $(this).parents('.comment-box-user').attr('data-postid');
        var Content = $(this).siblings('.comment-box-text').text();

        //Hien textarea		    
        $("<textarea class='comment-box-text'  onblur='UnfocusEditText({0})'>{1}</textarea>".format(postId, Content)).insertAfter(
            $(this).siblings('.comment-box-text')
        );
        $('textarea.comment-box-text').focus();
        //An div
        $(this).siblings('div.comment-box-text').hide(1);

    });

    $('.delete-function').unbind('click');
    $('.delete-function').click(function () {
        var answer = confirm("Bạn muốn xóa bình loạn này?");
        if (answer) {
            var postId = $(this).parent('.comment-box-user').attr('data-postid');
            $.ajax({
                url: AjaxUrl + '/Delete',
                type: 'POST',
                datatype: 'json',
                data: { postid: postId },
                success: function (data) {
                    if (data.Error == "")
                        $('.comment-box-user[data-postid={0}]'.format(postId)).remove();
                }
            });
        }
    });
}

function UnfocusEditText(postId)
{
    box = $('.comment-box-user[data-postid={0}]'.format(postId));
    var Content = $(box).children('textarea.comment-box-text').val();

    //delete textarea
    $(box).children('textarea.comment-box-text').remove();

    //Show div
    $(box).children('div.comment-box-text').show(1);

    //Thuc hien request		    
    $.ajax({
        url: AjaxUrl + '/Edit',
        type: 'POST',
        datatype: 'json',
        data: { postid: postId, content: Content },
        success: function (data) {
            if (data.Error == "")
                $(box).children('.comment-box-text').text(Content);
        }
    });
}