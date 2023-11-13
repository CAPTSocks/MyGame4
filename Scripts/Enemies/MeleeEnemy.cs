using Godot;
using Microsoft.VisualBasic;
using System;

public partial class MeleeEnemy : BaseEnemy
{

    private NavigationAgent2D agent;
    private TileMap map; 
    private Vector2[] path;
    private bool canMove = false;

    public override void _Ready()
    {
        map = GetParent().GetNode<TileMap>("TileMap");
        agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        target = GetParent().GetNode<CharacterBody2D>("Player");
        targetHealthAccess = target.GetNode<Health>("Health");
        attackTimer = GetNode<Timer>("AttackTimer");
        agent.SetNavigationMap(map.GetNavigationMap(0));

        Callable.From(ActorSetup).CallDeferred();
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
        if (canMove)
    {
        agent.TargetPosition = target.GlobalPosition;
         var curPos = GlobalPosition;
            var newPos = agent.GetNextPathPosition();
            var velocity = (newPos - curPos).Normalized();
            Velocity = velocity * speed;
            MoveAndSlide(); 
    }
    }

        private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        canMove = true;
    }
}
