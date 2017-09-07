//check to see if the browser supports the addEventListerner function

if (window.addEventListener) {
	window.addEventListener('load', onLoad,	false);
}

// the windows load event handler
function onLoad() {
	var canvas;
	var context;
	
	var topNode;
	var exampleFaultTree;
	var currentFaultTreeIndex;
	var isDown = false;	
	
	var canvasPan = new Vector (0, 0);	
	var canvasScale = 1;
	
	var mousePosition = new Vector (0, 0);
	var previousMousePosition = new Vector (0, 0);
	
	// lerp values
	var pan = new Vector (0, 0);
	var zoom = 1;
	
	
	function setupEventHandlers ()
	{
		// check to see if the browser supports 
		// the addEventListener function 
		if (window.addEventListener) { 
			window.addEventListener('keydown', onKeyDown, false);
			canvas.addEventListener('mousedown', onMouseDown, false);
			canvas.addEventListener('mouseup', onMouseUp, false);
			canvas.addEventListener('mousemove', onMouseMove, false);
			canvas.addEventListener('mousewheel', onMouseWheel, false);
		}		
	}
	
	function onKeyDown(event) { 
		var keyCode = event.keyCode;
		switch(keyCode) {
			case 37:
				// left arrow cycle backwards through fault tree
				exampleFaultTree.decrementCurrentFaultTreeIndex();
				centerFaultTree();
				break;
			case 39:
				// right arrow cycle fowards through fault tree
				exampleFaultTree.incrementCurrentFaultTreeIndex();
				centerFaultTree();
				break;
			case 90:		
				// z fits tree to canvas
				fitFaultTreeToCanvas();	
				centerFaultTree();
				break;
			case 72:		
				// h - help dialogue
				alert("\"Left\" and \"right\" arrows cycle through the example fault trees provided \n \"Z\" fits the fault tree to canvas \n Hold the left mouse button and drag to move the canvas around \n Scroll the mouse wheel 						to zoom in and out - the canvas will also pan to the mouse position \n Press \"H\" to bring up this help dialog again");			
				break;
		} 
	}
	
	function onMouseDown (event) {		
		isDown = true;
	}
	
	function onMouseUp (event) {		
		isDown = false;
	}
	
	function onMouseMove(event) {
		mousePosition = new Vector (event.clientX, event.clientY);
		
		// when mouse is clicked & moving
		// get difference between this frame mouse position and last frame
		// adds to canvas panning vector
		if (isDown == true) {
			var difference = new Vector(0, 0);
			difference = mousePosition.copy();
			difference.subtract(previousMousePosition);
			canvasPan.add(difference);
		}
		
		// sets last frame mouse position to this frame
		previousMousePosition = mousePosition;
	}
	
	function onMouseWheel (event) {
		var wheelAmount = event.wheelDelta;
		mousePosition = new Vector (event.clientX, event.clientY);
		
		canvasPan.subtract(mousePosition);
		canvasPan.divide(canvasScale);
		
		// sets lower increments
		wheelAmount = wheelAmount / 10000;
		
		// limits zoom
		canvasScale = canvasScale - wheelAmount;
		if (canvasScale <= 0.1)
			canvasScale = 0.1;
		if (canvasScale >= 5)
			canvasScale = 5;
		
		canvasPan.multiply(canvasScale);
		canvasPan.add(mousePosition);
	}
	
	function fitFaultTreeToCanvas () {
		var xRatio =  canvas.width / topNode.getWidth();
		var yRatio = canvas.height / topNode.getHeight();
		
		// gets lowest value and applies to scale
		canvasScale = Math.min(xRatio, yRatio);				
	}
	
	// this function will initialise our variables
	function initialise () {
		// Find the canvas element using its id attribute.
		canvas = document.getElementById('canvas');
		
		// if it can't be found
		if (!canvas) {
			// make a message box pop up with an error
			alert('Error: I cannot find the canvas element');
			return;
		}
		
		// check if there is a getContext function
		if (!canvas.getContext)	{
			// make a message box pop up with the error.
			alert ('Error: no canvas.getContext!');
			return;
		}
		
		// Get the 2D canvas context.
		context = canvas.getContext('2d');
		if (!context) {
			alert('Error: failed to getContext!');
			return;
		}		
		
		// set canvas width / height to window width / height
		canvas.width = window.innerWidth;
		canvas.height = window.innerHeight;
		
		exampleFaultTree = new ExampleFaultTrees();		
		
		// set canvas origin to centre
		centerFaultTree();
		
		// help dialogue
		
		alert("\"Left\" and \"right\" arrows cycle through the example fault trees provided \n \"Z\" fits the fault tree to canvas \n Hold the left mouse button and drag to move the canvas around \n Scroll the mouse wheel 						to zoom in and out - the canvas will also pan to the mouse position \n Press \"H\" to bring up this help dialog again");	
	}
	
	function centerFaultTree () {
		canvasPan.setX(canvas.width / 2);
		canvasPan.setY(0);
	}
	
	function drawBackground () {
		context.beginPath();			
		context.fillStyle = '#C2C2C2';		
		context.fillRect(0, 0, canvas.width, canvas.height);
	}
	
	function drawMinimapRect () {
		context.beginPath();			
		context.strokeStyle = '#FF0000';	
		context.lineWidth = 10;
		context.strokeRect(0, 0, canvas.width, canvas.height);
	}
	
	function draw () {		
		currentFaultTreeIndex = exampleFaultTree.getCurrentFaultTreeIndex();
		topNode = exampleFaultTree.getFaultTree(currentFaultTreeIndex);
		topNode.draw(context);
	}
	
	function drawOriginCross() {
		context.beginPath();
		context.strokeStyle = '#000000';
		context.moveTo(-20, 0);
		context.lineTo(20, 0)
		context.moveTo(0, -20);
		context.lineTo(0, 20)
		context.stroke();
	}
	
	function lerp (a, b, alpha) {
		return a + (b - a) * alpha;	
	}
	
	function gameLoop() {	
		
		context.save();
		
		zoom = lerp(zoom, canvasScale, 0.05);
		
		pan.setX(lerp(pan.getX(), canvasPan.getX(), 0.05));
		pan.setY(lerp(pan.getY(), canvasPan.getY(), 0.05));
		
		//console.log (zoom);
		//console.log (canvasScale);
		
		console.log (pan.getX());
		console.log (pan.getY());
		
		// set canvas width / height to window width / height
		canvas.width = window.innerWidth;
		canvas.height = window.innerHeight
		
		drawBackground();
		
		context.restore();		
		
		// panning & zooming
		context.save();	
		context.translate(pan.getX(), pan.getY());		
		context.scale(zoom, zoom);		
		
		draw();		
		context.restore();
		
		// drawing the minimap background
		context.save();
		context.scale(0.25, 0.25);
		context.rect(0, 0, canvas.width, canvas.height);
		context.stroke();
		context.clip();		
		drawBackground();		
		
		// drawing fault tree in the middle of the minimap
		context.save();
		context.translate(canvas.width / 2, canvas.height / 2);
		context.scale(0.1, 0.1);
		draw();
		context.restore();
		
		// draw border around current viewport on minimap	
		context.save();
		context.translate(canvas.width / 2, canvas.height / 2);
		context.scale(0.1, 0.1);
		context.scale(1 / zoom, 1 / zoom);
		context.translate(-pan.getX(), -pan.getY());
		
		drawMinimapRect();
		context.restore();
		
		context.restore();
		
		webkitRequestAnimationFrame(gameLoop);
	}
		
	// calling functions
	initialise();		
	setupEventHandlers();
	gameLoop();
}