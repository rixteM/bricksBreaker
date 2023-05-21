using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBrickBehaviour : MonoBehaviour
{

    private GameController gameController;
    public ParticleSystem explosion;
    public AudioSource destroySound;
    public AudioSource ballBonusSound;
    public AudioSource brickHitSound;
    public int hitsToBreak = 1;
    private MeshRenderer meshRenderer;
    private Transform cube1, cube2, cube3, cube4;
    private Collider boxCollider;
    


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
        boxCollider = GetComponent<Collider>();
        boxCollider.isTrigger = false;
        meshRenderer = GetComponent<MeshRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //Initialization of the appearance of the bricks 
        switch (hitsToBreak)
        {
            case 1:
                cube1 = transform.Find("lava_cubes_1");

                cube1.gameObject.SetActive(true);

                break;
            case 2:
                cube1 = transform.Find("lava_cubes_1");
                cube2 = transform.Find("lava_cubes_2");

                cube1.gameObject.SetActive(false);
                cube2.gameObject.SetActive(true);
                break;
            case 3:
                cube1 = transform.Find("lava_cubes_1");
                cube2 = transform.Find("lava_cubes_2");
                cube3 = transform.Find("lava_cubes_4");

                cube1.gameObject.SetActive(false);
                cube2.gameObject.SetActive(false);
                cube3.gameObject.SetActive(true);

                break;
            case 4:
                cube1 = transform.Find("lava_cubes_1");
                cube2 = transform.Find("lava_cubes_2");
                cube3 = transform.Find("lava_cubes_4");
                cube4 = transform.Find("lava_cubes_5");

                cube1.gameObject.SetActive(false);
                cube2.gameObject.SetActive(false);
                cube3.gameObject.SetActive(false);
                cube4.gameObject.SetActive(true);
                break;


        }
    }
    //Destroy brick on collision
    private void OnCollisionExit(Collision collision)
    {
        hitsToBreak--;

        switch (hitsToBreak)
        {
            case 0:
                cube1.gameObject.SetActive(false);
                explosion.enableEmission = true;
                explosion.Play();
                destroySound.Play();
                var em = explosion.emission;
                em.enabled = true;
                boxCollider.isTrigger = true;                

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
                

                break;
            case 1:
                cube1.gameObject.SetActive(true);
                cube2.gameObject.SetActive(false);
                break;
            case 2:
                cube2.gameObject.SetActive(true);
                cube3.gameObject.SetActive(false);
                break;
            case 3:
                cube3.gameObject.SetActive(true);
                cube4.gameObject.SetActive(false);
                break;
        }

        if (hitsToBreak != 0)
        {
            brickHitSound.Play();
        }

    }




}
