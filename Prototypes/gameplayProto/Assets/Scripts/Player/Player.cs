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

    public GameObject LastCheckpoint
    {
        get;
        set;
    }
	#endregion

    //Changes size depening on how long the player has been whistling
    private float whistlingTime = 0;
	
	// used to avoid bunnyhop
	[HideInInspector]
	public bool JumpKeyReleased;

    private Timer shushTimer = new Timer(); //So the shush is an effect, not an expanding 
    public float DelayBetweenShushes = 1F;

    public ParticleSystem FloatParticleSystem
    {
        get;
        set;
    }


    public int activePlayerInput
    {
        get;
        private set;
    }
	
	private List<Collider> currentFloorColliders;

    [HideInInspector]
    public OSCManager oscManager;

    private bool shoutingActivatedLastFrame;
    private BoxCollider shoutTrigger;
    private GameObject whistlingTrigger;
    private GameObject shushTrigger;
	
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
            if (childTransform.gameObject.CompareTag(GlobalNames.TAG.ShoutingTrigger))
            {
                shoutTrigger = childTransform.gameObject.GetComponent<BoxCollider>();
            }else if (childTransform.gameObject.CompareTag(GlobalNames.TAG.WhistlingTrigger))
            {
                whistlingTrigger = childTransform.gameObject;
            }
            else if (childTransform.gameObject.CompareTag(GlobalNames.TAG.ShushTrigger))
            {
                shushTrigger = childTransform.gameObject;
            }
        }
        if (shoutTrigger != null)
        {
            shoutTrigger.gameObject.active = false;
        }   
        else
        {
            Debug.Log("Shout trigger on player wasn't found");
        }

        if (whistlingTrigger != null)
        {
            whistlingTrigger.gameObject.active = true;
        }
        else
        {
            Debug.Log("Whistling trigger on player wasn't found");
        }

        if (shushTrigger != null)
        {
            shushTrigger.gameObject.active = false;
        }
        else
        {
            Debug.Log("Shush trigger on player wasn't found");
        }

        // float particle system
        Transform current;
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            current = transform.GetChild(i);
            if (current.gameObject.tag == GlobalNames.TAG.FloatParticleSystem)
            {
                FloatParticleSystem = current.gameObject.GetComponent<ParticleSystem>();
                //FloatParticleSystem.emissionRate = 0;
                break;
            }
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
        if (LastCheckpoint == null)
        {
            //Debug.Log("No checkpoint: This should only happen once, on startup!");
            // reset player position
            transform.position = startTransform.position;
        }
        else
        {
            Checkpoint check = LastCheckpoint.GetComponent<Checkpoint>();
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
        shushTimer.Update(Time.deltaTime);

		if (Input.GetKeyUp(settings.KeyJump))
		{
			JumpKeyReleased = true;
		}

        if (oscManager.Whistling)
        {
            
            whistlingTime += Time.deltaTime;
        }
        else
        {
            float timeRatio = settings.MaxExpandingTime / settings.DeflateTime;
            whistlingTime -= timeRatio * Time.deltaTime;
        }
        whistlingTime = Mathf.Clamp(whistlingTime,0, settings.MaxExpandingTime);
        
        if (whistlingTime > 0.3f)
        {
            whistlingTrigger.active = true;
        }
        float scale = Mathf.Lerp(0, settings.MaxWhistlingScale, whistlingTime / settings.MaxExpandingTime);
		
       	whistlingTrigger.gameObject.transform.localScale = new UnityEngine.Vector3(scale , scale, scale);

        if (shoutingActivatedLastFrame)
        {
            shoutingActivatedLastFrame = false;
            shoutTrigger.gameObject.active = false;
        }

        if ((/*oscManager.Shushing || */Input.GetKey(settings.DEBUG_KeyShushing)) )
        {
            shushTrigger.active = true;
            shushTimer.Start(DelayBetweenShushes);
        }
        else
            shushTrigger.active = false;

        // Update control of voice input
        // Note that player1 input has priority over player2!
        if (Input.GetKey(settings.KeyPlayer1Input))
        {
            activePlayerInput = 1;
        }
        else if (Input.GetKey(settings.KeyPlayer2Input))
        {
            activePlayerInput = 2;
        }
        else
        {
            activePlayerInput = 0;
        }

        // Player1 controls the shouting
        if ((activePlayerInput == 1 && oscManager.Shouting) || Input.GetKey(settings.DEBUG_KeyShout))
        {
            shoutTrigger.gameObject.active = true;
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
