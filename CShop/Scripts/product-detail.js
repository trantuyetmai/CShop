/// <reference path="product-detail.js" />
/// <reference path="product-detail.js" />
// JavaScript Document
var AjaxUrl;
var erroMess;

$(document).ready(function(){ 
    // First, checks if it isn't implemented yet.
    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                  ? args[number]
                  : match
                ;
            });
        };
    }

    setup();
   
	erroMess = $('#error-messeage');
	AjaxUrl = $('button[name=button-comment]').attr('data-url');
	
	RemoveFromCart();
	AddtoCart();
	
});

function setup()
{
    $('button[name=button-comment]').unbind('click');

    $('button[name=button-comment]').on('click', function () {
        var nameUser = $('#comment-name').val();
        var textCm = $('#comment-area').val();
        var productId = $('#comment-box-content').attr('data-productid');
        if (nameUser.length == 0 || textCm.length == 0) {
            erroMess.text("Chưa điền đủ thông tin");
            return;
        }

        // Thêm 1 comment mới
        $.ajax({
            url: AjaxUrl + '/Post',
            type: 'POST',
            datatype: 'Json',
            data: { productid: productId, name: nameUser, content: textCm },
            success: function (data) {
                if (data.Error == "") {
                    erroMess.text("");
                    cmBox = document.createElement('div');                   
                    $(cmBox).addClass('comment-box-user').insertAfter('#list-comment > .title');
                    $(cmBox).attr('data-postid', data.PostId);
                    userName = document.createElement('div');
                    $(userName).addClass('comment-box-username').appendTo(cmBox);
                    $(userName).text(nameUser);
                    cmText = document.createElement('div');
                    $(cmText).addClass('comment-box-text').appendTo(cmBox);
                    $(cmText).text(textCm);
                    $(cmBox).append("<div class='comment-function edit-function'>Sửa | </div>");
                    $(cmBox).append("<div class='comment-function delete-function'>Xóa</div>");

                    // Gọi tới hàm phân trang để load lại danh sách comment
                    $.ajax({
                        url: '/Comment/Paging',
                        type: 'POST',
                        datatype: 'Json',
                        data: { productid: productId, page: '1' },
                        success: function (data) {
                            erroMess.text('');
                            $(".comment-box-user").remove();
                            $(".pageComment").remove();

                            page = $(data).find('.pageComment');
                            page.appendTo('.pageNumber');

                            cm = $(data).find('.comment-box-user');
                            cm.insertAfter('#list-comment > .title');

                            setup();
                        },
                        error: function (data) {
                            erroMess.text('Da xay ra loi');
                            alert(data.erroMess);
                        }
                    });

                    setup();


                    $('#comment-area').val("");
                    $.getScript("./Scripts/product-detail.js", function (script, textStatus, jqXHR) {

                    });
                }
                else
                    erroMess.text(data.Error);


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

    // sửa 1 comment
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

    // xóa 1 comment
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
                success: function (data)
                {
                    if (data.Error == "") {
                        $('.comment-box-user[data-postid={0}]'.format(postId)).remove();

                        var productId = $('#comment-box-content').attr('data-productid');

                        // thực hiện lại phân trang
                        $.ajax({
                            url: '/Comment/Paging',
                            type: 'POST',
                            datatype: 'Json',
                            data: { productid: productId, page: '1' },
                            success: function (data) {
                                erroMess.text('');
                                $(".comment-box-user").remove();
                                $(".pageComment").remove();

                                page = $(data).find('.pageComment');
                                page.appendTo('.pageNumber');

                                cm = $(data).find('.comment-box-user');
                                cm.insertAfter('#list-comment > .title');

                                setup();
                            },
                            error: function (data) {
                                erroMess.text('Da xay ra loi');
                                alert(data.erroMess);
                            }
                        });
                    }
                    else
                        erroMess.text(data.Error);                 
                },
                error: function (jqXHR, status, error) {
                    erroMess.text('Khong duoc phep');
                }
            });
        }
    });

    // Xử lý sự kiện click vào từng page number
    $('.pageComment').unbind('click');
    $('.pageComment').on('click', function () {
        var productId = $('#comment-box-content').attr('data-productid');
        $.ajax({
            url: '/Comment/Paging',
            type: 'POST',
            datatype: 'Json',
            data: { productid: productId, page: $(this).text() },
            success: function (data) {
                erroMess.text('');
                $(".comment-box-user").remove();
                $(".pageComment").remove();


                page = $(data).find('.pageComment');
                page.appendTo('.pageNumber');

                cm = $(data).find('.comment-box-user');
                cm.insertAfter('#list-comment > .title');

                setup();
            },
            error: function (data) {
                erroMess.text('Da xay ra loi');
                alert(data.erroMess);
            }
        });

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
            else
                erroMess.text(data.Error);

        },
        error: function (jqXHR, status, error) {
            erroMess.text(error);
        }
    });
}

function AddtoCart ()
{
    erroMessCart = $('#erroMess-cart');

    $('.button-add').unbind('click');
    $('.button-add').on('click', function () {
        var productId = $('#comment-box-content').attr('data-productid');
        var quantity = $('#quantity').val();
        var ajaxUrl = $('.button-add').attr('data-url');
        $.ajax({
            url: ajaxUrl + '/AddToCart',
            type: 'POST',
            datatype: 'json',
            data: { productId: productId, quantity: quantity },
            success: function (data) {
                if (data.Error == "") {
                    erroMessCart.fadeOut(100);
                    erroMessCart.text(data.Success);
                    erroMessCart.fadeIn(100);
                }
                else
                    erroMessCart.text(data.Error);
            },
            error: function (data) {
                erroMessCart.text(data.Error);
            }
        });
    });
}

function RemoveFromCart ()
{
    erroMessCart = $('#erroMess-cart');

    $('.remove-btn').unbind('click');
    $('.remove-btn').on('click', function () {
        var productId = this.id;

        var answer = confirm("Bạn muốn xóa sản phẩm khỏi giỏ hàng?");
        if (answer) {
            var ajaxUrl = $('.remove-btn').attr('data-url');
            $.ajax({
                url: ajaxUrl + '/RemoveFromCart',
                type: 'POST',
                datatype: 'json',
                data: { productId: productId },
                success: function (data) {

                    erroMessCart.fadeOut(100);
                    erroMessCart.replaceWith($(data).find('#erroMess-cart'));
                    erroMessCart.fadeIn(100);

                    $('.cart-item-table').replaceWith($(data).find('.cart-item-table'));

                   
                        
                        RemoveFromCart();
                },
                error: function (data) {
                    erroMessCart.text(data.Error);
                }
            });
        }
    });
}