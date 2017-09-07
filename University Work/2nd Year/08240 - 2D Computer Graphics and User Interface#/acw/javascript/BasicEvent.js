var BasicEvent = (function (_super) {
	__extends(BasicEvent, _super);
    function BasicEvent() {
		_super.call(this);
    }
	
    BasicEvent.prototype.draw = function (context) {
		context.save();
		context.beginPath();	
		
		context.fillStyle = '#ffffff';
		context.strokeStyle = '#000000';		
		context.lineWidth = 2;
		
		// drawing note attachment lines
		context.moveTo(0, 10);
		context.lineTo(0, 0);
		
		context.translate(0, 10);
		
		// drawing main body using polygon creation algorithm
		var numSegments = 30;
		var anglePerSegment = Math.PI * 2 / numSegments;
		var centerX = 0
		var centerY = 40;
		var radius = 40;		
		
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

    return BasicEvent;
})(FaultTreeNode);
