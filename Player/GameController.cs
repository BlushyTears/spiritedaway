using Godot;
using System;

public partial class GameController : Node3D {  //TODO include this in every scene
	
	public static int _hp = 1;
	public static int _collectedCount = 0;
	
	public static void ResetGame() {  //TODO call this when restarting the game
		_hp = 1;
		_collectedCount = 0;
		
		//TODO add other things that should be reset here
	}

}
