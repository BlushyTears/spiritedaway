using Godot;
using System;

public partial class InformativeText : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetText("");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameController._victory) {
			SetText("The world grows is brighter! You win!");
		} else if (GameController._portalOpen) {
			SetText("Find the exit...");
		} else {
			SetText("");
		}
		
	}
}
