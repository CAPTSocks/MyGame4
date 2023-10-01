using Godot;
using System;

public partial class Global : Node
{
    int curPlayerHealth;
    float curPlayerEnergy;
    PlayerMovement playeScriptAccess;
    Vector2 spawnPos;
    string scenePath, currentPlayerAnim, spawnName;

    Godot.Collections.Array<Node> playerItems;

    AnimationPlayer fader;

    CollectiblesGlobol colGlobal;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        colGlobal = (CollectiblesGlobol)GetNode("/root/GlobalCollectibles");
        fader = GetNode<AnimationPlayer>("Fader");
    }

    //Brings fades the black over the screen
    void FadeIn()
    {
        fader.Play("FadeIn");
    }

    void OnFadeinFInished(string FadeIn)
    {
        if (FadeIn == "FadeIn")
            ChangeSceneToFile();
    }

    //Fades the black off the screen
    void FadeOut()
    {
        fader.Play("FadeOut");

    }

    void OnFadeoutFinished(string FadeOut)
    {
        if (FadeOut == "FadeOut")

            //Lets the player move once the screen is faded out
            playeScriptAccess.canMove = true;
    }

    public void StartSceneChange(int pHealth, float pEnergy, string sName, string sPath, string curAnim, Godot.Collections.Array<Node> pItems)
    {
        //Set variables to the recived ones
        curPlayerHealth = pHealth;
        curPlayerEnergy = pEnergy;
        scenePath = sPath;
        spawnName = sName;
        currentPlayerAnim = curAnim;
        playerItems = pItems;

        //Set up player spawn position


        //Fade the dark screen in
        FadeIn();
    }

    void ChangeSceneToFile()
    {

        //Gets the current scene from the root
        var root = GetTree().Root;
        var currentScene = root.GetChild(root.GetChildCount() - 1);

        //Deletes the current scene
        currentScene.QueueFree();

        //Loads the new scene and sets it as the current scene
        var newScene = (PackedScene)GD.Load(scenePath);
        currentScene = newScene.Instantiate<Node>();

        //Adds the new scene to the root
        root.AddChild(currentScene);

        //Sets the curretn scene to the new scene
        GetTree().CurrentScene = currentScene;

        //Set Player spawn pos
        spawnPos = currentScene.GetNode<Node2D>("SpawnPositions/" + spawnName).GlobalPosition;

        colGlobal.SetUpCollectiblesForNewScene();
        SetUpPlayer(spawnPos, currentScene);

    }

    void SetUpPlayer(Vector2 spawnPos, Node currentScene)
    {
        //Gets Acess to the player and their nodes
        var player = currentScene.GetNode<CharacterBody2D>("Player");
        var playerHealthAccess = player.GetNode<Health>("Health");
        playeScriptAccess = (PlayerMovement)currentScene.GetNode<CharacterBody2D>("Player");
        var playerAnim = player.GetNode<AnimationPlayer>("Anim");

        //Set HealthBar to current health
        var healthBarAccess = (UpdateHealthBar)currentScene.GetNode<Control>("HUD/HealthBar");
        healthBarAccess.SetBarsDirectly(curPlayerHealth);

        //Sets player health
        var newHealth = (100 - curPlayerHealth) * -1;
        playerHealthAccess.ChangeHealth(newHealth);

        //Set player energy
        int newEnergy = (int)(60 - curPlayerEnergy) * -1;
        playerHealthAccess.useEnergy(newEnergy);


        //Stops the player from moving and sets their anim
        playeScriptAccess.canMove = false;
        playerAnim.Play("Idle" + currentPlayerAnim);

        //Give Player inventory items
        var inventory = player.GetNode<Node2D>("Items");
        for (int i = 0; i < playerItems.Count; i++)
        {   
            inventory.AddChild(playerItems[i]);
            GD.Print(inventory.GetChild(i).Name);
        }

        //Moves the player to the spawn position
        player.GlobalPosition = spawnPos;

        //Fades the black out
        FadeOut();


    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
