using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenario_button : MonoBehaviour
{

    public GameObject NaviMap_button;
    public GameObject Spotify;
    public GameObject Nback;
    public GameObject RoadSign;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Navi_Map_Click(){
        NaviMap_button.SetActive(true);
    }
    public void Spotify_Click(){
        Spotify.SetActive(true);
        RoadSign.SetActive(true);
    }
    public void Nback_Click(){
        Nback.SetActive(true);
        RoadSign.SetActive(true);
    }
}
