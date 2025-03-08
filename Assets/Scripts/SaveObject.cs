using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveObject {
    //simple class that has all save important game values 
    public int goldAmount;
    public int jelatineAmount;
    public bool[] unlockList;
    public int[] idSave;
    public int[] levelSave;
    public float[] expSave;
    public int numLevel;
    public int clickLevel;
    public List<GameObject> tempJellyList;
}