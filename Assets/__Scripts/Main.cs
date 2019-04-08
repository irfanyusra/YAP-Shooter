using System.Collections;
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

    //enenmy spawning and padding
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    //UI Text 
    public Text highScoreText;
    public Text currScoreText;
    //static so that teh high score can be kept
    public int highScore = 0;
    // pulic so that enemy can access it when it gets destroyed
    public int currScore = 0;

    // instance of the bounds check class
    private BoundsCheck _bndCheck;

    public enum WeaponType
    {
        none, 
        gun,
        blaster,
        shield,
        movementSpeed,
        attackSpeed
    }

    private void Awake()
    {
        //sets the highscore and the curr score
        SetHighScore();
        SetCurrScore();

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
        // reload _Scene_0 to restart game
        SceneManager.LoadScene("_Scene_0");
        SetHighScore();
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

    //get the weapon definition in the weapon dictionary
    static public WeaponDefintion GetWeaponDefintion (WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt)) return (WEAP_DICT[wt]);
        //return nothing if it doesnt have the key
        return (new WeaponDefintion());
    }
}
