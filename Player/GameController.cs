using Godot;
using System;

public partial class GameController : Node3D {  //TODO include this in every (core) level scene
	
	private static GameController singleton = null;
	
	public override void _Ready() {
		if (singleton!=null) {
			GD.Print("Warning: Several GameControllers!");
		}
		singleton = this;
		Node3D player = thePlayer;
		if (player==null) {
			AppendCheckpoint(Vector3.Up*6);  //the the middle of the map if the player can't be found for some reason
		} else {
			AppendCheckpoint(player.GlobalPosition);  //use the initial spawn position of the payer
		}
	}
	
	public static int _hp = 1;
	public static int _collectedCount = 0;
	public static bool _victory = false;
	public static bool _portalOpen = false;
	public static int _timesDied = 0;
	public static Node3D thePlayer = null;  //set by Player.cs
	public static Node3D theBubble = null;  //set by PlayerBubble.cs
	public static bool insideBubble = false;  //set by Player.cs
	
	//deprecated
	//public static void ResetGame() {
		//_hp = 1;
		//_collectedCount = 0;
		//_victory = false;
		//_portalOpen = false;
		//_timesDied = 0;
	//}
	
	public static void VictoryStateViaExitPortal() {  //called by ExitGate.cs
		_victory = true;
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape")) {
			GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
			GetTree().Quit(0);
		}
		_portalOpen = _collectedCount>=10;
		if (thePlayer.GlobalPosition.Y<-5) {
			DeathStateViaAnything();  //fell off the edge of the map
		}
	}
	
	public static void DeathStateViaAnything() {  //hit a spike, or fell off the map, or suffocated on darkness
		Vector3 checkpoint = GetClosestActivatedCheckpoint(thePlayer.GlobalPosition);
		thePlayer.GlobalPosition = checkpoint;
		theBubble.GlobalPosition = checkpoint;
		_timesDied++;
	}
	
	private static Vector3 GetClosestActivatedCheckpoint(Vector3 currentPlayerPos) {
		Vector3 closestPos = checkpoints[0];  //one should have been added in _Ready()
		for (int i=1; i<checkpoints.Count; i++) {
			if (checkpoints[i].DistanceSquaredTo(currentPlayerPos)<closestPos.DistanceSquaredTo(currentPlayerPos)) {
				closestPos = checkpoints[i];
			}
		}
		return closestPos;
	}
	
	private static System.Collections.Generic.List<Vector3> checkpoints = new System.Collections.Generic.List<Vector3>();
	
	public static void AppendCheckpoint(Vector3 pos) {
		if (pos.Y<6) {
			pos.Y = 6;
		}
		for (int i=0; i<checkpoints.Count; i++) {
			if (checkpoints[i].DistanceTo(pos)<1) {
				return;  //abort! already added this checkpoint
			}
		}
		checkpoints.Add(pos);
	}

}
