using Godot;
using System;

public partial class LockedDoor : StaticBody2D
{
    //Key that opens door
    [Export] String keyName;
    private Node2D key;
    private Label doorText;
    private bool canOpen = false;
    public override void _Ready()
    {
        doorText = GetNode<Label>("DoorText");
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
        // key = GetNodeOrNull<Node2D>("Items/" + keyName);
        key.QueueFree();
        this.QueueFree();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Interact") && canOpen)
        {
            Open();
        }
    }
}
