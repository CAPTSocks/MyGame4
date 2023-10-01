using Godot;
using System.Collections.Generic;
using System;

public partial class CollectibleMenu : Control
{

    //private CollectiblesPage[] collectiblePages;
    private Collectible[] collectibles = new Collectible[7];
    private CollectibleResource collectibleData;
    private CollectiblesPage[] collectiblePages = new CollectiblesPage[7];
   // private CollectiblesPage currentPage, leftPage, RightPage;
    //private CollectiblesGlobol colGlobolAccess;
    private int currentPageIndex = 0;

    
    public override void _Ready()
    {
        //Get Access to all the pages and add them to the array
        var page0 = GetNode<CollectiblesPage>("CollectiblesMenu/Control/CollectibleItemPage0");
        var page1 = GetNode<CollectiblesPage>("CollectiblesMenu/Control/CollectibleItemPage1");
        var page2 = GetNode<CollectiblesPage>("CollectiblesMenu/Control/CollectibleItemPage2");
        var page3 = GetNode<CollectiblesPage>("CollectiblesMenu/Control/CollectibleItemPage3");

        collectiblePages[0] = page0;
        collectiblePages[1] = page1;
        collectiblePages[2] = page2;
        collectiblePages[3] = page3;
    }

    //Get the new collectible array and tell the page to update
    public void UpdateMenu(CollectibleResource newData)
    {
        collectibleData = newData;
        UpdatePages();
    }

    //Updates the page with the new collectible info
    public void UpdatePages()
    {        
        GD.Print("Update Pages!!");
        var id = collectibleData.ID;

        if (collectiblePages[id] == null)
            return;
        collectiblePages[id].nameLabel.Text = collectibleData.Name;
        collectiblePages[id].descriptionLabel.Text = collectibleData.Description;
        collectiblePages[id].image.Texture = collectibleData.Image;   
    }


    public void OnLeftArrowPressed()
    {
        //If its on the first page dont do anything
        if (currentPageIndex == 0)
        {
            return;
        }
        
        //Move pages to the left
        collectiblePages[currentPageIndex].MoveOffScreenRight();
        collectiblePages[currentPageIndex - 1].MoveOnScreenRight();
        currentPageIndex --;
    }

    public void OnRightArrowPressed()
    {
        //If its on the last page dont do anything
        if (currentPageIndex == 3)
        {
            return;
        }
        //Move the pages to the right
        collectiblePages[currentPageIndex].MoveOffScreenLeft();
        collectiblePages[currentPageIndex + 1].MoveOnScreenLeft();
        currentPageIndex ++;
    }
}
