//check to see if the browser supports the addEventListerner function

if (window.addEventListener)
{
	window.addEventListener
	(
		'load', //this is the load event
		onLoad, // this is the event handler we are going to write
		false // useCapture boolean value
	);
}

// the windows load event handler
function onLoad()
{
	var canvas;
	var context;
	var textPosition
	
	// this function will initialise our variables
	function initialise ()
	{
		// Find the canvas element using its id attribute.
		canvas = document.getElementById('canvas');
		// if it can't be found
		if (!canvas)
		{
			// make a message box pop up with an error
			alert('Error: I cannot find the canvas element');
			return;
		}
		
		// check if there is a getContext function
		if (!canvas.getContext)
		{
			// make a message box pop up with the error.
			alert ('Error: no canvas.getContext!');
			return;
		}
		
		// Get the 2D canvas context.
		context = canvas.getContext('2d');
		if (!context)
		{
			alert('Error: failed to getContext!');
			return;
		}
		textPosition = new Vector(150,100);
	}
	
	// this function will actually draw on the canvas
	function draw(pPosition)
	{
		//set the draw fill style colour to black
		context.fillStyle = "#000000";
		// fill the canvas with black 
		context.fillRect(0, 0, canvas.width, canvas.height);
		// choose a font for out message
		context.font = "40pt Calibri";
		//set the draw fill colour to white
		context.fillStyle = "#ffffff";
		//draw the text a the specified position
		context.fillText("Hello World!", pPosition.getX(), pPosition.getY());
	}
	
	// call the initialise and draw functions
	initialise();
	draw(textPosition);
}