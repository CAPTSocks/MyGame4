using Godot;
using System;
using Godot.Collections;

public partial class DialogueManagerResource : Node
{
	[Export] private float timerWait = .05f;
    [Export] private Color grey, normalColor;
    //Value between 0 and 2 that determines what pressing E does 
	private RichTextLabel dialogueText;
	private Label talkerName, continueText;
	private Timer nextLetterTimer;
	private AnimationPlayer anim;
	private Sprite2D dialogueBox, LeftPortrait, RightPortrait;

	private int currentTextLine, lastTextLine, inputAction = 0;
	private bool waitForInput, dialogueBeingShown;
	private string displayDialogue;
	private Array<DialogueResource> currentDialogue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dialogueText = GetNode<RichTextLabel>("DialogueBox/Dialogue");
        talkerName = GetNode<Label>("DialogueBox/Name");
        continueText = GetNode<Label>("DialogueBox/ContinueText");
        nextLetterTimer = GetNode<Timer>("NextLetterTimer");
        anim = GetNode<AnimationPlayer>("Anim");
        dialogueBox = GetNode<Sprite2D>("DialogueBox");
        LeftPortrait = GetNode<Sprite2D>("DialogueBox/LeftPortrait");
        RightPortrait = GetNode<Sprite2D>("DialogueBox/RightPortrait");

        continueText.Visible = false;
        dialogueBox.Visible = false;
	}

	void OnTimerTimeout()
	{
		      dialogueText.VisibleCharacters = dialogueText.VisibleCharacters + 1;

        if (dialogueText.VisibleCharacters == displayDialogue.Length)
        {
            if (currentTextLine == lastTextLine)
            {
                nextLetterTimer.Stop();
                inputAction = 2;
                continueText.Visible = true;
                waitForInput = true;
                return;
            }
            nextLetterTimer.Stop();
            inputAction = 0;
            continueText.Visible = true;
            waitForInput = true;
		}
	}

	  void SkipToEnd()
    {
        GD.Print("Hello");
        nextLetterTimer.Stop();
        dialogueText.VisibleCharacters = displayDialogue.Length;
        if (currentTextLine != lastTextLine)
        {
            inputAction = 0;
            waitForInput = true;
        }
        else
        {
            inputAction = 2;
            waitForInput = true;
        }
    }

	 void SetNextLine()
    {
        dialogueText.VisibleCharacters = 0;
        currentTextLine = currentTextLine + 1;
        continueText.Visible = false;

       // displayDict = (Dictionary)wholeDict[currentTextLine.ToString()];

        displayDialogue = currentDialogue[currentTextLine].Dialogue;
        dialogueText.Text = displayDialogue;

        if (currentDialogue[currentTextLine].LeftSpeaking == false)
        {
            LeftPortrait.Modulate = grey;
            RightPortrait.Modulate = normalColor;
        }
        else
        {
            LeftPortrait.Modulate = normalColor;
            RightPortrait.Modulate = grey;
        }

        inputAction = 1;
        waitForInput = true;
        nextLetterTimer.Start(timerWait);
    }

	    void EndDialogue()
    {
        continueText.Visible = false;
        anim.Play("MoveOffScreen");
    }

    void _on_Anim_animation_finished(StringName animName)
    {
        if (animName == "MoveOnScreen")
        {
            GD.Print("Anim Done");
            inputAction = 1;
            waitForInput = true;
            nextLetterTimer.Start(timerWait);
        }
    }

    public void StartDialogue(DialogueContainer sentDialogueContainer) //Sprite2D talkerPortrait)
    {
        currentTextLine = 0;


        // wholeDict = sentDict;
        // displayDict = (Dictionary)wholeDict[currentTextLine.ToString()];
        // displayDialogue = displayDict["dialogue"].ToString();
        // lastTextLine = wholeDict.Count - 1;

		currentDialogue = sentDialogueContainer.Dialogues;
		displayDialogue = currentDialogue[currentTextLine].Dialogue;
        lastTextLine = currentDialogue.Count - 1;


        talkerName.Text = currentDialogue[currentTextLine].SpeakerName;
        LeftPortrait.Texture = currentDialogue[currentTextLine].LeftSpeakerImage;
        RightPortrait.Texture = currentDialogue[currentTextLine].RightSpeakerImage;


        if (currentDialogue[currentTextLine].LeftSpeaking == false)
        {
            LeftPortrait.Modulate = grey;
            RightPortrait.Modulate = normalColor;
        }
        else
        {
            LeftPortrait.Modulate = normalColor;
            RightPortrait.Modulate = grey;
        }
        //displayDict["name"].ToString();
        dialogueText.Text = displayDialogue;
        dialogueText.VisibleCharacters = 0;

        dialogueBox.Position = new Vector2(0, -217);
        anim.Play("MoveOnScreen");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Interact") && waitForInput == true)
        {
            waitForInput = false;
            // SetNextLine();

            switch (inputAction)
            {
                //If Dialogue is finished and there is another line after
                case 0:
                    SetNextLine();
                    break;

                //If Dialogue is still running skip to the end
                case 1:
                    SkipToEnd();
                    break;

                //If dialogue is finished and there isnt another line
                case 2:
                    EndDialogue();
                    break;
            }
        }
	}
}
