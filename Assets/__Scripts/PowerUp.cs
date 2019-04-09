using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifetime = 6f; //seconds the power up exist
    public float fadetime = 4f; // seconds the power up takes to fade away

    [Header("Set Dynamically")]
    public Main.WeaponType type; // type of power up
    public GameObject cube; // reference to the cube child
    public TextMesh letter; // reference to the textmesh
    public Vector3 rotPerSecond; // euler rotation speed
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        cube = transform.Find("Cube").gameObject; // find the cube reference
        // find the textmesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        // set random velocity
        Vector3 vel = Random.onUnitSphere; // get random xyz velocity, 

        vel.z = 0; // flatten the vel to the xy plane
        vel.Normalize(); // normalize vector 3 makes it length 1m

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // set the rotation of this gameobject to r 0,0,0
        transform.rotation = Quaternion.identity;// identity means equals rotation

        //set up the rotpersecond for the cube
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //fade out powerup over time
        float u = (Time.time - (birthTime + lifetime)) / fadetime;
        // for lifetime seconds, u will be <= 0. then it will transition to 1 over the course of the fadetime seconds

        // handles the fade effect
        if (u>= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            //fade the letter, not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen) // if the power up drifts off the screen
        {
            Destroy(gameObject);
        }
    }

    public void SetType(Main.WeaponType wt)
    {
        // get weapon defintion from main
        WeaponDefintion def = Main.GetWeaponDefintion(wt);
        cubeRend.material.color = def.projectileColor; // set the colour
        type = wt; // set the type
        letter.text = def.letter; // set the letter
    }

    public void AbsorbedBy(GameObject target) // destroys this game object when absorbed
    { //for when hero class collects this powerup
        Destroy(this.gameObject);
    }
}
