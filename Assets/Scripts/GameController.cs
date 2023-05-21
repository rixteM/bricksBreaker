using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameController : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public int score;
    public bool gameOver;
    public int ballsRemaining;
    public int ballsInGame;
    public int ballsAtStart;
    public int bricksRemaining;
    public float ballSpeed;
    public GameObject ballPrefab;
    public GameObject leftHandPresence;
    public GameObject rightHandPresence;    
    public GameObject reflectorRayInteractor;
    public AudioSource gameOverSound;
    public AudioSource gameOverMusic;
    public AudioSource victorySound;
    public AudioSource levelMusic;
    public AudioSource startRoundSound;
    public Button restartButton;
    public Button mainMenuButton;
    public Button resumeButton;
    public Text winText;
    public Text loseText;
    public Text startText;
    public Text pauseText;
    public ParticleSystem fireworks1;
    public ParticleSystem fireworks2;
    public ParticleSystem fireworks3;
    public bool isStarted { get; set; }

    private bool primaryButtonLastState = false;
    private bool secondaryButtonLastState = false;
    private bool musicStarted = false;
    private bool paused = false;
    private HandPresence leftHandPresenceComponent;
    private HandPresence rightHandPresenceComponent;
    private ReflectorRayInteraction reflectorRayInteractorComponent;    
    private BallBehaviour ballBehaviour;    
    private InputDevice inputDevice;   

    // Start is called before the first frame updaterd
    void Start()
    {
        leftHandPresenceComponent = leftHandPresence.GetComponent<HandPresence>();
        rightHandPresenceComponent = rightHandPresence.GetComponent<HandPresence>();
        reflectorRayInteractorComponent = reflectorRayInteractor.GetComponent<ReflectorRayInteraction>();
        
        
        isStarted = false;
        score = 0;
        ballsInGame = 1;
        gameOver = false;        
        ballsRemaining = ballsAtStart;
        bricksRemaining = GameObject.FindGameObjectsWithTag("Brick").Length;
        Debug.Log("Bricks at start : " + bricksRemaining);

        /*restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);*/

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        if (inputDevices.Count > 0)
        {
            inputDevice = inputDevices[0];
        }

        Instantiate(ballPrefab, new Vector3(0, 1, -3.5f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Start round when trigger is pressed
        inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue);
        if (triggerValue && !isStarted && !paused)
        {
            if (!musicStarted)
            {
                levelMusic.Play();
                musicStarted = true;
            }
            startRoundSound.Play();
            ballBehaviour = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallBehaviour>();
            Debug.Log("START ROUND");
            ballBehaviour.AddInitialForce();
            isStarted = true;
            startText.gameObject.SetActive(false);
        }

        // Activate racket mode when pressing primary button
        if (!paused)
        {
            bool primaryButton;
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton);
            if (primaryButton != primaryButtonLastState)
            {
                if (primaryButton == true)
                {
                    // Button was pressed this frame
                }
                else
                {
                    // Button was released this frame
                    leftHandPresenceComponent.racketMode = !leftHandPresenceComponent.racketMode;
                    rightHandPresenceComponent.racketMode = !rightHandPresenceComponent.racketMode;

                    if (leftHandPresenceComponent.racketMode)
                    {
                        reflectorRayInteractorComponent.shouldBeVisible = false;
                    }
                    else
                    {
                        //Instantiate(reflector, transform);
                        //reflectorRayInteractorComponent.reflector = reflector;
                        reflectorRayInteractorComponent.shouldBeVisible = true;
                    }
                }
                // Set last known state for button
                primaryButtonLastState = primaryButton;
            }
        }

        //Pause game when secondary button pushed
        bool secondaryButton;
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButton);
        if (secondaryButton != secondaryButtonLastState)
        {
            if (!secondaryButton)
            {
                // Button was released this frame
                if (!paused)
                {
                    PauseGame();
                    ShowPauseUI();
                }
                else
                {
                    ResumeGame();
                    HidePauseUI();
                }
            }
            // Set last known state for button
            secondaryButtonLastState = secondaryButton;
        }

    }

    public void ShowPauseUI()
    {
        if (!gameOver)
        {
            startText.gameObject.SetActive(false);
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            leftHandPresenceComponent.showController = true;
            rightHandPresenceComponent.showController = true;
            reflectorRayInteractorComponent.shouldBeVisible = false;
        }
    }

    public void HidePauseUI()
    {
        if (!gameOver)
        {
            
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
            mainMenuButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            leftHandPresenceComponent.showController = false;
            rightHandPresenceComponent.showController = false;
            if (!leftHandPresenceComponent.racketMode)
            {
                reflectorRayInteractorComponent.shouldBeVisible = true;
            }
            if (!isStarted)
            {
                startText.gameObject.SetActive(true);
            }
            else
            {
                startText.gameObject.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public void CheckGameOver()
    {
        if(ballsRemaining == 0)
        {
            GameOver(false);
        }        

        if (bricksRemaining == 0)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool win)
    {
        gameOver = true;
        startText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);        
        leftHandPresenceComponent.showController = true;
        rightHandPresenceComponent.showController = true;


        if (win)
        {
            //Gagné !
            Debug.Log("YOU WIN");                        
            winText.gameObject.SetActive(true);
            loseText.gameObject.SetActive(false);
            levelMusic.Stop();
            victorySound.Play();
            fireworks1.Play();
            fireworks2.Play();
            fireworks3.Play();
            var em = fireworks1.emission;
            em.enabled = true;
            var em2 = fireworks2.emission;
            em2.enabled = true;
            var em3 = fireworks3.emission;
            em3.enabled = true;
        }
        else
        {            
            //Perdu :(
            Debug.Log("YOU LOSE");            
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(true);
            levelMusic.Stop();
            gameOverSound.Play();
            gameOverMusic.Play();
        }      
    }
}
