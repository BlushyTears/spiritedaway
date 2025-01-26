using Godot;
using System;

public partial class ExitGate : MeshInstance3D
{

	public override void _Ready()
	{
		Visible = false;  //hide quad inside portal that emits particles
		// note: class name below (CpuParticles3D) should be CpuParticles3D and not CPUParticles3D... Not upper-case CPU.
		GetNode<CpuParticles3D>("CPUParticles3Dcopy1").Emitting = false;
		GetNode<CpuParticles3D>("CPUParticles3Dcopy2").Emitting = false;
		GetParent().GetNode<OmniLight3D>("OmniLight3D").Visible = false;
		GetParent().GetNode<Node3D>("Portal/Portal/Gate").Visible = false;
		GetParent().GetNode<Node3D>("Portal/Portal/Beam").Visible = false;
		//GetParent().GetNode<SpotLight3D>("SpotLight3D1").Visible = false;
		//GetParent().GetNode<SpotLight3D>("SpotLight3D2").Visible = false;
	}

	public override void _Process(double delta)
	{
		if (GameController._portalOpen) {
			Visible = true;
			GetNode<CpuParticles3D>("CPUParticles3Dcopy1").Emitting = true;
			GetNode<CpuParticles3D>("CPUParticles3Dcopy2").Emitting = true;
			GetParent().GetNode<OmniLight3D>("OmniLight3D").Visible = true;
			GetParent().GetNode<Node3D>("Portal/Portal/Gate").Visible = true;
			GetParent().GetNode<Node3D>("Portal/Portal/Beam").Visible = true;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D1").ProcessMode = Node.ProcessModeEnum.Inherit;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D2").ProcessMode = Node.ProcessModeEnum.Inherit;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D1").Visible = true;
			//GetParent().GetNode<SpotLight3D>("SpotLight3D2").Visible = true;
		}
	}
}
