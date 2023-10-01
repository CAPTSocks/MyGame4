using Godot;
using Godot.Collections;
using System;

public partial class DialogueManager : Node
{

    [Export] private float timerWait = .05f;
    [Export] private Color grey, normalColor;
    //Value between 0 and 2 that determines what pressing E does 
    private int currentTextLine, lastTextLine, inputAction = 0;
    private string[] dialogueArray;
    private string displayDialogue;
    private string[] talkerNameArray;
    private Dictionary wholeDict, displayDict;
    private bool waitForInput, dialogueBeingShown;
    private Sprite2D dialogueBox, playerPortrait, npcPortrait;
    private Label talkerName, continueText;
    private RichTextLabel dialogueText;
    private Timer nextLetterTimer;
    private AnimationPlayer anim;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        dialogueText = GetNode<RichTextLabel>("DialogueBox/Dialogue");
        talkerName = GetNode<Label>("DialogueBox/Name");
        continueText = GetNode<Label>("DialogueBox/ContinueText");
        nextLetterTimer = GetNode<Timer>("NextLetterTimer");
        anim = GetNode<AnimationPlayer>("Anim");
        dialogueBox = GetNode<Sprite2D>("DialogueBox");
        playerPortrait = GetNode<Sprite2D>("DialogueBox/PlayerPortrait");
        npcPortrait = GetNode<Sprite2D>("DialogueBox/NPCPortrait");

        continueText.Visible = false;
        dialogueBox.Visible = false;
    }

    void _on_NextLetterTimer_timeout()
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

            //dialogueText.Text = dialogueArray[1];
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

        displayDict = (Dictionary)wholeDict[currentTextLine.ToString()];

        displayDialogue = displayDict["dialogue"].ToString();
        dialogueText.Text = displayDialogue;

        if (displayDict["name"].ToString() != "Player")
        {
            playerPortrait.Modulate = grey;
            npcPortrait.Modulate = normalColor;
        }
        else
        {
            playerPortrait.Modulate = normalColor;
            npcPortrait.Modulate = grey;
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

    void _on_Anim_animation_finished(String animName)
    {
        if (animName == "MoveOnScreen")
        {
            GD.Print("Anim Done");
            inputAction = 1;
            waitForInput = true;
            nextLetterTimer.Start(timerWait);
        }
    }

    public void StartDialogue(Dictionary sentDict, string npcName, Texture2D npcTexture) //Sprite2D talkerPortrait)
    {
        currentTextLine = 0;


        wholeDict = sentDict;
        displayDict = (Dictionary)wholeDict[currentTextLine.ToString()];
        displayDialogue = displayDict["dialogue"].ToString();
        lastTextLine = wholeDict.Count - 1;

        talkerName.Text = npcName;
        npcPortrait.Texture = npcTexture;
        if (displayDict["name"].ToString() != "Player")
        {
            playerPortrait.Modulate = grey;
            npcPortrait.Modulate = normalColor;
        }
        else
        {
            playerPortrait.Modulate = normalColor;
            npcPortrait.Modulate = grey;
        }
        //displayDict["name"].ToString();
        dialogueText.Text = displayDialogue;
        dialogueText.VisibleCharacters = 0;

        dialogueBox.Position = new Vector2(0, -217);
        anim.Play("MoveOnScreen");
    }

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
