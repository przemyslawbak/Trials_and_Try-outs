var buildings = {
    list: {
        "base": {
            name: "base",
            // Properties for drawing the object
            pixelWidth: 60,
            pixelHeight: 60,
            baseWidth: 40,
            baseHeight: 40,
            pixelOffsetX: 0,
            pixelOffsetY: 20,
            // Properties for describing structure for pathfinding
            buildableGrid: [
                [1, 1],
                [1, 1]
            ],
            passableGrid: [
                [1, 1],
                [1, 1]
            ],
            sight: 3,
            hitPoints: 500,
            cost: 5000,
            spriteImages: [
                { name: "healthy", count: 4 },
                { name: "damaged", count: 1 },
                { name: "contructing", count: 3 },
            ],
        },
        "starport": {
            name: "starport",
            pixelWidth: 40,
            pixelHeight: 60,
            baseWidth: 40,
            baseHeight: 55,
            pixelOffsetX: 1,
            pixelOffsetY: 5,
            buildableGrid: [
                [1, 1],
                [1, 1],
                [1, 1]
            ],
            passableGrid: [
                [1, 1],
                [0, 0],
                [0, 0]
            ],
            sight: 3,
            cost: 2000,
            hitPoints: 300,
            spriteImages: [
                { name: "teleport", count: 9 },
                { name: "closing", count: 18 },
                { name: "healthy", count: 4 },
                { name: "damaged", count: 1 },
            ],
        },
        "harvester": {
            name: "harvester",
            pixelWidth: 40,
            pixelHeight: 60,
            baseWidth: 40,
            baseHeight: 20,
            pixelOffsetX: -2,
            pixelOffsetY: 40,
            buildableGrid: [
                [1, 1]
            ],
            passableGrid: [
                [1, 1]
            ],
            sight: 3,
            cost: 5000,
            hitPoints: 300,
            spriteImages: [
                { name: "deploy", count: 17 },
                { name: "healthy", count: 3 },
                { name: "damaged", count: 1 },
            ],
        },
        "ground-turret": {
            name: "ground-turret",
            canAttack: true,
            canTargetLand: true,
            canTargetAir: false,
            weaponType: "cannon-ball",
            action: "guard", // Default action is guard unlike other buildings
            direction: 0, // Face upward (0) by default
            directions: 8, // Total of 8 turret directions allowed (0-7)
            orders: { type: "guard" },
            pixelWidth: 38,
            pixelHeight: 32,
            baseWidth: 20,
            baseHeight: 18,
            cost: 1500,
            pixelOffsetX: 9,
            pixelOffsetY: 12,
            buildableGrid: [
                [1]
            ],
            passableGrid: [
                [1]
            ],
            sight: 5,
            hitPoints: 200,
            spriteImages: [
                { name: "teleport", count: 9 },
                { name: "healthy", count: 1, directions: 8 },
                { name: "damaged", count: 1 },
            ],
        }


    },
    defaults: {
        type: "buildings",
        animationIndex: 0,
        direction: 0,
        orders: { type: "stand" },
        action: "stand",
        selected: false,
        selectable: true,
        animate: function () {
            if (this.life > this.hitPoints * 0.4) {
                this.lifeCode = "healthy";
            } else if (this.life <= 0) {
                this.lifeCode = "dead";
                game.remove(this);
                return;
            } else {
                this.lifeCode = "damaged";
            }

            switch (this.action) {
                case "stand":
                    this.imageList = this.spriteArray[this.lifeCode];
                    this.imageOffset = this.imageList.offset + this.animationIndex;
                    this.animationIndex++;
                    if (this.animationIndex >= this.imageList.count) {
                        this.animationIndex = 0;
                    }
                    break;
                case "teleport":
                    this.imageList = this.spriteArray["teleport"];
                    this.imageOffset = this.imageList.offset + this.animationIndex;
                    this.animationIndex++;
                    // Once teleporting is complete, move to either guard or stand mode
                    if (this.animationIndex >= this.imageList.count) {
                        this.animationIndex = 0;
                        if (this.canAttack) {
                            this.action = "guard";
                        } else {
                            this.action = "stand";
                        }
                    }
                    break;
                case "close":
                    this.imageList = this.spriteArray["closing"];
                    this.imageOffset = this.imageList.offset + this.animationIndex;
                    this.animationIndex++;
                    // Once closing is complete go back to standing
                    if (this.animationIndex >= this.imageList.count) {
                        this.animationIndex = 0;
                        this.action = "stand";
                    }
                    break;
                case "open":
                    this.imageList = this.spriteArray["closing"];
                    // Opening is just the closing sprites running backwards
                    this.imageOffset = this.imageList.offset + this.imageList.count - this.animationIndex;
                    this.animationIndex++;
                    // Once opening is complete, go back to close
                    if (this.animationIndex >= this.imageList.count) {
                        this.animationIndex = 0;
                        this.action = "close";
                    }
                    break;
                case "deploy":
                    this.imageList = this.spriteArray["deploy"];
                    this.imageOffset = this.imageList.offset + this.animationIndex;
                    this.animationIndex++;
                    // Once deploying is complete, go back to stand
                    if (this.animationIndex >= this.imageList.count) {
                        this.animationIndex = 0;
                        this.action = "stand";
                    }
                    break;
                case "guard":
                    if (this.lifeCode === "damaged") {
                        // The damaged turret has no directions
                        this.imageList = this.spriteArray[this.lifeCode];
                    } else {
                        // The healthy turret has 8 directions
                        this.imageList = this.spriteArray[this.lifeCode + "-" + this.direction];
                    }
                    this.imageOffset = this.imageList.offset;
                    break;
            }
        },
        drawLifeBar: function () {
            var x = this.drawingX + this.pixelOffsetX;
            var y = this.drawingY - 2 * game.lifeBarHeight;

            game.foregroundContext.fillStyle = (this.lifeCode === "healthy") ? game.healthBarHealthyFillColor : game.healthBarDamagedFillColor;
            game.foregroundContext.fillRect(x, y, this.baseWidth * this.life / this.hitPoints, game.lifeBarHeight)

            game.foregroundContext.strokeStyle = game.healthBarBorderColor;
            game.foregroundContext.lineWidth = 1;
            game.foregroundContext.strokeRect(x, y, this.baseWidth, game.lifeBarHeight)
        },
        drawSelection: function () {
            var x = this.drawingX + this.pixelOffsetX;
            var y = this.drawingY + this.pixelOffsetY;
            game.foregroundContext.strokeStyle = game.selectionBorderColor;
            game.foregroundContext.lineWidth = 1;
            game.foregroundContext.fillStyle = game.selectionFillColor;
            game.foregroundContext.fillRect(x - 1, y - 1, this.baseWidth + 2, this.baseHeight + 2);
            game.foregroundContext.strokeRect(x - 1, y - 1, this.baseWidth + 2, this.baseHeight + 2);
        },
        // Default function for drawing a building
        draw: function () {
            var x = (this.x * game.gridSize) - game.offsetX - this.pixelOffsetX;
            var y = (this.y * game.gridSize) - game.offsetY - this.pixelOffsetY;
            this.drawingX = x;
            this.drawingY = y;
            if (this.selected) {
                this.drawSelection();
                this.drawLifeBar();
            }
            // All sprite sheets will have blue in the first row and green in the second row
            var colorIndex = (this.team === "blue") ? 0 : 1;
            var colorOffset = colorIndex * this.pixelHeight;
            game.foregroundContext.drawImage(this.spriteSheet, this.imageOffset * this.pixelWidth, colorOffset, this.pixelWidth, this.pixelHeight, x, y, this.pixelWidth, this.pixelHeight);
        }
    },
    load: loadItem,
    add: addItem,
}