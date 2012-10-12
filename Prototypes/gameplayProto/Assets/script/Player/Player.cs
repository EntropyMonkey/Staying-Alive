using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public PlayerSettings settings;

	#region Properties

	public FiniteStateMachine<Player> FSM
	{
		get;
		private set;
	}

	public FSMState<Player> StandState
	{
		get;
		private set;
	}

	public FSMState<Player> WalkState
	{
		get;
		private set;
	}

	public FSMState<Player> JumpState
	{
		get;
		private set;
	}

	public FSMState<Player> FallState
	{
		get;
		private set;
	}

	public FSMState<Player> FloatState
	{
		get;
		private set;
	}

	public bool Grounded
	{
		get;
		private set;
	}
	#endregion

	private List<Collider> currentFloorColliders;

	void Awake()
	{
	}

	// Use this for initialization
	void Start ()
	{
		StandState = ScriptableObject.CreateInstance<PStandState>();
		WalkState = ScriptableObject.CreateInstance<PWalkState>();

		JumpState = ScriptableObject.CreateInstance<PJumpState>();
		FallState = ScriptableObject.CreateInstance<PFallState>();
		FloatState = ScriptableObject.CreateInstance<PFloatState>();

		// create the finite state machine
		FSM = new FiniteStateMachine<Player>();
		// configure it so that the player first falls and does not move r/l
		FSM.Configure(this, FallState, null);

		currentFloorColliders = new List<Collider>();
	}

	// used for game logic stuff
	void Update ()
	{
		FSM.Update();
	}

	// do physics stuff here!
	void FixedUpdate()
	{
		FSM.FixedUpdate();
	}

	// do animation overriding here
	void LateUpdate()
	{
		FSM.LateUpdate();
	}

	void OnCollisionEnter(Collision collision)
	{
	}

	void OnCollisionStay(Collision collision)
	{
		// test all contacts for the right collision angle
		bool foundAngle = false;
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			float angle = Vector3.Angle(collision.contacts[i].normal, Vector3.up);
			Debug.Log(angle);

			if (angle > -settings.MaxFloorAngle && angle < settings.MaxFloorAngle)
			{
				currentFloorColliders.Add(collision.contacts[i].otherCollider);
				foundAngle = true;
			}
		}

		if (foundAngle)
		{
			Grounded = true;
		}
		else
		{
			Grounded = false;
			if (FSM.CurrentState == JumpState)
				FSM.ChangeState(FallState);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (currentFloorColliders.Remove(collision.collider))
		{
			Grounded = false;
		}
	}
}
