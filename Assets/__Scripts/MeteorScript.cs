using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public float meteorHealth = 40;

    public Vector2 rotMinMax = new Vector2(10, 80);
    public Vector2 driftMinMax = new Vector2(0.25f, 0.25f);

    public GameObject sphere;
    public Vector3 rotPerSecond;

    //private BoundsCheck _bndCheck;
    //private Rigidbody rigid;

    private void Awake()
    {
        sphere = this.gameObject;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y),
             Random.Range(rotMinMax.x, rotMinMax.y));

        //_bndCheck = GetComponent<BoundsCheck>();

        Vector3 vel = Random.onUnitSphere;

        vel.z = 0;
        vel.Normalize();

        //vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        //rigid.velocity = vel;

        transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        sphere.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        if (meteorHealth <= 0)
        {
            Destroy(gameObject);
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
