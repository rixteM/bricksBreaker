using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pumkinBrickBehaviour : MonoBehaviour
{
    private GameController gameController;
    public ParticleSystem particule;
    public ParticleSystem particule2;
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
        //explosion = GetComponent<ParticleSystem>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        refreshColor();

        //Vector3 direction = (new Vector3(0, 0, 0) - gameObject.transform.position);
        //direction.y = 0;
        //gameObject.transform.localRotation = Quaternion.LookRotation(direction);   


    }

    //Destroy brick on collision
    private void OnCollisionExit(Collision collision)
    {
        hitsToBreak--;
        if (hitsToBreak == 0)
        {
            //explosion.enableEmission = true;
            explosion.Play();
            destroySound.Play();
            var em = explosion.emission;
            em.enabled = true;

            var em2 = particule.emission;
            em2.enabled = false;

            var em3 = particule2.emission;
            em3.enabled = false;

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
        ParticleSystem.MainModule settings1 = particule.main;
        ParticleSystem.MainModule settings2 = particule2.main;
        switch (hitsToBreak)
        {
            case 1:

                settings1.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.0f, 0.0f));
                settings2.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.0f, 0.0f));
                break;

            case 2:

                settings1.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.0f, 0.7f));
                settings2.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.0f, 0.7f));
                break;

            case 3:

                settings1.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.8376f, 0.0f));
                settings2.startColor = new ParticleSystem.MinMaxGradient(new Color(1.0f, 0.8376f, 0.0f));
                break;

            case 4:

                settings1.startColor = new ParticleSystem.MinMaxGradient(new Color(0.0f, 0.0f, 1.0f));
                settings2.startColor = new ParticleSystem.MinMaxGradient(new Color(0.0f, 0.0f, 1.0f));
                break;

        }
    }
}
