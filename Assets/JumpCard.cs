using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JumpCard : MonoBehaviour
{
    public float power;
    public float angle;
    public TextMeshProUGUI angleValue;
    public TextMeshProUGUI powerValue;
    protected Player player;
    public Slider sliderPower;
    public Slider sliderAngle;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        power = sliderPower.value;
        angle = sliderAngle.value;
        angleValue.text = ((int)angle).ToString();
        powerValue.text = ((int)power).ToString();
    }

    public void Jump(){
        if(power > 0 && angle !=0)
            player.Jump(power, angle);
        else
            Debug.Log("Za mały power lub angle");
    }
}
