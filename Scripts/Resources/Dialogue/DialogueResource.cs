using Godot;
using System;

[GlobalClass]
public partial class DialogueResource : Resource
{
    [Export]
    private string speakerName = "";
    [Export]
    private string dialogue = "";
    [Export]
    private Texture2D leftSpeakerImage; 
    [Export]
    private Texture2D rightSpeakerImage; 
    [Export]
    private bool leftSpeaking;


    //properties

    public string SpeakerName { get{return speakerName;} }
    public string Dialogue { get{return dialogue;} }
    public Texture2D LeftSpeakerImage { get{return leftSpeakerImage;} }
    public Texture2D RightSpeakerImage {get {return rightSpeakerImage;}}
    public bool LeftSpeaking { get{return leftSpeaking;} set{leftSpeaking = LeftSpeaking;} }
}
