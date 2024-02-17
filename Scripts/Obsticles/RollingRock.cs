using Godot;
using System;


namespace Obstacles
{
	public partial class RollingRock : CharacterBody2D
	{
		public enum RollDirection
		{
			Up,
			Down,
			Left,
			Right
		};

		public float rollSpeed = 5f;
		private RollDirection rollDirection = RollDirection.Up;
		private bool roll = false;
		private Vector2 velocity;
		private Sprite2D sprite;

		public override void _Ready()
		{
			sprite = GetNode<Sprite2D>("Sprite");
		}

		public void SpawnRock(RollDirection startDirection, float speed)
		{
			rollSpeed = speed;
			rollDirection = startDirection;
			StartRolling();
		}

		public void StopRolling()
		{
			 roll = false; 
		}

		public void ChangeDirection(RollDirection newDirection, float speed, bool changeSpeed)
		{
			roll = false;
			rollDirection = newDirection;
			if (changeSpeed)
			{
				rollSpeed = speed;
			}
			StartRolling();
		}

		private void StartRolling()
		{
			switch (rollDirection)
			{
				case RollDirection.Up:
					{
						velocity = Vector2.Up * rollSpeed;
						break;
					}
				case RollDirection.Down:
					{
						velocity = Vector2.Down * rollSpeed;
						break;
					}
				case RollDirection.Left:
					{
						velocity = Vector2.Left * rollSpeed;
						break;
					}
				case RollDirection.Right:
					{
						velocity = Vector2.Right * rollSpeed;
						break;
					}
			}

			roll = true;
		}



		public void DestroyRock()
		{
			this.QueueFree();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(double delta)
		{
			if (roll)
			{
				//GD.Print(velocity);
				GD.Print("Roll speed is: " + rollSpeed);
				MoveAndCollide(velocity);
				sprite.Rotate(.1f);
				
			}
		}
	}
}
