using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoveCard : MonoBehaviour
{
    public float distance;
    public float speed;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI distanceValue;
    protected Player player;
    public Slider sliderDistance;
    public Slider sliderSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        speed = sliderSpeed.value;
        distance = sliderDistance.value;
        distanceValue.text = ((int)distance).ToString();
        speedValue.text = ((int)speed).ToString();
    }

    public void Move(){
        if(speed>0 && distance != 0)
            player.Move(speed,distance);
        else
            Debug.Log("Za mały speed lub distance");
    }

}

