using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttonclick : MonoBehaviour {
    public GameObject Buttonon;
    public GameObject Texton;
    public GameObject Scores;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Scores.SetActive(false);
    }
    public void ToClick()
    {
        Time.timeScale = 1;
        if (Buttonon.transform.Find("Buttontext").GetComponent<Text>().text == "开始游戏")
        {
            Buttonon.SetActive(false);
            Texton.SetActive(false);
            Scores.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
