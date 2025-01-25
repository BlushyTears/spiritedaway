using Godot;
using System;

public partial class SpiritsCollectedUpdater : Label
{
	[Export]
	public String type = "spiritcnt";
	
	private String bufferText;
	
	public override void _Ready() {
		if (type=="deathlabel") {
			bufferText = GetText();
			SetText("");
		}
	}

	public override void _Process(double delta) {
		if (type=="deathlabel") {
			if (GameController._timesDied>0) {
				SetText(bufferText);
			}
		} else if (type=="deathcnt") {
			if (GameController._timesDied>0) {
				SetText(""+ GameController._timesDied);
			} else {
				SetText("");
			}
		} else if (type=="spiritcnt") {
			SetText(""+ GameController._collectedCount);
		}
	}
	
}
