using Godot;
using System;

public partial class QuestPickup : Node2D
{
    [Export] private Texture2D spriteTexture;
    private Sprite2D pickupSprite;
    private Label pressEText;
    private bool canPickUp = false;
    private Node2D playerInventory;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        pickupSprite = GetNode<Sprite2D>("Sprite2D");
        pressEText = GetNode<Label>("PressEText");

        pressEText.Visible = false;
        //If dont want texture to be the default key then change it to spriteTexture
        if (spriteTexture != null)
        {
            pickupSprite.Texture = spriteTexture;
        }

    }

    void OnAreaEntered(Node body)
    {
        if (body.Name == "Player")
        {
            canPickUp = true;
            playerInventory = body.GetNode<Node2D>("Items");
            pressEText.Visible = true;
            SetProcess(true);
        }

    }

    void OnAreaExited(Node body)
    {
        if (body.Name == "Player")
        {
            canPickUp = false;
            pressEText.Visible = false;
            SetProcess(false);
        }
    }

    void PickedUp()
    {
        canPickUp = false;
        this.Visible = false;
        SetProcess(false);

        this.GetParent().RemoveChild(this);
        playerInventory.AddChild(this);
        GD.Print(playerInventory.GetChildCount());
       // this.Position = new Vector2(0,0);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Interact") && canPickUp)
        {
            PickedUp();
        }
    }
}
