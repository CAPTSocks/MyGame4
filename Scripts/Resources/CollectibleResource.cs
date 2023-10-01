using Godot;
using System;

[GlobalClass]
public partial class CollectibleResource : Resource
{
    [Export]
    private int id = 0;
    [Export]
    private bool collected = false;
    [Export]
    private string name = "";
    [Export]
    private string description = "";
    [Export]
    private string scene = "";
    [Export]
    private Texture2D image; 

    //properties
    public int ID { get {return id;} }
    public bool Collected { get {return collected;} set {id = ID;} }
    public string Name { get {return name;} }
    public string Description { get {return description;} }
    public string Scene { get {return scene;} }
    public Texture2D Image { get {return image;}  }
}
