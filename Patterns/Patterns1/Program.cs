using System;
using Raylib_cs;

int windowHeight = 1080;
int windowWidth = 1920;

Random rng = new Random();
double acac = Math.Sqrt(2);
double angleChange = 0;
double angle = 0;
float baseLengthOfLine = .1f;
float lengthOfLine = baseLengthOfLine;
float widthOfLine = 1;
double xPos = 0;
double yPos = 0;
double oldXPos;
double oldYPos;
List<double> xPositions = new List<double>();
List<double> yPositions = new List<double>();
int simLength = 0;
Color color = new Color(255, 0, 0, 255);
List<Color> colors = new List<Color>();
Raylib.SetTargetFPS(60);
bool dataShowing = false;
int movement = 30;
int xOffset = 0;
int yOffset = 0;
int high = -10000;
int low = 100000;
int right = -10000;
int left = 10000;
bool doAutoZoom = true;

Raylib.InitWindow(windowWidth, windowHeight, "Pattern");

Raylib.ToggleFullscreen();

while (!Raylib.WindowShouldClose())
{
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
	{
		Skip(10000);
	}
	else if (Raylib.IsKeyPressed(KeyboardKey.KEY_D))
	{
		Skip(100000);
	}
	else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F))
	{
		Skip(1000);
	}
	else
	{
		NewPos();
		simLength++;
	}

	Draw();
	if (doAutoZoom)
	{
		AutoZoom(70);
	}

	if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
	{
		try
		{
			Raylib.TakeScreenshot(Convert.ToString(acac) + ".png");
		}
		catch
		{
			bool done = false;
			int num = 1;
			while (!done)
			{
				try
				{
					Raylib.TakeScreenshot(Convert.ToString(acac) + "(" + num + ").png");
				}
				catch
				{
					num++;
				}
			}
		}
	}

	if (Raylib.IsKeyPressed(KeyboardKey.KEY_T))
	{
		NewColors();
	}

	if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
	{
		if (doAutoZoom)
			doAutoZoom = false;
		else
			doAutoZoom = true;
	}

	if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
	{
		ResetPattern();
	}

	if (!doAutoZoom)
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_EQUAL) && Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
		{
			ZoomIn(2);
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_MINUS))
		{
			ZoomOut(2);
		}
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
		{
			yOffset -= movement;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
		{
			yOffset += movement;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
		{
			xOffset -= movement;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
		{
			xOffset += movement;
		}
		else if (Raylib.IsKeyPressed((KeyboardKey.KEY_C)))
		{
			AutoZoom(70);
		}
	}

	if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
	{
		Restart();
	}
}
void Restart()
{
	acac = rng.NextDouble() * 5;
	angleChange = 0;
	angle = 0;
	widthOfLine = 1;
	xPos = 0;
	yPos = 0;
	oldXPos = xPos;
	oldYPos = yPos;
	xPositions = new List<double>();
	yPositions = new List<double>();
	simLength = 1;
	color = new Color(255, 0, 0, 255);
	colors = new List<Color>();
	ResetOffsets();
	high = -10000;
	low = 10000;
	right = -10000;
	left = 10000;
	lengthOfLine = baseLengthOfLine;
}
void ResetOffsets()
{
	xOffset = 0;
	yOffset = 0;
}
void ZoomIn(float zoom)
{
	Console.WriteLine("Zoom");
	xPos = 0;
	yPos = 0;
	lengthOfLine *= zoom;
	angle = 0;
	angleChange = 0;
	xPositions = new List<double>();
	yPositions = new List<double>();
	CalculatePoints(simLength - 1);
}
void ZoomOut(float zoom)
{
	Console.WriteLine("Zoom Out");
	xPos = 0;
	yPos = 0;
	lengthOfLine /= zoom;
	angle = 0;
	angleChange = 0;
	xPositions = new List<double>();
	yPositions = new List<double>();
	CalculatePoints(simLength - 1);
}
void NewColors()
{
	colors = new List<Color>();
	int colorRandom = rng.Next(0, 3);
	int r;
	int g;
	int b;
	switch (colorRandom)
	{
		case 0:
			r = rng.Next(1, 256);
			color = new Color(r, 255 - r, 0, 255);
			break;
		case 1:
			g = rng.Next(1, 256);
			color = new Color(0, g, 255 - g, 255);
			break;
		case 2:
			b = rng.Next(1, 256);
			color = new Color(255 - b, 0, b, 255);
			break;
	}
	colors.Add(color);
	colorRandom = rng.Next(0, 2);
	switch (colorRandom)
	{
		case 0:
			{
				for (int i = 0; i < simLength - 1; i++)
				{
					if (colors[i].r > 0 && colors[i].b == 0)
					{
						color.r--;
						color.g++;
					}
					else if (colors[i].g > 0 && colors[i].r == 0)
					{
						color.g--;
						color.b++;
					}
					else if (colors[i].b > 0 && colors[i].g == 0)
					{
						color.b--;
						color.r++;
					}
					colors.Add(color);
				}
				break;
			}
		case 1:
			{
				for (int i = 0; i < simLength - 1; i++)
				{
					if (colors[i].r > 0 && colors[i].g == 0)
					{
						color.r--;
						color.b++;
					}
					else if (colors[i].g > 0 && colors[i].b == 0)
					{
						color.g--;
						color.r++;
					}
					else if (colors[i].b > 0 && colors[i].r == 0)
					{
						color.b--;
						color.g++;
					}
					colors.Add(color);
				}
				break;
			}
	}
}
void DrawData()
{
	Raylib.DrawText("FPS: " + Raylib.GetFPS(), 0, 0, 12, Color.WHITE);
	Raylib.DrawText("Angle Modifier: " + acac, 0, 12, 12, Color.WHITE);
	Raylib.DrawText("System Life: " + (simLength - 1), 0, 24, 12, Color.WHITE);
}
void Draw()
{
	Raylib.BeginDrawing();
	Raylib.ClearBackground(Color.BLACK);
	if (!doAutoZoom)
	{
		for (int i = 0; i < simLength - 2; i++)
		{
			if (((xPositions[i] + xOffset < windowWidth && xPositions[i] + xOffset > 0) || (xPositions[i + 1] + xOffset < windowWidth && xPositions[i + 1] + xOffset > 0)) && ((yPositions[i + 1] + yOffset < windowHeight && yPositions[i + 1] + yOffset > 0) || (yPositions[i] + yOffset < windowHeight && yPositions[i] + yOffset > 0)))
				Raylib.DrawLineEx(new System.Numerics.Vector2(Convert.ToInt32(xPositions[i] + xOffset), Convert.ToInt32(yPositions[i] + yOffset)), new System.Numerics.Vector2(Convert.ToInt32(xPositions[i + 1] + xOffset), Convert.ToInt32(yPositions[i + 1]) + yOffset), widthOfLine, colors[i]);
		}
	}
	else
	{
		for (int i = 0; i < simLength - 2; i++)
		{
			Raylib.DrawLineEx(new System.Numerics.Vector2(Convert.ToInt32(xPositions[i] + xOffset), Convert.ToInt32(yPositions[i] + yOffset)), new System.Numerics.Vector2(Convert.ToInt32(xPositions[i + 1] + xOffset), Convert.ToInt32(yPositions[i + 1]) + yOffset), widthOfLine, colors[i]);
		}
	}
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
	{
		if (!dataShowing)
		{
			dataShowing = true;
		}
		else
		{
			dataShowing = false;
		}
	}
	if (dataShowing)
	{
		DrawData();
	}
	Raylib.EndDrawing();
}


void NewPos()
{
	xPositions.Add(xPos);
	yPositions.Add(yPos);
	if (xPos > right)
		right = Convert.ToInt32(xPos);
	else if (xPos < left)
		left = Convert.ToInt32(xPos);
	if (yPos > high)
		high = Convert.ToInt32(yPos);
	else if (yPos < low)
		low = Convert.ToInt32(yPos);
	oldXPos = xPos;
	oldYPos = yPos;
	xPos = Math.Sin(angle) * Convert.ToDouble(lengthOfLine) + oldXPos;
	yPos = Math.Cos(angle) * Convert.ToDouble(lengthOfLine) + oldYPos;
	angleChange += acac;
	angle += angleChange;
	if (color.r > 0 && color.b == 0)
	{
		color.r--;
		color.g++;
	}
	else if (color.g > 0 && color.r == 0)
	{
		color.g--;
		color.b++;
	}
	else if (color.b > 0 && color.g == 0)
	{
		color.b--;
		color.r++;
	}
	colors.Add(color);
}
void Skip(int skip)
{
	for (int i = 0; i < skip; i++)
	{
		NewPos();
		simLength++;
	}
}
void AutoZoom(int space)
{
	float yRange = (high - low) + space;
	float xRange = (right - left) + space;
	yOffset = ((low + (windowHeight - high)) / 2) - low;
	xOffset = ((left + (windowWidth - right)) / 2) - left;
	if (yRange > windowHeight)
	{
		float ratio = yRange / windowHeight;
		ZoomOut(ratio);
	}
	else if (yRange + 50 < windowHeight && xRange + 50 < windowWidth)
	{
		float ratio = windowHeight / yRange;
		ZoomIn(ratio);
	}
	if (xRange > windowWidth)
	{
		float ratio = xRange / windowWidth;
		ZoomOut(ratio);
	}
	else if (xRange + 50 < windowWidth && yRange + 50 < windowHeight)
	{
		float ratio = windowWidth / xRange;
		ZoomIn(ratio);
	}
}

void CalculatePoints(int numOfPoints)
{
	xPositions = new List<double>();
	yPositions = new List<double>();
	angleChange = 0;
	angle = 0;
	xPos = 0;
	yPos = 0;
	oldXPos = xPos;
	oldYPos = yPos;
	high = -10000;
	low = 10000;
	right = -10000;
	left = 10000;

	for (int i = 0; i < numOfPoints; i++)
	{
		NewPos();
	}
}

void ResetPattern()
{
	angleChange = 0;
	angle = 0;
	widthOfLine = 1;
	xPos = 0;
	yPos = 0;
	oldXPos = xPos;
	oldYPos = yPos;
	xPositions = new List<double>();
	yPositions = new List<double>();
	simLength = 1;
	color = new Color(255, 0, 0, 255);
	colors = new List<Color>();
	ResetOffsets();
	high = -10000;
	low = 10000;
	right = -10000;
	left = 10000;
	lengthOfLine = baseLengthOfLine;
}
Raylib.ToggleFullscreen();
Raylib.CloseWindow();