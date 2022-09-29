using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FlappyBird
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		Texture2D birdSprite;
		Texture2D pipeSprite;
		Pipe pipe1;
		Pipe pipe2;
		float pipeSpeed = 100;
		float pipeGap = 380;
		float pipeSize = 20;
		float birdX = 50;
		float pipeStartX = 800;
		float birdStartY = 100;
		float learn = (float).5;
		static int numOfBirds = 100;
		float speedMultiplier = 2;
		float jump = 17;
		float minHeight = 450;
		float timeTweenJumps = .5f;
		Bird bird = new Bird(null, null, 0);
		Bird[] birds = new Bird[numOfBirds];
		Matrix<double> savedW1;
		Matrix<double> savedW2;


		Random rng = new Random();

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			for (int b = 0; b < numOfBirds; b++)
			{
				birds[b] = new Bird(MathNet.Numerics.LinearAlgebra.Double.Matrix.Build.Random(4, 6), MathNet.Numerics.LinearAlgebra.Double.Matrix.Build.Random(6, 1), birdStartY);
			}
			pipe1 = new Pipe(pipeStartX, rng.Next(-300, 0), pipeSize, pipeSpeed);
			pipe2 = new Pipe(pipeStartX, pipe1.y + pipeGap, pipeSize, pipeSpeed);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			birdSprite = Content.Load<Texture2D>("Bird");
			pipeSprite = Content.Load<Texture2D>("Pipe");
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			bool saved = false;
			pipe1.x -= pipe1.speed * gameTime.ElapsedGameTime.TotalSeconds * speedMultiplier;
			pipe2.x -= pipe2.speed * gameTime.ElapsedGameTime.TotalSeconds * speedMultiplier;
			for (int i = 0; i < birds.Length; i++)
			{
				if (birds[i].alive)
				{
					double[] input = { birds[i].vel, pipe1.x - birdX, (pipe1.y + pipe1.size * 16) - birds[i].y, birds[i].y - pipe2.y };
					if (birds[i].Decide(birds[i].w1, birds[i].w2, MathNet.Numerics.LinearAlgebra.Double.Matrix.Build.Dense(1, 4, input)) && birds[i].timeTilJump <= 0)
					{
						birds[i].vel = jump * speedMultiplier;
						birds[i].timeTilJump = timeTweenJumps;
					}
					else
						birds[i].vel -= 9.8 * gameTime.ElapsedGameTime.TotalSeconds * 5 * speedMultiplier;
					birds[i].y -= birds[i].vel * gameTime.ElapsedGameTime.TotalSeconds * 5;
					birds[i].timeTilJump -= gameTime.ElapsedGameTime.TotalSeconds * speedMultiplier;
					if (birds[i].y + 8 < 0 || birds[i].y + 8 > minHeight || (birds[i].y + 8 > pipe2.y && birdX + 8 > pipe2.x && birdX < pipe2.x + 16) || (birds[i].y + 8 < pipe1.y + 16 * pipeSize && birdX > pipe2.x && birdX < pipe2.x + 16))
					{
						birds[i].alive = false;
					}
					if (!saved && birds[i].alive)
					{
						savedW1 = birds[i].w1;
						savedW2 = birds[i].w2;
						saved = true;
					}
				}
			}
			if (pipe1.x < birdX - 20)
			{
				pipe1 = new Pipe(pipeStartX, rng.Next(-300, 0), pipeSize, pipeSpeed * speedMultiplier);
				pipe2 = new Pipe(pipeStartX, pipe1.y + pipeGap, pipeSize, pipeSpeed * speedMultiplier);
			}
			if (!saved)
			{
				for (int i = 0; i < birds.Length; i++)
				{
					birds[i].newWeights(savedW1, savedW2, learn);
					birds[i].y = birdStartY;
					birds[i].alive = true;
				}
				pipe1 = new Pipe(pipeStartX, rng.Next(-300, 0), pipeSize, pipeSpeed * speedMultiplier);
				pipe2 = new Pipe(pipeStartX, pipe1.y + pipeGap, pipeSize, pipeSpeed * speedMultiplier);
			}
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			for (int i = 0; i < birds.Length; i++)
			{
				if (birds[i].alive)
					_spriteBatch.Draw(birdSprite, new Vector2(birdX, (float)birds[i].y), Color.White);
			}
			_spriteBatch.Draw(pipeSprite, new Vector2((float)pipe1.x, (float)pipe1.y), null, Color.White, 0, new Vector2(0, 0), new Vector2(1, pipe1.size), new SpriteEffects(), 0);
			_spriteBatch.Draw(pipeSprite, new Vector2((float)pipe2.x, (float)pipe2.y), null, Color.White, 0, new Vector2(0, 0), new Vector2(1, 100), new SpriteEffects(), 0);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
