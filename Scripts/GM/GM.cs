using Godot;
using System;

public partial class GM : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    void _on_RestartButton_pressed()
    {
        RestartLevel();
    }

    void _on_ExitButton_pressed()
    {
        QuitGame();
    }

    void RestartLevel()
    {
        GetTree().ReloadCurrentScene();
    }

    void QuitGame()
    {
        GetTree().Quit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(double delta)
//  {
//      
//  }
}
