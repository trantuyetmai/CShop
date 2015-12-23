// JavaScript Document

var curInterval;
var IsSlideHovered;
var tickValue = 5000;
var DEFAULT_TICK = 5000;


$(window).load(function(e) {
	//init	
    var box = $('.nslider-box');		
	box.attr('showed-item', '0');
	box.width(box.attr('width'));
	box.height(box.attr('height'));
	
	//center item
	var box = $('.nslider-box');
	$('.nslider-item').each(function(index, element) {
		var i = $(this);

		scaleRatio(i, box.width()*0.8, box.height()*0.8);
		var centerHeight = (box.height()/2 - i.height()/2);
		var centerWidth =  (box.width()/2 - i.width()/2);
		i.css('margin', '{0}px 0 0 {1}px'.f(centerHeight,centerWidth));
    });

	
	
	
	
	$('.nslider-item').hover(function(){IsSlideHovered = true;}, function(){IsSlideHovered = false;});
	curInterval = setInterval(update, 10);
	addNavigation();
});

function update()
{
	
	
	tickValue += 10;
	if(tickValue >= DEFAULT_TICK)
	{
		tick();
		tickValue= 0;
	}	
}

function tick()
{		
		var box = $('.nslider-box');
		var val = parseInt(box.attr('showed-item'));
		var newVal = ( val + 1) % ($('.nslider-box').children('.nslider-item').length);
		
	
		box.children('.nslider-item').removeClass('nslider-animation-1');
		box.children('.nslider-item').removeClass('nslider-animation-2');
		box.children('.nslider-item').eq(val).addClass('nslider-animation-2');
		box.children('.nslider-item').eq(newVal).addClass('nslider-animation-1');					
		box.attr('showed-item', newVal);
	
}

function scaleRatio(element, maxWidth, maxHeight)
{
	var ratio =	element.width()/element.height();
	element.width(maxHeight*ratio);
	element.height(maxHeight);
	if(element.width()>maxWidth)
		element.width(maxWidth);
}

function changeIndex(i){
		var box = $('.nslider-box');
		var val = parseInt(box.attr('showed-item'));
		var newVal = ( val + i) % ($('.nslider-box').children('.nslider-item').length);
		tickValue = 0;
		
		box.children('.nslider-item').removeClass('nslider-animation-1');
		box.children('.nslider-item').removeClass('nslider-animation-2');
		box.children('.nslider-item').eq(val).addClass('nslider-animation-2');
		box.children('.nslider-item').eq(newVal).addClass('nslider-animation-1');					
		box.attr('showed-item', newVal);
	};

function addNavigation()
{
	$('.nslider-box').append('<div id="nslide-left-arrow"  />');
	$('.nslider-box').append('<div id="nslide-right-arrow"  />');
	$('#nslide-left-arrow').click(function(){
		//tickValue = 5000;
		//var index = $('.nslider-box').attr('showed-item');
		//index--;
		//if(index<0) index = $('.nslider-item').length;
		//$('.nslider-box').attr('showed-item',index);
		changeIndex(-1);
	});
	$('#nslide-right-arrow').click(function(){
		//tickValue = 5000;
		//var index = $('.nslider-box').attr('showed-item');
		//index = (index+1)%$('.nslider-item').length;
		//$('.nslider-box').attr('showed-item',index);
		changeIndex(1);
	});
}

String.prototype.format = String.prototype.f = function() {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};

function getOriginalWidthOfImg(img_element) {
    var t = new Image();
    t.src = img_element.attr('src');
    return t.naturalWidth;
}