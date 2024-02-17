using Godot;
using System;

namespace Obstacles
{
	public partial class RollingRockTrap : Node2D
	{
		[Export]
		private RollingRock.RollDirection rollDirection; 
		[Export]
		private float rockSpeed = 5f; 
		private Node2D rockSpawn;
		private bool triggered = false;
		private PackedScene rock = (PackedScene)GD.Load("res://Prefabs/Obsticles/RollingRock/RollingRock.tscn");


		public override void _Ready()
		{
			rockSpawn = GetNode<Node2D>("RockSpawnPoint");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		void OnBodyEntered (Node body)
		{
			if (body.Name == "Player" && !triggered)
			{
				CallDeferred("SpawnRock");
			}
		}

		void SpawnRock()
		{
			var newRock = rock.Instantiate<RollingRock>();
			newRock.SpawnRock(rollDirection, rockSpeed);
			newRock.GlobalPosition = rockSpawn.GlobalPosition;
			GetParent().AddChild(newRock);
			this.QueueFree();
		}
	}
}
