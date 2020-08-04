using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightMoveCard : MoveCard
{
    public void MoveRight(){
        if(speed>0 && distance != 0)
            player.Move(speed,distance);
    }
}
