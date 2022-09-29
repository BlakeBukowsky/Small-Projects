using System;
using Raylib_cs;
namespace Fourier
{
	internal class Program
	{
		public static double[] sizes = new double[1000];
		public static double[] speeds = new double[1000];
		public static double[] rotation = new double[1000];
		public static List<double> x = new List<double>();
		public static List<double> y = new List<double>();
		public static Random rng = new Random();
		public static void Main(string[] args)
		{
			Raylib.InitWindow(1080, 720, "Vectors");
			Raylib.SetCameraSmoothZoomControl(KeyboardKey.KEY_Z);
			NewArray();
			for (int q = 0; q < 10000; q++)
			{
				NewPoint();
			}
			while (true)
			{
				Draw();
			}
		}
		static void NewArray()
		{
			for (int i = 0; i < sizes.Length; i++)
			{
				sizes[i] = rng.NextDouble() * 10;
				speeds[i] = rng.NextDouble() * 2 - 1;
				rotation[i] = rng.NextDouble();
			}
			x = new List<double>();
			y = new List<double>();
		}
		static void NewPoint()
		{
			Program p = new Program();
			double xPoint = Raylib.GetScreenWidth() / 2;
			double yPoint = Raylib.GetScreenHeight() / 2;
			for (int i = 0; i < sizes.Length; i++)
			{
				xPoint += Math.Cos(rotation[i]) * sizes[i];
				yPoint += Math.Sin(rotation[i]) * sizes[i];
			}
			x.Add(xPoint);
			y.Add(yPoint);
			for (int i = 0; i < sizes.Length; i++)
			{
				rotation[i] += speeds[i] / 100;
			}
			Console.WriteLine(x.Count());
		}
		static void Draw()
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);
			for (int i = 0; i < x.Count() - 1; i++)
			{
				Raylib.DrawLine(Convert.ToInt16(x[i]), Convert.ToInt16(y[i]), Convert.ToInt16(x[i + 1]), Convert.ToInt16(y[i + 1]), Color.WHITE);
			}
			Raylib.EndDrawing();
		}
	}
}