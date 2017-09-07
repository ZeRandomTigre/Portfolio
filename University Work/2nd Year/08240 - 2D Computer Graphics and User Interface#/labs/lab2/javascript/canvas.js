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
	var houses;
	
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
		
		houses = new Array();
    	var doorColour = '#008000';
		var count = 0;
		for (var j = 0; j < 4; j = j + 1)
		{
			var yPosition = j * 225;
			for (var i = 0; i < 4; i = i + 1)
			{
				var xPosition = i * 200;
				var housePostion = new Vector(xPosition, yPosition);
				if (count % 2 == 0)
				{
					doorColour = '#862A51';
				}
				else
				{
					doorColour = '#008000';
				}
				var house = new House(housePostion, doorColour);
				houses.push(house);				
				count = count + 1;
			}
		}
	}
	
	// this function will actually draw on the canvas
	function drawBackground()
	{
		context.fillStyle = "#d3d3d3"; // mid grey
		context.fillRect(0, 0, canvas.width, canvas.height);        
        context.lineWidth = 1.5;
	}
    
    function draw()
    {
		for (var i = 0; i < houses.length; i = i + 1)
		{
			var house = houses[i];
			house.drawHouse(context);
		}
    }
		
	// call the initialise and draw functions
	initialise();
	drawBackground();
	draw();
}