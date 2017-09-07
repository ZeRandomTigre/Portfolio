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
	var sun;
	var flowers;
	
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
		
		sun = new Sun(new Vector(-600, -250), '#FDB813', 1.5, 0);
		
		context.save();
		houses = new Array();
    	var doorColour = '#008000';
		var count = 0;
		var scale = 0.5;
		var rotationAngle = 0;
		for (var j = 0; j < 1; j = j + 1)
		{
			var yPosition = j * 225 + 157.5;
			for (var i = 0; i < 13; i = i + 1)
			{
				var xPosition = i * 125 - 800;
				var housePostion = new Vector(xPosition, yPosition);
				if (count % 2 == 0)
				{
					doorColour = '#862A51';
				}
				else
				{
					doorColour = '#008000';
				}
				var house = new House(housePostion, doorColour, scale, rotationAngle);
				houses.push(house);				
				count = count + 1;
			}
		}
		context.restore();
		
		context.save();		
		flowers = new Array();
		var flowerColour = '#b2b2ff';
		var count = 0;
		var scale = 0.1;
		var rotationAngle = 0;
		for (var j = 0; j < 3; j = j + 1)
		{
			var yPosition = j * 10 + 400;
			for (var i = 0; i < 75; i = i + 1)
			{
				var xPosition = i * 20 + - 750;
				var flowerPosition = new Vector(xPosition, yPosition);
				if (count % 2 == 0)
				{
					flowerColour = '#b2b2ff';
				}
				else
				{
					flowerColour = '#FFC0CB';
				}
				var flower = new Flower(flowerPosition, flowerColour, scale, rotationAngle);
				flowers.push(flower);				
				count = count + 1;
			}
		}
		context.restore();
	}
	
	function drawGround()
	{
		context.fillStyle = "#4DBD33";
		context.strokeStyle = '#000000';
		context.fillRect(0, canvas.height / 2, canvas.width, canvas.height / 2);
		context.strokeRect(0, canvas.height / 2, canvas.width, canvas.height / 2); 
        context.lineWidth = 2;
	}
	
	function drawSky()
	{
		context.fillStyle = "#7EC0EE"
		context.strokeStyle = '#000000';
		context.fillRect(0, 0, canvas.width, canvas.height * 0.75);
		context.strokeRect(0, 0, canvas.width, canvas.height * 0.75);  
        context.lineWidth = 2;
	}
	
	function drawRoad()
	{
		context.fillStyle = "#545454";
		context.strokeStyle = '#000000';
		context.fillRect(0, canvas.height - canvas.height / 6, canvas.width, canvas.height / 8);
		context.strokeRect(0, canvas.height - canvas.height / 6, canvas.width, canvas.height / 8); 
        context.lineWidth = 2;
	}
    
    function draw()
    {
		context.translate(canvas.width * 0.5, canvas.height * 0.5);
		for (var i = 0; i < houses.length; i = i + 1)
		{
			var house = houses[i];
			house.drawHouse(context);
		}
		sun.drawSun(context);
		
		for (var i = 0; i < flowers.length; i = i + 1)
		{
			var flower = flowers[i];
			flower.drawFlower(context);
		}
    }
		
	// call the initialise and draw functions
	initialise();
	drawGround();
	drawSky();
	drawRoad();
	draw();
}