using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public AudioSource ballBounce;
    private Rigidbody rigidBody;
    private GameObject walls;
    private float speed;
   
    private void Start()
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        walls = GameObject.FindGameObjectWithTag("Walls");
        speed = gameController.ballSpeed;
        rigidBody = GetComponent<Rigidbody>();
        ballBounce.spatialize = true;
    }

    //Fix brick velocity to prevent weird physics
    void FixedUpdate()
    {       
        rigidBody.velocity = rigidBody.velocity.normalized * speed;        
    }
/*
    public static float Clamp(float value, float min, float max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }


    void Update()
    {
        
        foreach (Transform wall in walls.transform)
        {
            float distance = Clamp(Vector2.Distance(new Vector2(transform.position.x,transform.position.y), new Vector2(wall.GetComponent<MeshCollider>().transform.position.x, wall.GetComponent<MeshCollider>().transform.position.x)),0f,1f);
            Debug.Log(wall.gameObject.name + " " + wall.position.x + " " + wall.position.y);
            MeshRenderer renderer = wall.gameObject.GetComponent<MeshRenderer>();
            Debug.Log(renderer.material.name);
            Color newColor = renderer.material.color;
            newColor.a = distance;
            renderer.material.color = newColor;        
        }
        
    }*/


    private void OnCollisionEnter(Collision collision)
    {
        if (!ballBounce.isPlaying)
        {
            ballBounce.Play();
        }
    }

    public void AddInitialForce()
    {
        rigidBody.AddForce(transform.forward * 5.0f, ForceMode.VelocityChange);
    }
}