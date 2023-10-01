using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Dialogue : Node
{
    [Export] private String dialogueFilePath;
    [Export] private string npcName;
    [Export] private Texture2D dialoguePortrait;
    private Dictionary dialogueDictionary;
    //  [Export] private bool hasMoreDialogue;
    private bool playerInRange = false, interactedWith = false;
    private Node dialogueManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcess(false);

        StreamReader r = new StreamReader(@dialogueFilePath);
        string jsonstr = r.ReadToEnd().ToString();
        //var json = JSON.Parse(jsonstr);
        
       // dialogueDictionary = (Godot.Collections.Dictionary)json.Result;
       // GD.Print("HERE: "  + dialogueDictionary.Count + " and " + dialogueDictionary["0"].ToString());
         //Godot.Collections.Dictionary dict2 = (Godot.Collections.Dictionary)dialogueDictionary["1"];
        // GD.Print("and " + dict2["name"] + " &&& " + dict2["dialogue"]);
        

        // load a list of dialogue
        // dialogue = new string[2];
        // dialogue[0] = ("Hi");
        // dialogue[1] = ("i need to peeeeee!");

        // name = new string[2];
        // name[0] = ("Fredricka");
        // name[1] = ("GREEN GUY");
    }

    void OnBodyEntered (Node body)
    {
        if (body.Name == "Player")
        {
            playerInRange = true;
            SetProcess(true);
        }
    }

    void OnBodyExited (Node body)
    {
        if (body.Name == "Player")
        {
            playerInRange = false;
            SetProcess(false);
            interactedWith = false;
        }
    }

    void StartDialogue()
    {
         interactedWith = true; 
         DialogueManager dm = GetTree().Root.GetNode<DialogueManager>("TestScene/HUD/DialogueManager");
         //Root().GetNode<DialogueManager>("HUD/DialogueManager");
         dm.StartDialogue(dialogueDictionary, npcName, dialoguePortrait);
    }


 public override void _Process(double delta)
 {
     if(Input.IsActionJustPressed("Interact") && interactedWith == false)
     {
         StartDialogue();
        //  dm.DisplayDialogue(dialogue, name); //dialoguePortrait);
     }
 }
}
