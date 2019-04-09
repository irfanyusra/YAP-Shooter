using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public float meteorHealth = 100; // health  

    public Vector2 rotMinMax = new Vector2(10, 80);
    //public Vector2 driftMinMax = new Vector2(0.25f, 0.25f);

    public GameObject sphere;
    public Vector3 rotPerSecond; // rotations per second

    protected BoundsCheck bndCheck;

    public static float SPEED = 3f; // speed
    public bool meteorDir; // direction of the meteor

    private void Awake()
    {
        sphere = this.gameObject;

        transform.rotation = Quaternion.identity; // no rotation

        // random rotations
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y));

        bndCheck = GetComponent<BoundsCheck>(); // set up the bnd check

        meteorDir = (Random.value <= 0.5f); // randomly selects the meteor direction
    }
    private void Update()
    {
        Move(); // calls the move function
        sphere.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time); // rotates the sphere 
        if (meteorHealth <= 0) // if the health is less than 0, destroy game object
        {
            Destroy(gameObject);
        }
        if (bndCheck != null) // if the bnd check is not null, check which direction it spawned from, and destroy object once it crosses other side
        {
            if (meteorDir)
            {
                if (bndCheck.offRight) Destroy(gameObject);
            } else
            {
                if (bndCheck.offLeft) Destroy(gameObject);
            }
        }
    }

    public virtual void Move() // handles the movement of the meteor
    {
        Vector3 tempPos = pos;

        if (meteorDir)
        {
            tempPos.x += SPEED * Time.deltaTime;
        } else
        {
            tempPos.x += SPEED * Time.deltaTime * -1;
        }
        pos = tempPos;
    }

    public Vector3 pos // property for position
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

    public void damageHealth(int dmg) { meteorHealth -= dmg; } // damage function

    private void OnTriggerEnter(Collider other) // managing the collisions
    {
        if (other.CompareTag("Hero")) // if hero -25 health
        {
            meteorHealth -= 25;
            Debug.Log(meteorHealth);
        }
        else if (other.CompareTag("ProjectileHero")) // if projectile hero, find the defintion and do damage
        {
            Projectile p = other.gameObject.GetComponent<Projectile>();
            meteorHealth -= Main.GetWeaponDefintion(p.type).damageOnHit;
            Destroy(other.gameObject);
        }

  }
}
