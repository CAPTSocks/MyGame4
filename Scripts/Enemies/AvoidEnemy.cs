using Godot;
using System;

public partial class AvoidEnemy : BaseEnemy
{
    enum currentStateEnum
    {
        getInRange,
        keepDistance,
        charge,
        rest
    }

    [Export]
    currentStateEnum currentState = currentStateEnum.getInRange;
    [Export]
    private float avoidTime = 5, chargeTime = 2, chargeSpeedMultiplyer = 2, moveToSpeed = 250, avoidSpeed = 200, chargeTurnSpeed = .25f;

    private NavigationAgent2D nav2D;
    private Vector2[] path;
    private bool charging = false, avoiding = false;

    public override void _Ready()
    {
       // nav2D = GetParent().GetNode<NavigationAgent2D>("Navigation2D");
        target = GetParent().GetNode<CharacterBody2D>("Player");
        targetHealthAccess = target.GetNode<Health>("Health");
        attackTimer = GetNode<Timer>("AttackTimer");
        targetDistance = attackRange + 1;
    }

    void TimerTimeout()
    {
        if (avoiding)
        {
            avoiding = false;
        }

        if (charging)
        {
            charging = false;
        }
    }

    //Moves Enemy towards the player along a path
    void MoveToPlayer(float delta)
    {
        // if (targetDistance > attackRange)
        // {
        // //    GD.Print(targetDistance);
        //     path = nav2D.GetSimplePath(this.GlobalPosition, target.GlobalPosition);
        //     var moveSpeed = speed * delta;

        //     var startPoint = Position;

        //     for (int i = 0; i < path.Length; i++)
        //     {
        //         var distanceToNext = startPoint.DistanceTo(path[i]);
        //         if (moveSpeed <= distanceToNext && moveSpeed >= 0.0)
        //         {
        //             Position = startPoint.Lerp(path[i], moveSpeed / distanceToNext);
        //             break;
        //         }
        //         else if (moveSpeed < 0.0)
        //         {
        //             Position = path[i];
        //             break;
        //         }
        //         moveSpeed -= distanceToNext;
        //         startPoint = path[i];
        //     }
        // }
        // else
        // {
        //     avoiding = true;
        //     speed = avoidSpeed;
        //     attackTimer.Start(avoidTime);
        //     currentState = currentStateEnum.keepDistance;
        // }
    }

    void KeepDistance(float delta)
    {
        if (avoiding)
        {
            var tarDir = (target.GlobalPosition - this.GlobalPosition).Normalized();
            if (targetDistance > attackRange + 10)
            {
                velocity = tarDir * speed * delta;
            }
            else if (targetDistance < attackRange)
            {
                velocity = -tarDir * speed * delta;
            }
            else
            {
                velocity = Vector2.Zero;
            }

            MoveAndCollide(velocity);
        }
        else
        {
            charging = true;
            speed = avoidSpeed * chargeSpeedMultiplyer;
            targetDirection = (target.GlobalPosition - this.GlobalPosition).Normalized();
            attackTimer.Start(chargeTime);
            currentState = currentStateEnum.charge;
        }

    }

    void Charge(float delta)
    {
        if (charging)
        {
            var tarDir = (target.GlobalPosition - this.GlobalPosition).Normalized();
            tarDir = new Vector2(Mathf.MoveToward(targetDirection.X, tarDir.X, chargeTurnSpeed * delta), Mathf.MoveToward(targetDirection.Y, tarDir.Y, chargeTurnSpeed * delta));
            GD.Print(tarDir);
            velocity = tarDir * speed * delta;
            MoveAndCollide(velocity);
        }
        else
        {
            currentState = currentStateEnum.rest;
        }
    }

    void Rest()
    {
     //   GD.Print("I am resting");
    }




    public override void _Process(double delta)
    {
        switch ((int)currentState)
        {
            //Move to player
            case 0:

                MoveToPlayer((float)delta);
                break;
            //Keep distance from player
            case 1:
                KeepDistance((float)delta);
                break;
            //Charge at player    
            case 2:
                Charge((float)delta);
                break;
            //Rest after Charge    
            case 3:
                Rest();
                break;

            default:
                return;
        }

        if (target != null)
        {
            targetDistance = this.GlobalPosition.DistanceTo(target.GlobalPosition);
        }
    }
}
