using UnityEngine;
using System.Collections;

public class Player2 : Character 
{
    public Vector3 respawnPos;
    // Use this for initialization
    public override void Start () 
	{
		base.Start();
		spawnPos = _transform.position;
        respawnPos = new Vector3(0.0f, 7.0f, 0.0f);
    }
	
	// Update is called once per frame
	public void Update () 
	{
		UpdateMovement();
	}

	public void FixedUpdate()
	{
		// these are false unless one of keys is pressed
		currentInputState = inputState.None;
		
		// keyboard input
		if(Input.GetKey(KeyCode.LeftArrow)) 
		{ 
			currentInputState = inputState.WalkLeft;
			facingDir = facing.Left;
		}
		if (Input.GetKey(KeyCode.RightArrow) && currentInputState != inputState.WalkLeft) 
		{ 
			currentInputState = inputState.WalkRight;
			facingDir = facing.Right;
		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow)) 
		{ 
			currentInputState = inputState.Jump;
		}
		
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			currentInputState = inputState.Pass;
		}
		
		UpdatePhysics();
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ball") && hasBall == false)
		{
			PickUpBall();
		}
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.layer == xa.Team1Goal || other.gameObject.layer == xa.Team2Goal) && xa.gameOver == false)
        {
            xa.player2.Respawn();
        }
    }

    public void Respawn()
	{
		if (alive == true)
		{
			_transform.position = respawnPos;
			hasBall = false;
		}
	}
}
