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


    void Awake() {
        // since it is a singleton design, if there is more than one instance it will print out an error
		if (HERO_INSTANCE == null) {
			HERO_INSTANCE = this;
		} else {
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
            shieldLevel--;
            Destroy(go);

            // if the hero's shield level is less than 0 it destroys the game object and resets the game
            if (shieldLevel < 0)
            {
                // destroy this hero instance
                Destroy(this.gameObject);

                // restarts the game after delay using main
                Main.MAIN_INSTANCE.DelayedRestart(gameRestartDelay);
            }
        }
        else if(go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("triggered by non-enemy: " + go.name); // if it isnt an enemy, then print that it triggered something else
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case Main.WeaponType.shield:
                if (shieldLevel == 4)
                {
                    shieldLevel = 4;
                }
                else
                {
                    shieldLevel++;
                }

                break;
            case Main.WeaponType.movementSpeed:
                speed += 10;
                break;
            case Main.WeaponType.attackSpeed:
                if (Main.GetWeaponDefintion(Main.WeaponType.blaster).delayBetweenShots > 0.15f)
                {
                    Main.GetWeaponDefintion(Main.WeaponType.gun).delayBetweenShots *= 0.7f;
                    Main.GetWeaponDefintion(Main.WeaponType.blaster).delayBetweenShots *= 0.7f;
                }
                break;
            default: // nothing
                break;

        }
        pu.AbsorbedBy(this.gameObject);
    }


    //Weapon GetEmptyWeaponSlot()
    //{
    //    for (int i=0; i<weapons.Length; i++)
    //    {
    //        if (weapons[i].typeOfWeapon == Main.WeaponType.none)
    //        {
    //            return (weapons[i]);
    //        }
    //    }
    //    return (null);
    //}

    //void ClearWeapons()
    //{
    //    foreach (Weapon w in weapons)
    //    {
    //        w.SetType(Main.WeaponType.none);
    //    }
    //}
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
