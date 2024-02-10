using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class EncounterWave : Resource
{
    public int currentWave = 0;
    [Export]
    public Array<EnemyWaveInfo> enemiesTypesToSpawn = new Array<EnemyWaveInfo>(); 

}
