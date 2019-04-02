using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Set in inspector
    public float speed = 10f;
    public float fireRate = 0.3f; //seconds per shot
    public float health = 20;
    public int score = 100;	//value

    protected BoundsCheck bndCheck;

    public bool enemy1Direction = true;


    private void Awake()
    {
        // gets the bounds check component
        bndCheck = GetComponent<BoundsCheck>();
        //determines whether the enemy 1 will go right or left
        enemy1Direction = (Random.value > 0.5f);
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
                health -= Main.GetWeaponDefintion(p.type).damageOnHit; // health decreased by the weapons damage specs
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

                }
                Destroy(otherGO); // destroy the projectile
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }
}