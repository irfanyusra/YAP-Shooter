﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // variable that is class boundscheck
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        // gets the bound check component
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        // checks if the object is off the screen in the upper boundary
        if (bndCheck.offUp)
        {
            // destroys the game object if it is
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType)
    {
        //set the type
        _type = eType;
        WeaponDefintion def = Main.GetWeaponDefintion(_type);
        rend.material.color = def.projectileColor;
    }
}