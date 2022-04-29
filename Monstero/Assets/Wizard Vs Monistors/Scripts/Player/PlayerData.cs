using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    // This class is used to get the data of the player to be saved by SystemSave class
    public int CoinsCount;


    public PlayerData(GameManager manager)
    {
        CoinsCount = manager.CoinsCount;
    }
}
