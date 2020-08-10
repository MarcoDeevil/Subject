using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpCard : MonoBehaviour
{
    public float power;
    public float angle;
    public TextMeshProUGUI angleValue;
    public TextMeshProUGUI powerValue;
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

    public void IncreaseAngle(){
        angle+=5;
        angleValue.text = angle.ToString();
    }

    public void DecreaseAngle(){
         angle-=5;
        angleValue.text = angle.ToString();
    }

     public void Increasepower(){
        power++;
        powerValue.text = power.ToString();
    }

    public void Decreasepower(){
        if(power>0){
            power--;
            powerValue.text = power.ToString();
        }
    }

    public void Jump(){
        if(power > 0 && angle !=0)
            player.Jump(power, angle);
        else
            Debug.Log("Za mały power lub angle");
    }
}
