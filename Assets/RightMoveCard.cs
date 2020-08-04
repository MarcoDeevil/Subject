using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightMoveCard : MoveCard
{
    public void MoveRight(){
        if(distance>0 && speed>0)
            player.MoveRight(speed,distance);
    }
}
