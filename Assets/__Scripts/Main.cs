using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    static public Main S;

    //Set in inspector
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck _bndCheck;

    private void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
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
        SceneManager.LoadScene("_Scene_0");
    }
}
