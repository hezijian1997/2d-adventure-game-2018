using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePersist : MonoBehaviour {

    int StartScene;
    private void Awake()
    {
        int numGamePersist = FindObjectsOfType<GamePersist>().Length;
        if (numGamePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        StartScene = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update ()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        if (CurrentScene != StartScene)
        {
            Destroy(gameObject);
        }
	}
}
