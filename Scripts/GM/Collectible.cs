using Godot;
using System;

public partial class Collectible : Node
{

    [Export] public int id;
    [Export] public string name;
    public string description; 
    public string level;
    public string imagePath;
    public bool collected = false;

    private CollectiblesGlobol colGlobol;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //CheckIfCollected();
        colGlobol = (CollectiblesGlobol)GetNode("/root/GlobalCollectibles");

        collected = colGlobol.CheckIfCollected(id);
        GD.Print(collected);
        if (collected == true)
        {
            this.QueueFree();
        }

    }

    public void OnTimeout()
    {

    }

    public void CheckIfCollected()
    {

    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Collision Detected");
        if (body.Name == "Player")
        {
            GD.Print("Collision Detected");

            colGlobol.CollectCollectible(id);
            GD.Print(name + " Collected!");
            this.QueueFree();
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
