using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Waves : Resource
{
    [Export]
    public Array<EncounterWave> wave = new Array<EncounterWave>();
}
