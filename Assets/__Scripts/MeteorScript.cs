using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public float meteorHealth = 100;

    public Vector2 rotMinMax = new Vector2(10, 80);
    public Vector2 driftMinMax = new Vector2(0.25f, 0.25f);

    public GameObject sphere;
    public Vector3 rotPerSecond;

    protected BoundsCheck bndCheck;
    public static float speed = 3f;
    public bool meteorDir;

    private void Awake()
    {
        sphere = this.gameObject;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y));

        transform.rotation = Quaternion.identity;
        bndCheck = GetComponent<BoundsCheck>();
        meteorDir = (Random.value <= 0.5f);
    }
    private void Update()
    {
        Move();
        sphere.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        if (meteorHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (bndCheck != null)
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

    public virtual void Move()
    {
        Vector3 tempPos = pos;

        if (meteorDir)
        {
            tempPos.x += speed * Time.deltaTime;
        } else
        {
            tempPos.x += speed * Time.deltaTime * -1;
        }

        pos = tempPos;
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

    public void damageHealth(int dmg) { meteorHealth -= dmg; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            meteorHealth -= 25;
            Debug.Log(meteorHealth);
        }
        else if (other.CompareTag("ProjectileHero"))
        {
            Projectile p = other.gameObject.GetComponent<Projectile>();
            meteorHealth -= Main.GetWeaponDefintion(p.type).damageOnHit;
            Destroy(other.gameObject);

        }

  }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.tag == "ProjectileHero")
        {
            Debug.Log("HITMEBABYONEMORETIME");
            Projectile p = otherGO.GetComponent<Projectile>();
            Destroy(otherGO);
            meteorHealth -= Main.GetWeaponDefintion(p.type).damageOnHit;
            if (meteorHealth <= 0)
            {
                Destroy(this.gameObject);
            }

        }

    }
}
