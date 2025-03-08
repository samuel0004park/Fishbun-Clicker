using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/BeanData", fileName = "BeanFish Data")]
public class BeanData :ScriptableObject
{
    public int ID=0;
    public int Level=1;
    public float Exp=0f;
}
