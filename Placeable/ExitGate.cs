using Godot;
using System;

public partial class ExitGate : MeshInstance3D
{

	public override void _Ready()
	{
		// note: class name should be CpuParticles3D and not CPUParticles3D...
		GetNode<CpuParticles3D>("CPUParticles3Dcopy").Emitting = false;
	}

	public override void _Process(double delta)
	{
		GetNode<CpuParticles3D>("CPUParticles3Dcopy").Emitting = GameController._portalOpen;
	}
}
