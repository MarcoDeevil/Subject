using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentScene = 0;
    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void Awake(){
        if(instance == null){
                DontDestroyOnLoad(gameObject);
                instance = this;     
            }
            else {
                Destroy(gameObject);
        }
    }

    public void nextScene(){
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }
    public void restartScene(){
        SceneManager.LoadScene(currentScene);
    }
}
