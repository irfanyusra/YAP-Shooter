using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,
    gun,
    blaster
}

[System.Serializable]
public class WeaponDefintion{
    public WeaponType type = WeaponType.none;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0f;
    public float delayBetweenShots = 0f;
    public float velocity = 20f;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")] [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefintion def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer _collarRend;


    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();

        // call set type for default _type of weapontype.none
        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //find the fire delegate of the root game object
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }


    }

    public WeaponType type
    {
        get {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType( WeaponType wt)
    {
        _type = wt;

        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefintion(_type);
        _collarRend.material.color = def.projectileColor;
        lastShotTime = 0;
    }

    public void Fire()
    {
        // if game objcet is inactive, return
        if (!gameObject.activeInHierarchy) return;

        //if it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots) return;

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.gun:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                // right projectile
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                //left projectile
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;

        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            //go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}

