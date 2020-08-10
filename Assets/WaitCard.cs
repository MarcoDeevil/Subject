using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitCard : MonoBehaviour
{

    public float time;
    public TextMeshProUGUI timeValue;

    protected Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseTime(){
        time++;
        timeValue.text = time.ToString();
    }

    public void DecreaseTime(){
        if(time > 0)
            time--;
        timeValue.text = time.ToString();
    }

    public void Wait(){
        if(time > 0)
            player.Wait(time);
        else
            Debug.Log("Za mały time");
    }
}
