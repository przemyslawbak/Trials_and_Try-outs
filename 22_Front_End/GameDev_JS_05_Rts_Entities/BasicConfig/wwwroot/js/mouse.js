var mouse = {
    // x,y coordinates of mouse relative to top left corner of canvas
    x:0,
    y:0,
    // x,y coordinates of mouse relative to top left corner of game map
    gameX:0,
    gameY:0,
    // game grid x,y coordinates of mouse 
    gridX:0,
    gridY:0,
    // whether or not the left mouse button is currently pressed
    buttonPressed:false,
    // whether or not the player is dragging and selecting with the left mouse button pressed
    dragSelect:false,
    // whether or not the mouse is inside the canvas region
    insideCanvas:false,

	click:function(ev,rightClick){
		// Player clicked inside the canvas

		var clickedItem = this.itemUnderMouse();
		var shiftPressed = ev.shiftKey;

		if (!rightClick){ // Player left clicked
			if (clickedItem){				
				// Pressing shift adds to existing selection. If shift is not pressed, clear existing selection
				if(!shiftPressed){
					game.clearSelection();
				}
				game.selectItem(clickedItem,shiftPressed);
			}
		} else { // Player right clicked
			// Handle actions like attacking and movement of selected units
		}
	},
	itemUnderMouse:function(){
		for (var i = game.items.length - 1; i >= 0; i--){
	        var item = game.items[i];
	        if (item.type=="buildings" || item.type=="terrain"){
	            if(item.lifeCode != "dead" 
					&& item.x<= (mouse.gameX)/game.gridSize 
	                && item.x >= (mouse.gameX - item.baseWidth)/game.gridSize
	                && item.y<= mouse.gameY/game.gridSize 
	                && item.y >= (mouse.gameY - item.baseHeight)/game.gridSize
	                ){
	                    return item;
	            }
	        } else if (item.type=="aircraft"){
	            if (item.lifeCode != "dead" && 
					Math.pow(item.x-mouse.gameX/game.gridSize,2)+Math.pow(item.y-(mouse.gameY+item.pixelShadowHeight)/game.gridSize,2) < Math.pow((item.radius)/game.gridSize,2)){
	                return item;
	            }
		   }else {
	            if (item.lifeCode != "dead" && Math.pow(item.x-mouse.gameX/game.gridSize,2)+Math.pow(item.y-mouse.gameY/game.gridSize,2) < Math.pow((item.radius)/game.gridSize,2)){
	                return item;
	            }
	        }
		}
	},
    draw:function(){
        if(this.dragSelect){    
            var x = Math.min(this.gameX,this.dragX);
            var y = Math.min(this.gameY,this.dragY);
            var width = Math.abs(this.gameX-this.dragX)
            var height = Math.abs(this.gameY-this.dragY)
            game.foregroundContext.strokeStyle = 'white';
            game.foregroundContext.strokeRect(x-game.offsetX,y-game.offsetY, width, height);
        }    
    },
	calculateGameCoordinates:function(){
		mouse.gameX = mouse.x + game.offsetX ;
		mouse.gameY = mouse.y + game.offsetY;

		mouse.gridX = Math.floor((mouse.gameX) / game.gridSize);
		mouse.gridY = Math.floor((mouse.gameY) / game.gridSize);	
	},
    init:function(){
        var $mouseCanvas = $("#gameforegroundcanvas");
        $mouseCanvas.mousemove(function(ev) {
            var offset = $mouseCanvas.offset();
            mouse.x = ev.pageX - offset.left;
            mouse.y = ev.pageY - offset.top;  
            
            mouse.calculateGameCoordinates();

            if (mouse.buttonPressed){
                if  ((Math.abs(mouse.dragX - mouse.gameX) > 4 || Math.abs(mouse.dragY - mouse.gameY) > 4)){
                        mouse.dragSelect = true
                }
            } else {
                mouse.dragSelect = false;
            }                     
        });
        
        $mouseCanvas.click(function(ev) {
            mouse.click(ev,false);
            mouse.dragSelect = false;                
            return false;
        });
        
        $mouseCanvas.mousedown(function(ev) {
            if(ev.which == 1){
                mouse.buttonPressed = true;
                mouse.dragX = mouse.gameX;
                mouse.dragY = mouse.gameY;
                ev.preventDefault();
            }
            return false;
        });
        
        $mouseCanvas.bind('contextmenu',function(ev){
            mouse.click(ev,true);
            return false;
        });
        
		$mouseCanvas.mouseup(function(ev) {
		    var shiftPressed = ev.shiftKey;
		    if(ev.which==1){
		    //Left key was released
		        if (mouse.dragSelect){
		            if (!shiftPressed){
		                // Shift key was not pressed                        
		                game.clearSelection();
		            }

			        var x1 = Math.min(mouse.gameX,mouse.dragX)/game.gridSize;
			        var y1 = Math.min(mouse.gameY,mouse.dragY)/game.gridSize;
			        var x2 = Math.max(mouse.gameX,mouse.dragX)/game.gridSize;
			        var y2 = Math.max(mouse.gameY,mouse.dragY)/game.gridSize;
		        	for (var i = game.items.length - 1; i >= 0; i--){
		                var item = game.items[i];
		                if (item.type != "buildings" && item.selectable && item.team==game.team && x1<= item.x && x2 >= item.x){
		                    if ((item.type == "vehicles" && y1<= item.y && y2 >= item.y)
		                    || (item.type == "aircraft" && (y1 <= item.y-item.pixelShadowHeight/game.gridSize) && (y2 >= item.y-item.pixelShadowHeight/game.gridSize))){
		                        game.selectItem(item,shiftPressed);
		                    }

		                } 
		            };
		        }
		        mouse.buttonPressed = false;
		        mouse.dragSelect = false;
		    }
		    return false;
		});

        $mouseCanvas.mouseleave(function(ev) {
            mouse.insideCanvas = false;
        });
        
        $mouseCanvas.mouseenter(function(ev) {
            mouse.buttonPressed = false;
            mouse.insideCanvas = true;
        });
    }
}
