var House = (function () 
{ 
	function House(pPosition, pDoorColour, pScale, pRotationAngle) 
	{ 
		this.setPosition(pPosition);
		this.setDoorColour(pDoorColour);
		this.setScale(pScale);
		this.setRotationAngle(pRotationAngle);
	}
	
	House.prototype.getPosition = function()
	{
		return this.mPosition;
	}
	
	House.prototype.setPosition = function (pPosition)
	{
		this.mPosition = pPosition;
	}
	
	House.prototype.getDoorColour = function()
	{
		return this.mDoorColour;
	}
	
	House.prototype.setDoorColour = function(pDoorColour)
	{
		this.mDoorColour = pDoorColour;
	}
	
	House.prototype.getScale = function()
	{
		return this.mScale;
	}
	
	House.prototype.setScale = function (pScale)
	{
		this.mScale = pScale;
	}
	
	House.prototype.getRotationAngle = function()
	{
		return this.mRotationAngle;
	}
	
	House.prototype.setRotationAngle = function (pRotationAngle)
	{
		this.mRotationAngle = pRotationAngle;
	}
	
	House.prototype.drawWall = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#ffffff';
		pContext.strokeStyle = '#000000';
        pContext.fillRect( 0,  100, 200, 125);
        pContext.strokeRect( 0,  100, 200, 125);
	}
	
	House.prototype.drawRoof = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#A52A2A';
		pContext.strokeStyle = '#000000';
		pContext.moveTo( 0,  100);
		pContext.lineTo( 100,  0);
		pContext.lineTo( 200,  100);
		pContext.closePath();	
		pContext.fill();
		pContext.stroke();
	}	
	
	House.prototype.drawWindow = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#4F94CD';
        pContext.strokeStyle = '#000000';
        pContext.fillRect( 0,  0, 45, 65);
        pContext.strokeRect( 0,  0, 45, 65);
        pContext.moveTo( 0,  32.5);
        pContext.lineTo( 45,  32.5);
        pContext.moveTo( 22.5,  0);
        pContext.lineTo( 22.5,  65);
        pContext.fill();
		pContext.stroke();
	}
	
	
	House.prototype.drawLeftWindow = function(pContext)
	{
		pContext.save();
		pContext.translate(15, 135);
		this.drawWindow(pContext);
		pContext.restore();
	}
	
	House.prototype.drawRightWindow = function(pContext)
	{
		pContext.save();
		pContext.translate(140, 135);
		this.drawWindow(pContext);
		pContext.restore();
	}
	
	House.prototype.drawDoor = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = this.getDoorColour();
		pContext.strokeStyle = '#000000';
        pContext.fillRect( 75,  150, 50, 75);
        pContext.strokeRect( 75,  150, 50, 75);
	}
	
	House.prototype.drawHouse = function(pContext)
	{
		pContext.save();
		pContext.lineWidth = 2;
		pContext.translate(this.getPosition().getX(), this.getPosition().getY());
		pContext.scale(this.getScale(), this.getScale());
		pContext.rotate(this.getRotationAngle());
		this.drawWall(pContext);
		this.drawRoof(pContext);
		this.drawLeftWindow(pContext);
		this.drawRightWindow(pContext);
		this.drawDoor(pContext);
		pContext.restore();
	}
	
	return House; 
})();