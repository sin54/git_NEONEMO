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
    [InspectorName("E")]
    E = 0,

    [InspectorName("D")]
    D = 1,

    [InspectorName("C")]
    C = 2,

    [InspectorName("B")]
    B = 3,

    [InspectorName("B+")]
    Bp = 4,

    [InspectorName("A")]
    A = 5,

    [InspectorName("A+")]
    Ap = 6,

    [InspectorName("S")]
    S = 7,

    [InspectorName("S+")]
    Sp = 8,

    [InspectorName("X")]
    X = 9,

    [InspectorName("Ư��")]
    Uni = 10,
}