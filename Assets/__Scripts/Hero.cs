using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero HERO_INSTANCE; // singleton hero

    //Set in inspector
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    //delay for the game to restart
    public float gameRestartDelay = 2f;

    //Set dynamically
    public float shieldLevel = 1f;

    // holds a reference to the last triggering game object
    private GameObject _lastTriggerGo = null;

    // new delegate type 
    public delegate void WeaponFireDelegate();
    // weaponfiredelegate field named firedelegate
    public WeaponFireDelegate fireDelegate;

    private bool _activeNuke = false;


    void Awake()
    {
        // since it is a singleton design, if there is more than one instance it will print out an error
        if (HERO_INSTANCE == null)
        {
            HERO_INSTANCE = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // gets input in up, down, left, and right controls
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // moves the hero object using input, speed, and time
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // adds rotation to make movement more juciy
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        // calls the function delegate fireDelegate if the space or any jump key/button is pressed
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

        if (Input.GetKeyDown(KeyCode.X)) // if the keycode of x is pressed it will attempt to call launch nuke
        {
            launchNuke();
        }
    }

    public void launchNuke() // launch nuke handles the destruction of enemies
    {
        if (_activeNuke) // will only activate if there is a nuke
        {
            Main.MAIN_INSTANCE.boomAs.Play(); // play sound
            Main.MAIN_INSTANCE.nukeBlastImage(); // apply image affect
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects) // uses the hierachy to destroy enemies
            {
                if (go.CompareTag("Enemy") || go.CompareTag("Enemy_2"))
                {
                    Destroy(go);
                }
            }
            _activeNuke = false; // sets the nuke active to false
        }
    }

    public void shieldDamaged() // function to take off shield
    {
        shieldLevel--;
        Main.MAIN_INSTANCE.ouchAs.Play();
    }

    // On the trigger collision of any object
    private void OnTriggerEnter(Collider other)
    {
        // gets the root parent of the go of other and saves it to go
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // make sure it's not the same triggering game object as last time if it is do nothing
        if (go == _lastTriggerGo)
        {
            return;
        }

        // set the last triggering object to game object
        _lastTriggerGo = go;

        // checks if it is an enemy, if it is the shield level is deceased and the go is destroyed
        if (go.tag == "Enemy" || go.tag == "Enemy_2")
        {
            // shield level decrease and destroy the enemy game object
            shieldDamaged();
            Destroy(go);
        }
        else if (go.tag == "PowerUp") // if power up absorb
        {
            AbsorbPowerUp(go);
        }
        else if (go.tag == "Meteor") // if meteor damage shield and return last object after delay
        {
            shieldDamaged();
            StartCoroutine(ExecuteAfterTime(2));
        }
        else
        {
            print("triggered by non-enemy: " + go.name); // if it isnt an enemy, then print that it triggered something else
        }
        // if the hero's shield level is less than 0 it destroys the game object and resets the game
        if (shieldLevel < 0)
        {
            // destroy this hero instance
            Destroy(this.gameObject);

            // restarts the game after delay using main
            Main.MAIN_INSTANCE.DelayedRestart(gameRestartDelay);
        }
        
    }

    public void AbsorbPowerUp(GameObject go) // absorbs the power up
    {
        Main.MAIN_INSTANCE.powerUpAs.Play();
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case Main.WeaponType.shield: // depending on which level the shield is do nothing or add 1
                if (shieldLevel == 4)
                {
                    shieldLevel = 4;
                }
                else
                {
                    shieldLevel++;
                }

                break;
            case Main.WeaponType.movementSpeed: // if movement speed add 10
                speed += 10;
                break;
            case Main.WeaponType.attackSpeed: // delay between shots decrease by percentage but only to a point 
                if (Main.GetWeaponDefintion(Main.WeaponType.blaster).delayBetweenShots > 0.25f)
                {
                    Main.GetWeaponDefintion(Main.WeaponType.gun).delayBetweenShots *= 0.7f;
                    Main.GetWeaponDefintion(Main.WeaponType.blaster).delayBetweenShots *= 0.7f;
                }
                break;
            case Main.WeaponType.nuke: // if nuke is picked up set active nuke to true
                _activeNuke = true;
                break;
            default: // nothing
                break;

        }
        pu.AbsorbedBy(this.gameObject);// call the absorbed function to destroy object
    }

    IEnumerator ExecuteAfterTime(float time) // used to reset the last trigger go
    {
        _lastTriggerGo = null;
        yield return new WaitForSeconds(time);
    }
}