using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour 
{
	public MyTeam myTeam = MyTeam.Team1;
    public Transform GroundCheck;
    public LayerMask ground_layer;


    public float knockBack;
    public float knockBackLength;
    public float knockBackCount;
    public bool knockFromRight;
    public bool knockFromLeft;

	public enum inputState 
	{ 
		None, 
		WalkLeft, 
		WalkRight, 
		Jump, 
		Pass
	}
	[HideInInspector] public inputState currentInputState;
	
	[HideInInspector] public enum facing { Right, Left }
	[HideInInspector] public facing facingDir;

	[HideInInspector] public bool alive = true;
	[HideInInspector] public Vector3 spawnPos;
    public GameObject[] jumpBalls;
	
	protected Transform _transform;
	protected Rigidbody2D _rigidbody;

	// edit these to tune character movement	
	private float runVel = 2.5f; 	// run speed when not carrying the ball
	private float walkVel = 2f; 	// walk speed while carrying ball
	private float jumpVel = 6f; 	// jump velocity
	private float jump2Vel = 4f; 	// double jump velocity
	private float fallVel = 1f;		// fall velocity, gravity
	private float passVel = 3f;		// horizontal velocity of ball when passed

	private float moveVel;
	private float pVel = 0f;
	
	private int jumps;
    private int maxJumps = 6; 		// max number of jumps
		
	protected bool hasBall = false;
	protected string team = "";


	private Vector2 physVel = new Vector2();
	[HideInInspector] public bool grounded = false;
	private int groundMask = 1 << 8; // Ground layer mask

	public virtual void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Use this for initialization
	public virtual void Start () 
	{
		moveVel = walkVel;
        jumps = 0;
        
    }
	
	// Update is called once per frame
	public virtual void UpdateMovement() 
	{
		if(xa.gameOver == true || alive == false) return;

		// if the other team took the ball from me, then remove it from my inventory
		if(myTeam == MyTeam.Team1 && xa.teamWithBall == xa.TeamWithBall.Team2)
		{
			RemoveBall();
		}

		if(myTeam == MyTeam.Team2 && xa.teamWithBall == xa.TeamWithBall.Team1)
		{
			RemoveBall();
		}

		// if I have then ball, then tell it to follow me
		if(hasBall == true)
		{
			xa.ball.UpdateBallFollowPos(_transform);
		}

        
    }

    public void hasJumped()
    {

        for (int i = 0; i < jumps; i++)
        {
            jumpBalls[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // ============================== FIXEDUPDATE ============================== 

    public virtual void UpdatePhysics()
	{
		if(xa.gameOver == true || alive == false) return;

		physVel = Vector2.zero;

		// move left
		if(currentInputState == inputState.WalkLeft)
		{
			physVel.x = -moveVel;
		}

		// move right
		if(currentInputState == inputState.WalkRight)
		{
			physVel.x = moveVel;
		}

		// jump
		if(currentInputState == inputState.Jump)
		{
            Debug.Log(jumps);
            if (jumps < maxJumps)
			{  
                jumps += 1;
                hasJumped();
                if (jumps == 1)
				{
					_rigidbody.velocity = new Vector2(physVel.x, jumpVel);
				}
				else if(jumps > 1)
				{
					_rigidbody.velocity = new Vector2(physVel.x, jump2Vel);
				}
                
            }
		}

		// pass the ball
		if(currentInputState == inputState.Pass && hasBall == true)// && _transform.childCount > 1)
		{
			if(facingDir == facing.Left)
				pVel = -passVel;
			else
				pVel = passVel;

			xa.ball.PassBall(pVel);
			RemoveBall();
		}



        if (Physics2D.OverlapCircle(GroundCheck.position, 0.1f, ground_layer))
        {
            //Debug.Log("Grounded");
			grounded = true;
			jumps = 0;
            // turn ball renderers on 
            for (int i = 0; i < jumpBalls.Length; i++)
            {
                jumpBalls[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
		else
		{
			grounded = false;
			_rigidbody.AddForce(-Vector3.up * fallVel);
		}

		// actually move the player
        if (knockBackCount <= 0)
        {
            _rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y);
        }
        else
        {
            if (knockFromRight)
            {
                _rigidbody.velocity = new Vector2(-knockBack, knockBack);
            }
            if (!knockFromRight)
            {
                _rigidbody.velocity = new Vector2(knockBack, knockBack);
            }
            knockBackCount -= Time.deltaTime;
        }
		
	}

	// ============================== BALL HANDLING ==============================
	
	public virtual void PickUpBall()
	{
		hasBall = true;
		moveVel = walkVel;
		
		if(myTeam == MyTeam.Team1)
		{
			team = "Team1";
			xa.teamWithBall = xa.TeamWithBall.Team1;
		}
		else if(myTeam == MyTeam.Team2)
		{
			team = "Team2";
			xa.teamWithBall = xa.TeamWithBall.Team2;
		}
		
		xa.ball.PickUp(_transform, team);
		
		if(xa.scoreManager.gameType == GameType.Keepaway)
		{
			StartCoroutine(IncreaseScore());
		}
	}
	
	void RemoveBall()
	{
		hasBall = false;
		moveVel = runVel;
	}
	
	// if the ball gets stuck, player1 can reset it by pressing a key
	public virtual void ResetBall()
	{
		StartCoroutine(xa.ball.SpawnBall());
	}
	
	// ============================== KEEPAWAY SCORE ==============================
	
	IEnumerator IncreaseScore()
	{
		if(myTeam == MyTeam.Team1)
		{
			team = "Team1";
		}
		else
		{
			team = "Team2";
		}
		
		while(xa.gameOver == false)
		{
			xa.scoreManager.IncreaseScore(team);
			yield return null;
		}
	}
}

public enum MyTeam 
{
	Team1,
	Team2,
	None
}
