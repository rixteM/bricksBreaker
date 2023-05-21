using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ReflectorBehaviour : MonoBehaviour
{
    public InputDevice leftHand;
    public InputDeviceCharacteristics controllerCharacteristics;


    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            leftHand = devices[0];
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        HapticCapabilities capabilities;
        if (leftHand.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.2f;
                float duration = 0.2f;
                leftHand.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
