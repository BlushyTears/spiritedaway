using Godot;
using System;

public partial class EndMenu : CanvasLayer
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameController._victory)
			Visible = true;
	}
}
