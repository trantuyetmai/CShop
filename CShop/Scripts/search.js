$(document).ready(function () {
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
    init();
});

function setup()
{
    //filter
    $('.filter ul li').click(function () {
        //uncheck sibling
        $(this).parent('ul').children().removeClass('checked');

        //check current
        $(this).addClass('checked');
       

        var filterName = $(this).parents('.filter').attr('data-name');
        var value = $(this).attr('data-id');
        $('input[name={0}]'.format(filterName)).val(value);
    });
}

function init()
{

}