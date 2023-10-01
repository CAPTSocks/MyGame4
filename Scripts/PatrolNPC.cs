using Godot;
using System;

public partial class PatrolNPC : CharacterBody2D
{
    [Export]private Vector2[] waypoints;
    [Export]private float speed = 100;

    private float curWaypointDis, closeEnoughDis = 5f;
    private int curWaypoint = 0;
    private Sprite2D sprite; 


    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector2.Zero;

        direction = waypoints[curWaypoint] - this.GlobalPosition;
        curWaypointDis = this.GlobalPosition.DistanceTo(waypoints[curWaypoint]);
        if (curWaypointDis < closeEnoughDis)
        {
            if (curWaypoint < waypoints.Length-1)
            {
                curWaypoint ++;
            }
            else 
            {
                curWaypoint = 0;
            }

        }

            direction = direction.Normalized();
            HandleAnimation(direction);
            Velocity = direction * speed;
          //  var moveDir = direction * speed;
         //   MoveAndSlide(moveDir);
         MoveAndSlide();
    }

    public void HandleAnimation(Vector2 direction)
    {
         if (direction.X <= -.5f)
        {
            sprite.Frame = 2;
            // meleeOrigin.Position = new Vector2(-25, 0);
            // meleeOrigin.RotationDegrees = 90;
        }

        else if (direction.X >= .5f)
        {
            sprite.Frame = 3;
            // meleeOrigin.Position = new Vector2(25, 0);
            // meleeOrigin.RotationDegrees = -90;
        }

        else if (direction.Y >= .5f)
        {
            sprite.Frame = 0;
            // meleeOrigin.Position = new Vector2(0, 40);
            // meleeOrigin.RotationDegrees = 0;
        }

        else if (direction.Y <= -.5f)
        {
            sprite.Frame = 1;
            // meleeOrigin.Position = new Vector2(0, -40);
            // meleeOrigin.RotationDegrees = 180;
        }
    }


//      public float actualSpeed = 10.0f;
//  public GameObject[] checkpoints;
//  int counter = 0;
//  public float distance = 2.0f; //on which distance you want to switch to the next waypoint
//  void FixedUpdate ()
//      {
//          direction = Vector3.zero;
//          //get the vector from your position to current waypoint
//          direction = checkpoints[counter].transform.position - transform.position;
//          //check our distance to the current waypoint, Are we near enough?
//          if(direction.magnitude < distance)
//          {
//              if(counter < checkpoints.Length-1) //switch to the nex waypoint if exists
//              {
//                  counter++;
//              }
//              else //begin from new if we are already on the last waypoint
//              {
//                  counter = 0;
//              }
//          }
//          direction = direction.normalized;
//          Vector3 dir = direction;
 
//          GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * actualSpeed, direction.y * actualSpeed);
//      }


//  public override void _Process(double delta)
//  {
//      
//  }
}
