using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{

	//Set in inspector
	public float radius = 1f;
    public bool keepOnScreen = true;

    //Set dynamically
    public bool isOnScreen = true;
	public float camWidth;
	public float camHeight;

    //Hide in inspector
    public bool offRight, offLeft, offUp, offDown;

    void Awake() {
        // set camera hieghts and widths
    	camHeight = Camera.main.orthographicSize;
    	camWidth = camHeight * Camera.main.aspect;
    }

    // to avoid race condition set to late update
    void LateUpdate() {
        // get the postion 
    	Vector3 pos = transform.position;
        // set is on screen to true to correct it if it was false, set the boundary triggers to false
        isOnScreen = true;
        offRight = offUp = offDown = offLeft = false;

        // if the position is off the screen at a particular point correct it
    	if (pos.x > camWidth - radius) {
    		pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
    	}
    	if (pos.x < -camWidth + radius) {
    		pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
    	}
    	if (pos.y > camHeight - radius) {
    		pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
    	}
    	if (pos.y < -camHeight + radius) {
    		pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
    	}

        isOnScreen = !(offRight || offLeft || offUp || offDown);

        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    //Draw the bounds in the scene pane using OnDrawGizmos()
    void OnDrawGizmos() {
    	if (!Application.isPlaying) return;
    	Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
    	Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
