using Raylib_cs;

Random rng = new Random();
int seqL = 12;
float[] sequence = new float[seqL];

List<float> xList = new List<float>();
List<float> yList = new List<float>();
float thick = .2f;
float len = 5;
for (int i = 0; i < seqL; i++)
{
	sequence[i] = ((float)(rng.NextDouble() * 350));
}
Raylib.InitWindow(1920, 1080, "Cull Patterns");

Color[] colors = new Color[] { Color.RED, Color.BLUE, Color.GREEN };
List<Color> allColors = new List<Color>();
int currentColor = 0;


int frame = 0;
float angle = 0;
float x = Raylib.GetScreenWidth() / 2;
float y = Raylib.GetScreenHeight() / 2;
float high = -10000;
float low = 10000;
float right = -10000;
float left = 10000;

int border = 50;

int xOff = 0;
int yOff = 0;

while (!Raylib.WindowShouldClose())
{
	NewPos();
	DrawLines();
	AutoCam();
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
		Skip(100);
	else if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
		Skip(1000);
	else if (Raylib.IsKeyPressed(KeyboardKey.KEY_D))
		Skip(10000);
	else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F))
		Skip(100000);
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
	{
		Reset(5);
	}
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
	{
		sequence = new float[seqL];
		for (int i = 0; i < seqL; i++)
			sequence[i] = ((float)(rng.NextDouble() * 350));
		Reset(5);
	}
	if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
	{
		Raylib.DrawText("FPS: " + Raylib.GetFPS(), 0, 0, 12, Color.WHITE);
		Raylib.DrawText("Sim Length: " + frame, 0, 12, 12, Color.WHITE);
		Raylib.DrawText("SeqL: " + seqL, 0, 24, 12, Color.WHITE);
	}
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
		Raylib.ToggleFullscreen();
	if (Raylib.IsKeyPressed(KeyboardKey.KEY_K))
		Raylib.TakeScreenshot(seqL.ToString() + " " + sequence[0].ToString() + ".png");
}

void NewPos()
{
	if (y > high)
		high = y;
	if (y < low)
		low = y;
	if (x > right)
		right = x;
	if (x < left)
		left = x;
	angle += sequence[frame % seqL];
	x += ((float)Math.Sin(angle) * len);
	y += ((float)Math.Cos(angle) * len);
	xList.Add(x);
	yList.Add(y);
	if (frame % seqL == 0)
		currentColor++;
	allColors.Add(colors[currentColor % colors.Length]);
	frame++;
}
void Skip(int skipLen)
{
	for (int i = 0; i < skipLen; i++)
	{
		NewPos();
	}
}
void DrawLines()
{
	Raylib.BeginDrawing();
	Raylib.ClearBackground(Color.BLACK);
	for (int i = 1; i < xList.Count; i++)
	{
		Raylib.DrawLineEx(new System.Numerics.Vector2(xList[i - 1] + xOff, yList[i - 1] + yOff), new System.Numerics.Vector2(xList[i] + xOff, yList[i] + yOff), thick, allColors[i]);
	}
	Raylib.EndDrawing();
}

void Reset(float leng)
{
	xList = new List<float>();
	yList = new List<float>();
	thick = .2f;
	len = leng;
	frame = 0;
	angle = 0;
	high = -10000;
	low = 10000;
	right = -10000;
	left = 10000;
	x = Raylib.GetScreenWidth() / 2;
	y = Raylib.GetScreenHeight() / 2;
	colors = new Color[] { Color.RED, Color.BLUE, Color.GREEN };
	currentColor = 0;
	xOff = 0;
	yOff = 0;
}

void ReCalc(int what)
{
	Reset(len);
	Skip(what);
}

void AutoCam()
{
	float xRange = right - left + border * 2;
	float yRange = high - low + border * 2;
	if (xRange > Raylib.GetScreenWidth() || xRange + 50 < Raylib.GetScreenWidth())
	{
		len *= Raylib.GetScreenWidth() / (xRange + 1);
		thick *= Raylib.GetScreenWidth() / (xRange + 1);
		ReCalc(frame);
		xRange = right - left + border * 2;
		yRange = high - low + border * 2;
	}
	if (yRange > Raylib.GetScreenHeight() || yRange + 50 < Raylib.GetScreenHeight())
	{
		len *= Raylib.GetScreenHeight() / (yRange + 1);
		thick *= Raylib.GetScreenHeight() / (yRange + 1);
		ReCalc(frame);
		xRange = right - left + border * 2;
		yRange = high - low + border * 2;
	}
	yOff += Convert.ToInt32(((low + yOff + (Raylib.GetScreenHeight() - high - yOff)) / 2) - low - yOff);
	xOff += Convert.ToInt32(((left + xOff + (Raylib.GetScreenWidth() - right - xOff)) / 2) - left - xOff);
}
Raylib.CloseWindow();