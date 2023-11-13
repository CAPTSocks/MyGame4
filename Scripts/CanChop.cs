using Godot;
using System;

//Add to the can chop group when added to scene

public partial class CanChop : Node2D
{
    // [Export]private int chopTimes = 1;
    [Export] private int lastFrame;
    private int currentFrame = 0;
    private bool canBeChopped = true;
    private AnimationPlayer anim;
    private Sprite2D sprite;
    
    private CollisionShape2D col;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        col = GetNode<CollisionShape2D>("CollisionShape2D");
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
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

    private void ChoppedDown()
    {
        var animations = anim.GetAnimationList();
        if (animations.Length > 0)
        {
            anim.Play(animations[0]);
        }
    }
}
