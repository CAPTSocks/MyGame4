using Godot;
using System;

public partial class UpdateHealthBar : Control
{
    private TextureProgressBar healthBar, energyBar, underBar;

    private Tween underbarTween;

    [Export] private Texture2D highHealth, halfHealth, lowHealth;

    [Export] private Color gray, white;

    public override void _Ready()
    {
        healthBar = GetNode<TextureProgressBar>("HealthBar");
        underBar = GetNode<TextureProgressBar>("UnderBar");
        energyBar = GetNode<TextureProgressBar>("EnergyBar");
        underbarTween = GetNode<Tween>("TweenUnderBar");
    }

    ///Sets the length of the health bar directly. Only call at the start of the scene. 
    public void SetBarsDirectly(int newHealthSize)
    {
        healthBar.Value = newHealthSize;
        underBar.Value = newHealthSize;
    }

    private void _on_Health_HealthChanged(int currentHealth)
    {
        healthBar.Value = currentHealth;

        switch (healthBar.Value)
        {
            case var expression when healthBar.Value <= healthBar.MaxValue / 4:
                healthBar.TextureProgress = lowHealth;
                break;

            case var expression when healthBar.Value <= healthBar.MaxValue / 2:
                //healthBar.TintProgress = halfHealth;
                healthBar.TextureProgress = halfHealth;
                break;

            default:
                healthBar.TextureProgress = highHealth;
                break;
        }



        // if (healthBar.Value <= healthBar.MaxValue / 2)
        // {
        //     healthBar.TintProgress = halfHealth;
        // }
        // else if ()

        //TODO:Fix Tween
     //   underbarTween.InterpolateProperty(underBar, "value", underBar.Value, healthBar.Value, .5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
      //  underbarTween.Start();
    }

    void _on_Health_EnergyChanged(int currentEnergy)
    {
        energyBar.Value = currentEnergy;

        if (energyBar.Value < 25)
        {
            energyBar.TintProgress = gray;
        }
        else
        {
            energyBar.TintProgress = white;
        }
    }

}
