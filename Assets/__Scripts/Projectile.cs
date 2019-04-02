using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // variable that is class boundscheck
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    private WeaponType _type;

    // weapon type property
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            //calls set type functino
            SetType(value);
        }
    }

    void Awake()
    {
        // gets the bound check component
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the object is off the screen in the upper boundary
        if (_bndCheck.offUp)
        {
            // destroys the game object if it is
            Destroy(gameObject);
        }
    }

    //sets the type of projectile based on weapon type
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefintion def = Main.GetWeaponDefintion(_type);
        _rend.material.color = def.projectileColor;
    }
}