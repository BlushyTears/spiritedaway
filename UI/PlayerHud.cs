using Godot;
using System;

public partial class PlayerHud : CanvasLayer
{
	[Export]
	public Label _lblCollected;
	[Export]
	public Label _lblBubbleHint;
	[Export]
	private Player _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_lblBubbleHint.Text = "";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Show collected count
		//_lblCollected.Text = 0 + "/" + 10;

		// Prevent displaying
		if (GameController._introMenuOpen)
		{
			_lblBubbleHint.Text = "";
			return;
		}

		// Show bubble hint
		if (_player.IsInBubble())
			_lblBubbleHint.Text = "E - leave";
		else if (!_player.IsInBubble() && _player._collidingBubble != null)
			_lblBubbleHint.Text = "E - enter";
		else if (!_player.IsInBubble() && _player._collidingBubble == null)
		{ 
			if (_player._outBubbleTime > _player._outBubbleTimeWarning)
				_lblBubbleHint.Text = "Return to bubble!";
			else
				_lblBubbleHint.Text = "";
		}
	}
}
