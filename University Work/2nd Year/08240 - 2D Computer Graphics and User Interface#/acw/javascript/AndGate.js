var AndGate = (function (_super) {	
	__extends(AndGate, _super);
    function AndGate()	{
		_super.call(this);
    }
	    
	AndGate.prototype.draw = function (context) {
		context.save();
		context.beginPath();	
		
		context.fillStyle = '#ffffff';
		context.strokeStyle = '#000000';		
		context.lineWidth = 2;
				
		// drawing node attachment lines
		context.moveTo(0, 0);
		context.lineTo(0, 10);
		
		context.moveTo(0, 90);
		context.lineTo(0, 100)
		
		// drawing main body
		context.moveTo(50, 60);
		context.arc(0, 60, 50, 0, Math.PI, true);
		context.moveTo(-50, 60);
		context.lineTo(-50, 90);
		context.lineTo(50, 90);
		context.lineTo(50, 60);
		
		context.fill();
		context.stroke();
		context.restore();

		this.drawChildren(context);
	}     
	
    return AndGate;
})(FaultTreeGate);
