using System;
using UnityEngine;
[System.Flags]
public enum ItemTag
{
    [InspectorName("������")]
    Staff = 1 << 0,
    [InspectorName("ȭ��")]
    Fire_Master=1<<1,
    [InspectorName("�ڿ�")]
    Nature=1<<2,
    [InspectorName("�ھ�")]
    Core=1<<3,
    [InspectorName("����")]
    Melee=1<<4,
    [InspectorName("�ű�")]
    X=1<<5,
    [InspectorName("������")]
    MagicBook=1<<6,
    [InspectorName("��Ƽ")]
    Party=1<<7,
    [InspectorName("�Ͻ�")]
    Sneak=1<<8, 

}
public enum ItemRarity
{
    [InspectorName("Alpha")]
    Alpha = 0,

    [InspectorName("Beta")]
    Beta = 1,

    [InspectorName("Gamma")]
    Gamma = 2,

    [InspectorName("Delta")]
    Delta = 3,

    [InspectorName("Epsilon")]
    Epsilon = 4,

    [InspectorName("Unique")]
    Uni = 5,
}