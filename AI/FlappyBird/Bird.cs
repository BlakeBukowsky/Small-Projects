using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace FlappyBird
{
	class Bird
	{
		public Matrix<double> w1;
		public Matrix<double> w2;
		public double y;

		public double vel;
		public bool alive = true;

		public double timeTilJump = 0;

		public Bird(Matrix<double> w1, Matrix<double> w2, float y)
		{
			this.w1 = w1;
			this.w2 = w2;
			this.y = y;
		}

		public void newWeights(Matrix<double> w1, Matrix<double> w2, double learn)
		{
			Random rng = new Random();
			for (int i = 0; i < w1.ColumnCount; i++)
			{
				for (int c = 0; c < w1.RowCount; c++)
				{
					w1[c, i] = w1[c, i] * ((1 + rng.NextDouble() * 2 - 1) * learn);
				}
			}
			for (int i = 0; i < w2.ColumnCount; i++)
			{
				for (int c = 0; c < w2.RowCount; c++)
				{
					w2[c, i] = w2[c, i] * ((1 + rng.NextDouble() * 2 - 1) * learn);
				}
			}
			for (int i = 0; i < w1.ColumnCount; i++)
			{
				for (int c = 0; c < w1.RowCount; c++)
				{
					w1[c, i] = w1[c, i] * ((1 + rng.NextDouble() * 2 - 1) * learn);
				}
			}
		}
		public bool Decide(Matrix<double> w1, Matrix<double> w2, Matrix<double> input)
		{
			Matrix<double> h = input.Multiply(w1);
			Matrix<double> o = h.Multiply(w2);
			if (o[0, 0] > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}