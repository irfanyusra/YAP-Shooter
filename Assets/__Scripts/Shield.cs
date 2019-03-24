using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    // non-public material will not appear in the inspector
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        // gets the material
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // reads the current shield
        int currLevel = Mathf.FloorToInt(Hero.S._shieldLevel);

        // if the current shield is different from the level shown,  change it so its the same
        if (levelShown != currLevel)
        {
            levelShown = currLevel;

            // adjust the textrue to show different sheild level
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        //rotate the shield a bit every frame (time-based)
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
