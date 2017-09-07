var House = (function () 
{ 
	function House(pPosition, pDoorColour) 
	{ 
		this.setPosition(pPosition);
		this.setDoorColour(pDoorColour);
	};
	
	House.prototype.getPosition = function()
	{
		return this.mPosition;
	}
	
	House.prototype.setPosition = function (pPosition)
	{
		this.mPosition = pPosition;
	};
	
	House.prototype.getDoorColour = function()
	{
		return this.mDoorColour;
	}
	
	House.prototype.setDoorColour = function(pDoorColour)
	{
		this.mDoorColour = pDoorColour;
	}
	
	House.prototype.drawWall = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#ffffff';
		pContext.strokeStyle = '#000000';
        pContext.fillRect(this.getPosition().getX(), this.getPosition().getY()+100, 200, 125);
        pContext.strokeRect(this.getPosition().getX(), this.getPosition().getY()+100, 200, 125);
	}
	
	House.prototype.drawRoof = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#A52A2A';
		pContext.strokeStyle = '#000000';
		pContext.moveTo(this.getPosition().getX(), this.getPosition().getY()+100);
		pContext.lineTo(this.getPosition().getX()+100, this.getPosition().getY());
		pContext.lineTo(this.getPosition().getX()+200, this.getPosition().getY()+100);
		pContext.closePath();	
		pContext.fill();
		pContext.stroke();
	}
	
	House.prototype.drawLeftWindow = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#4F94CD';
        pContext.strokeStyle = '#000000';
        pContext.fillRect(this.getPosition().getX()+15, this.getPosition().getY()+125, 45, 65);
        pContext.strokeRect(this.getPosition().getX()+15, this.getPosition().getY()+125, 45, 65);
        pContext.moveTo(this.getPosition().getX()+15, this.getPosition().getY()+157.5);
        pContext.lineTo(this.getPosition().getX()+60, this.getPosition().getY()+157.5);
        pContext.moveTo(this.getPosition().getX()+37.5, this.getPosition().getY()+125);
        pContext.lineTo(this.getPosition().getX()+37.5, this.getPosition().getY()+190);
        pContext.fill();
		pContext.stroke();
	}
	
	House.prototype.drawRightWindow = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = '#4F94CD';
        pContext.strokeStyle = '#000000';
        pContext.fillRect(this.getPosition().getX()+140, this.getPosition().getY()+125, 45, 65);
        pContext.strokeRect(this.getPosition().getX()+140, this.getPosition().getY()+125, 45, 65);
        pContext.moveTo(this.getPosition().getX()+140, this.getPosition().getY()+157.5);
        pContext.lineTo(this.getPosition().getX()+185, this.getPosition().getY()+157.5);
        pContext.moveTo(this.getPosition().getX()+162.5, this.getPosition().getY()+125);
        pContext.lineTo(this.getPosition().getX()+162.5, this.getPosition().getY()+190);
        pContext.fill();
		pContext.stroke();	
	}
	
	House.prototype.drawDoor = function(pContext)
	{
		pContext.beginPath();
		pContext.fillStyle = this.getDoorColour();
		pContext.strokeStyle = '#000000';
        pContext.fillRect(this.getPosition().getX()+75, this.getPosition().getY()+150, 50, 75);
        pContext.strokeRect(this.getPosition().getX()+75, this.getPosition().getY()+150, 50, 75);
	}
	
	House.prototype.drawHouse = function(pContext)
	{
		this.drawWall(pContext);
		this.drawRoof(pContext);
		this.drawLeftWindow(pContext);
		this.drawRightWindow(pContext);
		this.drawDoor(pContext);
	}
	
	return House; 
})();