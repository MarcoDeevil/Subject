using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentScene = 0;
    public static GameManager instance;
    List<Level> levels = new List<Level>();

    class Level {
        int level;
        int attempt;
        bool blocked;
    }
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
   // public void restartScene(){
    //    SceneManager.LoadScene(currentScene);
    //}
}
