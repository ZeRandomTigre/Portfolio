var Vector = (function () {
	function Vector(pX, pY)	{
		this.setX(pX);
		this.setY(pY);
	}
	
	Vector.prototype.getX = function() {
		return this.mX;
	}
		
	Vector.prototype.getY = function() {
		return this.mY;
	}
	
	Vector.prototype.setX = function (pX) {
		this.mX = pX;
	}
	
	Vector.prototype.setY = function (pY) {
		this.mY = pY;
	}
	
	Vector.prototype.add = function (pVector) {
		this.mX = this.mX + pVector.mX;
		this.mY = this.mY + pVector.mY;
	}
	
	Vector.prototype.subtract = function (pVector) {
		this.mX = this.mX - pVector.mX;
		this.mY = this.mY - pVector.mY;
	}
	
	Vector.prototype.multiply = function(pScalar) {
		this.mX = this.mX * pScalar;
		this.mY = this.mY * pScalar;
	}
	
	Vector.prototype.divide = function(pScalar) {
		this.mX = this.mX / pScalar;
		this.mY = this.mY / pScalar;
	}
	
	Vector.prototype.getMagnitude = function() {
		this.mMagnitude = Math.sqrt((this.mX * this.mX) * (this.mY * this.mY));
		return this.mMagnitude;
	}
	
	Vector.prototype.normalise = function() {
		this.mX = this.mX / this.mMagnitude;
		this.mY = this.mY / this.mMagnitude;
	}
	
	Vector.prototype.copy = function () {
		return new Vector (this.mX, this.mY);
	}
	return Vector;
})();