using Godot;
using System;

public partial class CombatStarter : Node
{
	public Node2D cameraTarget;
	public Area2D DetectionArea;
	public Camera2D camAccess;
	public RemoteTransform2D remote;
	public NodePath cameraPath, playersCamPath, emptyPath;
	public StaticBody2D leftWall, rightWall, topWall, bottomWall;

	private EncounterRunner encounterRunner;
	private Timer startupTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cameraTarget = GetNode<Node2D>("CameraTarget");
		DetectionArea = GetNode<Area2D>("DetectionArea");
		remote = GetNode<RemoteTransform2D>("CameraGrabber");
		startupTimer = GetNode<Timer>("StartupTimer");
		var cam = GetNode<Camera2D>("/root/" + GetTree().CurrentScene.Name + "/Camera2D");

		//Get access to the encounter runner
		encounterRunner = GetNode<EncounterRunner>("EncounterRunner");

		//Get access to all the walls
		leftWall = GetNode<StaticBody2D>("LeftWall");
		rightWall = GetNode<StaticBody2D>("RightWall");
		topWall = GetNode<StaticBody2D>("TopWall");
		bottomWall = GetNode<StaticBody2D>("BottomWall");

		//Get the screen size for the walls
		var screenSize = (Vector2)DisplayServer.ScreenGetSize();
		screenSize *= cam.Zoom;
		float xPos = screenSize.X / 6;
		float yPos = screenSize.Y / 6;

		//Place the walls on the edge of the screen
		leftWall.Position = new Vector2(-xPos, 0);
		rightWall.Position = new Vector2(xPos, 0);
		topWall.Position = new Vector2(0, -yPos);
		bottomWall.Position = new Vector2(0, yPos);
	}

	//If the player enters the area then start the encounter
	public void StartEncounter()
	{
		leftWall.GetNode<CollisionShape2D>("LeftWallCol").SetDeferred("disabled", false);
		rightWall.GetNode<CollisionShape2D>("RightWallCol").SetDeferred("disabled", false);
		topWall.GetNode<CollisionShape2D>("TopWallCol").SetDeferred("disabled", false);
		bottomWall.GetNode<CollisionShape2D>("BottomWallCol").SetDeferred("disabled", false);
		startupTimer.Start();
	}

	public void EndEncounter()
	{
		GD.Print("endEncounter called");
		NodePath r = new NodePath();
		var playerAccess = GetTree().CurrentScene.GetNode<Node>("Player");
		var playersRemote = playerAccess.GetNode<RemoteTransform2D>("CameraRemote");
		remote.RemotePath = r; 
		playersRemote.RemotePath = playersCamPath;
		this.QueueFree();

		leftWall.GetNode<CollisionShape2D>("LeftWallCol").SetDeferred("disabled", true);
		rightWall.GetNode<CollisionShape2D>("RightWallCol").SetDeferred("disabled", true);
		topWall.GetNode<CollisionShape2D>("TopWallCol").SetDeferred("disabled", true);
		bottomWall.GetNode<CollisionShape2D>("BottomWallCol").SetDeferred("disabled", true);
	}

	//Check if player enters area
	public void PlayerDetected(Node body)
	{
		if (body.Name == "Player")
		{
			StartEncounter();
			//Get the players remote, set it to null, and set this remote to the camera
			var playerRemote = body.GetNode<RemoteTransform2D>("CameraRemote");
			NodePath r = new NodePath();
			playersCamPath = playerRemote.RemotePath;
			remote.RemotePath = GetTree().CurrentScene.GetNode("Camera2D").GetPath();
			playerRemote.RemotePath = r;

			var coll = DetectionArea.GetChild<CollisionShape2D>(0);
			coll.SetDeferred("disabled", true);
		}
	}

	public void StarupTimerTimeout()
	{
		encounterRunner.StartEncounter();
		startupTimer.Stop();
	}
}
