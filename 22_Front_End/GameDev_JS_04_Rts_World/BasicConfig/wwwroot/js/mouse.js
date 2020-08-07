var mouse = {
    // x,y coordinates of mouse relative to top left corner of canvas
    x: 0,
    y: 0,
    // x,y coordinates of mouse relative to top left corner of game map
    gameX: 0,
    gameY: 0,
    // game grid x,y coordinates of mouse
    gridX: 0,
    gridY: 0,
    // whether or not the left mouse button is currently pressed
    buttonPressed: false,
    // whether or not the player is dragging and selecting with the left mouse button pressed
    dragSelect: false,
    // whether or not the mouse is inside the canvas region
    insideCanvas: false,
    click: function (ev, rightClick) {
        // Player clicked inside the canvas
    },
    draw: function () {
        if (this.dragSelect) {
            var x = Math.min(this.gameX, this.dragX);
            var y = Math.min(this.gameY, this.dragY);
            var width = Math.abs(this.gameX - this.dragX)
            var height = Math.abs(this.gameY - this.dragY)
            game.foregroundContext.strokeStyle = 'white';
            game.foregroundContext.strokeRect(x - game.offsetX, y - game.offsetY, width, height);
        }
    },
    calculateGameCoordinates: function () {
        mouse.gameX = mouse.x + game.offsetX;
        mouse.gameY = mouse.y + game.offsetY;
        mouse.gridX = Math.floor((mouse.gameX) / game.gridSize);
        mouse.gridY = Math.floor((mouse.gameY) / game.gridSize);
    },
    init: function () {
        var $mouseCanvas = $("#gameforegroundcanvas");
        $mouseCanvas.mousemove(function (ev) {
            var offset = $mouseCanvas.offset();
            mouse.x = ev.pageX - offset.left;
            mouse.y = ev.pageY - offset.top;
            mouse.calculateGameCoordinates();
            if (mouse.buttonPressed) {
                if ((Math.abs(mouse.dragX - mouse.gameX) > 4 || Math.abs(mouse.dragY - mouse.gameY)
                    > 4)) {
                    mouse.dragSelect = true;
                }
            } else {
                mouse.dragSelect = false;
            }
        });
        $mouseCanvas.click(function (ev) {
            mouse.click(ev, false);
            mouse.dragSelect = false;
            return false;
        });
        $mouseCanvas.mousedown(function (ev) {
            if (ev.which === 1) {
                mouse.buttonPressed = true;
                mouse.dragX = mouse.gameX;
                mouse.dragY = mouse.gameY;
                ev.preventDefault();
            }
            return false;
        });
        $mouseCanvas.bind('contextmenu', function (ev) {
            mouse.click(ev, true);
            return false;
        });
        $mouseCanvas.mouseup(function (ev) {
            var shiftPressed = ev.shiftKey;
            if (ev.which === 1) {
                //Left key was released
                mouse.buttonPressed = false;
                mouse.dragSelect = false;
            }
            return false;
        });
        $mouseCanvas.mouseleave(function (ev) {
            mouse.insideCanvas = false;
        });
        $mouseCanvas.mouseenter(function (ev) {
            mouse.buttonPressed = false;
            mouse.insideCanvas = true;
        });
    }
};