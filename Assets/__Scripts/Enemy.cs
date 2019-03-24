using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Set in inspector
    public float speed = 10f;
    public float fireRate = 0.3f; //seconds per shot
    public float health = 10;
    public int score = 100;	//value

    protected BoundsCheck bndCheck;

    public bool boolValue = true;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        boolValue = (Random.value > 0.5f);
    }

    public Vector3 pos {
    	get {
    		return(this.transform.position);
    	}
    	set {
    		this.transform.position = value;
    	}
    }

    void Update() {
        Move();

        if (bndCheck != null)
        {
            if (bndCheck.offDown|| bndCheck.offLeft||bndCheck.offRight) Destroy(gameObject);
        }
    }

    public virtual void Move() {
    	Vector3 tempPos = pos;
    	tempPos.y -= speed * Time.deltaTime;
    	pos = tempPos;
    }
}