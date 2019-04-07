using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    private void Start()
    {
        health = 10;
        score = 75;
    }
    public override void Move()
    {
        Vector3 tempPos = pos;
        if (enemy1Direction) // if the enemy1 direction is positive it will move to the left
        {
            tempPos.x -= speed * Time.deltaTime;
        }
        else // else move right
        {
            tempPos.x += speed * Time.deltaTime;
        }
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
