using Godot;
using System;

public partial class RangedEnemy : BaseEnemy
{
    [Export]private PackedScene arrow;
    [Export] private float targetTooCloseRange = 5;
    private bool touchingWall = false;
    Node2D wallDetector;
    RaycastRotator rotator;
    RayCast2D wallDectorRaycast;

    public override void _Ready()
    {
        speed = 100;

        target = GetParent().GetNode<CharacterBody2D>("Player");
        attackTimer = GetNode<Timer>("AttackTimer");
        wallDectorRaycast = GetNode<RayCast2D>("RayCast2D");
        attackTimer.Start(attackRate);
    }

    private void OnAttackTimerEnded()
    {
        Shoot();
    }

    private void Shoot()
    {
        var tardir = (target.GlobalPosition - this.GlobalPosition).Normalized();
        var b = arrow.Instantiate<ArrowProjectile>();
        b.Spawn(this.GlobalPosition, tardir);
        GetParent().AddChild(b);

        // var targetdir = 

        // targetDistance = this.GlobalPosition.DistanceTo(target.GlobalPosition);
    }

    private void AvoidTarget()
    {
        var tardir = (target.GlobalPosition - this.GlobalPosition).Normalized();
        if (DetectWalls(tardir) == false)
        {
            velocity = tardir * -speed;
        }
        else
        velocity = Vector2.Zero;
    }

    private bool DetectWalls(Vector2 tardir)
    {
        tardir *= -1;
        wallDectorRaycast.Rotation = (tardir.Angle() - 1.5708f);

        var col = wallDectorRaycast.GetCollider();
        //wallDectorRaycast.
        if (col != null)
        {
            return true;
        }
        else
            return false;
    }

    public override void _Process(double delta)
    {
        targetDistance = GlobalPosition.DistanceTo(target.GlobalPosition);
        //GD.Print(targetDistance);
    }

    public override void _PhysicsProcess(double delta)
    {

        if (targetDistance <= targetTooCloseRange)
        {
            AvoidTarget();
            MoveAndCollide(velocity * (float)delta);
        }
    }
}
