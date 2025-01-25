using Godot;
using System;
using System.IO;

public partial class Player : CharacterBody3D
{
	// Movement
	[ExportCategory("Movement")]
	[Export]
	public float _bubbleSpeed = 5.0f;
	[Export]
	public float _outSpeed = 3.0f;
	public float _overridenSpeed = 0;
	[Export]
	public float _bubbleJumpVelocity = 10f;
	[Export]
	public float _outJumpVelocity = 4.5f;
	public float _overridenJumpVelocity = 0;
	[Export]
	public float _jumpGravityModified = 1.5f;
	[Export]
	public float _fallGravityModified = 1.5f;
	[Export]
	public Node3D _avatarModel;
	[Export]
	public CollisionShape3D _capsuleCollider;

	// Rotation
	[ExportCategory("Rotation and Cam")]
	[Export]
	public float _mouseSensitivity = 0.5f;
	[Export]
	public float _rotationVLimit = 50f;
	[Export]
	public Node3D _cameraHolder;
	[Export]
	public Camera3D _camera;
	[Export]
	public RayCast3D _cameraRayCast;
	[Export]
	public float _cameraLerpSpeed = 0.2f;

	public float _cameraRotationV = 0;

	// Bubble
	[ExportCategory("Bubble")]
	[Export]
	public PlayerBubble _controlledBubble;

	public PlayerBubble _collidingBubble;
	[Export]
	public CollisionShape3D _bubbleCollider;

	// State
	[ExportCategory("State")]
	[Export]
	public int _hp = 1;
	public int _collectedCount = 10;

	// Outside of bubble settings
	[ExportCategory("Out of bubble")]
	[Export]
	public float _outBubbleTimeWarning = 5;
	[Export]
	public float _outBubbleTimeDead = 20;
	public float _outBubbleTime = 0;

	// Audio
	[ExportCategory("Audio")]
	[Export]
	public AudioStreamPlayer3D _audioPlayer;
	[Export]
	public AudioStream _soundEnterBubble;
	[Export]
	public AudioStream _soundLeaveBubble;
	[Export]
	public AudioStream _soundOutWarning;
	bool _outWarningPlayed = false;

	//--------------------------------------------------
	// Overrides
	//--------------------------------------------------

	public override void _Ready()
	{
		base._Ready();
		GameController.thePlayer = this;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		// Enter bubble
		if (Input.IsActionJustPressed("pl_enterBubble"))
		{
			
			if (_controlledBubble == null)
				ControlBubble(_collidingBubble);
			else
				LeaveBubble();
		}

		// Process time out of bubble
		if (_controlledBubble == null)
		{
			_outBubbleTime += (float)delta;
			_avatarModel.Position = _avatarModel.Position.Lerp(Vector3.Zero, (float)delta * 10);
			GameController.insideBubble = false;
		} else {
			GameController.insideBubble = true;  //memo: used by OmniLight3dLogic.cs
		}

		if (_outBubbleTime > _outBubbleTimeWarning && !_outWarningPlayed) // Play warning sound
		{
			_outWarningPlayed = true;
			_audioPlayer.Stream = _soundOutWarning;
			_audioPlayer.Play();
		}

		//if (_outBubbleTime >= _outBubbleTimeWarning && _outBubbleTime < _outBubbleTimeWarning)

		if (_outBubbleTime >= _outBubbleTimeDead)
			SubtractHP(_hp);
			
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			if (velocity.Y > 0)
				velocity += GetGravity() * (float)delta * _jumpGravityModified;
			else
				velocity += GetGravity() * (float)delta * _fallGravityModified;
		}

		// Handle Jump.
		float jumpVelocity = CurrentJumpVelocity();

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("pl_left", "pl_right", "pl_front", "pl_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		float curSpeed = CurrentSpeed();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * curSpeed;
			velocity.Z = direction.Z * curSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, curSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, curSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Camera
		HandleCamera();
	}

	public override void _Input(InputEvent @event)
	{
		// Handle mouse rotation
		InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;

		if (mouseEvent != null)
		{
			// Horizontal player rotation
			RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * _mouseSensitivity));

			float xRot = Mathf.Clamp(Mathf.DegToRad(-mouseEvent.Relative.Y * _mouseSensitivity), -_mouseSensitivity, _mouseSensitivity);
			//_cameraHolder.RotateX(xRot);

			// Vertical camera rotation
			float changeV = -mouseEvent.Relative.Y * _mouseSensitivity;
			float rotationVUpdated = Mathf.RadToDeg(_cameraHolder.Rotation.X) + changeV;

			// Check vertical limit
			if (rotationVUpdated > -_rotationVLimit && rotationVUpdated < _rotationVLimit)
			{
				_cameraRotationV = _cameraRotationV + changeV; 
				_cameraHolder.RotateX(Mathf.DegToRad(changeV));
			}
		}
	}

	//--------------------------------------------------
	// Methods
	//--------------------------------------------------

	// Speed handling
	private float CurrentSpeed()
	{
		// Speed overriden from outside
		if (_overridenSpeed > 0)
			return _overridenSpeed;

		// Outside of bubble
		if (_controlledBubble == null)
			return _outSpeed;

		// In bubble
		return _bubbleSpeed;
	}

	public void OverrideSpeed(float speed)
	{
		_overridenSpeed = speed;
	}

	public void ClearOverridenSpeed()
	{
		_overridenSpeed = 0;
	}

	// Jump handling
	public float CurrentJumpVelocity()
	{
		// Jump overriden from outside
		if (_overridenJumpVelocity > 0)
			return _overridenJumpVelocity;

		// Outside of bubble
		if (_controlledBubble == null)
			return _outJumpVelocity;

		// In bubble
		return _bubbleJumpVelocity;
	}

	public void OverrideJumpVelocity(float velocity)
	{
		_overridenJumpVelocity = velocity;
	}

	public void ClearOverridenJump()
	{
		_overridenJumpVelocity = 0;
	}

	public void SetCollidingBubble(PlayerBubble bubble)
	{
		_collidingBubble = bubble;
	}

	public void ControlBubble(PlayerBubble bubble)
	{
		if (bubble == null)
			return;

		// Setup bubble
		bubble.Reparent(this);
		bubble.Position = Vector3.Zero;
		bubble.SetControlled(true);
		float bubbleScale = bubble.SetGrowth(_collectedCount);

		// Setup colliders 
		_capsuleCollider.Disabled = true;
		_bubbleCollider.Disabled = false;
		_bubbleCollider.Scale = new Vector3(bubbleScale, bubbleScale, bubbleScale);
		_avatarModel.Position = Vector3.Down * bubbleScale * 0.25f;

		_controlledBubble = bubble;
		_outWarningPlayed = false;

		// Timer
		_outBubbleTime = 0;

		// Audio
		_audioPlayer.Stream = _soundEnterBubble;
		_audioPlayer.Play();
	}

	public void LeaveBubble()
	{
		// Setup bubble
		_controlledBubble.Reparent(GetTree().Root);
		_controlledBubble.SetControlled(false);
		//_controlledBubble.Position = Position;
		_controlledBubble = null;

		// Setup colliders 
		_capsuleCollider.Disabled = false;
		_bubbleCollider.Disabled = true;

		// Audio
		_audioPlayer.Stream = _soundLeaveBubble;
		_audioPlayer.Play();
	}

	public bool IsInBubble()
	{
		return _controlledBubble != null;
	}

	private void HandleCamera()
	{
		// Check obstacles
		if (!_cameraRayCast.IsColliding())
		{
			Vector3 target = _cameraHolder.GlobalTransform * _cameraRayCast.TargetPosition;
			_camera.GlobalPosition = _camera.GlobalPosition.Lerp(target, _cameraLerpSpeed);
			return;
		}

		// Move camera
		_camera.GlobalPosition = _camera.GlobalPosition.Lerp(_cameraRayCast.GetCollisionPoint() * 0.8f, _cameraLerpSpeed);
	}

	public void SubtractHP(int dmg)
	{
		_hp -= dmg;
		

		// Die
		if (_hp <= 0)
		{
			
			_outBubbleTime = 0;
			GameController.DeathStateViaAnything();
		}
	}

	public void CollectPoint()
	{
		_collectedCount++;
	}
}
