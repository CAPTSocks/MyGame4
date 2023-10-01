using Godot;
using System;

public partial class RaycastRotator : CharacterBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private CharacterBody2D target; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetParent().GetNode<CharacterBody2D>("Player");
    }

    public void RotateRaycast()//Vector2 rot)
    {
        var tardir = (target.GlobalPosition - GlobalPosition).Normalized();
      //  rot *= -1;
      //TargetPosition = rot;
        Rotation = tardir.Angle();
        //(rot.Angle() - 1.6f);
    }

    public override void _PhysicsProcess(Double delta)
    {
        RotateRaycast();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(double delta)
//  {
//      
//  }
}
