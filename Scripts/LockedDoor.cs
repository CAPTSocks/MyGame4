using Godot;
using System;

public partial class LockedDoor : Node2D
{
    //Key that opens door
    [Export] String keyName;
    [Export]
    private Vector2 leftDoorOpenPos = new Vector2(-25, 0);
    [Export]
    private Vector2 rightDoorOpenPos = new Vector2(25, 0);
    private Vector2 doorClosedPos = new Vector2(8, 0);
    [Export]
    private float doorSpeed;

    private Sprite2D leftDoor;
    private Sprite2D rightDoor;

    private bool openingDoor = false;
    private Node2D key;
    private Label doorText;
    private bool canOpen = false, DoorOpen = false;
    public override void _Ready()
    {
        doorText = GetNode<Label>("DoorText");
        leftDoor = GetNode<Sprite2D>("LeftDoor");
        rightDoor = GetNode<Sprite2D>("RightDoor");
        doorText.Visible = false;
        if (this.Rotation != 0)
        {
            doorText.Rotation = -90;
        }
    }

    void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
        {
            if (!DoorOpen)
            {
                key = body.GetNodeOrNull<Node2D>("Items/" + keyName);
                if (key != null)
                {
                    SetProcess(true);
                    doorText.Visible = true;
                    doorText.Text = "Press E to unlock";
                    canOpen = true;
                }
                else
                {
                    doorText.Visible = true;
                    doorText.Text = "Find key to open";
                }
            }
        }
    }

    void OnBodyExited(Node body)
    {
        if (body.Name == "Player")
        {
            doorText.Visible = false;
            canOpen = false;
            SetProcess(false);
        }
    }

    void Open()
    {
        //key = GetNodeOrNull<Node2D>("Items/" + keyName);
        key.QueueFree();
        canOpen = false;
        DoorOpen = true;
        openingDoor = true;
        doorText.Visible = false;
    }

    private void OpeningDoor()
    {
        if (leftDoor.Position != leftDoorOpenPos && rightDoor.Position != rightDoorOpenPos)
        {
            leftDoor.Position = leftDoor.Position.MoveToward(leftDoorOpenPos, doorSpeed);
            rightDoor.Position = rightDoor.Position.MoveToward(rightDoorOpenPos, doorSpeed);
        }
        else
        {
            openingDoor = false;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Interact") && canOpen)
        {
            Open();
        }

        if (openingDoor)
        {
            OpeningDoor();
        }
    }
}
