var OrGate = (function (_super) {
	__extends(OrGate, _super);
			  
    function OrGate() {
		_super.call(this);
    }
    
	OrGate.prototype.draw = function (context) {
		context.save();
		context.beginPath();	
		
		context.fillStyle = '#ffffff';
		context.strokeStyle = '#000000';		
		context.lineWidth = 2;
				
		// drawing node attachment lines
		context.moveTo(0, 10);
		context.lineTo(0, 0);
		
		context.moveTo(0, 75);
		context.lineTo(0, 100);
		
		// draw main body
		context.moveTo(0, 10);
		context.bezierCurveTo(25, 25, 50, 25, 50, 90);
		context.bezierCurveTo(15, 70, -15, 70, -50, 90);
		context.bezierCurveTo(-50, 25, -25, 25, 0, 10);
		context.closePath();
		
		context.fill();
		context.stroke();
		context.restore();
		
		this.drawChildren(context);
    }    
	
    return OrGate;
})(FaultTreeGate);
