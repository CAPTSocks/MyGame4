using Godot;
using System;

[GlobalClass]
public partial class QuestProgress : Resource
{
    [Export]
    private int questProgressNum = Mathf.Clamp(0, 0, 3);

    public int QuestProgressNum { get{return questProgressNum;} }

    public void SaveQuestProgress(int newProgress)
    {
        questProgressNum = newProgress;
        ResourceSaver.Save(this, this.ResourcePath);
    }
}
