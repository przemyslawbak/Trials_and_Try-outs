﻿var canvas = document.getElementById('canvas'),
    context = canvas.getContext('2d'),
    repeatRadio = document.getElementById('repeatRadio'),
    noRepeatRadio = document.getElementById('noRepeatRadio'),
    repeatXRadio = document.getElementById('repeatXRadio'),
    repeatYRadio = document.getElementById('repeatYRadio'),
    image = new Image();
// Functions..........................................................
function fillCanvasWithPattern(repeatString) {
    var pattern = context.createPattern(image, repeatString);
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.fillStyle = pattern;
    context.fillRect(0, 0, canvas.width, canvas.height);
    context.fill();
}
// Event handlers.....................................................
repeatRadio.onclick = function (e) {
    fillCanvasWithPattern('repeat');
};
repeatXRadio.onclick = function (e) {
    fillCanvasWithPattern('repeat-x');
};
repeatYRadio.onclick = function (e) {
    fillCanvasWithPattern('repeat-y');
};
noRepeatRadio.onclick = function (e) {
    fillCanvasWithPattern('no-repeat');
};
// Initialization.....................................................
image.src = '200.gif';
image.onload = function (e) {
    fillCanvasWithPattern('repeat');
};