using Godot;
using System;

namespace Obstacles
{
	public partial class RockRedirector : Node2D
	{
		[Export]
		private RollingRock.RollDirection rollDirection = RollingRock.RollDirection.Up;
		[Export]
		private float newRockSpeed = 0;
		[Export]
		RockStateAfterCollision rockState = RockStateAfterCollision.DestroyRock;

		enum RockStateAfterCollision
		{
			ChangeDirection,
			ChangeDirectionAndSpeed,
			DestroyRock,
			StopRock
		};

		void OnBodyEntered(Node body)
		{
			GD.Print(body.Name);
			if (body.Name == "RollingRock")
			{
				RollingRock rockRef = (RollingRock)body;
				if (rockRef != null)
			{
				switch (rockState)
				{	
					case RockStateAfterCollision.ChangeDirection:
					{
						rockRef.ChangeDirection(rollDirection, 0, false);
						break;
					}
					case RockStateAfterCollision.ChangeDirectionAndSpeed:
					{
						rockRef.ChangeDirection(rollDirection, newRockSpeed, true);
						break;
					}
					case RockStateAfterCollision.DestroyRock:
					{
						rockRef.DestroyRock();
						break;
					}
					case RockStateAfterCollision.StopRock:
					{
						rockRef.StopRolling(); 
						break;
					}
				}
			}
			}
		}
	}
}
