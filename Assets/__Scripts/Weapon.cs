using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// weapon defintion 
public class WeaponDefintion{
    public Main.WeaponType type = Main.WeaponType.none;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0f;
    public float delayBetweenShots = 0f;
    public float velocity = 20f;
    public string letter = "L";
}

//weapon class
public class Weapon : MonoBehaviour
{
    // parent transform of the projectiles
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")] [SerializeField]
    private Main.WeaponType _type = Main.WeaponType.none;
    public WeaponDefintion def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer _collarRend;

   

    void Start()
    {
        //Main.MAIN_INSTANCE.pppAS.clip = Main.MAIN_INSTANCE.pppAc;

        collar = transform.Find("Collar").gameObject; // gets the collar gameobject
        _collarRend = collar.GetComponent<Renderer>(); // gets the renderer for the game object

        // call set type for default _type of weapontype.none
        SetType(_type);

        if (PROJECTILE_ANCHOR == null) // if there hasnt been a parent transform
        {
            GameObject go = new GameObject("_ProjectileAnchor"); // create new gameobject
            PROJECTILE_ANCHOR = go.transform;
        }

        //find the fire delegate of the root game object - Hero, check it its hero and then add the method firfe to the delegate so it can call t
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    void Update()
    {
        // switches the weapon if the z key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchWeapon();
        }
    }

        public void SwitchWeapon()
    {
        // changes the weapon type
        if (typeOfWeapon == Main.WeaponType.gun)
            {
                typeOfWeapon = Main.WeaponType.blaster;
            }
            else
            {
                typeOfWeapon = Main.WeaponType.gun;
            }
    }

    // property weapon type
    public Main.WeaponType typeOfWeapon
    {
        get {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    // sets the weapon type based on the definition in the main
    public void SetType(Main.WeaponType wt)
    {
        _type = wt;
        this.gameObject.SetActive(true); 
        def = Main.GetWeaponDefintion(_type); // gets the defintion for the type of weapon
        _collarRend.material.color = def.projectileColor; // sets the collar to projectile colour
        lastShotTime = 0; // set the last shot time to be 0 so that the weapon can be shot
    }

    //fires the currently selected weapon
    public void Fire()
    {

        // if game object is inactive, return
        if (!gameObject.activeInHierarchy) return;

        //if it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots) return;

        Projectile p; // set up projectile
        Vector3 vel = Vector3.up * def.velocity; // set up velocity
        if (transform.up.y < 0) //to make sure that the shots are facing up
        {
            vel.y = -vel.y;
        }

        switch (typeOfWeapon) // depending on the type of weapon selected change the shot
        {
            case Main.WeaponType.gun:
                Main.MAIN_INSTANCE.yapAs.Play();
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case Main.WeaponType.blaster:
                Main.MAIN_INSTANCE.pppAs.Play();
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
    public Projectile MakeProjectile() // making the projectiles and setting the defintion of the projectile
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
        p.type = typeOfWeapon;
        lastShotTime = Time.time;
        return (p);
    }
}

