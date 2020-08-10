using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaitCard : MonoBehaviour
{

    public float time;
    public TextMeshProUGUI timeValue;
    public Slider slider;

    protected Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        time = slider.value;
        timeValue.text = ((int)time).ToString();        
    }

    public void Wait(){
        if(time > 0)
            player.Wait(time);
        else
            Debug.Log("Za mały time");
    }
}
