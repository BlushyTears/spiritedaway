using Godot;
using System;

public partial class ExitGate : MeshInstance3D
{

	public override void _Ready()
	{
		// note: class name should be CpuParticles3D and not CPUParticles3D...
		GetNode<CpuParticles3D>("CPUParticles3Dcopy1").Emitting = false;
		GetNode<CpuParticles3D>("CPUParticles3Dcopy2").Emitting = false;
		GetParent().GetNode<SpotLight3D>("SpotLight3D1").Visible = false;
		GetParent().GetNode<SpotLight3D>("SpotLight3D2").Visible = false;
	}

	public override void _Process(double delta)
	{
		if (GameController._portalOpen) {
			GetNode<CpuParticles3D>("CPUParticles3Dcopy1").Emitting = true;
			GetNode<CpuParticles3D>("CPUParticles3Dcopy2").Emitting = true;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D1").ProcessMode = Node.ProcessModeEnum.Inherit;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D2").ProcessMode = Node.ProcessModeEnum.Inherit;
			GetParent().GetNode<SpotLight3D>("SpotLight3D1").Visible = true;
			GetParent().GetNode<SpotLight3D>("SpotLight3D2").Visible = true;
		}
	}
}
