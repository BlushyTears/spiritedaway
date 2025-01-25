using Godot;
using System;

public partial class ExitGate : MeshInstance3D
{

	public override void _Ready()
	{
		GetNode<Node3D>("CPUParticles3D").SetProcess(false);
	}

	public override void _Process(double delta)
	{
		GetNode<Node3D>("CPUParticles3D").SetProcess(GameController._portalOpen);
	}
}
