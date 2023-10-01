using Godot;
using System;

public partial class Health : Node
{

    // SignalAttribute healthChanged(currentHealth);
    [Export] private int maxHealth = 100, currentHealth = 100, maxEnergy = 60;

    [Export] private float energyReginRate = .1f, currentEnergy = 60;

    [Signal] public delegate void HealthChangedEventHandler(int currentHealthEvt);
    [Signal] public delegate void EnergyChangedEventHandler(float currentEnergyEvt);
    [Signal] public delegate void PlayerDeadEventHandler();
    public bool isDead = false, CanRegin = true;

    //Properties
    public float CurrentEnergy { get => currentEnergy; }
    public int CurrentHealth { get => currentHealth; }

    public int MaxHealth { get => maxHealth; }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (currentEnergy < maxEnergy)
        {
            ReginEnergy();
        }

        if (Input.IsActionJustPressed("ui_up"))
        {
            ChangeHealth(10);
            GD.Print("Health is " + currentHealth);
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            ChangeHealth(-10);
            GD.Print("Health is " + currentHealth);
        }

        // if (Input.IsActionJustPressed("Dodge"))
        // {
        //     useEnergy(-25);
        //     GD.Print("Energy is " + currentEnergy);
        // }
    }

    void ReginEnergy()
    {
        if (!isDead && CanRegin != false)
        {
            currentEnergy = Mathf.MoveToward(currentEnergy, maxEnergy, energyReginRate);
            EmitSignal(SignalName.EnergyChanged, currentEnergy);
           // EmitSignal(nameof(EnergyChanged), currentEnergy);
        }
    }

    public void useEnergy(int energyAmount)
    {
        if (!isDead)
        {
            if (energyAmount <= 0)
            {
                if (currentEnergy > energyAmount * -1)
                {
                    currentEnergy += energyAmount;
                    Mathf.Clamp(currentEnergy, 0, maxEnergy);
                    EmitSignal(SignalName.EnergyChanged, currentEnergy);
                }
            }
            else
            {
                currentEnergy += energyAmount;
                Mathf.Clamp(currentEnergy, 0, maxEnergy);
                EmitSignal(SignalName.EnergyChanged, currentEnergy);
            }
        }
    }

    public void ChangeHealth(int changeAmount)
    {
        if (!isDead)
        {
            var healthBeforeChange = currentHealth;
            currentHealth += changeAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (healthBeforeChange == currentHealth)
            {
                return;
            }

            EmitSignal(SignalName.HealthChanged, currentHealth);

            if (currentHealth == 0)
            {
                isDead = true;
                EmitSignal(SignalName.PlayerDead);
            }
        }
    }
}
