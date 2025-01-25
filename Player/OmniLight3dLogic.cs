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
			if (OmniRange<15) {
				OmniRange += (float)delta*3;
				if (OmniRange>15) {
					OmniRange = 15;
				}
			}
		} else if (OmniRange>10) {
			OmniRange -= (float)delta;
		} else if (OmniRange>5) {
			OmniRange -= (float)delta/2f;
			if (OmniRange<5) {
				OmniRange = 5;
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
