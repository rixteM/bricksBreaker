using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackWallBehaviour : MonoBehaviour
{
    public GameController gameController;
    public AudioSource hitSound;
    public Text livesRemaining;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        livesRemaining.text = "" + gameController.ballsAtStart;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ball destroyed, spawn new ball and update score
        Debug.Log("BALL DESTROYED");
        Destroy(collision.gameObject);
        gameController.ballsInGame--;
        hitSound.Play();
        if (gameController.ballsInGame == 0)
        {            
            gameController.ballsRemaining--;
            livesRemaining.text = ""+gameController.ballsRemaining;
            if (!gameController.gameOver)
            {
                gameController.CheckGameOver();
                if (!gameController.gameOver)
                {
                    gameController.ballsInGame++;
                    Instantiate(gameController.ballPrefab, new Vector3(0, 1, -3.5f), Quaternion.identity);
                    gameController.isStarted = false;
                    gameController.startText.gameObject.SetActive(true);
                }
            }
        }
    }

}