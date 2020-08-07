var singleplayer = {
    // Begin single player campaign
    start:function(){
        // Hide the starting menu layer
        $('.gamelayer').hide();
        
        // Begin with the first level
        singleplayer.currentLevel = 0;
        game.type = "singleplayer";
        game.team = "blue";
                
        // Finally start the level
        singleplayer.startCurrentLevel();
    },    
    exit:function(){
        // Show the starting menu layer
        $('.gamelayer').hide();
        $('#gamestartscreen').show();
    },
    currentLevel:0,    
	startCurrentLevel:function(){                
	    // Load all the items for the level
	    var level = maps.singleplayer[singleplayer.currentLevel];

	    // Don't allow player to enter mission until all assets for the level are loaded
	    $("#entermission").attr("disabled", true);

	    // Load all the assets for the level
	    game.currentMapImage = loader.loadImage(level.mapImage);
	    game.currentLevel = level;

	    game.offsetX = level.startX * game.gridSize;
	    game.offsetY = level.startY * game.gridSize;

	    // Load level Requirements 
	    game.resetArrays();
	    for (var type in level.requirements){
	           var requirementArray = level.requirements[type];
	           for (var i=0; i < requirementArray.length; i++) {
	               var name = requirementArray[i];
	               if (window[type]){
	                   window[type].load(name);
	               } else {
	                   console.log('Could not load type :',type);
	               }
	           };
	       }

	    for (var i = level.items.length - 1; i >= 0; i--){
	        var itemDetails = level.items[i];
	        game.add(itemDetails);
	    };        

	    // Enable the enter mission button once all assets are loaded
	    if (loader.loaded){
	        $("#entermission").removeAttr("disabled");
	    } else {
	        loader.onload = function(){
	            $("#entermission").removeAttr("disabled");
	        }
	    }

	    // Load the mission screen with the current briefing
	    $('#missonbriefing').html(level.briefing.replace('\n','<br><br>'));    
	    $("#missionscreen").show();        
	},    

   
    play:function(){
        game.animationLoop();                        
        game.animationInterval = setInterval(game.animationLoop,game.animationTimeout);
        game.start();
    },   
     
};
