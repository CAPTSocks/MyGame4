using Godot;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

public partial class PatrolNPC : CharacterBody2D
{
    [Export] private Vector2[] waypoints;
    [Export] private float speed = 100;
    [Export]
    private DialogueContainer dialogueContainer;

    private float curWaypointDis, closeEnoughDis = 5f;
    private int curWaypoint = 0;
    private Sprite2D sprite;
    //private Node playerAccess;
    private bool playerInRange = true, patrol = true, interactedWith = false;
    private RichTextLabel pressEText;
    private Node playerAccess;

    private DialogueManagerResource dm;


    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        pressEText = GetNode<RichTextLabel>("PressEText");
        dm = GetTree().Root.GetNode<DialogueManagerResource>("TestScene/HUD/DialogueManager");
        SetProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (patrol)
        {
            Vector2 direction = Vector2.Zero;

            direction = waypoints[curWaypoint] - this.GlobalPosition;
            curWaypointDis = this.GlobalPosition.DistanceTo(waypoints[curWaypoint]);
            if (curWaypointDis < closeEnoughDis)
            {
                if (curWaypoint < waypoints.Length - 1)
                {
                    curWaypoint++;
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
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Interact") && interactedWith == false)
        {
            patrol = false;
            StartDialogue();
            CalculatePlayerDirection();
        }
    }

    void CalculatePlayerDirection()
    {
        if (playerAccess != null)
        {
            CharacterBody2D playerBody = (CharacterBody2D)playerAccess;
            if (playerBody != null)
            {
                GD.Print(playerBody);
                Vector2 playerDirection = playerBody.GlobalPosition - this.GlobalPosition;
                playerDirection = playerDirection.Normalized();
                HandleAnimation(playerDirection);
            }
        }
    }

    void StartDialogue()
    {
        GD.Print("start talking");
        interactedWith = true;
        dm?.StartDialogue(dialogueContainer);
    }

    void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
        {
            playerAccess = body;
            playerInRange = true;
            SetProcess(true);
            pressEText.Visible = true;
        }
    }

    void OnBodyExited(Node body)
    {
        if (body.Name == "Player")
        {
            pressEText.Visible = false;
            playerInRange = false;
            SetProcess(false);
            interactedWith = false;
            patrol = true;
        }
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
