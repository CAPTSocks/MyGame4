using Godot;
using System;


public partial class EnemySpawnInfo : Node
{
        [Export]
        public int enemyType;
        [Export]
        public int spawner;
        public float spawnDelay = 1;
    // Called when the node enters the scene tree for the first time.


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(double delta)
//  {
//      
//  }
}
