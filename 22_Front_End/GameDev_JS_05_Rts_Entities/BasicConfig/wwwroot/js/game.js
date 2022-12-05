$(window).load(function() {
	game.init();
});

var game = {
    // Start preloading assets
	init: function(){
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
    start:function(){
        $('.gamelayer').hide();
        $('#gameinterfacescreen').show();
		game.running = true;
		game.refreshBackground = true;
		
		game.drawingLoop();
    },
	
	// The map is broken into square tiles of this size (20 pixels x 20 pixels)
	gridSize:20, 
	
	// Store whether or not the background moved and needs to be redrawn
	refreshBackground:true,	
		
	// A control loop that runs at a fixed period of time 
	animationTimeout:100, // 100 milliseconds or 10 times a second
	offsetX:0,	// X & Y panning offsets for the map
	offsetY:0,
	panningThreshold:60, // Distance from edge of canvas at which panning starts
	panningSpeed:10, // Pixels to pan every drawing loop
	handlePanning:function(){
		// do not pan if mouse leaves the canvas
		if (!mouse.insideCanvas){
			return;
		}

		if(mouse.x<=game.panningThreshold){
			if (game.offsetX>=game.panningSpeed){
				game.refreshBackground = true;
				game.offsetX -= game.panningSpeed;		
			}
		} else if (mouse.x>= game.canvasWidth - game.panningThreshold){
			if (game.offsetX + game.canvasWidth + game.panningSpeed <= game.currentMapImage.width){
				game.refreshBackground = true;
				game.offsetX += game.panningSpeed;
			}
		}
	
		if(mouse.y<=game.panningThreshold){
			if (game.offsetY>=game.panningSpeed){
				game.refreshBackground = true;
				game.offsetY -= game.panningSpeed;
			}
		} else if (mouse.y>= game.canvasHeight - game.panningThreshold){
			if (game.offsetY + game.canvasHeight + game.panningSpeed <= game.currentMapImage.height){
				game.refreshBackground = true;
				game.offsetY += game.panningSpeed;
			}
		}	
	
		if (game.refreshBackground){
			// Update mouse game coordinates based on game offsets
			mouse.calculateGameCoordinates();
		}
	},
	animationLoop:function(){
	    // Animate each of the elements within the game
	    for (var i = game.items.length - 1; i >= 0; i--){
	        game.items[i].animate();
	    };

	    // Sort game items into a sortedItems array based on their x,y coordinates
	       game.sortedItems = $.extend([],game.items);        
	       game.sortedItems.sort(function(a,b){
	        return b.y-a.y + ((b.y==a.y)?(a.x-b.x):0);
	       });
	},    
	drawingLoop:function(){    
	    // Handle Panning the Map    
	    game.handlePanning();

	    // Since drawing the background map is a fairly large operation, 
	    // we only redraw the background if it changes (due to panning)
	    if (game.refreshBackground){
	        game.backgroundContext.drawImage(game.currentMapImage,game.offsetX,game.offsetY,game.canvasWidth,game.canvasHeight, 0,0,game.canvasWidth,game.canvasHeight);
	        game.refreshBackground = false;
	    }

	    // fast way to clear the foreground canvas
	    game.foregroundCanvas.width = game.foregroundCanvas.width;

	    // Start drawing the foreground elements
	    for (var i = game.sortedItems.length - 1; i >= 0; i--){
	        game.sortedItems[i].draw();
	    };

	    // Draw the mouse 
	    mouse.draw()

	    // Call the drawing loop for the next frame using request animation frame
	    if (game.running){
	        requestAnimationFrame(game.drawingLoop);    
	    }                        
	},

	resetArrays:function(){
	    game.counter = 0;
	    game.items = [];
	    game.sortedItems = [];
	    game.buildings = [];
	    game.vehicles = [];
	    game.aircraft = [];
	    game.terrain = [];
	    game.triggeredEvents = [];
	    game.selectedItems = [];
	    game.sortedItems = [];
	},
	add:function(itemDetails) {
	    // Set a unique id for the item
	    if (!itemDetails.uid){
	        itemDetails.uid = game.counter++;
	    }

	    var item = window[itemDetails.type].add(itemDetails);

	    // Add the item to the items array
	    game.items.push(item);
	    // Add the item to the type specific array
	    game[item.type].push(item);        
	    return item;        
	},
	remove:function(item){
	    // Unselect item if it is selected
	    item.selected = false;
	    for (var i = game.selectedItems.length - 1; i >= 0; i--){
	           if(game.selectedItems[i].uid == item.uid){
	               game.selectedItems.splice(i,1);
	               break;
	           }
	       };

	    // Remove item from the items array
	       for (var i = game.items.length - 1; i >= 0; i--){
	           if(game.items[i].uid == item.uid){
	               game.items.splice(i,1);
	               break;
	           }
	       };

	    // Remove items from the type specific array
	    for (var i = game[item.type].length - 1; i >= 0; i--){
	           if(game[item.type][i].uid == item.uid){
	               game[item.type].splice(i,1);
	               break;
	           }
	       };    
	},
	/* Selection Related Code */
	selectionBorderColor:"rgba(255,255,0,0.5)",
	selectionFillColor:"rgba(255,215,0,0.2)",
	healthBarBorderColor:"rgba(0,0,0,0.8)",
	healthBarHealthyFillColor:"rgba(0,255,0,0.5)",
	healthBarDamagedFillColor:"rgba(255,0,0,0.5)",
	lifeBarHeight:5,	
	clearSelection:function(){
		while(game.selectedItems.length>0){
			game.selectedItems.pop().selected = false;
		}
	},
	selectItem:function(item,shiftPressed){
		// Pressing shift and clicking on a selected item will deselect it
		if (shiftPressed && item.selected){
			// deselect item
			item.selected = false;
			for (var i = game.selectedItems.length - 1; i >= 0; i--){
		        if(game.selectedItems[i].uid == item.uid){
		            game.selectedItems.splice(i,1);
		            break;
		        }
		    };			
			return;
		}

		if (item.selectable && !item.selected){
			item.selected = true;
			game.selectedItems.push(item);	        
		}
	},
	
}