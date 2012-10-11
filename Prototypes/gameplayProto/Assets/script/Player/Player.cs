using UnityEngine;
using System.Collections;

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
		if (collision.collider.transform.position.y < transform.position.y)
		{
			Grounded = true;
		}
		else
		{
			Grounded = false;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.collider.transform.position.y < transform.position.y)
		{
			Grounded = false;
		}
	}
}
