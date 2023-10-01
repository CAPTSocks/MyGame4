using Godot;
using System;

public partial class HUD : CanvasLayer
{
   
    private AnimationPlayer deathScrenAnim;
    private ColorRect deathScreenBG;

    public override void _Ready()
    {
        deathScrenAnim = GetNode<AnimationPlayer>("DeathScreen/AnimationPlayer");
        deathScreenBG = GetNode<ColorRect>("DeathScreen");

        deathScreenBG.Hide();
    }

    void _on_Health_PlayerDead()
    {
        deathScreenBG.Show();
        deathScrenAnim.Play("FadeIn");
    }

//  public override void _Process(double delta)
//  {
//      
//  }
}
