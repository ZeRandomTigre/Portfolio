var TransferGate = (function (_super) {
	__extends(TransferGate, _super);
    function TransferGate() {
		_super.call(this);
    }
	
    TransferGate.prototype.draw = function (context) {
		context.save();
		context.beginPath();	
		
		context.fillStyle = '#ffffff';
		context.strokeStyle = '#000000';		
		context.lineWidth = 2;
		
		// drawing node attachment lines
		context.moveTo(0, 10);
		context.lineTo(0, 0);
		
		// rotate so that triangle point is upwards
		context.rotate(-90 * Math.PI / 180)
		context.translate(-60, 0);
		
		// drawing main body using polygon algorithm
		var numSegments = 3;
		var anglePerSegment = Math.PI * 2 / numSegments;
		var centerX = 0
		var centerY = 0;
		var radius = 50;
		
		for (var i = 0; i <= numSegments; i++) {
			var angle = anglePerSegment * i;
			var x = centerX + radius * Math.cos(angle);
			var y = centerY + radius * Math.sin(angle);
			if (i == 0) {
				context.moveTo(x,y);
			}
			else {
				context.lineTo(x,y);
			}
		}
				
		context.fill();
		context.stroke();		
		context.restore();
    };
	
    return TransferGate;
})(FaultTreeNode);
