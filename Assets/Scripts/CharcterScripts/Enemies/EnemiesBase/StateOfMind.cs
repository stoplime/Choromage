using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
	
/// <summary>
///  State which describes how the monster is behaving and the manner of whatever they are doing affects StateOfBeing. Only one StateOfMind at a time but can have a StateOfBeing at the same time. 
/// Examples: Passive, Patrolling, Agressive, Defensive, Dead
/// </summary>
// public class StateOfMind{

// 	/// <summary>
// 	/// how far monster(/person?) can see in this state
// 	/// </summary>
// 	private float sightDistance;
// 	/// <summary>
// 	/// how quickly monster(/person?) can move in this state
// 	/// </summary>
// 	private float speed;

// 	//TODO: Add the defense to each state passive < patrol < defensive
// 	//private float defense;

// 	/// <summary>
// 	/// The distance the monster wants between it and the player. Returns -1 if it doesn't matter/care.
// 	/// TODO: Implement and add max and min distance.
// 	/// </summary>
// 	//private float bufferDistance;

// 	public float SightDistance
// 	{
// 		get{return sightDistance;}
// 	}

// 	public float Speed
// 	{
// 		get{return speed;}
// 	}

// 	// Use this for initialization
// 	public StateOfMind(float moveSpeed, float seeDistance)
// 	{
// 		sightDistance = seeDistance;
// 		speed = moveSpeed;
// 	}
// }
