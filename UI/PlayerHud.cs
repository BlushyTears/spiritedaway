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
	[Export]
	public TextureRect _dark;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_lblBubbleHint.Text = "";
		_dark.Visible = false;
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


		// Death
		if (_player._hp <= 0)
		{
			_dark.Visible = true;
			float fade = _dark.Modulate.A;
			fade = Mathf.Lerp(fade, 1, (float)delta);
			fade = Mathf.Clamp(fade, 0, 1);
			_dark.Modulate = new Color(0, 0, 0, fade * 1.05f);
		}
		else
		{
			_dark.Visible = false;
			_dark.Modulate = new Color(0, 0, 0, 0);
		}
	}
}
