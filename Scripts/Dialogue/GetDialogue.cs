using Godot;
using System;
using Godot.Collections;

public partial class GetDialogue : Node
{
    //[Export] private String dialogueFilePath;

    public override void _Ready()
    {
        // GD.Print(LoadDialogue(dialogueFilePath));
    }

    void Interact()
    {
        //Dictionary<string, string> dialogue = LoadDialogue();

    }

    // Dictionary LoadDialogue(String filePath)
    // {
    //     var file = new File();
    //     if (file.FileExists(filePath))
    //     {
    //         file.Open(filePath, File.ModeFlags.Read);
    //         Dictionary dic = new Dictionary();
    //         dic = JSON.Parse(file.GetAsText()).Result as Dictionary;
    //         if (dic != null)
    //         {
    //             GD.Print("HI: " + dic[0]);
    //             return dic;
    //         }

    //         else
    //         {
    //             GD.Print("No path");
    //             return null;
    //         }
    //     }
    //     else
    //     {
    //         GD.Print("no Path3D");
    //         return null;
    //     }

    // }
}
