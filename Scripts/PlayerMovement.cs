using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
    [Export] public int speed = 200, dodgeCost = 25, currentAttackDamage = 10, defaultAttackDamage = 10, comboModifier = 5;
    [Export]
    private float dodgeSpeed = 2, dodgeLength = .5f, attackTime = .5f, attackCoolDown = .2f, attackCoolDownLong = .5f,
                            attackCoolDownDefault = .1f;
    private int comboPresses = 0;
    private AnimatedSprite2D playerSprite;
    private Node2D meleeOrigin;
   // private Vector2 velocity = new Vector2();
    private Vector2 dodgeVelocity = new Vector2();
    private string animDir = "Down";
    private bool dodging = false, attacking = false, comboPressed = false, attackOnCooldown = false, hasWalked = false;
    public bool canMove = true;
    private AnimationPlayer anim;

    private CollisionShape2D meleeCollison;
    private Timer dodgeTimer, attackTimer, attackCoolDownTimer;
    private Health healthAccess;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("Anim");
        healthAccess = GetNode<Health>("Health");
        attackTimer = GetNode<Timer>("Timers/AttackTimer");
        dodgeTimer = GetNode<Timer>("Timers/DodgeTimer");
        attackCoolDownTimer = GetNode<Timer>("Timers/AttackCooldown");
        meleeOrigin = GetNode<Node2D>("MeleeOrigin");
        meleeCollison = meleeOrigin.GetNode<CollisionShape2D>("Sprite2D/Area2D/MeleeCollision");
        meleeOrigin.Visible = false;
        meleeCollison.Disabled = true;
    }

    //When dodge button is pressed dodge
    private void Dodge()
    {

        canMove = false;
        healthAccess.useEnergy(-dodgeCost);
        Velocity *= dodgeSpeed;
        dodgeVelocity = Velocity;
        dodgeTimer.Start(dodgeLength);
        anim.Stop();
        dodging = true;
    }

    //When dodge timer ends
    void _on_DodgeTimer_timeout()
    {
        GD.Print("Timer done");
        canMove = true;
        dodging = false;
    }

    //When the attack timer ends
    void _on_AttackTimer_timeout()
    {

        attacking = false;
        meleeOrigin.Visible = false;
        meleeCollison.Disabled = true;
        attackOnCooldown = true;
        canMove = true;
        comboPressed = false;
        comboPresses++;
        GD.Print(currentAttackDamage);
        if (comboPresses > 2)
        {
            attackCoolDown = attackCoolDownLong;
        }
        attackCoolDownTimer.Start(attackCoolDown);
    }

    void _on_AttackCooldown_timeout()
    {
        if (comboPressed == false)
        {
            currentAttackDamage = defaultAttackDamage;
            comboPresses = 0;
            attackOnCooldown = false;
            attackCoolDown = attackCoolDownDefault;
        }
    }

    //When Player's sword collider is entered
    void _on_Area_entered(Node2D body)
    {
        if (attacking && body.IsInGroup("Enemies"))
        {
            BaseEnemy enemyAccess = (BaseEnemy)body;
            enemyAccess.TakeDamage(100);
        }
        else if (attacking && body.IsInGroup("CanChop"))
        {
            CanChop chopAccess = (CanChop)body;
            chopAccess.Chopped();

        }
    }

    //When the player has zero health
    void _on_Health_PlayerDead()
    {
        canMove = false;
        RotationDegrees = 90;
    }

    //Make player attack when the attack button is pressed
    void DoAttack()
    {
        attacking = true;
        meleeOrigin.Visible = true;
        attackTimer.Start(attackTime);
        canMove = false;
        meleeCollison.Disabled = false;
        HandleAttackAnimation();
    }

    void ComboAttack()
    {
        if (Input.IsActionJustPressed("Attack") && attacking == false)
        {
            if (attackOnCooldown == false)
            {
                DoAttack();
            }
            else if (comboPresses <= 2 && comboPressed == false)
            {
                currentAttackDamage += comboModifier;
                comboPressed = true;
                DoAttack();
            }
        }
    }

    //Move the player when the movement buttons are pressed
    public void HandleMovementInput()
    {
       // velocity.X = Convert.ToInt32(Input.IsActionPressed("Right")) - Convert.ToInt32(Input.IsActionPressed("Left"));
       // velocity.Y = Convert.ToInt32(Input.IsActionPressed("Down")) - Convert.ToInt32(Input.IsActionPressed("Up"));
       Vector2 inputDir = Input.GetVector("Left", "Right", "Up", "Down");
        Velocity = inputDir * speed;
        if (healthAccess.CurrentEnergy > dodgeCost)
        {
            if (Input.IsActionJustPressed("Dodge") && Velocity != Vector2.Zero)
            {
                Dodge();
                return;
            }
        }

        if (Velocity != Vector2.Zero)
        {
            HandleAnimation("Walk");
            hasWalked = true;
        }
        else
        {
            if (hasWalked == true)
            {
                HandleAnimation("Idle");
            }
        }

        MoveAndSlide();
    }

    //Change player animations based on where they are moving
    public void HandleAnimation(string animation)
    {
        if (Velocity.X <= -1)
        {
            animDir = "Left";
            // meleeOrigin.Position = new Vector2(-25, 0);
            // meleeOrigin.RotationDegrees = 90;
        }

        else if (Velocity.X >= 1)
        {
            animDir = "Right";
            // meleeOrigin.Position = new Vector2(25, 0);
            // meleeOrigin.RotationDegrees = -90;
        }

        else if (Velocity.Y >= 1)
        {
            animDir = "Down";
            // meleeOrigin.Position = new Vector2(0, 40);
            // meleeOrigin.RotationDegrees = 0;
        }

        else if (Velocity.Y <= -1)
        {
            animDir = "Up";
            // meleeOrigin.Position = new Vector2(0, -40);
            // meleeOrigin.RotationDegrees = 180;
        }

        var newAnim = animation + animDir;
        if (anim.CurrentAnimation != newAnim)
        {
            anim.Play(newAnim);
        }
        hasWalked = false;
        //GD.Print(newAnim);
    }

    //Play attack animation when the player attacks
    void HandleAttackAnimation()
    {
        var newAnim = "Attack" + animDir;
        anim.Play(newAnim);

    }

    public override void _PhysicsProcess(double delta)
    {
        if (canMove)
        {
            HandleMovementInput();
            ComboAttack();
            //GD.Print(Velocity);
        }

        if (dodging)
        {
            Velocity = dodgeVelocity;
            MoveAndSlide();
        }

        if (Input.IsActionJustPressed("Interact"))
        {
            var items = GetNode<Node2D>("Items");
            GD.Print(items.GetChildCount());
        }
    }
}
