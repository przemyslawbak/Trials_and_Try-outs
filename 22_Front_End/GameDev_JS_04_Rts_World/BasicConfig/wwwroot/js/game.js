$(window).load(function () {
    game.init();
});

var game = {
    // Start preloading assets
    init: function () {
        loader.init();
        mouse.init();

        $('.gamelayer').hide();
        $('#gamestartscreen').show();

        game.backgroundCanvas = document.getElementById('gamebackgroundcanvas');
        game.backgroundContext = game.backgroundCanvas.getContext('2d');

        game.foregroundCanvas = document.getElementById('gameforegroundcanvas');
        game.foregroundContext = game.foregroundCanvas.getContext('2d');

        game.canvasWidth = game.backgroundCanvas.width;
        game.canvasHeight = game.backgroundCanvas.height;
    },
    start: function () {
        $('.gamelayer').hide();
        $('#gameinterfacescreen').show();
        game.running = true;
        game.refreshBackground = true;

        game.drawingLoop();
    },

    // The map is broken into square tiles of this size (20 pixels x 20 pixels)
    gridSize: 20,

    // Store whether or not the background moved and needs to be redrawn
    refreshBackground: true,

    // A control loop that runs at a fixed period of time
    animationTimeout: 100, // 100 milliseconds or 10 times a second
    offsetX: 0,	// X & Y panning offsets for the map
    offsetY: 0,
    panningThreshold: 60, // Distance from edge of canvas at which panning starts
    panningSpeed: 10, // Pixels to pan every drawing loop
    handlePanning: function () {
        // do not pan if mouse leaves the canvas
        if (!mouse.insideCanvas) {
            return;
        }

        if (mouse.x <= game.panningThreshold) {
            if (game.offsetX >= game.panningSpeed) {
                game.refreshBackground = true;
                game.offsetX -= game.panningSpeed;
            }
        } else if (mouse.x >= game.canvasWidth - game.panningThreshold) {
            if (game.offsetX + game.canvasWidth + game.panningSpeed <= game.currentMapImage.width) {
                game.refreshBackground = true;
                game.offsetX += game.panningSpeed;
            }
        }

        if (mouse.y <= game.panningThreshold) {
            if (game.offsetY >= game.panningSpeed) {
                game.refreshBackground = true;
                game.offsetY -= game.panningSpeed;
            }
        } else if (mouse.y >= game.canvasHeight - game.panningThreshold) {
            if (game.offsetY + game.canvasHeight + game.panningSpeed <= game.currentMapImage.height) {
                game.refreshBackground = true;
                game.offsetY += game.panningSpeed;
            }
        }

        if (game.refreshBackground) {
            // Update mouse game coordinates based on game offsets
            mouse.calculateGameCoordinates();
        }
    },
    animationLoop: function () {

        // Animate each of the elements within the game
    },
    drawingLoop: function () {
        // Handle Panning the Map
        game.handlePanning();

        // Since drawing the background map is a fairly large operation,
        // we only redraw the background if it changes (due to panning)
        if (game.refreshBackground) {
            game.backgroundContext.drawImage(game.currentMapImage, game.offsetX, game.offsetY, game.canvasWidth, game.canvasHeight, 0, 0, game.canvasWidth, game.canvasHeight);
            game.refreshBackground = false;
        }

        // fast way to clear the foreground canvas
        game.foregroundCanvas.width = game.foregroundCanvas.width;

        // Start drawing the foreground elements

        // Draw the mouse
        mouse.draw();

        // Call the drawing loop for the next frame using request animation frame
        if (game.running) {
            requestAnimationFrame(game.drawingLoop);
        }
    }
};