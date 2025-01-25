using Godot;
using System;

public partial class PlayerCollideDetector : Node3D
{
	[Export]
	public String type = "spirit";
	
	//PackedScene spiritCollectedAnimation = GD.Load<PackedScene>("res://scenes/PlaySlot/spirit_appearance_collected.tscn");
	
	private Node3D thePlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		thePlayer = GetTree().CurrentScene.GetNode<Node3D>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float dist = GlobalTransform.Origin.DistanceTo(thePlayer.GlobalTransform.Origin);
		if (dist<1) {
			if (type=="spirit") {
				
				
				//spiritCollectedAnimation.Instantiate();
				//spiritCollectedAnimation.GlobalTransform.Origin = 
				GameController._collectedCount++;
				GetParent().QueueFree();
				
			}
		}
	}
}
