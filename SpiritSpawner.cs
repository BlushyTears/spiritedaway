using Godot;
using System;

public partial class SpiritSpawner : Node3D
{
	private int spiritsToSpawn = 100;
	private int maxAttempts = 11000;
	private float mapWidthX = 100;
	private float mapDepthZ = 100;
	private float acceptableMinimumY = 3;
	private float acceptableMaximumY = 15;
	private int spiritsSpawned = 0;
	
	private PackedScene theSpiritPrefab = GD.Load<PackedScene>("res://Enemy/very_scared_spirit.tscn");
	
	public override void _Ready(){
		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
		for (int i=0; i<maxAttempts; i++) {
			Vector3 randomPos = new Vector3((float)GD.RandRange(-mapWidthX/2f, mapWidthX/2f), 99999, (float)GD.RandRange(-mapDepthZ/2f, mapDepthZ/2f));
			randomPos.Y = 10;
			spiritSpawnAt(randomPos);
			
			// Note: No time to get raycasts to work correctly (or to check that it works properly, rather).
			/*
			PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(randomPos, Vector3.Down);
			Godot.Collections.Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(query);
			if (result.Count!=0) {
				Vector3 candidatePos = (Vector3)result["position"];
				candidatePos = randomPos;
				if (candidatePos.Y>=acceptableMinimumY && candidatePos.Y<=acceptableMaximumY) {
					spiritSpawnAt(candidatePos + Vector3.Up);
				}
			}
			*/
			if (spiritsSpawned>=spiritsToSpawn) {
				break;
			}
		}
	}
	
	private void spiritSpawnAt(Vector3 pos) {
		Node3D sss = theSpiritPrefab.Instantiate<Node3D>();
		sss.GlobalPosition = pos;
		AddChild(sss);
		spiritsSpawned++;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
