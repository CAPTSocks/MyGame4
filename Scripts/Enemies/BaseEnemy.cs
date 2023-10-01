using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D
{

    [Export] public int currentHealth = 100, maxHealth = 100, attackDamage = 10;
    [Export] public float speed, detectionRange, attackRange, attackRate, targetDistance;
    [Signal] public delegate void enemyDiedEventHandler(CharacterBody2D body);
    private PackedScene food = (PackedScene)GD.Load("res://Prefabs/Items/FoodPickup.tscn");
    public Timer attackTimer;
    public Vector2 velocity, targetDirection;
    public bool isDead = false, attacking = true;
    public CharacterBody2D target;
    public Health targetHealthAccess;

    public override void _Ready()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GD.Print("Enemy Health " + currentHealth);

        if (currentHealth <= 0)
        {
            GD.Print("Enemy is dead");
            HasDied();
        }
    }

    public void HasDied()
    {
        isDead = true;
        EmitSignal(nameof(enemyDied), this);
        SetProcess(false);
        SetPhysicsProcess(false);

        Die();
    }

    public virtual void Die()
    {
          //Randomly spawn food if rand num is 1
            var rand = new Random();
            var randNum = rand.Next(1, 11);
            if (randNum == 1)
            {
                CallDeferred("SpawnFood");
            }

        this.QueueFree();
    }

    private void SpawnFood()
    {
        var f = food.Instantiate<FoodPickup>();
        f.Spawn(this.Position);
        GetParent().AddChild(f);

        GD.Print("I am called");

    }


}
