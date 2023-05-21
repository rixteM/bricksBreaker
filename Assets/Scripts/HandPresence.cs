using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public bool racketMode = false;
    public bool leftHand;
    public bool rightHand;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    public GameObject racketPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private GameObject spawnedRacket;
    private Animator handAnimator;
    

    // Start is called before the first frame updaterdhr
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[8], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            spawnedRacket = Instantiate(racketPrefab);

            RacketBehaviour racketBehaviour = spawnedRacket.GetComponent<RacketBehaviour>();

            if (controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                racketBehaviour.left = true;
                racketBehaviour.right = false;
            }

            if (controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                racketBehaviour.right = true;
                racketBehaviour.left = false;
            }



            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
                spawnedRacket.SetActive(false);
            }
            else if (racketMode)
            {
                spawnedRacket.SetActive(true);
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(false);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                spawnedRacket.SetActive(false);
                UpdateHandAnimation();
            }
        }        
    }
}
