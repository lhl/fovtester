using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Reset (doesn't matter w/ locked quad, but whevs
        if (Input.GetKeyDown("r"))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }


        if (Camera.current)
        {
            // Lock Position 
            this.transform.position = Camera.current.transform.position + (Camera.current.transform.forward * 10);

            // Lock Angle
            this.transform.forward = Camera.current.transform.forward;

            // Don't care about rotation
        }
    }
}


/* 

TODO:
x Main Scene
x 120 degree Quad

http://www.panohelp.com/lensfov.html
x = DistanceFromWall
y = WallDistance/2
α = FOV° / 2

α = tan-1(y/x)
x = y / tan(α)
y = tan(α) * x

FOV° = 2 * tan-1(y/x)
    
    
    * Support for:
  * CV1
  * DK2
  * Vive

* Controller Projection (Raycast Pointer)
* Painting
* Export Painted Image
* Analytics
    * Detect HMD
    * Unique ID for count?




    
    
    
Analytics - Detect Which HMD
https://analytics.cloud.unity3d.com/integration/b89d34b1-cb46-4cff-a475-88c8d9d4dd3d?unity_version=

// Reference the Unity Analytics namespace
using UnityEngine.Analytics;

// Use this call for wherever a player triggers a custom event
Analytics.CustomEvent(string customEventName,
IDictionary<string, object> eventData);

*/
