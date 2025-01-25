using Godot;
using System;

public partial class Control : Godot.Control
{
	// A flag to track the visibility state
	private bool isVisible = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ensure the UI starts visible
		Visible = isVisible;
	}

	// Called to handle input events
	public override void _Input(InputEvent @event)
	{
		// Check if the 'P' key is pressed
		if (@event is InputEventKey eventKey && eventKey.Keycode == Key.P && eventKey.Pressed)
		{
			// Toggle visibility
			isVisible = !isVisible;
			Visible = isVisible;
		}
	}
}
