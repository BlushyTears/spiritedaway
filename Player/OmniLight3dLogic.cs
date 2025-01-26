using Godot;
using System;

public partial class OmniLight3dLogic : OmniLight3D
{
	
	[Export]
	public MeshInstance3D thePhere = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameController.insideBubble) {
			if (OmniRange<7.5f) {
				OmniRange += (float)delta*15;
			} else if (OmniRange<15) {
				OmniRange += (float)delta*10;
			} else if (OmniRange<30) {
				OmniRange += (float)delta*5;
				if (OmniRange>30) {
					OmniRange = 30;
				}
			}
		} else if (OmniRange>24) {
			OmniRange -= (float)delta*2.5f;
		} else if (OmniRange>20) {
			OmniRange -= (float)delta*2.2f;
		} else if (OmniRange>16) {
			OmniRange -= (float)delta*1.9f;
		} else if (OmniRange>12) {
			OmniRange -= (float)delta*1.6f;
		} else if (OmniRange>8) {
			OmniRange -= (float)delta*1.3f;
		} else if (OmniRange>6) {
			OmniRange -= (float)delta*1.0f;
		} else if (OmniRange>5) {
			OmniRange -= (float)delta*0.7f;
		} else if (OmniRange>4.5) {
			OmniRange -= (float)delta*0.5f;
		} else if (OmniRange>4.25) {
			OmniRange -= (float)delta*0.3f;
		} else if (OmniRange>4) {
			OmniRange -= (float)delta*0.2f;
			if (OmniRange<4) {
				OmniRange = 4;
			}
		}
		if (thePhere!=null) {
			//hm, how to access the surface material override?
			//GD.Print(thePhere.MaterialOverride);
			//GD.Print(thePhere.GetActiveMaterial(0).Get("shader_paramater/base_color"));
			//GD.Print(thePhere.MaterialOverride.Get("shader_paramater/base_color"));
			//GD.Print(thePhere.GetActiveMaterial(0).get("shader_paramater/rgb"));
			//GD.Print(thePhere.SurfaceMaterialOverride[0]);
		}
	}
}
