using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public Text team1ProgressTxt;
	public Text team2ProgressTxt;

	public Text team1Wins;
	public Text team2Wins;
	
	public Text roundWinnerTxt;
	public Text roundNextTxt;

	// game options
	public int nextRoundTime = 5;						// how much time should pass between rounds?
	public bool decreaseScoreWhenNotInGoal = false;		// should score slowly decrease when not holding the ball in a goal?
	public GameType gameType;							// capture the flag or keepaway. Keepaway will score points any time a player is holding the ball
	public Transform[] level;							// add multiple levels to this array, when a new round starts the next level will move into position

	private int team1Progress = 0;
	private int team2Progress = 0;
	
	private int team1Score = 0;
	private int team2Score = 0;
	
	private int nextRoundTimer = 0;
	
	private Color orange = new Color(0.91f,0.57f,0f);
	private Color blue = new Color(0.03f,0.68f,92f);

	protected float scoreTime = 0f;
	protected float decreaseRate = 0.5f;
	protected float nextDecrease = 0f;
	
	protected int levelNum = 0;

	// player is holding the ball in own goal so increment the progress text and score
	public void IncreaseScore(string team)
	{

        if (team == "Team1")
        {
            if (team1Progress < 5)
            {
                team1Progress += 1;
                team1ProgressTxt.text = team1Progress.ToString("D2") + "Point(s)"; // leading zeroes!
            }

            if (team1Progress >= 5)
            {
                Team1Wins();
            }

        }
			if (team == "Team2")
			{
				if (team2Progress < 5)
				{
					team2Progress += 1;
					team2ProgressTxt.text = team2Progress.ToString("D2") + "Point(s)"; // leading zeroes!
				}
				
				if (team2Progress >= 5)
				{
					Team2Wins();
				}
			}

			// play progress sound
			xa.audioManager.PlayProgress();
		
	}
	

	// RECORD TOTAL WINS FOR TEAM 1
	void Team1Wins()
	{
		team1Score += 1;
		team1Wins.text = team1Score.ToString();
		
		StartCoroutine(ShowResults("PLAYER 1 WINS!", orange));
		xa.audioManager.PlayWin();
	}

	// RECORD TOTAL WINS FOR TEAM 2
	void Team2Wins()
	{
		team2Score += 1;
		team2Wins.text = team2Score.ToString();
		
		StartCoroutine(ShowResults("PLAYER 2 WINS!", blue));
		xa.audioManager.PlayWin();
	}

	// show the results at the end of the round and start the next round
	IEnumerator ShowResults(string results, Color col)
	{
		xa.gameOver = true;

		// show the text
		roundWinnerTxt.enabled = true;
		roundWinnerTxt.text = results.ToString();
		roundWinnerTxt.color = col;

		yield return new WaitForSeconds(2f);
		
		roundNextTxt.enabled = true;
		
		nextRoundTimer = nextRoundTime;

		while(nextRoundTimer > 0)
		{
			// increment the timer text
			roundNextTxt.text = nextRoundTimer.ToString();
			nextRoundTimer -= 1;

			// play beep sound
			xa.audioManager.PlayBeep1();

			// wait a second
			yield return new WaitForSeconds(1f);
		}
	
		StartNextRound();
		roundWinnerTxt.enabled = false;
		
		roundNextTxt.text = "GO!";

		// play beep sound
		xa.audioManager.PlayBeep2();

		yield return new WaitForSeconds(1f);
		
		roundNextTxt.enabled = false;
	}

	// reset scores back to zero at the start of a new round
	void ResetProgress()
	{
		team1Progress = 0;
		team1ProgressTxt.text = team1Progress.ToString("D3") + "Point(s)"; // leading zeroes!
		
		team2Progress = 0;
		team2ProgressTxt.text = team2Progress.ToString("D3") + "Point(s)"; // leading zeroes!
	}
	
	void StartNextRound()
	{
		xa.player1.Respawn();
		xa.player2.Respawn();
		
		StartCoroutine(xa.ball.SpawnBall());
		xa.gameOver = false;

		ResetProgress();
		
		NextLevel();
 	}
	
	void NextLevel()
	{
		for(int i=0;i<level.Length;i+=1)
		{
			level[i].position = new Vector3(-22,0,0);
		}
		levelNum += 1;
		
		if(levelNum == level.Length)
		{
			levelNum = 0;
		}
		
		if(level.Length > 0)
		{
			print(level.Length+", "+levelNum);
			level[levelNum].position = new Vector3(0,0,0);
		}
	}
}

public enum GameType 
{
	CTF,
	Keepaway
}
