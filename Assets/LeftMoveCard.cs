using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMoveCard : MoveCard
{
 public void MoveLeft(){
        if(distance >0 && speed>0)
            player.MoveLeft(speed,distance);
    }
}
