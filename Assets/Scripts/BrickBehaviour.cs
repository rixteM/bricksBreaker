using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    private GameController gameController;
    public Material matBricks1;
    public Material matBricks2;
    public Material matBricks3;
    public ParticleSystem explosion;
    public AudioSource destroySound;
    public AudioSource ballBonusSound;
    public AudioSource brickHitSound;
    public int hitsToBreak = 1;
    private MeshRenderer meshRenderer;


    public enum BrickType
    {
        Normal,
        NewBall,
        DoubleReflector
    }

    public BrickType type;
    // Start is called before the first frame updaterdhd
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        refreshColor();        
    }

    //Destroy brick on collision, or update material if not destroyed
    private void OnCollisionExit(Collision collision)
    {
        hitsToBreak--;
        if (hitsToBreak == 0)
        {
            explosion.Play();
            destroySound.Play();
            var em = explosion.emission;
            em.enabled = true;
            Destroy(gameObject);
            gameController.bricksRemaining--;
            Debug.Log("Bricks remaining : " + gameController.bricksRemaining);
            gameController.CheckGameOver();


            

            switch (type)
            {
                case BrickType.Normal:
                    break;
                case BrickType.NewBall:
                    ballBonusSound.Play();
                    GameObject newBall = Instantiate(gameController.ballPrefab, transform.position, Quaternion.identity);
                    gameController.ballsInGame++;
                    Rigidbody rigidBody = newBall.GetComponent<Rigidbody>();
                    rigidBody.AddForce(transform.forward * 2.0f, ForceMode.VelocityChange);
                    break;
            }
        }
        else
        {
            brickHitSound.Play();
        }
        refreshColor();
    }

    void refreshColor()
    {
        switch (hitsToBreak)
        {
            case 1:
                meshRenderer.material = matBricks1;
                break;
            case 2:
                meshRenderer.material = matBricks2;
                break;
            case 3:
                meshRenderer.material = matBricks3;
                break;
        }
    }



}
