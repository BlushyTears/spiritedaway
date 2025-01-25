using Godot;
using System;

public partial class PlayerBubble : Node3D
{
	[Export]
	public float _scaleGrowthStep = 0.25f;
	[Export]
	public float _groundingSpeed = 5;
	private float _originalScale;

	private bool _controlled = false;

	[Export]
	public RayCast3D _rayCast;

	//-----------------------------------------------
	// Override
	//-----------------------------------------------

	public override void _Ready()
	{
		base._Ready();
		_originalScale = Scale.X;
		SetControlled(false);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// Move to ground if levitating in the air
		if (!_controlled && _rayCast.GetCollider() == null)
			Position += Vector3.Down * _groundingSpeed * (float)delta;
	}

	//-----------------------------------------------
	// Signals
	//-----------------------------------------------

	private void OnBodyEntered(Node3D body)
	{
		// Is player - setup colliding bubble
		Player player = body as Player;
		if (player != null)
		{
			player.SetCollidingBubble(this);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		// Is player - remove colliding bubble
		Player player = body as Player;
		if (player != null)
		{
			player.SetCollidingBubble(null);
		}
	}

	//-----------------------------------------------
	// Methods
	//-----------------------------------------------

	public float SetGrowth(int steps)
	{
		Scale = new Vector3(_originalScale, _originalScale, _originalScale) * (1 + _scaleGrowthStep * steps);
		return Scale.X;
	}

	public void SetControlled(bool controlled)
	{
		_controlled = controlled;
	}
}
