using Godot;
using System;
using System.Diagnostics;
using System.Transactions;

public partial class DialogueNPC : Node
{
	[Export]
	private DialogueContainer dialogueContainer;
	private bool playerInRange = false, interactedWith = false;
	private DialogueManagerResource dm;
	private RichTextLabel pressEText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
		dm = GetTree().Root.GetNode<DialogueManagerResource>("TestScene/HUD/DialogueManager");
		pressEText = GetParent().GetNode<RichTextLabel>("PressEText");
		pressEText.Visible = false;
	}

	void OnBodyEntered(Node body)
	{
		GD.Print("Body entered");
		if (body.Name == "Player")
		{
			playerInRange = true;
			pressEText.Visible = true;
			SetProcess(true);
		}
	}

	void OnBodyExited(Node body)
	{
		GD.Print("Body Exited");
		if (body.Name == "Player")
		{
			pressEText.Visible = false;
			playerInRange = false;
			SetProcess(false);
			interactedWith = false;
		}
	}

	void StartDialogue()
	{
		GD.Print("start talking");
		interactedWith = true;
		dm?.StartDialogue(dialogueContainer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Interact") && interactedWith == false)
		{
			StartDialogue();
		}
	}
}
