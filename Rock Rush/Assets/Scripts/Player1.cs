using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player1 : Character 
{
    public Vector3 respawnPos;
    public GameObject Player;
    // Use this for initialization
    public override void Start () 
	{
		base.Start();
        
        // grab the players position at startup and use it for the spawn position when starting new rounds
        spawnPos = _transform.position;
        respawnPos = new Vector3(0.0f, 7.0f, 0.0f);
    }
	
	public void Update () 
	{
		// in case the ball ever gets stuck or is lost
		if(Input.GetKeyDown(KeyCode.R))
		{
			ResetBall();
		}

		// reload the scene to reset scores etc
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
		{
            SceneManager.LoadScene(0);
		}

        UpdateMovement();
        
    }

    public void FixedUpdate()
	{
		// inputstate is none unless one of the movement keys are pressed
		currentInputState = inputState.None;
		
		// move left
		if(Input.GetKey(KeyCode.A)) 
		{ 
			currentInputState = inputState.WalkLeft;
			facingDir = facing.Left;
		}

		// move right
		if (Input.GetKey(KeyCode.D) && currentInputState != inputState.WalkLeft) 
		{ 
			currentInputState = inputState.WalkRight;
			facingDir = facing.Right;
		}

		// jump
		if (Input.GetKeyDown(KeyCode.W)) 
		{ 
			currentInputState = inputState.Jump;
		}

		// pass the ball
		if(Input.GetKeyDown(KeyCode.S))
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
            xa.player1.Respawn();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == xa.Water)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            GetComponent<Rigidbody2D>().mass = 1.0f;
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(3);
        Instantiate(Player, respawnPos, Quaternion.identity);
    }

    public void Respawn()
    {
        if (alive == true)
        {
            hasBall = false;
            Destroy(gameObject);
           

            // _transform.position = respawnPos;
        }
    }

}
