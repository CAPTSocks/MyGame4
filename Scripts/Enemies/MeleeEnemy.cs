using Godot;
using System;

public partial class MeleeEnemy : BaseEnemy
{

    private NavigationAgent2D nav2D;
    private Vector2[] path;

    public override void _Ready()
    {
        nav2D = GetParent().GetNode<NavigationAgent2D>("Navigation2D");
        target = GetParent().GetNode<CharacterBody2D>("Player");
        targetHealthAccess = target.GetNode<Health>("Health");
        attackTimer = GetNode<Timer>("AttackTimer");
        
    }

    void _on_AttackTimer_timeout()
    {
        attacking = true;
    }

    //Moves Enemy towards the player along a path
    void MoveToPlayer(float moveSpeed)
    {
        var startPoint = Position;

        for (int i = 0; i < path.Length; i++)
        {
            var distanceToNext = startPoint.DistanceTo(path[i]);
            if (moveSpeed <= distanceToNext && moveSpeed >= 0.0)
            {
                Position = startPoint.Lerp(path[i], moveSpeed / distanceToNext);
                break;
            }
            else if (moveSpeed < 0.0)
            {
                Position = path[i];
                break;
            }
            moveSpeed -= distanceToNext;
            startPoint = path[i];
        }
    }


    public override void _Process(double delta)
    {
        if (target != null)
        {
            targetDistance = this.GlobalPosition.DistanceTo(target.GlobalPosition);
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        if (detectionRange >= targetDistance && attackRange <= targetDistance)
        {
            // path = nav2D.GetSimplePath(this.GlobalPosition, target.GlobalPosition);
            // var moveSpeed = speed * delta;
            // MoveToPlayer((float)moveSpeed);



            // velocity = (target.GlobalPosition - this.GlobalPosition).Normalized();
            // MoveAndCollide(velocity * speed * delta);
        }

        if (attackRange >= targetDistance && attacking)
        {
            targetHealthAccess.ChangeHealth(-attackDamage);
            attacking = false;
            attackTimer.Start(attackRate);
        }
    }
}
