using Godot;
using Godot.Collections;
using System;
//using System.IO;


//Have a list of collectibles thare collected
//Have a list of pages for each collectible
//if the collectible is collected add it to the page list with the collected screen
//If it isnt collected add the not collected screen to the page list 
//give the list to the some menu code for it to work with

public partial class CollectiblesGlobol : Node
{
    // public Collectible[] collectedCollectibles = new Collectible[7];
    [Export] private String filePath = "Collectibles/Collectibles.json";

    [Export]
    private CollectibleResource[] CollectiblesData;
    [Signal]
    public delegate void updateMenuEventHandler();
    private CollectibleMenu menuAccess;

    private Dictionary CollectibleDic, data;
    private Collectible[] collectibles = new Collectible[7];
    private String[] names;

    // Link: https://godotforums.org/discussion/19326/parsing-json-with-c
    // Other Link: https://godotengine.org/qa/29925/how-to-modify-a-json-file

    //public Collectible[] Collectibles { get { return collectibles; } }

    public override void _Ready()
    {
        //get access to the menu
        menuAccess = GetCollectibleMenu();
        //get access to the JSON
        //  var f = new File();
        //   f.Open(filePath, File.read);
        //   var jsonC = JSON.Parse(f.GetAsText());
        //    f.Close();
        //    data = (Godot.Collections.Dictionary)jsonC.Result;

        //GD.Print("I Am Called!!!");
    }

    ///Get and send back access to the levels collectible menu 
    public CollectibleMenu GetCollectibleMenu()
    {
        var root = GetTree().Root;
        var currentScene = root.GetChild(root.GetChildCount() - 1);

        var m = currentScene.GetNode<CollectibleMenu>("HUD/PauseMenu/CollectiblesMenu");
        return m;
    }

    ///When the scene is changed resetup all the collectible pages
    public void SetUpCollectiblesForNewScene()
    {
        menuAccess = GetCollectibleMenu();
        for (int i = 0; i < 3; i++)
        {
            UpdateCollectedArray(i);
        }
    }


    //checks if a collectible has been collected already
    public bool CheckIfCollected(int collectibleID) //, string[] collectibles)
    {

        // var itemData = (Godot.Collections.Dictionary)data[collectibleID.ToString()];

        // if (itemData != null)
        // {
        //     if ((bool)itemData["Collected"] == true)
        //     {
        //         return true;
        //     }
        //     else return false;
        // }
        // return false;

        if (CollectiblesData[collectibleID] != null)
        {
            if (CollectiblesData[collectibleID].Collected)
            {
                menuAccess.UpdateMenu(CollectiblesData[collectibleID]);
                return true;
            }
            return false;
        }
        return false;
    }

    //When a collectible is collected get its info and send it
    //to the update collectible array
    public void CollectCollectible(int collectibleID)
    {
      //  var data2 = (Godot.Collections.Dictionary)data[collectibleID.ToString()];

     //   GD.Print(data2["Collected"]);

        //Change Json file
      //  data2["Collected"] = true;
       // data[collectibleID.ToString()] = (Godot.Collections.Dictionary)data2;
        GD.Print(CollectiblesData[collectibleID].Name + " Collected");
        CollectiblesData[collectibleID].Collected = true;
        menuAccess.UpdateMenu(CollectiblesData[collectibleID]);
        UpdateCollectedArray(collectibleID);
        //Save Json File
        //     var f = new File();
        //     f.Open(filePath, File.ModeFlags.Write);
        //    f.StoreString(JSON.Print(data, " ", true));
        //     f.Close();

        //  UpdateCollectedArray(collectibleID);
    }

    //Takes the information from the JSON and creates a collectible
    //that it sends to the collectible menu to update the pages
    public void UpdateCollectedArray(int ID)
    {
        menuAccess.UpdateMenu(CollectiblesData[ID]);
        //Get the collectible
        // var itemData = (Godot.Collections.Dictionary)data[ID.ToString()];
        // if ((bool)itemData["Collected"] == true)
        // {
        //     //Create new collectible item from the json
        //     Collectible collect = new Collectible();
        //     collect.id = ID;
        //     collect.name = (string)itemData["Name"];
        //     collect.description = (string)itemData["Description"];
        //     collect.level = (string)itemData["Scene"];
        //     collect.imagePath = (string)itemData["FilePath"];

        //     //update the array and send it to the menu
        //     collectibles[ID] = collect;
        //  menuAccess.UpdateMenu(ID, collectibles);

        //CollectiblesData[ID].ID = ID;
    }
}
