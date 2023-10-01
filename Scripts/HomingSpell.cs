using Godot;
using System;

public partial class HomingSpell : CharacterBody2D
{
    private CharacterBody2D target;
    private float targetDistance;
    [Export] private float stopHomingRange = 100, speed = 5, lifeTime;
    [Export] private int damage = -20;
    private bool closeEnough = false;
    private AnimationPlayer anim;
    private Timer deleteTimer;
    private Vector2 velocity;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetParent().GetNode<CharacterBody2D>("Player");
        deleteTimer = GetNode<Timer>("DeleteTimer");
        anim = GetNode<AnimationPlayer>("Anim");

        deleteTimer.WaitTime = lifeTime;
        deleteTimer.Start();

        this.LookAt(target.GlobalPosition);
        velocity = (target.GlobalPosition - this.GlobalPosition).Normalized();
    }

    public void Spawn(Vector2 startPosition)
    {
        this.GlobalPosition = startPosition;   
    }

    //Delete Spell when timer ends
    private void OnDeletetimerTimeout()
    {
        this.QueueFree();
    }
    private void OnBodyEntered(Node body)
    {
        //When player touches the body delete the spell and hurt the player
        if (body.Name == "Player")
        {
            var pHealth = body.GetNode<Health>("Health");
            pHealth.ChangeHealth(damage);
            this.QueueFree();
        }
    }

    public override void _Process(double delta)
    {
        //Get distance from player
        targetDistance = GlobalPosition.DistanceTo(target.GlobalPosition);

        //Once spell gets close enough stop homing
        if (targetDistance <= stopHomingRange)
        {
            closeEnough = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        //As long as spell isnt too close to player home in on them.
        if (!closeEnough)
        {
            this.LookAt(target.GlobalPosition);
            velocity = (target.GlobalPosition - this.GlobalPosition).Normalized();
        }

        //Move the spell
        MoveAndCollide(velocity * speed);
    }
}
