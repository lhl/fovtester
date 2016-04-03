using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
    private bool locked;
    // Use this for initialization
    void Start () {
        locked = true;
	}
	
	// Update is called once per frame
	void Update () {
        // Reset (doesn't matter w/ locked quad, but whevs
        if (Input.GetKeyDown("r"))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }

        if (Input.GetKeyDown("l"))
        {
            locked = !locked;
        }

        if (locked && Camera.current)
        {
            // Lock Position 
            this.transform.position = Camera.current.transform.position + (Camera.current.transform.forward * 100);

            // Lock Angle
            this.transform.forward = Camera.current.transform.forward;

            // Lock Rotation
            this.transform.rotation = Camera.current.transform.rotation;
        }

        /* 
        We  probably should disable tracking but this is good enough...

        http://forum.unity3d.com/threads/way-to-disable-position-tracking.350322/
        http://forum.unity3d.com/threads/how-to-disable-head-orientation-sensor-on-oculus.362933/
        https://forums.oculus.com/community/discussion/1792/a-simple-way-to-disable-tracking
        https://forums.oculus.com/community/discussion/19863/need-to-disable-movement-completely
        
        */
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
