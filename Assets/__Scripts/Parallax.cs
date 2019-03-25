using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi; // player ship
    public GameObject[] panels; // the scrolling foregrounds
    public float scrollSPeed = -30f; // speed of scrolling

    // motion mult control how much panels react to player movement
    public float motionMult = 0.25f;

    private float _panelHt; //height of panel
    private float _depth; // depth of panel


    // Start is called before the first frame update
    void Start()
    {
        _panelHt = panels[0].transform.localScale.y; // gets the pane height
        _depth = panels[0].transform.position.z; // gets the depth of the pane

        //set intial position of panel
        panels[0].transform.position = new Vector3(0, 0, _depth); // sets one at 0, the other one a height away
        panels[1].transform.position = new Vector3(0, _panelHt, _depth);
        
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0; // the movement variables 
        tY = Time.time * scrollSPeed % _panelHt + (_panelHt * 0.5f);

        // changes the x direction to match the player
        if (poi != null){
            tX = -poi.transform.position.x * motionMult;
        }
        //position panel [0] and handles the movemen
        panels[0].transform.position = new Vector3(tX, tY, _depth); 

        //handles the looping of the second panel
        if(tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _panelHt, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _panelHt, _depth);
        }
    }
}
