using Godot;
using System;

public partial class CollectiblesPage : Control
{
    private int itemID;
    public string name = "???";
    private string description = "???";
    private string levelFrom = "???";
    private bool collected = false;
    private Timer visTurnOffTimer; 

    public Sprite2D image = null;
    public Label nameLabel;
    public Label descriptionLabel;
    public Label levelFromLabel;
    public AnimationPlayer anim; 


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        nameLabel = GetNode<Label>("Menu/Name");
        descriptionLabel = GetNode<Label>("Menu/Description");
        image = GetNode<Sprite2D>("Menu/CollectibleSprite");
        anim = GetNode<AnimationPlayer>("Anim");
        image.Texture = (Texture2D)GD.Load("res://Sprites/Collectibles/axe.png");
        visTurnOffTimer = GetNode<Timer>("VisTurnOffTimer");

        
    }

  

    //Play the animation to move the page off the screen to the left
    public void MoveOffScreenLeft()
    {
        anim.Play("MoveLeftOffScreen");
        visTurnOffTimer.Start();
    }

    //Play the animation to move the page on to the screen from the left
    public void MoveOnScreenRight()
    {
        anim.Play("MoveRightOnScreen");
        SetPageVisible();
    }

    //Play the animation to move the page off the screen to the right
    public void MoveOffScreenRight()
    {
        anim.Play("MoveRightOffScreen");
        visTurnOffTimer.Start();
    }

    //Play the animation to move the page on to the screen from the right
    public void MoveOnScreenLeft()
    {
        anim.Play("MoveLeftOnScreen");
        SetPageVisible();
    }

    //when page is off screen make it invisible
    public void TimerTimeout()
    {
        SetPageInvisible();
    }

    //Make the page invisible
    public void SetPageInvisible()
    {
        this.Visible = false;
    }

    //Make the page visible
    public void SetPageVisible()
    {
        this.Visible = true;
    }

    


    // public override void _Process(double delta)
    // {
    //     nameLabel.Text = name;
    // }
}
