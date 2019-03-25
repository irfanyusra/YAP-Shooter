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

    //delay for the game to restart
    public float gameRestartDelay = 2f;

    //projectiles
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    //Set dynamically
    [SerializeField]
	public float _shieldLevel = 1f;

    // holds a reference to the last triggering game object
    private GameObject lastTriggerGo = null;

    // new delegate type 
    public delegate void WeaponFireDelegate();
    // weaponfiredelegate field named firedelegate
    public WeaponFireDelegate fireDelegate;

	void Awake() {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
		}
       //fireDelegate += TempFire;

	}

  
    // Update is called once per frame
    void Update()
    {
        // gets input in up, down, left, and right controls
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // moves the hero object
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // adds a rotation to make movement more juciy
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
       
       // //gets input to fire 
       //if (Input.GetKey(KeyCode.Space))
        //{
        //    TempFire();
        //}

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    //void TempFire()
    //{
    //    //instantiates projectile
    //    GameObject projGO = Instantiate<GameObject>(projectilePrefab);
    //    // sets the location to be the same location as the hero game object
    //    projGO.transform.position = transform.position;
    //    Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
    //    //// moves the projectile based off velocity
    //    //rigidB.velocity = Vector3.up * projectileSpeed;

    //    Projectile proj = projGO.GetComponent<Projectile>();
    //    proj.type = WeaponType.gun;
    //    float tSpeed = Main.GetWeaponDefintion(proj.type).velocity;
    //    rigidB.velocity = Vector3.up * tSpeed;

    //}


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
        if (go.tag == "Enemy" || go.tag == "Enemy_2")
        {
            _shieldLevel--;
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
