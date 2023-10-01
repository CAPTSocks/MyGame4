using Godot;
using System;

public partial class SceneChanger : Node
{
    [Export] string spawnName;
    [Export] string newScenePath;
    //Node[] pItems;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }


    void OnSceneChangerBodyEntered(Node body)
    {
        if (body.Name == "Player")
        {

            CallDeferred(nameof(CallGlobal), body);
        }
    }

    Godot.Collections.Array<Node> SavePlayersInventory(Node player)
    {
        var pInventory = player.GetNode<Node2D>("Items");
        Godot.Collections.Array<Node> pItems = new Godot.Collections.Array<Node>();
        GD.Print(pInventory.GetChildCount());
        var childCount = pInventory.GetChildCount();

        // for (int i = 0; i < childCount; i++)
        // {
        //     var child = pInventory.GetChild(i);
        //     pItems.Add(child);
        //     // pItems.Add(pInventory.GetChild(i));
        //     // pInventory.RemoveChild(pInventory.GetChild(i));
        // }

      foreach (Node c in pInventory.GetChildren())
      {
          pItems.Add(c);
          pInventory.RemoveChild(c);
         // c.QueueFree();
      }

        // pInventory.child

        return pItems;
    }

    void CallGlobal(Node body)
    {
        //Find Global
        var glob = (Global)GetNode("/root/Global");

        //Stop the player from moving
        var playerScriptAccess = (PlayerMovement)body;
        playerScriptAccess.canMove = false;

        //Get player's health to send over
        var playerHealthAccess = body.GetNode<Health>("Health");
        playerHealthAccess.CanRegin = false;
        var curPlayerHealth = playerHealthAccess.CurrentHealth;
        var curPlayerEnergy = playerHealthAccess.CurrentEnergy;

        //Get the player's animation to send over
        var playeranim = body.GetNode<AnimationPlayer>("Anim");

        //Make the player stop the walk animation
        var currentAnim = playeranim.CurrentAnimation.Substring(4);
        playeranim.Play("Idle" + currentAnim);

        //Save the players inventory between scenes
        var pItems = SavePlayersInventory(body);

        if (glob != null)
        {
            //Start the new scene
            glob.StartSceneChange(curPlayerHealth, curPlayerEnergy, spawnName, newScenePath, currentAnim, pItems);
        }
    }






    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
