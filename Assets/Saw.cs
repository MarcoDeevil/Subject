using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    //adjust this to change speed
    [SerializeField]
    float speed = 5f;
    //adjust this to change how high it goes
    [SerializeField]
    float height = 0.5f;

    [SerializeField]
    bool up = true;

    Vector3 pos;

    private void Start()
    {
    pos = transform.position;
    }
    void Update()
    {
        if(up){
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
            //set the object's Y to the new calculated Y
            transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
        }   
        else if(!up){
            float newX = Mathf.Sin(Time.time * speed) * height + pos.y;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
}
}
