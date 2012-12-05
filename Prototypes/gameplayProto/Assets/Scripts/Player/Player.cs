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
	
	public GameObject ShushSpawn;

	#endregion

    //Changes size deepening on how long the player has been whistling
    private float whistlingTime = 0;
	
	// used to avoid bunnyhop
	[HideInInspector]
	public bool JumpKeyReleased;

    private Timer shushTimer = new Timer(); //So the shush is an effect, not an expanding 
    public float DelayBetweenShushes = 1F;
	bool justTriggeredShush = false;
	GameObject lastSpawnedShushObject;

	Renderer meshRenderer;

    public ParticleSystem FloatParticleSystem
    {
        get;
        set;
    }


    private float shoutEmissionRate; //Assigned on player start 
	public ParticleSystem ShoutParticleSystem
	{
		get;
		set;
	}

    private WhistlingSpawner whistlingSpawn;

	//public enum PlayerActive { NONE, PLAYER_ONE, PLAYER_TWO, BOTH }
	//public PlayerActive activePlayerInput
	//{
	//    get;
	//    private set;
	//}

	public bool PlayerOneActive
	{
		get;
		private set;
	}

	public bool PlayerTwoActive
	{
		get;
		private set;
	}

	public bool BothPlayersActive
	{
		get;
		private set;
	}

    [HideInInspector]
    public OSCManager oscManager;

    private bool isShouting;
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
		// find child triggers
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject.CompareTag(GlobalNames.TAG.ShoutingTrigger))
            {
                shoutTrigger = childTransform.gameObject.GetComponent<BoxCollider>();
            }
            else if (childTransform.gameObject.CompareTag(GlobalNames.TAG.WhistlingTrigger))
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
            Debug.LogWarning("Shout trigger on player wasn't found");
        }

		// whistling trigger
        if (whistlingTrigger != null)
        {
            whistlingTrigger.gameObject.active = true;
        }
        else
        {
            Debug.LogWarning("Whistling trigger on player wasn't found");
        }

		// shush trigger
        if (shushTrigger != null)
        {
            shushTrigger.gameObject.active = false;
        }
        else
        {
            Debug.LogWarning("Shush trigger on player wasn't found");
        }

        whistlingSpawn = transform.GetComponentInChildren<WhistlingSpawner>();
        if(whistlingSpawn == null)
            Debug.LogWarning("Whistling spawn wasn't found");

        // float particle system
        Transform current;
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            current = transform.GetChild(i);
            if (current.gameObject.tag == GlobalNames.TAG.FloatParticleSystem)
            {
                FloatParticleSystem = current.gameObject.GetComponent<ParticleSystem>();
                FloatParticleSystem.emissionRate = 0;
                continue;
            }
			else if (current.gameObject.tag == GlobalNames.TAG.ShoutParticleSystem)
			{
				ShoutParticleSystem = current.gameObject.GetComponent<ParticleSystem>();
                shoutEmissionRate = ShoutParticleSystem.emissionRate;
				continue;
			}
        }

		// get renderer
		meshRenderer = transform.GetComponentInChildren<MeshRenderer>();

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
		// start in fall state
		FSM.ChangeState(FallState);

		// used for bunnyhop avoidance
		JumpKeyReleased = true;
		
		// reset velocity
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

        // Reset player and objects according to check point
        if (LastCheckpoint == null)
        {
			Debug.Log ("Only happens at the start of each level");
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
		// reset angular velocity
		rigidbody.angularVelocity = Vector3.zero;
		
		FSM.Update();

		if (Input.GetKeyUp(settings.KeyJump))
		{
			JumpKeyReleased = true;
		}

		UpdateWhistling();

		UpdateShouting();

		UpdateShushing();

        // Update control of voice input
        // Note that if both players are pressing, noone gets voice control!
		if (Input.GetKey(settings.KeyPlayer1Input) &&
			Input.GetKey(settings.KeyPlayer2Input))
		{
			PlayerOneActive = PlayerTwoActive = false;
			BothPlayersActive = true;
		}
		else if (Input.GetKey(settings.KeyPlayer1Input))
		{
			PlayerOneActive = true;
			BothPlayersActive = false;
		}
		else if (Input.GetKey(settings.KeyPlayer2Input))
		{
			PlayerTwoActive = true;
			BothPlayersActive = false;
		}
		else
		{
			PlayerOneActive = PlayerTwoActive = false;
			BothPlayersActive = false;
		}
		
		if (PlayerOneActive)
        {
			meshRenderer.material.color = Color.blue;
        }
        else if (PlayerTwoActive)
        {
			meshRenderer.material.color = Color.red;
        }
        else
		{
			meshRenderer.material.color = Color.white;
        }
	}
	
	void UpdateShushing()
	{
		shushTimer.Update(Time.deltaTime);

		if (Input.GetKey(settings.DEBUG_KeyShushing))
		{
			if(!justTriggeredShush)	
			{
				Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject, 
					GlobalNames.SOUND.Player_Shushing, false);
				justTriggeredShush = true;
				shushTrigger.active = true;
				shushTimer.Start(DelayBetweenShushes);
				lastSpawnedShushObject = Instantiate(ShushSpawn, transform.position + new Vector3(0,1,0), Quaternion.identity) as GameObject;
			}
		}
		else
		{
			Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject,
					GlobalNames.SOUND.Player_Shushing, true);
			justTriggeredShush = false;
			shushTrigger.active = false;
			if(lastSpawnedShushObject != null)
			{lastSpawnedShushObject.BroadcastMessage("StopShush");}
		}
	}

	void UpdateWhistling()
	{
        if (oscManager.Whistling || Input.GetKey(settings.DEBUG_KeyWhistling))
        {
			if (!whistlingSpawn.Enabled)
			{
				Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject,
					GlobalNames.SOUND.Player_Whistling, false);
			}
            whistlingSpawn.EnableSpawning();
            whistlingTrigger.gameObject.transform.localScale = new UnityEngine.Vector3(settings.MaxWhistlingScale, settings.MaxWhistlingScale, settings.MaxWhistlingScale);
        }
        else
        {
			if (whistlingSpawn.Enabled)
				Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject,
					GlobalNames.SOUND.Player_Whistling, true);

			whistlingSpawn.DisableSpawning();
            whistlingTrigger.gameObject.transform.localScale = new UnityEngine.Vector3(0, 0, 0);
        }
	}

	void UpdateShouting()
	{
		// Player1 controls the shouting
		if ((PlayerOneActive && oscManager.Shouting) || Input.GetKey(settings.DEBUG_KeyShout))
		{
			if (shoutTrigger.gameObject.active == false)
			{
				Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject,
					GlobalNames.SOUND.Player_Shouting, false);
			}
			shoutTrigger.gameObject.active = true;
            ShoutParticleSystem.emissionRate = shoutEmissionRate;
		}
		else
		{
			if (shoutTrigger.gameObject.active == true)
			{
				Messenger<GameObject, string, bool>.Invoke(
					GlobalNames.EVENT.Player_Noise, gameObject,
					GlobalNames.SOUND.Player_Shouting, true);
			}
            ShoutParticleSystem.emissionRate = 0;
			shoutTrigger.gameObject.active = false;
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
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			float angle = Vector3.Angle(collision.contacts[i].normal, Vector3.up);
			
			if (angle > -settings.MaxFloorAngle && angle < settings.MaxFloorAngle)
			{
				Grounded = true;
			}
		}
	}

	void OnCollisionExit(Collision collision)
	{
		// if there's one or less angle according to the angle at which
		// the player's grounded -> the player is not grounded anymore
		int foundAngle = 0;
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			float angle = Vector3.Angle(collision.contacts[i].normal, Vector3.up);

			if (angle > -settings.MaxFloorAngle && angle < settings.MaxFloorAngle)
			{
				foundAngle++;
			}
		}

		if (foundAngle <= 1)
		{
			Grounded = false;
		}

		// unparent player when exiting collision with the moving platform
		if (collision.collider.tag == GlobalNames.TAG.MovingPlatformTag)
		{
			transform.parent = null;
		}
	}
}
