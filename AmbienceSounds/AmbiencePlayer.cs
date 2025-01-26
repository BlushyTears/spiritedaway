using Godot;
using System;
using System.IO;

public partial class AmbiencePlayer : AudioStreamPlayer2D
{
	[Export]
	public AudioStream _soundInBubble;
	[Export]
	public AudioStream _soundOutBubble;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayInBubble();
	}

	
	// In bubble 
	public void PlayInBubble()
	{
		Stop();
		Stream = _soundInBubble;
		Play();
	}

	// Out of bubble
	public void PlayOutBubble()
	{
		Stop();
		Stream = _soundOutBubble;
		Play();
	}
}
