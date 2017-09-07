var Sun = (function () 
{ 
	function Sun(pPosition, pColour, pScale, pRotationAngle) 
	{ 
		this.setPosition(pPosition);
		this.setColour(pColour);
		this.setScale(pScale);
		this.setRotationAngle(pRotationAngle);
	}
	
	Sun.prototype.getPosition = function()
	{
		return this.mPosition;
	}
	
	Sun.prototype.setPosition = function (pPosition)
	{
		this.mPosition = pPosition;
	}
	
	Sun.prototype.getColour = function()
	{
		return this.mColour;
	}
	
	Sun.prototype.setColour = function(pColour)
	{
		this.mColour = pColour;
	}
	
	Sun.prototype.getScale = function()
	{
		return this.mScale;
	}
	
	Sun.prototype.setScale = function (pScale)
	{
		this.mScale = pScale;
	}
	
	Sun.prototype.getRotationAngle = function()
	{
		return this.mRotationAngle;
	}
	
	Sun.prototype.setRotationAngle = function (pRotationAngle)
	{
		this.mRotationAngle = pRotationAngle;
	}
	
	Sun.prototype.drawCircle = function (pContext)
	{
		pContext.save();
		pContext.fillStyle = this.getColour();
		pContext.strokeStyle = '#000000';
		pContext.arc(0, 0, 50, 0, 2 * Math.PI);
		pContext.fill();
		pContext.scale(1/this.getScale(), 1/this.getScale());
		pContext.stroke();
		pContext.restore();		
	}
	
	Sun.prototype.drawRay = function (pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = this.getColour();
		pContext.strokeStyle = '#000000';
		pContext.moveTo(-10, 0);
		pContext.lineTo(0, 40);
		pContext.lineTo(10, 0);
		pContext.closePath();
		pContext.fill();
		pContext.stroke();
		pContext.restore();
	}
	
	Sun.prototype.drawRays = function (pContext)
	{
		var numberOfRays = 12;
		var angleBetweenRays = (2 * Math.PI) / numberOfRays
		for (var i = 0; i < numberOfRays; i++)
		{
			var currentAngle = angleBetweenRays * i;
			pContext.save();
			pContext.rotate(currentAngle);
			pContext.translate(0, 55);
			this.drawRay(pContext);
			pContext.restore();	
		}

	}	
	
	Sun.prototype.drawSun = function (pContext)
	{
		pContext.save();
		pContext.lineWidth = 2;
		pContext.translate(this.getPosition().getX(), this.getPosition().getY());
		pContext.scale(this.getScale(), this.getScale());
		pContext.rotate(this.getRotationAngle());
		
		this.drawCircle(pContext);
		this.drawRays(pContext);
		
		pContext.restore();
	}
	
	return Sun; 
})();