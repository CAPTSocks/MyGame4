using Godot;
using System;
using System.Transactions;

public partial class DialogueNPC : Node
{
	[Export]
	private DialogueContainer dialogueContainer;
	private bool playerInRange = false, interactedWith = false;
	private DialogueManagerResource dm; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
		dm = GetTree().Root.GetNode<DialogueManagerResource>("TestScene/HUD/DialogueManager");
	}

	    void OnBodyEntered (Node body)
    {
		GD.Print("Body entered");
        if (body.Name == "Player")
        {
            playerInRange = true;
			if (dm != null)
			{
				dm.StartDialogue(dialogueContainer);
			}
            //SetProcess(true);
        }
    }

	    void OnBodyExited (Node body)
    {
		GD.Print("Body Exited");
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
		dm?.StartDialogue(dialogueContainer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//StartDialogue();
	}
}
