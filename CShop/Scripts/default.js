// JavaScript Document
$(document).ready(function(e) {
    $('#login').click(function(ev) {
        $('#login-panel').fadeIn(300);
		ev.stopPropagation();
    });
	$('body').click(function(){
			$('#login-panel').fadeOut(200);
	});

	String.prototype.format = String.prototype.f = function () {
	    var s = this,
            i = arguments.length;

	    while (i--) {
	        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
	    }
	    return s;
	};

	$('#search-img').click(function () {
	    $('#quick-search').submit();
	});

	$('ul.cat-1 > li').hover(function () {
	    $(this).children('ul').fadeIn();
	});

	$('ul.cat-1 > li > ul').fadeOut();
	$('ul.cat-1 > li').mouseleave(function () {
	    $(this).children('ul').fadeOut();
	});
});