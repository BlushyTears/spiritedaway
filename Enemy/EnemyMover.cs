using Godot;
using System;

public partial class EnemyMover : NavigationAgent3D {
	
	public static System.Collections.Generic.List<EnemyScarer> enemyScarers = new System.Collections.Generic.List<EnemyScarer>();
	
	[Export]
	public float distanceToFlee = 40f;
	[Export]
	public float fleeSpeed = 5;
	[Export]
	public float roamingSpeed = 1.0f;
	
	private bool firstMove = true;
	
	private Node3D enemyNodeToMove = null;
	private NavigationAgent3D navAgent = null;
	private double newTargetCooldown = 0;
	
	private bool roaming = false;
	private double roamingNewTargetCooldown = 0;

	public override void _Ready() {
		if (enemyNodeToMove==null) {
			enemyNodeToMove = GetParent<Node3D>();
		}
		navAgent = this;
	}

	public override void _Process(double delta) {
		newTargetCooldown -= delta;
		roamingNewTargetCooldown -= delta;
		if (newTargetCooldown<0) {
			
			var myCachedTransform = enemyNodeToMove.GlobalTransform;
			Vector3 myPos = myCachedTransform.Origin;
			
			Vector3 summedScaryPos = Vector3.Zero;
			int summedScaryPosCount = 0;
			
			for (var i=0; i<enemyScarers.Count; i++) {
				if (myPos.DistanceTo(enemyScarers[i].GlobalTransform.Origin)<enemyScarers[i].scaryRadius) {
					summedScaryPos = enemyScarers[i].GlobalTransform.Origin;
					summedScaryPosCount++;
				}
			}
			
			if (summedScaryPosCount>0) {
				
				roaming = false;
				
				Vector3 fleeFromPosFlatY = summedScaryPos/summedScaryPosCount;
				fleeFromPosFlatY.Y = 0;
				
				Vector3 myPosFlatY = myCachedTransform.Origin;
				myPosFlatY.Y = 0;
				Vector3 directionAway = (myPosFlatY-fleeFromPosFlatY).Normalized();
				if (directionAway==Vector3.Zero) {
					directionAway = Vector3.Forward;
				}
				Vector3 projectedPos = fleeFromPosFlatY + directionAway*distanceToFlee;
						
				Vector3 closestPointOnNavmesh =
					NavigationServer3D.MapGetClosestPointToSegment(enemyNodeToMove.GetWorld3D().NavigationMap, projectedPos, projectedPos);
			
				navAgent.SetTargetPosition(closestPointOnNavmesh);
				newTargetCooldown = 0.75;
				
			} else if (roamingNewTargetCooldown<0) {
				
				roaming = true;
				
				Vector3 randXYZ = enemyNodeToMove.GlobalPosition;
				randXYZ.X += GD.RandRange(-50, 50);
				randXYZ.Z += GD.RandRange(-50, 50);
				
				Vector3 closestPointOnNavmesh =
					NavigationServer3D.MapGetClosestPointToSegment(enemyNodeToMove.GetWorld3D().NavigationMap, randXYZ, randXYZ);
			
				navAgent.SetTargetPosition(closestPointOnNavmesh);
				newTargetCooldown = 0.75;
				roamingNewTargetCooldown = 5;
			}
		}
	}
	
	private Vector2 randomnessTarget = Vector2.Zero;
	private Vector2 randomness = Vector2.Zero;
	
	public override void _PhysicsProcess(double delta) {
		if (!navAgent.IsNavigationFinished()) {
			Vector3 nextPosOrig = navAgent.GetNextPathPosition();
			Vector3 nextPos = nextPosOrig;
			if (!roaming) {
				randomnessTarget.X = (float)GD.RandRange(-10.0*fleeSpeed, 10.0*fleeSpeed);
				randomnessTarget.Y = (float)GD.RandRange(-10.0*fleeSpeed, 10.0*fleeSpeed);
				randomness = randomness.Lerp(randomnessTarget, (float)delta*0.5f);
				nextPos.X += randomness.X;
				nextPos.Z += randomness.Y;
			}
			var myCachedTransform = enemyNodeToMove.GlobalTransform;
			if (nextPos.DistanceTo(myCachedTransform.Origin)<nextPosOrig.DistanceTo(myCachedTransform.Origin)) {
				nextPos = nextPosOrig;  //regret randomness
				randomness = Vector2.Zero;
			}
			Vector3 dirTowards = (nextPos - enemyNodeToMove.GlobalPosition).Normalized();
			dirTowards.Y = 0;
			enemyNodeToMove.LookAt(enemyNodeToMove.GlobalPosition + dirTowards, Vector3.Up);
			if (firstMove) {
				myCachedTransform.Origin = nextPosOrig;
				firstMove = false;
			} else {
				myCachedTransform.Origin = myCachedTransform.Origin.MoveToward(nextPos, (float)delta*(roaming?roamingSpeed:fleeSpeed));
			}
			enemyNodeToMove.GlobalTransform = myCachedTransform;
		}
	}
	
}
