namespace FlappyBird
{
	class Pipe
	{
		public double x;
		public double y;
		public float size;
		public float speed;

		public Pipe(double x, double y, float size, float speed)
		{
			this.x = x;
			this.y = y;
			this.size = size;
			this.speed = speed;
		}
	}
}