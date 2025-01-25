using Godot;
using System;

public partial class GameController : Node3D {  //TODO include this in every (core) level scene
	
	private static GameController singleton = null;
	
	public override void _Ready() {
		if (singleton!=null) {
			GD.Print("Warning: Several GameControllers!");
		}
		singleton = this;
	}
	
	public static int _hp = 1;
	public static int _collectedCount = 0;
	public static bool _victory = false;
	public static bool _portalOpen = false;
	public static bool _timesDied = false;
	
	public static void ResetGame() {  //TODO call this when restarting the game
		_hp = 1;
		_collectedCount = 0;
		_victory = false;
		_portalOpen = false;
		//TODO add other things that should be reset here
	}
	
	public static void VictoryStateViaExitPortal() {  //called by ExitGate.cs
		_victory = true;
	}
	
	public override void _Process(double delta)
	{
		_portalOpen = _collectedCount>=5;
		if (singleton.GetTree().CurrentScene.GetNode<Node3D>("Player").GlobalPosition.Y<-2) {
			DeathStateViaAnything();  //fell off the edge of the map
		}
	}
	
	public static void DeathStateViaAnything() {  //hit a spike, or fell off the map, or suffocated on darkness
		Vector3 checkpoint = GetClosestActivatedCheckpoint();
		Node3D thePlayer = singleton.GetTree().CurrentScene.GetNode<Node3D>("Player");
		thePlayer.GlobalPosition = checkpoint;
		Node3D theBubble = singleton.GetTree().CurrentScene.GetNode<Node3D>("PlayerBubble");
		theBubble.GlobalPosition = checkpoint;
	}
	
	private static Vector3 GetClosestActivatedCheckpoint() {
		return Vector3.Zero + Vector3.Up*6;  //TODO
	}

}
