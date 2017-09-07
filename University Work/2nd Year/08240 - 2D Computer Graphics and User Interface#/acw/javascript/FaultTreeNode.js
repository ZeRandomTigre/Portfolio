var FaultTreeNode = (function () {
    function FaultTreeNode() {
        this.setId(FaultTreeNode.sTotalNodes++);
    }
	
	FaultTreeNode.sTotalNodes = 0;
	
    FaultTreeNode.prototype.getId = function () {
        return this.mId;
    }
	
    FaultTreeNode.prototype.setId = function (pId) {
        this.mId = pId;
    }
		
	FaultTreeNode.prototype.drawChildren = function (context) {
		var totalWidth = this.getWidth();
		
		var xSpacing = 0;
		var finalSpacing = 0;
		
		context.beginPath();
		context.strokeStyle = '#00000';
		context.lineWidth = 2;
		
		
		context.translate(0, 100);
		
		
		// loops through children and draws them
		for (var i = 0; i < this.numChildren(); i++) {
			
			var childNode = this.getChild(i);
			var childNodeWidth = childNode.getWidth();
			
			// first child
			if (i == 0) {
				xSpacing = (-totalWidth / 2) + (childNodeWidth / 2);
			// subsequent children
			} else {
				xSpacing = 50 + (childNodeWidth / 2);
			}			
			
			// drawing lines between nodes
			context.moveTo(0, 0);	
			context.translate(xSpacing, 0);	
			context.lineTo(0, 0);			
			context.stroke();
			
			childNode.draw(context);
		}
				
		context.translate(0, -100);		
    }
	
		
	FaultTreeNode.prototype.getWidth = function () {
		var totalWidth = 0;
		
		// checks if child is a basic event / transfer gate
		if ((this.constructor === BasicEvent) || (this.constructor === TransferGate)) {
		totalWidth = 100;
			
		} else {
			// loops through children and checks if they are basic event / transfer gate
			// if not then recursively calls this function again to determine child[i] width & add to total width
			for (var i = 0; i < this.numChildren(); i++) {
				var childNode = this.getChild(i);
				if ((childNode.constructor === BasicEvent) || (childNode.constructor === TransferGate)) {
					totalWidth = totalWidth + 100
				} else {
					totalWidth += childNode.getWidth();
				}				
			}
		}
		return totalWidth;
	}
	
	FaultTreeNode.prototype.getHeight = function () {
		var height = 0;
		
		// checks if child is a basic event / transfer gate
		if ((this.constructor === BasicEvent) || (this.constructor === TransferGate)) {
			height += 100;
			
		} else {
			// loops through chilldren and checks if the current tallied height is greater than
			// the height of the child(i) and it's children
			// determines the height by recursively calling this function
			for (var i = 0; i < this.numChildren(); i++) {
				var childNode = this.getChild(i);
				var currentHeight = childNode.getHeight();
				if (currentHeight > height) {
					height = currentHeight + 100;
				}
			}
		}
		return height;
	}
	
    return FaultTreeNode;
})();
