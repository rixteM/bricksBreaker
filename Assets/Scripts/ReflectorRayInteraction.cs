using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReflectorRayInteraction : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public GameObject reflector;
    public GameObject innerWall;
    public bool shouldBeVisible;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        rayInteractor.GetCurrentRaycastHit(out hit);
        if(!hit.Equals(null))
        {
            reflector.transform.position = hit.point;
            reflector.transform.rotation = Quaternion.FromToRotation(Vector3.right, hit.normal);
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if (ball != null)
            {
                if (ball.transform.position.z > innerWall.transform.position.z)
                {
                    reflector.SetActive(false);
                }
                else
                {
                    if (shouldBeVisible)
                    {
                        reflector.SetActive(true);
                    }
                    else
                    {
                        reflector.SetActive(false);
                    }
                }
            }
        }
        else
        {
            Debug.Log("No hit");
            reflector.SetActive(false);
            reflector.transform.position = new Vector3(0, -10, 0);
        }
    }
}
