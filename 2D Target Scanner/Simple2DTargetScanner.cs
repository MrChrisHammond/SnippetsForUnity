using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DTargetScanner : MonoBehaviour 
{


    public float scanFrequency; //in seconds
    public float scanRadius = 5; //in world units
    private float scanFrequencyCounter = 0; //counter for time between scans
  
  	//delegate event to notify subscribed scripts
    public delegate void TargetHit(RaycastHit2D[] raycastHit2D);
    public event TargetHit OnTargetHit;
  
    public Color editorCircleColor = Color.green; //circle colour for editor
    public bool scannerEnabled = true; //enable or disable scanner (or you can just enable/disable script if you prefer)
  
	// Update is called once per frame
	void FixedUpdate ()
    {
	    //if not enabled, return to stop code below from running.
        if (!scannerEnabled)
            return;
	  
	    //counter to track time between scans
        scanFrequencyCounter += Time.deltaTime;
	    //once counter reaches the total time we set between scans, we scan for target and reset counter to zero.
        if (scanFrequencyCounter >= scanFrequency)
        {
            scanFrequencyCounter = 0;
		    //using Physics2D we will draw a circle and use a 2d raycast to see what objects are within it. 
		    //Targets must have collider for raycast to return or it won't be detected with this method.
            RaycastHit2D[] raycastHit2D = Physics2D.CircleCastAll(this.transform.position, scanRadius, Vector2.right,scanRadius*2f,targetLayer);
		    //if we hit any targets, notify listeners that need this info
		    //make sure to check if OnTargetHit is null or not in case no subscribers (but if no subscribers, why using this at all? ;) ) 
            if (raycastHit2D.Length > 0 && OnTargetHit != null)
            {
			    //delgate event
                OnTargetHit(raycastHit2D);
            }
        }

    }
  //Unity editor code. Make sure to use hashtag UNITY_EDITOR so this code doesn't compile to non-editor build.
#if UNITY_EDITOR
    //Draw gizmo wire disc (circle) when GameObject is selected.
    //If you want it to always draw, then you can use void OnDrawGizmos() instead
    void OnDrawGizmosSelected()
    {
	        //set gizmo colour.
            UnityEditor.Handles.color = editorCircleColor;
	        //draw wire circle (disc in unity lingo) based on radius variable you set.
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.forward, scanRadius);
    }
#endif

}
