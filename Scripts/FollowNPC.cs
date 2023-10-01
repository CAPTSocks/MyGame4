using Godot;
using System;

public partial class FollowNPC : CharacterBody2D
{

    private CharacterBody2D target;
    private Sprite2D sprite;
    private float targetDistance;
    [Export] private float CloseEnoughDistance, speed = 5, closeSpeed, normalSpeed, farSpeed;
    private bool followPlayer = true;
    private NavigationAgent2D nav2D;
    private Vector2[] path;

    public override void _Ready()
    {
        nav2D = GetParent().GetNode<NavigationAgent2D>("Navigation2D");
        target = GetParent().GetNode<CharacterBody2D>("Player");
        sprite = GetNode<Sprite2D>("Sprite2D");
    }


    public override void _Process(double delta)
    {
         if (target != null)
        {
            targetDistance = this.GlobalPosition.DistanceTo(target.GlobalPosition);

            if (targetDistance > 250f)
            {
                speed = farSpeed;
            }
            else if (targetDistance < 125f)
            {
                speed = closeSpeed;
            }
            else 
            {
                speed = normalSpeed;
            }

            var tardir = (target.GlobalPosition - this.GlobalPosition).Normalized();
            var tardirDegree = Math.Atan2(tardir.Y, tardir.X)*180/Math.PI;
            HandleAnimation(Math.Round(tardirDegree));
        }
    }

    public override void _PhysicsProcess(double delta)
    {

        if (followPlayer && targetDistance >= CloseEnoughDistance)
        {
            // path = nav2D.GetSimplePath(this.GlobalPosition, target.GlobalPosition);
            // var moveSpeed = speed * delta;
            // MoveToPlayer(moveSpeed);

        }
    }

    private void HandleAnimation(double direction)
    {
        //up
        if (direction >= -135 && direction < -50)
        {
            sprite.Frame = 1;
        }
        //left
        else if (direction >= 135 || direction < -135)
        {
            sprite.Frame = 2;
        }
        //down
        else if (direction >= 45 && direction < 135)
        {
            sprite.Frame = 0;
        }
        //right
        else if (direction >= -45 && direction < 45)
        {
            sprite.Frame = 3;
        }
    }

    private void MoveToPlayer(float moveSpeed)
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
}
