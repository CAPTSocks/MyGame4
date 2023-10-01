using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EnemyWaves : Node
{
    [Export]
    public int amountOfWaves = 2;

    //MAKE SURE TOTAL MATCHES AMOUNT OF ENEMYTOSPAWN INFO CHILDREN
    //[Export]
    //public Array<int> enemySpawnPerWave = new Array<int>(); 
	public List<EnemySpawnInfo> preList = new List<EnemySpawnInfo>();
    public List<List<EnemySpawnInfo>> EnemiesToSpawnList = new List<List<EnemySpawnInfo>>();

    public override void _Ready()
    {
        var children = GetChildren();
        GD.Print(children.Count);
        for (int i = 0; i < children.Count; i++)
        {
            var wave = (Node)children[i];
            var waveChildren = wave.GetChildren();
            for (int j = 0; j < waveChildren.Count; j++)
            {
                preList.Add((EnemySpawnInfo)waveChildren[j]);
                if (j == waveChildren.Count - 1)
                {
                    EnemiesToSpawnList.Add(preList);
					preList = new List<EnemySpawnInfo>();
                }
            }
        }
    }

}

