﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Set in inspector
    public float speed = 10f;
    public float fireRate = 0.3f; //seconds per shot
    public float health = 20;
    public int score = 100; //value
    public bool enemy1Direction = true;
    protected BoundsCheck bndCheck;

    Main.WeaponType puType; // type for power up

    public GameObject prefabPowerUp; // prefab for power up
    public Main.WeaponType[] powerUps = new Main.WeaponType[] { 
    Main.WeaponType.shield, Main.WeaponType.movementSpeed }; // list of power ups avaiable

    public int powerUpDropChance = 5;  // # out of 10 chance

    private GameObject lastTriggerGameObject = null;


    void Awake()
    {
        // gets the bounds check component
        bndCheck = GetComponent<BoundsCheck>();
        //determines whether the enemy 1 will go right or left
        enemy1Direction = (Random.value > 0.5f);

        int dropChance = Random.Range(0, 11); // to select if this enemy will have a drop chance
        if (dropChance <= powerUpDropChance) // if the the random number is less than or equal to powerupdrop chance it will drop a power up
        {
            int randomPowerUp = Random.Range(0, powerUps.Length); // selects the powerup from the list
            puType = powerUps[randomPowerUp];
        }
    }

    private void Start()
    {
        health = 20;
        score = 50;
    }
    //position proerty
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        // calls the move function 
        Move();

        // gameobject is not on the screen
        if (bndCheck != null)
        {
            // if the checks for which bounds it is off is true destroy the game object
            if (bndCheck.offDown || bndCheck.offLeft || bndCheck.offRight) Destroy(gameObject);
        }
    }

    //move function
    public virtual void Move()
    {
        // simple movement upwards
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        if (lastTriggerGameObject == otherGO)
        {
            return;
        }

        lastTriggerGameObject = otherGO;
        
        //checking what type of go it is 
        switch (otherGO.tag)
        {
            case "ProjectileHero": // if its projectilehero see if it was hit on screen or not
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                else
                {
                    Destroy(otherGO);
                    health -= Main.GetWeaponDefintion(p.type).damageOnHit; // health decreased by the weapons damage specs
                    Debug.Log(Main.GetWeaponDefintion(p.type).damageOnHit + " " + health);
                    // if health of the enemy is less than 0 or equal to 0
                    if (health <= 0)
                    {
                        Main.MAIN_INSTANCE.currScore += score; // current score increases
                        if (Main.MAIN_INSTANCE.currScore >= PlayerPrefs.GetInt("highScore"))
                        {
                            PlayerPrefs.SetInt("highScore", Main.MAIN_INSTANCE.currScore);
                        }
                        Main.MAIN_INSTANCE.SetCurrScore(); // calls to set the current score
                        Main.MAIN_INSTANCE.SetHighScore();
                        Destroy(this.gameObject); // destroys the enemy

                        if (puType == Main.WeaponType.shield || puType == Main.WeaponType.movementSpeed)
                        {
                            GameObject go = Instantiate(prefabPowerUp) as GameObject;
                            PowerUp pu = go.GetComponent<PowerUp>();
                            pu.SetType(puType);

                            pu.transform.position = this.transform.position;
                        }
                    }
                }

                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break; 
        }
    }
}