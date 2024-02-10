using Godot;
using System;

[GlobalClass]
public partial class EnemyWaveInfo : Resource
{
    public enum EnemyTypeEnum
    {
        MeleeEnemy,
        RangedEnemy,
        HomingEnemy,
        ChargeEnemy
    };
    public enum SpawnEnum
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    };

    [Export]
    public SpawnEnum spawnLocation;

    [Export]
    public EnemyTypeEnum enemyType;
    [Export]
    public float nextEnemySpawnDelay = .5f;
}
