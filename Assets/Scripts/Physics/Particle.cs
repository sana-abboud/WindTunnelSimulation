using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Particle : AbstractObject
{
	public float radius;
	public float lifespan;
	public Vector3 location;
	public Color color;
	private Material particleMaterial;
	private List<Vector3> pathHistory = new List<Vector3>();



	public Particle(Vector3 location)
	{
		// this.obj = gameObject;
		this.acceleration = new Vector3(0, -0.05f, 0);
		this.velocity = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0.05f, UnityEngine.Random.Range(-0.05f, 0.05f));
		this.lifespan = 255;
		this.location = location;
		pathHistory.Add(location);

		// Load the material
		Shader lineShader = Shader.Find("Custom/LineShader");
		if (lineShader == null)
		{
			Debug.LogError("Shader not found! Make sure the shader file is named correctly and placed in the project.");
		}
		particleMaterial = new Material(lineShader);

		// // Create the GameObject and apply the material
		// obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// obj.transform.position = location;
		// Renderer renderer = obj.GetComponent<Renderer>();
		// if (renderer != null)
		// {
		// 	renderer.material = particleMaterial;
		// }
	}


	public void UpdateVelocity(Vector3 vel, float dt)
	{
		// acceleration += force / mass;
		this.velocity += vel;
	}

	public override bool DetectCollision(AbstractObject anotherObject)
	{
		if (anotherObject is Particle anotherParticle)
		{
			if (Math.Sqrt(Math.Pow(anotherObject.GetObj().transform.position.x - this.obj.transform.position.x, 2) +
				Math.Pow(anotherObject.GetObj().transform.position.y - this.obj.transform.position.y, 2) +
				Math.Pow(anotherObject.GetObj().transform.position.z - this.obj.transform.position.z, 2)) <= 10)
			{
				return true;
			}
		}
		return false;
	}

	public void ChangeColor(Color color)
	{
		this.color = color;
		if (particleMaterial != null)
		{
			particleMaterial.color = color;
		}
	}

	public void MoveRandomly()
	{
		Vector3 randomOffset = new Vector3(
			UnityEngine.Random.Range(-0.01f, 0.01f),
			UnityEngine.Random.Range(-0.01f, 0.01f),
			UnityEngine.Random.Range(-0.01f, 0.01f)
		);
		this.GetObj().transform.position += randomOffset;
	}



	public void Move()
	{
		float randomX = UnityEngine.Random.Range(-1f, 0f);
		float randomY = UnityEngine.Random.Range(-1f, 0);
		float randomZ = UnityEngine.Random.Range(-0.5f, 0.5f);

		Vector3 randomAcceleration = new Vector3(randomX, 0, randomZ);

		this.velocity += randomAcceleration * Time.deltaTime;
		this.location += this.velocity * Time.deltaTime;
		this.lifespan -= 0.01f;
		// this.color = new Color(0f, 1f, 1f, this.lifespan / 255f);
		// Update path history
		pathHistory.Add(this.location);
		if (pathHistory.Count > 50) // Limit the history size
		{
			pathHistory.RemoveAt(0);
		}

		// if (particleMaterial != null)
		// {
		// 	particleMaterial.color = this.color;
		// }
		// Update the GameObject position
		// obj.transform.position = this.location;
	}

	public List<Triangles> Getneighbors(MeshClass meshClass) //// mouayad
	{
		List<Triangles> triangles = new();
		if (meshClass != null)
		{
			foreach (Triangles triangle in meshClass.meshTriangles)
			{
				if (triangle.v1.x >= 0.8 && triangle.v2.x >= 0.8 && triangle.v3.x >= 0.8) ////// 
					triangles.Add(triangle);
			}
		}
		return triangles;

	}

	public void HandleCollisionPT(MeshClass meshClass)
	{
		foreach (Triangles triangle in Getneighbors(meshClass))
		{
			Debug.Log(triangle);
			if (triangle.PointTriangle(this.location, this.radius))
			{
				UpdateVelocity(new Vector3(-0.05f, 0.05f, 0), 0.01f);
				UpdateVelocity(new Vector3(0, -0.02f, 0), 0.01f);
				Debug.Log("collision");
				break;
				// this.velocity = new Vector3(0, 0, 0);
				// return true;
			}
		}
	}

	public bool IsDead()
	{
		if (this.lifespan <= 0)
			return true;
		else
			return false;
	}

	public void Draw()
	{
		// Update the material color before drawing
		// if (particleMaterial != null)
		// {
		// 	particleMaterial.color = this.color;
		// }
		// Gizmos.color = this.color;
		// Gizmos.DrawSphere(this.location, 0.05f);

		for (int i = 0; i < pathHistory.Count - 1; i++)
		{
			Gizmos.color = Color.Lerp(Color.blue, Color.red, (float)i / pathHistory.Count);
			Gizmos.DrawLine(pathHistory[i], pathHistory[i + 1]);
		}
	}
}
