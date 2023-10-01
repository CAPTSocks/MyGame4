using Godot;
using System.Collections.Generic;
using System;

public partial class FoodPickup : Area2D
{

    [Export] public int healAmountLow, healAmountMed, healAmountLarge, fullHeal, currHealAmount;
    private Sprite2D mySprite;
    private Health pHealth;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       
    }

    public void Spawn(Vector2 position)
    {
        this.Position = position;
        mySprite = GetNode<Sprite2D>("Sprite2D");
        PickFood();
    }

    //Determines what the food spawns as
    private void PickFood()
    {
        int randNum;
        Random rand = new Random();
        randNum = rand.Next(23);
        mySprite.Frame = randNum;
        GD.Print(randNum);

        // if (randNum <= 9)
        // {
        //     currHealAmount = healAmountLow;
        // }
        // else if (randNum > 9 && randNum <= 16)
        // {
        //     currHealAmount = healAmountMed;
        // }
        // else if (randNum > 16 && randNum < 22)
        // {
        //     currHealAmount = healAmountLarge;
        // }
        // else if (randNum == 22)
        // {
        //     currHealAmount = fullHeal;
        // }

        switch (randNum)
        {
            case var expression when randNum <= 9:
                currHealAmount = healAmountLow;
                break;

            case var expression when randNum <= 16:
                currHealAmount = healAmountMed;
                break;

            case var expression when randNum < 22:
                currHealAmount = healAmountLarge;
                break;

            case 22:
                currHealAmount = fullHeal;
                break;
        }

    }

    //on player collision heal player and destroy this food
    void _on_body_entered(Node body)
    {
        if (body.Name == "Player")
        {
            GD.Print("Collision Detected");
            pHealth = body.GetNode<Health>("Health");

            if (pHealth.CurrentHealth == pHealth.MaxHealth)
            {
                return;
            }

            pHealth.ChangeHealth(currHealAmount);
            GD.Print("Healed by " + currHealAmount);

            this.QueueFree();
        }
    }

    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
