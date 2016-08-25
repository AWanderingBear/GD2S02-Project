using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public ParticleSystem particleVFX;
	
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Collider2D _collider;
	
	private string team;

    private Color white = new Color(0.5f, 1.0f, 92.0f);
	private Color orange = new Color(0.91f,0.57f,0f);
	private Color blue = new Color(0.03f,0.68f,92f);
	private Color green = new Color(0.76f,1f,0f);

	private float offsetY = 0.5f;
	
	void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
	}
	
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(0.1f);
        particleVFX.startColor = white;
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void PickUp(Transform trans, string tm)
	{
		team = tm;

		_transform.position = new Vector3(trans.position.x, trans.position.y + offsetY, trans.position.z);

		// play pickup sound
		xa.audioManager.PlayPickup();

		// change particle colors
		if(tm == "Team2")
		{
            particleVFX.startColor = blue;
		}
        if (tm == "Team1") 
		{
            particleVFX.startColor = orange;
		}
	}

	// update position so that it's above the player's head when being carried
	public void UpdateBallFollowPos(Transform trans)
	{
		_transform.position = new Vector3(trans.position.x, trans.position.y +offsetY, trans.position.z);
	}

	public IEnumerator SpawnBall()
	{
		// move to spawn position
		_transform.position = new Vector3(0,3.5f,0);
		_rigidbody.isKinematic = true;

		// allow the ball physics to calm down before turning physics on again
		yield return new WaitForSeconds(0.1f);
		_rigidbody.isKinematic = false;
	}

	public void PassBall(float velX)
	{
		// send the ball flying away from the player who threw it
		_rigidbody.velocity = new Vector3(velX, 5, 0);

		// play pass sound
		xa.audioManager.PlayPass();
	}


	void IncreaseScore()
	{
		xa.scoreManager.IncreaseScore(team);
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.layer == xa.Team1Goal || other.gameObject.layer == xa.Team2Goal) && xa.gameOver == false)
        {

            if (other.gameObject.layer == xa.Team1Goal)
            {
                team = "Team1";
                IncreaseScore();
                StartCoroutine(xa.ball.SpawnBall());
            }

            if (other.gameObject.layer == xa.Team2Goal)

            {
                team = "Team2";
                IncreaseScore();
                StartCoroutine(xa.ball.SpawnBall());
            }
            else
            {
                StartCoroutine(xa.ball.SpawnBall());
            }
        }
    }


}
