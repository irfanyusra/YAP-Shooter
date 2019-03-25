﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;

    //dictionary
    static Dictionary<WeaponType, WeaponDefintion> WEAP_DICT;


    //Set in inspector
    public GameObject[] prefabEnemies;
    public WeaponDefintion[] weaponDefintions;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public Text highScoreText;
    public Text currScoreText;
    static public int HIGH_SCORE = 0;
    public int currScore = 0;
    private BoundsCheck _bndCheck;

    private void Awake()
    {
        SetHighScore();
        SetCurrScore();
        
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //dictionary key = weapontype, value = weapon definiton
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefintion>();
        foreach(WeaponDefintion def in weaponDefintions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);


        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set initial position
        Vector3 pos = Vector3.zero;

        float xMin;
        float xMax;

        if (go.CompareTag("Enemy_2"))
        {
            xMin = -_bndCheck.camWidth + enemyPadding + 6f;
            xMax = _bndCheck.camWidth - enemyPadding - 6f;
        }
        else
        {
            xMin = -_bndCheck.camWidth + enemyPadding;
            xMax = _bndCheck.camWidth - enemyPadding;
        }


        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart (float delay)
    {
        // invoke the restart method
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        // reload _Scene_0 to restart game
        if (HIGH_SCORE < currScore)
        {
            HIGH_SCORE = currScore;
            SetHighScore();
        }
        currScore = 0;

        SceneManager.LoadScene("_Scene_0");
        SetHighScore();
    }

    void SetHighScore()
    {
        highScoreText.text = "High Score: " + HIGH_SCORE;
    }
    void SetCurrScore()
    {
        currScoreText.text = "Current Score: " + currScore;
    }
    static public WeaponDefintion GetWeaponDefintion (WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt)) return (WEAP_DICT[wt]);
        return (new WeaponDefintion());
    }
}
