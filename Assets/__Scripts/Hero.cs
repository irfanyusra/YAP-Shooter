using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

	static public Hero S;

    //Set in inspector
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

    public float gameRestartDelay = 2f;

    //Set dynamically
    [SerializeField]
	public float _shieldLevel = 1f;

    // holds a reference to the last triggering game object
    private GameObject lastTriggerGo = null;

	void Awake() {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
		}
	}

  
    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // gets the root parent of the go of other, and prints out object name
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        //print("Triggered: " + go.name);

        // make sure it's not the same triggering game object as last time
        if (go == lastTriggerGo)
        {
            return;
        }

        // set the last triggering object to game object
        lastTriggerGo = go;

        // checks if it is an enemy, if it is the shield level is deceased and the go is destroyed
        if (go.tag == "Enemy")
        {
            print("ENEMYYYYYYY");
            _shieldLevel--;
            print("shield level decreased:" + _shieldLevel);
            Destroy(go);
            if (_shieldLevel < 0)
            {
                Destroy(this.gameObject);

                // restarts the game after delay using main
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
        else
        {
            print("triggered by non-enemy: " + go.name); // if it isnt an enemy, then print that it triggered something else
        }
    }

    /*

    //property
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            // if the shield is less than 0
            print(value);
            if (value < 0)
            {
                Destroy(this.gameObject);

                // restarts the game after delay using main
                Main.S.DelayedRestart(gameRestartDelay);
            }
            else
            {
                _shieldLevel = Mathf.Min(value, 4);
            }


        }


    }
   */
}
