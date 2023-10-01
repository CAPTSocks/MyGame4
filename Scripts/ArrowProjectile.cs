using Godot;
using System;

public partial class ArrowProjectile : CharacterBody2D
{
    [Export] private float speed = 100, lifetime = 5;
    [Export] private int damage = -10;
    
    private Vector2 velocity;
    private Timer deleteTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        deleteTimer = GetNode<Timer>("DeleteTimer");
        deleteTimer.Start(lifetime);
    }

    public void Spawn(Vector2 pos, Vector2 rot)
    {
        Rotation = rot.Angle();
        velocity = rot * speed;
        GlobalPosition = pos;
    }

    private void OnTimerTimeout()
    {
        this.QueueFree();
    }

    private void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
        {
            var pHealth = body.GetNode<Health>("Health");
            pHealth.ChangeHealth(damage);
            this.QueueFree();
        }
        else 
        {
            this.QueueFree();
        }
    }

    public override void _PhysicsProcess(Double delta)
    {
        var col = MoveAndCollide(velocity * (float)delta);
    }
}
