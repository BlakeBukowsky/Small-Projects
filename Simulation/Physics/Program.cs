using System;
using Raylib_cs;

Random rng = new Random();

int windowSizeX = 1080;
int windowSizeY = 720;

float mass = rng.Next(5, 20);
float size = rng.Next(20, 300);
float xForce = rng.Next(-2000, 2000);
float yForce = rng.Next(-2000, 2000);
float xVel = 0;
float yVel = 0;
float xPos = rng.Next(Convert.ToInt32(size / 2), Convert.ToInt32(windowSizeX - size / 2));
float yPos = rng.Next(Convert.ToInt32(size / 2), Convert.ToInt32(windowSizeY - size / 2));

float timeTweenChange = 5;
float tilChange = timeTweenChange;

float bounce = rng.NextSingle();

Raylib.InitWindow(windowSizeX, windowSizeY, "Physics Project");
while (!Raylib.WindowShouldClose())
{
	tilChange -= Raylib.GetFrameTime();
	if (tilChange < 0)
	{
		tilChange = timeTweenChange;
		xForce = rng.Next(-2000, 2000);
		yForce = rng.Next(-2000, 2000);
		mass = rng.Next(5, 20);
		size = rng.Next(20, 300);
		bounce = rng.NextSingle();
		if (xPos + size / 2 > windowSizeX)
			xPos = windowSizeX - size / 2;
		if (xPos - size / 2 < 0)
			xPos = size / 2;
		if (yPos + size / 2 > windowSizeY)
			yPos = windowSizeY - size / 2;
		if (yPos - size / 2 < 0)
			yPos = size / 2;
	}
	xVel += (xForce / mass) * Raylib.GetFrameTime();
	yVel += (yForce / mass) * Raylib.GetFrameTime();
	xPos += xVel * Raylib.GetFrameTime();
	yPos += yVel * Raylib.GetFrameTime();
	if ((xPos - size / 2 < 0 && xVel < 0) || (xPos + size / 2 > windowSizeX && xVel > 0))
	{
		xVel *= -bounce;
	}
	if ((yPos - size / 2 < 0 && yVel < 0) || (yPos + size / 2 > windowSizeY && yVel > 0))
	{
		yVel *= -bounce;
	}
	Raylib.BeginDrawing();
	Raylib.ClearBackground(Color.BLACK);
	Raylib.DrawCircle(Convert.ToInt32(Math.Round(xPos)), Convert.ToInt32(Math.Round(yPos)), size / 2, Color.WHITE);
	Raylib.DrawLineEx(new System.Numerics.Vector2(xPos, yPos), new System.Numerics.Vector2(xPos + xForce / 10, yPos + yForce / 10), 5, Color.BLUE);
	Raylib.DrawLineEx(new System.Numerics.Vector2(xPos, yPos), new System.Numerics.Vector2(xPos + xVel / 2, yPos + yVel / 2), 5, Color.RED);
	Raylib.DrawText("X Force: " + xForce, 0, 12, 12, Color.WHITE);
	Raylib.DrawText("Y Force: " + yForce, 0, 24, 12, Color.WHITE);
	Raylib.DrawText("FPS: " + Raylib.GetFPS(), 0, 0, 12, Color.WHITE);
	Raylib.DrawText("Mass: " + mass, 0, 36, 12, Color.WHITE);
	Raylib.DrawText("Radius: " + (size / 2), 0, 48, 12, Color.WHITE);
	Raylib.DrawText("Density: " + mass / ((size * size) * Math.PI), 0, 64, 12, Color.WHITE);
	Raylib.DrawText("Bounce: " + bounce, 0, 76, 12, Color.WHITE);
	Raylib.EndDrawing();
}
Raylib.CloseWindow();