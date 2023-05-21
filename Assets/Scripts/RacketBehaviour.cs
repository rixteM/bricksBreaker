using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RacketBehaviour : MonoBehaviour
{
    public InputDevice controller;    
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject leftHandPresence;
    public GameObject rightHandPresence;
    public bool left;
    public bool right;

    private Rigidbody rigidBody;
    private Transform racketTransform;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        Debug.Log(left);
        Debug.Log(right);
        rigidBody = GetComponent<Rigidbody>();
        foreach (var device in devices)
        {
            if(left)
            {
                if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
                {
                    controller = device;
                }
            }

            if (right)
            {
                if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                {
                    controller = device;
                }
            }
                        
        } 

        Debug.Log(controller.name + controller.characteristics);
    }

    void FixedUpdate()
    {
        if (left)
        {
            racketTransform = GameObject.FindGameObjectWithTag("Left Hand Dummy").transform;
        }
        else
        {
            racketTransform = GameObject.FindGameObjectWithTag("Right Hand Dummy").transform;
        }
        rigidBody.MovePosition(racketTransform.position);
        rigidBody.MoveRotation(racketTransform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Vibrate controller on collision
        Debug.Log("Collision");
        HapticCapabilities capabilities;
        if (controller.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.2f;
                float duration = 0.2f;
                Debug.Log("send impulse");
                controller.SendHapticImpulse(channel, amplitude, duration);                
            }
        }
    }
}
