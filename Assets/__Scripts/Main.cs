﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main MAIN_INSTANCE;

    //dictionary to hold weapon type and definitons
    static Dictionary<WeaponType, WeaponDefintion> WEAP_DICT;

    //Set in inspector
    public GameObject[] prefabEnemies;
    public WeaponDefintion[] weaponDefintions;
    public GameObject meteorPrefab;

    //enenmy spawning and padding
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    //UI Image
    public Image NuclearBlast;

    //UI Text 
    public Text highScoreText;
    public Text currScoreText;
    public Text currLevelPrefab;
    private Text currLevelText;
    //static so that the high score can be kept
    public int highScore = 0;
    // pulic so that enemy can access it when it gets destroyed
    public int currScore = 0;
    public int level = 1;

    GameObject canvas;

    public bool meteorDir;

    // instance of the bounds check class
    private BoundsCheck _bndCheck;

    // set up audio files
    public AudioClip backgroundMusicAC;
    public AudioSource backgroundMusicAS;

    public AudioClip pppAc;
    public AudioSource pppAs;

    public AudioClip yapAc;
    public AudioSource yapAs;

    public AudioClip powerUpAc;
    public AudioSource powerUpAs;

    public AudioClip levelUpAc;
    public AudioSource levelUpAs;

    public AudioClip ouchAc;
    public AudioSource ouchAs;

    public AudioClip blehAc;
    public AudioSource blehAs;

    public AudioClip boomAc;
    public AudioSource boomAs;

    public enum WeaponType // weapon types
    {
        none, 
        gun,
        blaster,
        shield,
        movementSpeed,
        attackSpeed,
        nuke
    }

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        //sets the highscore and the curr score
        SetHighScore();
        SetCurrScore();

        ShowLevelText(); // calls the function to show level

        //sets the main instance to this
        MAIN_INSTANCE = this;

        // gets the bounds check component
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //dictionary key = weapontype, value = weapon definiton
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefintion>();

        // for each of the weapon definitions inside the weapon definition list add it to the dictionary
        foreach(WeaponDefintion def in weaponDefintions)
        {
            WEAP_DICT[def.type] = def;
        }

        // plays the background music
        backgroundMusicAS.clip = backgroundMusicAC;
        backgroundMusicAS.Play();

        // sets up all the other clips and sources (linking them)
        pppAs.clip = pppAc;
        yapAs.clip = yapAc;
        powerUpAs.clip = powerUpAc;
        levelUpAs.clip = levelUpAc;
        ouchAs.clip = ouchAc;
        blehAs.clip = blehAc;
        boomAs.clip = boomAc;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetHighScore();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            resetHighScore();
            Restart();
        }
    }

    private void resetHighScore()
    {
        PlayerPrefs.SetInt("highScore", 0);
        SetHighScore();
    }

    //function that spawns enemies
    public void SpawnEnemy()
    {
        // generates a random number ranging in the number of enemies
        int ndx = Random.Range(0, prefabEnemies.Length);

        // instantiates the enemy corresponding to the random number
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        // checks to see if there is bounds
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyDefaultPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set initial position
        Vector3 pos = Vector3.zero;

        float xMin;
        float xMax;

        // compares tag too see if its enemy 2, if it is change the x min and max
        if (go.CompareTag("Enemy_2"))
        {
            xMin = -_bndCheck.camWidth + enemyDefaultPadding + 6f;
            xMax = _bndCheck.camWidth - enemyDefaultPadding - 6f;
        }
        else
        {
            xMin = -_bndCheck.camWidth + enemyDefaultPadding;
            xMax = _bndCheck.camWidth - enemyDefaultPadding;
        }

        // sets the position of the new enemy
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyDefaultPadding;
        go.transform.position = pos;
        // spawns the enemy
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    //delayed restart function
    public void DelayedRestart (float delay)
    {
        // invoke the restart method with the delay
        Invoke("Restart", delay);
    }

    // restart function
    public void Restart()
    {
        //SetHighScore();
        // resets to base values (level changed values)
        enemySpawnPerSecond = 0.5f; 
        Enemy.speed = 10f;
        // reload _Scene_0 to restart game
        SceneManager.LoadScene("_Scene_0");
    }

    public void nukeBlastImage() // handles nuke blast and fade effect
    {
        Image nuclearBlast = Instantiate<Image>(NuclearBlast, new Vector3(480, 700, 0), Quaternion.identity);
        nuclearBlast.transform.SetParent(canvas.transform);
        nuclearBlast.CrossFadeAlpha(0f, 1f, false);
    }

    public void ShowLevelText() // show level text
    {
        currLevelText = Instantiate<Text>(currLevelPrefab, new Vector3(420, 300, 0), Quaternion.identity);
        SetCurrLevel();
        currLevelText.transform.SetParent(canvas.transform);

    }

    public void nextLevel() // starts next level by increasing the spawn and introducint a meteor
    {
        levelUpAs.Play();
        level++;
        enemySpawnPerSecond *= 1.25f;
        ShowLevelText();

        Vector3 pos = Vector3.zero;

        GameObject meteor = Instantiate<GameObject>(meteorPrefab);

        float yMin = -_bndCheck.camHeight;
        float yMax = _bndCheck.camHeight;

        if (meteor.GetComponent<MeteorScript>().meteorDir)
        {
            pos.x = -_bndCheck.camWidth;
        } else
        {
            pos.x = _bndCheck.camWidth;
        }
        pos.y = Random.Range(yMin + 15, yMax - 15);
        meteor.transform.position = pos;
    }

    // set highscore function
    public void SetHighScore()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
    }

    //set current score function
    public void SetCurrScore()
    {
        currScoreText.text = "Current Score: " + currScore;
    }

    // set current level
    public void SetCurrLevel()
    {
        currLevelText.text = "LEVEL " + level;
    }

    //get the weapon definition in the weapon dictionary
    static public WeaponDefintion GetWeaponDefintion (WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt)) return (WEAP_DICT[wt]);
        //return nothing if it doesnt have the key
        return (new WeaponDefintion());
    }
}
