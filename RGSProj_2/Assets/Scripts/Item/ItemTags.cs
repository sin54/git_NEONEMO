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