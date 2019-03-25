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

    public bool boolValue = true;


    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        boolValue = (Random.value > 0.5f);
    }

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
        Move();

        if (bndCheck != null)
        {
            if (bndCheck.offDown || bndCheck.offLeft || bndCheck.offRight) Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                break;
                }
                health -= Main.GetWeaponDefintion(p.type).damageOnHit;
                if (health <= 0)
                {
                    Main.S.currScore += score;
                    Main.S.currScoreText.text = "Score: "+ Main.S.currScore;
                    Destroy(this.gameObject);

                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }


        //if (otherGO.tag == "ProjectileHero")
        //{
        //    Destroy(otherGO);
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        //}
    }
}