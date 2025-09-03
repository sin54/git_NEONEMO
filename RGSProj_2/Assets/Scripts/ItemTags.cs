using System;
using UnityEngine;
[System.Flags]
public enum ItemTag
{
    [InspectorName("지팡이")]
    Staff = 1 << 0,
    [InspectorName("화염")]
    Fire_Master=1<<1,
    [InspectorName("자연")]
    Nature=1<<2,
    [InspectorName("코어")]
    Core=1<<3,
    [InspectorName("근접")]
    Melee=1<<4,
    [InspectorName("신기")]
    X=1<<5,
    [InspectorName("마법서")]
    MagicBook=1<<6,
    [InspectorName("파티")]
    Party=1<<7,
    [InspectorName("암습")]
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

    [InspectorName("특수")]
    Uni = 10,
}