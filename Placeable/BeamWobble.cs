using Godot;
using System;

public partial class BeamWobble : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new Vector3((float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f));
		
	}
}
