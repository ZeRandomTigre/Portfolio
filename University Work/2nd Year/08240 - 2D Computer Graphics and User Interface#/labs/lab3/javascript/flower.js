var Flower = (function () 
{ 
	function Flower(pPosition, pColour, pScale, pRotationAngle) 
	{ 
		this.setPosition(pPosition);
		this.setColour(pColour);
		this.setScale(pScale);
		this.setRotationAngle(pRotationAngle);
	}
	
	Flower.prototype.getPosition = function()
	{
		return this.mPosition;
	}
	
	Flower.prototype.setPosition = function (pPosition)
	{
		this.mPosition = pPosition;
	}
	
	Flower.prototype.getColour = function()
	{
		return this.mColour;
	}
	
	Flower.prototype.setColour = function(pColour)
	{
		this.mColour = pColour;
	}
	
	Flower.prototype.getScale = function()
	{
		return this.mScale;
	}
	
	Flower.prototype.setScale = function (pScale)
	{
		this.mScale = pScale;
	}
	
	Flower.prototype.getRotationAngle = function()
	{
		return this.mRotationAngle;
	}
	
	Flower.prototype.setRotationAngle = function (pRotationAngle)
	{
		this.mRotationAngle = pRotationAngle;
	}
			
	Flower.prototype.drawStem = function (pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = '#005900';
		pContext.stokeStyle = '#000000';
		pContext.fillRect(-10, 0, 20, 200);
		pContext.strokeRect(-10, 0, 20, 200);
		pContext.restore();
	}

	Flower.prototype.drawHead = function (pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = '#3e1a1a';
		pContext.strokeStyle = '#000000';	 
		pContext.arc(0, 0, 50, 0, 2 * Math.PI);
		pContext.fill();
		pContext.stroke();
		pContext.restore();
	}
	
	Flower.prototype.drawPetal = function(pContext)
	{
		pContext.save();
		pContext.beginPath();
		pContext.fillStyle = this.getColour();
		pContext.strokeStyle = '#000000';
		pContext.arc(0, 0, 30, Math.PI, 2 * Math.PI);
		pContext.fill();
		pContext.stroke();
		pContext.restore();
	}	

	Flower.prototype.drawPetals = function (pContext)
	{
		var numberOfPetals = 10;
		var angleBetweenPetals = (2 * Math.PI) / numberOfPetals;
		for (var i = 0; i < numberOfPetals; i++)
		{
			var currentAngle = angleBetweenPetals * i;
			pContext.save();
			pContext.rotate(currentAngle);
			pContext.translate(0, -45);
			this.drawPetal(pContext);
			pContext.restore();
		}
	}
	
	Flower.prototype.drawFlower = function (pContext)
	{
		pContext.save();
		pContext.lineWidth = 2;
		pContext.translate(this.getPosition().getX(), this.getPosition().getY());
		pContext.scale(this.getScale(), this.getScale());
		pContext.rotate(this.getRotationAngle());
				
		this.drawStem(pContext);				
		this.drawPetals(pContext);
		this.drawHead(pContext);		
		pContext.restore();
	}
	
	return Flower; 
})();