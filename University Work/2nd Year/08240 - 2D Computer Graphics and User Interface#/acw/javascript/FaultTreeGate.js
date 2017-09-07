var FaultTreeGate = (function (_super) {
	__extends(FaultTreeGate, _super);
    function FaultTreeGate(pPosition) {
		_super.call(this);
        this.mChildren = new Array();
    }
	
    FaultTreeGate.prototype.getChild = function (pIndex) {
        return this.mChildren[pIndex];    
	}
	
	FaultTreeGate.prototype.numChildren = function () {
        return this.mChildren.length;
    }
	
    FaultTreeGate.prototype.addChild = function (pChild) {
        this.mChildren.push(pChild);
    }
	
    return FaultTreeGate;
})(FaultTreeNode);
