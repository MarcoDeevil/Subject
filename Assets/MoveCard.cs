using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveCard : MonoBehaviour
{
    public float distance;
    public float speed;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI distanceValue;
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

    public void IncreaseSpeed(){
        speed++;
        speedValue.text = speed.ToString();
    }

    public void DecreaseSpeed(){
         speed--;
        speedValue.text = speed.ToString();
    }

     public void IncreaseDistance(){
        distance++;
        distanceValue.text = distance.ToString();
    }

    public void DecreaseDistance(){
            distance--;
            distanceValue.text = distance.ToString();
        }
    }

