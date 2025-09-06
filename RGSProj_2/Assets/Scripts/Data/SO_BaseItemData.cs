using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Base Item",menuName ="Data/Item Data/Base Data")]
public class SO_BaseItemData : ScriptableObject
{
    public string itemName;
    public string[] itemDescription;
    public Sprite itemIcon;
    public MaxLevelData[] levelDatas = new MaxLevelData[3];
}
[System.Serializable]
public class MaxLevelData
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int[] synerge;
}
