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
    static public int HIGH_SCORE = 0;
    public int currScore = 0;

    // instance of the bounds check class
    private BoundsCheck _bndCheck;

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

        // checks to see if there is an
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyDefaultPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set initial position
        Vector3 pos = Vector3.zero;

        float xMin;
        float xMax;

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


        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyDefaultPadding;
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
