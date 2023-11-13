using Godot;
using System;

public partial class PauseMenu : Control
{

    private bool paused = false;
    private Sprite2D background;
    [Export] private Color greyedOutColor, normalColor;
    private Control confirmMenu;
    private Control CollectiblesMenu;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Hide();
        background = GetNode<Sprite2D>("BackGround");
        confirmMenu = GetNode<Control>("ConfirmMenu");
        CollectiblesMenu = GetNode<Control>("CollectiblesMenu");
        confirmMenu.Hide();
    }

    void PauseGame()
    {
        GetTree().Paused = true;
        this.Visible = true;
    }

    void UnPauseGame()
    {
        GetTree().Paused = false;
        CollectiblesMenu.Hide();
        confirmMenu.Hide();
        background.Modulate = normalColor;
        this.Hide();
    }

    void ResumeButtonPressed()
    {
        UnPauseGame();
        paused = false;
    }

    void RestartButtonPressed()
    {
        var colGlobol = (CollectiblesGlobol)GetNode("/root/GlobalCollectibles");
        colGlobol.SceneChanged = true; 
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    void OptionsButtonPressed()
    {

    }

    void CollectiblesButtonPressed()
    {
        CollectiblesMenu.Show();
        background.Modulate = greyedOutColor;
    }

    void QuitButtonPressed()
    {
        confirmMenu.Show();
        background.Modulate = greyedOutColor;
    }

    void YesButtonPressed()
    {
        GetTree().Quit();
    }

    void NoButtonPressed()
    {
        confirmMenu.Hide();
        CollectiblesMenu.Hide();
        background.Modulate = normalColor;
    }

 public override void _PhysicsProcess(double delta)
 {
     if (Input.IsActionJustPressed("Pause"))
     {
        
         if (!paused)
         {
             PauseGame();
             paused = true;
         }
         else
         {
             UnPauseGame();
             paused = false;
         }
     }
 }
}
