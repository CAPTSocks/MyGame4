using Godot;
using System;

//Add to the can chop group when added to scene

public partial class CanChop : Node2D
{
    // [Export]private int chopTimes = 1;
    [Export] private int lastFrame;
    private int currentFrame = 0;
    private bool canBeChopped = true;
    private Tween fadeoutTween;
    private Sprite2D sprite;
    
    private CollisionShape2D col;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        fadeoutTween = GetNode<Tween>("FadeoutTween");
        col = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void Chopped()
    {
        if (canBeChopped)
        {
            currentFrame++;
            sprite.Frame = currentFrame;

            if (currentFrame == lastFrame)
            {
                canBeChopped = false;
                ChoppedDown();
                CallDeferred(nameof(DisableCollision));    
            }
        }
    }

    private void DisableCollision()
    {
        col.Disabled = true;
    }

    //TODO: Fix fadeout with new tween or animation
    private void ChoppedDown()
    {
        //fadeoutTween.InterpolateProperty(sprite, "modulate", new Color(1,1,1,1), new Color(1,1,1,0),
        //                                     .5f, Tween.TransitionType.Linear, Tween.EaseType.OutIn);
        //fadeoutTween.Start();
        //this.QueueFree();
    }

 //   private void OnFadeoutTweenCompleted(Godot.Object obj, NodePath key)
  //  {
        //this.QueueFree();
   // }



    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
