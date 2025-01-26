using Godot;
using System;

public partial class PlayerCollideDetector : Node3D
{
	[Export]
	public String type = "spirit";
	
	private PackedScene spiritCollectedAnimation = GD.Load<PackedScene>("res://Enemy/spirit_appearance_collected.tscn");
	
	//private Node3D thePlayer;
	
	public override void _Ready() {
		//thePlayer = GetTree().CurrentScene.GetNode<Node3D>("Player");
	}

	public override void _Process(double delta)
	{
		float dist = GlobalTransform.Origin.DistanceTo(GameController.thePlayer.GlobalTransform.Origin);
		if (dist<1.75f && type=="spirit") {
			Node3D ani = spiritCollectedAnimation.Instantiate<Node3D>();
			ani.GlobalPosition = GlobalTransform.Origin;
			GetParent().GetParent().AddChild(ani);
			GameController._collectedCount++;
			GetParent().QueueFree();
		} else if (dist<2 && type=="exitgate") {
			if (GameController._portalOpen && !GameController._victory) {
				GameController.VictoryStateViaExitPortal();
			}
		} else if (dist<4 && type=="checkpoint") {
			GameController.AppendCheckpoint(GlobalPosition);
		}
	}
}
