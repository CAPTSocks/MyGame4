using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class QuestDialogue : Node
{

	[Export] private String questItemName;
	[Export] private String[] dialoguePaths = new String[4];
	[Export] private string npcName;
	[Export] private Texture2D dialoguePortrait;
	private Dictionary dialogueDictionary;
	private Dictionary[] dialogues = new Dictionary[4];
	private bool playerInRange = false, interactedWith = false, firstInteraction = true, questTurnedIn = false;
	private Node dialogueManager, playerAccess;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);

		for (int i = 0; i < dialogues.Length; i++)
		{
			StreamReader r = new StreamReader(@dialoguePaths[i]);
			string jsonstr = r.ReadToEnd().ToString();
		//	JSON json = Json.ParseString;

		//	dialogueDictionary = (Godot.Collections.Dictionary)json.Result;
			dialogues[i] = dialogueDictionary;
		}
	}

	void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")
		{
			playerAccess = body;
			playerInRange = true;
			SetProcess(true);
		}
	}

	void OnBodyExited(Node body)
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
		dialogueDictionary = DetermineDialogue();

		dm.StartDialogue(dialogueDictionary, npcName, dialoguePortrait);
	}

	///Determines what dialogue the npc should say
	Dictionary DetermineDialogue()
	{
		Dictionary dictionaryToReturn;

		//if it is the players first time talking to this npc
		if (firstInteraction)
		{
			firstInteraction = false;
			dictionaryToReturn = dialogues[0];
			return dictionaryToReturn;
		}

		//If player talks to npc without the item
		if (CheckPlayerInventory() == false && questTurnedIn == false)
		{
			dictionaryToReturn = dialogues[1];
			return dictionaryToReturn;
		}
		//If player talks to npc with the item
		else if (CheckPlayerInventory() == true && questTurnedIn == false)
		{
			dictionaryToReturn = dialogues[2];
			var questItem = playerAccess.GetNodeOrNull<Node2D>("Items/" + questItemName);
			questItem.QueueFree();
			questTurnedIn = true;
			return dictionaryToReturn;
		}
		
		//if player talks to the npc after the quest is completed
		if (questTurnedIn == true)
		{
			dictionaryToReturn = dialogues[3];
			return dictionaryToReturn;
		}

		//This should never be reached
		return null;
	}

	///If the player has the quest item retruns true, returns false if they dont
	bool CheckPlayerInventory()
	{
		var questItem = playerAccess.GetNodeOrNull<Node2D>("Items/" + questItemName);

		if (questItem != null)
		{
			return true;
		}
		else 
		{
			return false;
		}

	}


	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Interact") && interactedWith == false)
		{
			StartDialogue();
			//  dm.DisplayDialogue(dialogue, name); //dialoguePortrait);
		}
	}
}
