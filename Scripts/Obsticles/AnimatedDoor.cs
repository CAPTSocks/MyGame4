using Godot;
using System;

public partial class AnimatedDoor : Node
{
	[Export]
	private string bodyName = string.Empty;
	[Export]
	private bool doorClosed = false, lockedDoor = false;
	[Export]
	private Vector2 leftDoorStartpos = new Vector2(-25, 0);
	[Export]
	private Vector2 rightDoorStartpos = new Vector2(25, 0);
	private AnimationPlayer anim;
	private Sprite2D leftDoor, rightDoor;
	private Area2D area;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		leftDoor = GetNode<Sprite2D>("LeftDoor");
		rightDoor = GetNode<Sprite2D>("RightDoor");
		area = GetNode<Area2D>("Area2D");

		if (lockedDoor)
		{
			area.Monitoring = false;
		}

		if (doorClosed)
		{
			
			leftDoor.Position = new Vector2(-8, 0);
			rightDoor.Position = new Vector2(8, 0);
		}
		else 
		{
			leftDoor.Position = leftDoorStartpos;
			rightDoor.Position = rightDoorStartpos;
			
		}
	
		
	}

	private void OpenDoor()
	{
		if (doorClosed)
		{
			anim.Play("DoorOpen");
		}
	}

	private void CloseDoor()
	{
			if (!doorClosed)
		{
			anim.Play("DoorClose");
		}
	}

	void OnBodyEntered(Node body)
	{
		if (bodyName != string.Empty)
		{
			if (body.Name == bodyName)
			{
				if (doorClosed)
				{
					OpenDoor();
				}
				else
				{
					CloseDoor();
				}
			}
		}
	}
}
