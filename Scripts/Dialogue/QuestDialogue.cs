using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class QuestDialogue : Node
{

	[Export] private String questItemName;
	//[Export] private String[] dialoguePaths = new String[4];
	//[Export] private string npcName;
	//[Export] private Texture2D dialoguePortrait;

	[Export] private Array<DialogueContainer> questDialogues = new Array<DialogueContainer>();
	//private Dictionary dialogueDictionary;
	private bool playerInRange = false, interactedWith = false, firstInteraction = true, questTurnedIn = false;
	private Node playerAccess;
	private DialogueManagerResource dm;
	[Export]
	private QuestProgress qp;	
	private QuestStateEnum questState = QuestStateEnum.FirstInteraction;

	enum QuestStateEnum
	{
		FirstInteraction,
		QuestInProgress,
		QuestTurnedIn,
		PostQuest
	}

	//Add Int or enum to determine what state quest giver is in
	//Fix to use resource
	//Create questdialogue resource to use
	//see if you need to do the above thing or not


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
		questState = (QuestStateEnum)qp.QuestProgressNum;

		dm = GetTree().Root.GetNode<DialogueManagerResource>("TestScene/HUD/DialogueManager");
	}

	void OnBodyEntered(Node body)
	{
		GD.Print("Body entered");
		if (body.Name == "Player")
		{
			playerAccess = body;
			playerInRange = true;
			if (dm != null)
			{
				StartDialogue();
			}
			//SetProcess(true);
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
		DetermineDialogue();
		//DialogueManager dm = GetTree().Root.GetNode<DialogueManager>("TestScene/HUD/DialogueManager");
		//dialogueDictionary = DetermineDialogue();
		//dm.StartDialogue(dialogueDictionary, npcName, dialoguePortrait);


	}

	///Determines what dialogue the npc should say
	void DetermineDialogue()
	{
		// Dictionary dictionaryToReturn;

		// //if it is the players first time talking to this npc
		// if (firstInteraction)
		// {
		// 	firstInteraction = false;
		// 	dictionaryToReturn = dialogues[0];
		// 	return dictionaryToReturn;
		// }

		// //If player talks to npc without the item
		// if (CheckPlayerInventory() == false && questTurnedIn == false)
		// {
		// 	dictionaryToReturn = dialogues[1];
		// 	return dictionaryToReturn;
		// }
		// //If player talks to npc with the item
		// else if (CheckPlayerInventory() == true && questTurnedIn == false)
		// {
		// 	dictionaryToReturn = dialogues[2];
		// 	var questItem = playerAccess.GetNodeOrNull<Node2D>("Items/" + questItemName);
		// 	questItem.QueueFree();
		// 	questTurnedIn = true;
		// 	return dictionaryToReturn;
		// }
		
		// //if player talks to the npc after the quest is completed
		// if (questTurnedIn == true)
		// {
		// 	dictionaryToReturn = dialogues[3];
		// 	return dictionaryToReturn;
		// }

		switch (questState)
		{
			
			case QuestStateEnum.FirstInteraction:
			FirstInteraction();
			break;
			case QuestStateEnum.QuestInProgress:
			QuestInProgress();
			break;
			case QuestStateEnum.QuestTurnedIn:
			QuestTurnedIn();
			break;
			case QuestStateEnum.PostQuest:
			PostQuest();
			break;
		}
	}

	private void FirstInteraction()
	{
		firstInteraction = false;
		dm.StartDialogue(questDialogues[0]);
		questState = QuestStateEnum.QuestInProgress;
		qp.SaveQuestProgress(1);
			// dictionaryToReturn = dialogues[0];
			// return dictionaryToReturn;
	}

	private void QuestInProgress()
	{
		
		if (CheckPlayerInventory() == true)
		{
			//questState = QuestStateEnum.QuestTurnedIn;
			//DetermineDialogue();
			QuestTurnedIn();
			qp.SaveQuestProgress(2);
			return;
		}
		
			dm.StartDialogue(questDialogues[1]);

		//change dialogue to 2
	}

	private void QuestTurnedIn()
	{
		//change dialogue to 3
		dm.StartDialogue(questDialogues[2]);
		questState = QuestStateEnum.PostQuest;
		qp.SaveQuestProgress(3);
	}

	private void PostQuest()
	{
		dm.StartDialogue(questDialogues[3]);
		//change dialogue to 4
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
