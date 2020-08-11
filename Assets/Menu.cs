using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject LevelsPanel;
    GameManager gm;

    [SerializeField]
    public List<Level> levels = new List<Level>();

    [Serializable]
    public class Level {
        public int level;
        public Image blokada;
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int level){
        gm.LoadLevel(level);
    }

    public void SetUnlockedLevels(){
        for(int i = 0; i <GameManager.numberOfPassedLevels+1; i++){
            levels[i].blokada.gameObject.SetActive(false);
        }
    }

    public void Play(int level){
        gm.nextScene();
    }

}
