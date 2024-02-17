using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EncounterRunner : Node
{                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    private Node2D topLeftSpawn, topRightSpawn, bottomLeftSpawn, bottomRightSpawn;
    private Timer encounterTimer;
    [Export]
    private bool verticalEncounter = false;
    private bool encounterStarted = false, stillSpawning = false;
    [Export]
    private int waveCounter = 0, enemySpawnCounter = 0, numOfWaves = 0;
    [Export]
    private Array<string> enemyPathList = new Array<string>();
    //private List<EnemySpawnInfo> waveEnemiesToSpawn = new List<EnemySpawnInfo>();
    //private Godot.Collections.Array<EnemySpawnInfo> waveEnemiesToSpawnG = new Array<EnemySpawnInfo>();
    private List<Node2D> spawnerList = new List<Node2D>();
    private List<BaseEnemy> aliveEnemies = new List<BaseEnemy>();
   // public List<EnemySpawnInfo> preList = new List<EnemySpawnInfo>();
   // public Array<EnemySpawnInfo> preListg = new Array<EnemySpawnInfo>();
   // public List<List<EnemySpawnInfo>> enemiesToSpawnList = new List<List<EnemySpawnInfo>>();
   // public Array<Array<EnemySpawnInfo>> enemiesToSpawnListG = new Array<Array<EnemySpawnInfo>>();
    private Camera2D mainCam;

    //Resources
    [Export]
    private Waves wavesResource;
    private EncounterWave currentWave;

    public override void _Ready()
    {
        topLeftSpawn = GetNode<Node2D>("TopLeftSpawner");
        topRightSpawn = GetNode<Node2D>("TopRightSpawner");
        bottomLeftSpawn = GetNode<Node2D>("BottomLeftSpawner");
        bottomRightSpawn = GetNode<Node2D>("BottomRightSpawner");
        spawnerList.Add(topLeftSpawn);
        spawnerList.Add(topRightSpawn);
        spawnerList.Add(bottomLeftSpawn);
        spawnerList.Add(bottomRightSpawn);

        encounterTimer = GetNode<Timer>("EncounterTimer");

        mainCam = GetNode<Camera2D>("/root/" + GetTree().CurrentScene.Name + "/Camera2D");

        getEnemyWaves();


        var screenSize = DisplayServer.WindowGetSize();
        Vector2 screenSizeFloat;
        screenSizeFloat.X = screenSize.X;
        screenSizeFloat.Y = screenSize.Y;
        screenSizeFloat *= mainCam.Zoom;

        if (!verticalEncounter)
        {
            //Place spawners for horizontal encounter
            float x = screenSize.X / 4 - 100;
            float y = screenSize.Y / 7;

            topLeftSpawn.Position = new Vector2(-x, -y);
            bottomLeftSpawn.Position = new Vector2(-x, y);
            topRightSpawn.Position = new Vector2(x, -y);
            bottomRightSpawn.Position = new Vector2(x, y);
        }
        else
        {
            //Place spawners for vertical encounter
            float x = screenSize.X / 4;
            float y = screenSize.Y / 2 + 55;

            topLeftSpawn.Position = new Vector2(-x, -y);
            bottomLeftSpawn.Position = new Vector2(-x, y);
            topRightSpawn.Position = new Vector2(x, -y);
            bottomRightSpawn.Position = new Vector2(x, y);
        }
    }

    public void getEnemyWaves()
    {
        currentWave = wavesResource.wave[waveCounter];
        numOfWaves = wavesResource.wave.Count - 1;
        // var wavesChild = GetNode<Node>("Waves");
        // var children = wavesChild.GetChildren();
        // for (int i = 0; i < children.Count; i++)
        // {
        //     var wave = (Node)children[i];
        //     var waveChildren = wave.GetChildren();
        //     for (int j = 0; j < waveChildren.Count; j++)
        //     {
        //         preListg.Add((EnemySpawnInfo)waveChildren[j]);
        //         if (j == waveChildren.Count - 1)
        //         {
        //             enemiesToSpawnListG.Add(preListg);
        //             preListg = new Array<EnemySpawnInfo>();
        //         }
        //     }
        // }
    }

    public void StartEncounter()
    {
        encounterStarted = true;
        stillSpawning = true;
      //  waveEnemiesToSpawnG = enemiesToSpawnListG[waveCounter];

        SpawnEnemies();
    }

    public void StartNextWave()
    {
        //Increment wave counter and reset enemySpawnCounter
        waveCounter++;
        enemySpawnCounter = 0;

        //Change the enemies to spawn to the new wave of enemies
       // waveEnemiesToSpawnG = enemiesToSpawnListG[waveCounter];
       currentWave = wavesResource.wave[waveCounter];

        stillSpawning = true;

        //Spawn the next enemy after the timer ends
        encounterTimer.Start();
        GD.Print("Starting Wave " + waveCounter);
    }

    public void EndEncounter()
    {
        var CombatEnder = GetParent<CombatStarter>();
        CombatEnder.EndEncounter();
    }

    public void EncounterTimerTimeout()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (enemySpawnCounter < currentWave.enemiesTypesToSpawn.Count)
        {
            EnemyWaveInfo newEnemy = currentWave.enemiesTypesToSpawn[enemySpawnCounter];
            int enemyType = (int)newEnemy.enemyType;
            PackedScene enemyScene = (PackedScene)GD.Load(enemyPathList[enemyType]);
            BaseEnemy newEnemyInstance = (BaseEnemy)enemyScene.Instantiate();
            newEnemyInstance.GlobalPosition = spawnerList[(int)newEnemy.spawnLocation].GlobalPosition;
            GetTree().CurrentScene.AddChild(newEnemyInstance);
            
            aliveEnemies.Add(newEnemyInstance);
            newEnemyInstance.Connect("enemyDied",new Callable(this,nameof(EnemyDied)));
            enemySpawnCounter++;
            encounterTimer.Start(newEnemy.nextEnemySpawnDelay);
        }
        else
        {
            stillSpawning = false;
            encounterTimer.Stop(); 
        }


        // if (enemySpawnCounter < waveEnemiesToSpawnG.Count)
        // {

        //     var newEnemy = waveEnemiesToSpawnG[enemySpawnCounter];
        //     var enemyScene = (PackedScene)GD.Load(enemyPathList[newEnemy.enemyType]);
        //     var enemyInstance = (BaseEnemy)enemyScene.Instantiate();
        //     enemyInstance.GlobalPosition = spawnerList[newEnemy.spawner].GlobalPosition;
        //     GetTree().CurrentScene.AddChild(enemyInstance);

        //     aliveEnemies.Add(enemyInstance);
        //     enemyInstance.Connect("enemyDied",new Callable(this,nameof(EnemyDied)));
        //     enemySpawnCounter++;

        //     encounterTimer.Start(newEnemy.spawnDelay);
        // }
        // else
        // {
        //     stillSpawning = false;
        //     encounterTimer.Stop();
        // }
    }

    public void EnemyDied(CharacterBody2D deadEnemy)
    {
        BaseEnemy enemyToRemove = null;

        foreach (var enemy in aliveEnemies)
        {
            if (enemy == deadEnemy)
            {
                enemyToRemove = enemy;
            }
        }

        if (enemyToRemove != null)
        {
            aliveEnemies.Remove(enemyToRemove);
            if (aliveEnemies.Count < 1 && waveCounter != numOfWaves && !stillSpawning)
            {
                StartNextWave();
            }
            else if (waveCounter == numOfWaves && aliveEnemies.Count == 0)
            {
                EndEncounter();
            }
        }
    }

    // public override void _Process(double delta)
    // {

    // }
}
