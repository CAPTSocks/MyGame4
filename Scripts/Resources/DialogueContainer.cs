using Godot;
using Godot.Collections;
using System;
//using System.Collections.Generic;

[GlobalClass]
public partial class DialogueContainer : Resource
{
    [Export]
    private Array<DialogueResource> dialogues = new Array<DialogueResource>(); 
}
