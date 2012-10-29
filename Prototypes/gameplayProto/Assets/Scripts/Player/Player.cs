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

	public float Points
	{
		get;
		set;
	}

    public GameObject lastCheckpoint
    {
        get;
        set;
    }
	#endregion
	
	// used to avoid bunnyhop
	[HideInInspector]
	public bool JumpKeyReleased;
	
	private List<Collider> currentFloorColliders;

    [HideInInspector]
    public OSCManager oscManager;

    private bool shoutingActivatedLastFrame;
    private BoxCollider ShoutTrigger;
	
	private Transform startTransform;    

	void Awake()
	{
        oscManager = GetComponent<OSCManager>();
	}

    // Use this for initialization
    void Start()
	{
        foreach (Transform childTransform in transform)
        {
            BoxCollider collider = childTransform.gameObject.GetComponent<BoxCollider>();
            if (collider != null && collider.isTrigger)
            {
                ShoutTrigger = collider;
            }
        }
        if (ShoutTrigger != null)
        {
            ShoutTrigger.gameObject.active = false;
        }
        else
        {
            Debug.Log("Shout trigger on player wasn't found");
        }

		// initialize states
		StandState = ScriptableObject.CreateInstance<PStandState>();
		WalkState = ScriptableObject.CreateInstance<PWalkState>();

		JumpState = ScriptableObject.CreateInstance<PJumpState>();
		FallState = ScriptableObject.CreateInstance<PFallState>();
		FloatState = ScriptableObject.CreateInstance<PFloatState>();

		// create the finite state machine
		FSM = new FiniteStateMachine<Player>();
		// configure it so that the player first falls and does not move r/l
		FSM.Configure(this, FallState, null);
		
		// counts the current floor colliders, needed for double collisions
		currentFloorColliders = new List<Collider>();
		
		GameObject temp = GameObject.FindGameObjectWithTag(GlobalNames.TAG.StartPoint);
		if (temp != null)
		{
			startTransform = temp.transform;
		}
		else
		{
			Debug.LogWarning("The scene does not have an object tagged with \"StartPoint\"");
		}

		// set tag
		tag = GlobalNames.TAG.Player;

		Reset ();
	}
	
	public void Reset()
	{
		// used for bunnyhop avoidance
		JumpKeyReleased = true;
		
		// reset velocity
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

        // Reset player and objects according to check point
        if (lastCheckpoint == null)
        {
            Debug.Log("No checkpoint: This should only happen once, on startup!");
            // reset player position
            transform.position = startTransform.position;
        }
        else
        {
            Checkpoint check = lastCheckpoint.GetComponent<Checkpoint>();
            if (check != null)
            {
                check.loadCheckpoint(transform);
            }
            else
            {
                // reset player position
                transform.position = startTransform.position;
            }
        }
	}
	
	// used for game logic stuff
	void Update ()
	{
		FSM.Update();

		if (Input.GetKeyUp(settings.KeyJump))
		{
			JumpKeyReleased = true;
		}

        if (shoutingActivatedLastFrame)
        {
            shoutingActivatedLastFrame = false;
            ShoutTrigger.gameObject.active = false;
        }

        if (oscManager.Shouting || Input.GetKey(settings.DEBUG_KeyShout))
        {
            ShoutTrigger.gameObject.active = true;
            shoutingActivatedLastFrame = true;
        }
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
		if (collision.collider.tag == GlobalNames.TAG.MovingPlatformTag)
		{
			transform.parent = collision.collider.transform;
		}
	}

	void OnCollisionStay(Collision collision)
	{
		// test all contacts for the right collision angle
		bool foundAngle = false;
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			float angle = Vector3.Angle(collision.contacts[i].normal, Vector3.up);
			
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

		if (collision.collider.tag == GlobalNames.TAG.MovingPlatformTag)
		{
			transform.parent = null;
		}
	}
}
