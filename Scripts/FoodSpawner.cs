using Godot;
using System;

public partial class FoodSpawner : StaticBody2D
{
    private PackedScene food = (PackedScene)GD.Load("res://Prefabs/Items/FoodPickup.tscn");


    // private void OnTweenStarted(Godot.Object obj, NodePath key)
    // {

    // }

    private void OnFadeOutStarted(StringName animName)
    {
        var rand = new Random();
        var randNum = rand.Next(1, 11);
        if (randNum <= 4)
        {
            SpawnFood();
        }

    }

    public void SpawnFood()
    {
        var f = food.Instantiate<FoodPickup>();
        f.Spawn(this.Position);
        AddChild(f);

        GD.Print("Spawning Food");
    }
}
