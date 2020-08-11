using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    
    [DllImport("__Internal")]
    private static extern void SyncFiles();
    
    public GameObject SideMenu;
    public TextMeshProUGUI LevelText;
    int currentScene = 0;
    public static GameManager instance;
    public static int numberOfPassedLevels = 0;

    void Update(){
        if(currentScene > 0)
            SideMenu.SetActive(true);
        else
            SideMenu.SetActive(false);
        LevelText.text = currentScene.ToString();
    }

     void Awake(){
        if(instance == null){
                DontDestroyOnLoad(gameObject);
                instance = this;     
            }
            else {
                Destroy(gameObject);
        }
        LoadData();
    }

    public void nextScene(){
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadLevel(int level){
        currentScene = level;
        SceneManager.LoadScene(level);
    }

    public void SaveData(int numberOfPassedLevels){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfoSubject.dat");

        PlayerData2 data = new PlayerData2();
        data.numberOfPassedLevels = numberOfPassedLevels;

        bf.Serialize(file, data);
        file.Close();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SyncFiles();
        }
    }

    public void LoadData(){
        if(File.Exists(Application.persistentDataPath + "/playerInfoSubject.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfoSubject.dat", FileMode.Open);
            PlayerData2 data = (PlayerData2)bf.Deserialize(file);
            file.Close();
            
            numberOfPassedLevels = data.numberOfPassedLevels;
        }
    }   

}
    [Serializable]
class PlayerData2 {
    public int numberOfPassedLevels;
}
