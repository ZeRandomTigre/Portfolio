var Car = (function () 
{ 
	function Car(pPosition, pCarSpeed, pCarColour, pScale, pRotationAngle) 
	{ 
		this.setPosition(pPosition);		
		this.setCarSpeed(pCarSpeed);
		this.setCarColour(pCarColour);
		this.setScale(pScale);
		this.setRotationAngle(pRotationAngle);
	}
	
	Car.prototype.getPosition = function()
	{
		return this.mPosition;
	}
	
	Car.prototype.setPosition = function (pPosition)
	{
		this.mPosition = pPosition;
	}
	
	Car.prototype.getCarColour = function()
	{
		return this.mCarColour;
	}
	
	Car.prototype.setCarColour = function(pCarColour)
	{
		this.mCarColour = pCarColour;
	}
	
	Car.prototype.getScale = function()
	{
		return this.mScale;
	}
	
	Car.prototype.setScale = function (pScale)
	{
		this.mScale = pScale;
	}
	
	Car.prototype.getRotationAngle = function()
	{
		return this.mRotationAngle;
	}
	
	Car.prototype.setRotationAngle = function (pRotationAngle)
	{
		this.mRotationAngle = pRotationAngle;
	}
	
	Car.prototype.getCarSpeed = function()
	{
		return this.mCarSpeed;
	}
	
    Car.prototype.setCarSpeed = function (pCarSpeed)
	{
		this.mCarSpeed = pCarSpeed;
	}	

	
	Car.prototype.drawBody = function(pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = this.getCarColour();
		pContext.strokeStyle = '#000000';
		
		pContext.moveTo(0, 0);
		pContext.lineTo(20, 0);
		pContext.lineTo(40, 20);
		pContext.lineTo(70, 20);
		pContext.lineTo(70, 40);
		pContext.lineTo(50, 40);
		pContext.arc(40, 40, 10, 0, Math.PI, true);
		pContext.lineTo(-30, 40);
		pContext.arc(-40, 40, 10, 0, Math.PI, true);
		pContext.lineTo(-70, 40);
		pContext.lineTo(-70, 20);
		pContext.lineTo(-40, 20);
		pContext.lineTo(-20, 0);
		pContext.closePath();
		
		pContext.fill();
		pContext.stroke();
		pContext.restore();
	}
	
	Car.prototype.drawWheel = function(pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = '#191919';
		pContext.strokeStyle = '#000000';
		
		pContext.arc(0, 0, 8, 0, 2 * Math.PI);
		pContext.fill();
		
		pContext.beginPath();
		pContext.fillStyle = '#cccccc';
		pContext.strokeStyle = '#000000';
		
		pContext.arc(0, 0, 4, 0, 2 * Math.PI);
		pContext.fill();
		pContext.stroke();
		
		pContext.restore();
	}
	
	Car.prototype.drawLeftWheel = function(pContext)
	{
		pContext.save();		
		pContext.translate(-40, 40);
		this.drawWheel(pContext);
		pContext.restore();
	}
	
	Car.prototype.drawRightWheel = function(pContext)
	{
		pContext.save();		
		pContext.translate(40, 40);
		this.drawWheel(pContext);
		pContext.restore();
	}	
	
	Car.prototype.update = function()
	{
		var newCarSpeed = this.getCarSpeed();
		this.getPosition().add(newCarSpeed);
	}
	
	Car.prototype.drawCar = function(pContext)
	{
		pContext.save();
		pContext.lineWidth = 2;
		pContext.translate(this.getPosition().getX(), this.getPosition().getY());
		pContext.scale(this.getScale(), this.getScale());
		pContext.rotate(this.getRotationAngle());
		
		this.drawBody(pContext);
		this.drawLeftWheel(pContext);
		this.drawRightWheel(pContext);
		
		pContext.restore();
	}
	
	return Car; 
})();