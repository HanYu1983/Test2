
var dispatchMouseEvent = function(target, var_args) {
    var e = document.createEvent("MouseEvents");
    e.initEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchKeyboardEvent = function(target, initKeyboradEvent_args) {
    var e = document.createEvent("KeyboardEvents");
    e.initKeyboardEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchTextEvent = function(target, initTextEvent_args) {
    var e = document.createEvent("TextEvent");
    e.initTextEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchSimpleEvent = function(target, type, canBubble, cancelable) {
    var e = document.createEvent("Event");
    e.initEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};


/*
var btn = $(document).find('a[href="load?lottery=PK10JSC&page=110"]')
dispatchMouseEvent(btn[0], 'click', true, true);
*/

/*
var field = $('input[name="B1_1"]')[0]
console.log(field)
dispatchTextEvent(field, 'textInput', true, true, null, '5', 0)
*/
console.log("trigger")
/*
console.log($('input[name="B1_1"]')[0])
$('input[name="B1_1"]').val(5)
*/

/*
var script = document.createElement( "script" )
script.setAttribute('type', 'text/javascript');
script.innerHTML = '$(\'input[name="B1_1"]\').val(5)';

$('iframe')[0].contentDocument.body.append(script)
*/