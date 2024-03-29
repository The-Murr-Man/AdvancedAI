﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flock : MonoBehaviour
{
    public static Flock instance;
    public Text hitCountText;
    public FlockAgent agentPrefab;
    public FlockBehaviour behaviour;
    List<FlockAgent> agents = new List<FlockAgent>();

    int hitCount;
    [Range(10, 1000)]
    public int startingCount = 250;
    const float agentDensisty = 0.08f;

    [Range(1f, 100f)]
    public float agentSpeed = 10;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;

    [Range(0f, 1f)]
    public float avoidanceMulti = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

	public void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
	}
	// Start is called before the first frame update
	void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceMulti * avoidanceMulti;

        for (int i = 0; i < startingCount; ++i)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingCount * agentDensisty, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hitCountText.text = "Hit Count: " + hitCount;
        foreach(FlockAgent agent in agents)
		{
            List<Transform> context = GetNearbyObjects(agent);

			Vector2 move = behaviour.CalulateMove(agent, context, this);
			move *= agentSpeed;
			if (move.sqrMagnitude > squareMaxSpeed)
			{
				move = move.normalized * maxSpeed;
			}
			agent.Move(move);
		}
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
	{
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach (Collider2D c in contextColliders)
        { 
            if(c != agent.AgentCollider)
			{
                context.Add(c.transform);
			}
        }

        return context;
	}

    public void increaseHitCount()
	{
        hitCount++;
	}
}
