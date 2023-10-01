using Godot;
using System;

public partial class DartTrap : Node2D
{
    // Declare member variables here. Examples:

    private PackedScene dart = (PackedScene)GD.Load("res://Prefabs/Enemies/EnemyProjectile.tscn");

    private bool trapHasBeenActivated = false;

    private Timer dartRate, delayStartTimer;
    [Export]private float delayStart = .2f, attackRate = .5f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        delayStartTimer = GetNode<Timer>("DelayStartTimer");
        dartRate = GetNode<Timer>("DartRate");
        
    }

    private void TrapActivated(Node body)
    {
        if (body.Name == "Player" && !trapHasBeenActivated)
        {
            trapHasBeenActivated = true;
            StartFiring();
        }
    }

    private void DeactivateTrap(Node Body)
    {
        if (Body.Name == "Player" && trapHasBeenActivated)
        {
            dartRate.Stop();
        }
    }

    public void StartFiring()
    {
        delayStartTimer.Start(delayStart);
    }

    private void OnDelayStart()
    {
        dartRate.Start(attackRate);
    }
    private void OnDartRateTimeout()
    {
        FireDart();
    }

    private void FireDart()
    {
        var d = dart.Instantiate<ArrowProjectile>();
        Vector2 rotation = new Vector2(Mathf.Cos(this.GlobalRotation), Mathf.Sin(this.GlobalRotation));
        d.Spawn(this.GlobalPosition, rotation);
        GetParent().AddChild(d);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(double delta)
//  {
//      
//  }
}
