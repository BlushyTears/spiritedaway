using Godot;
using System;

public partial class DestroyAfterDelay : Node3D
{
	
	[Export]
	public ulong delayMS = 12500;
	
	private ulong timestamp;
	
	public override void _Ready()
	{
		timestamp = Time.GetTicksMsec();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec()-timestamp>delayMS) {
			QueueFree();
		}
	}
}
