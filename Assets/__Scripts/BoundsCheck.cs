using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{

	//Set in inspector
	public float radius = 1f;

	//Set dynamically
	public float camWidth;
	public float camHeight;

    void Awake() {
    	camHeight = Camera.main.orthographicSize;
    	camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate() {
    	Vector3 pos = transform.position;

    	if (pos.x > camWidth - radius) {
    		pos.x = camWidth - radius;
    	}
    	if (pos.x < -camWidth + radius) {
    		pos.x = -camWidth + radius;
    	}
    	if (pos.y > camHeight - radius) {
    		pos.y = camHeight - radius;
    	}
    	if (pos.y < -camHeight + radius) {
    		pos.y = -camHeight + radius;
    	}

    	transform.position = pos;
    }

    //Draw the bounds in the scene pane using OnDrawGizmos()
    void OnDrawGizmos() {
    	if (!Application.isPlaying) return;
    	Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
    	Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
