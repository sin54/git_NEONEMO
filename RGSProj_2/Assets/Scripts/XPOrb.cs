using UnityEngine;
using Core;

public class XPOrb : BaseItem
{
    public int xpAmount;
    protected override void EndDay()
    {
        base.EndDay();
        xpAmount /= 2;
        isCollected = true;
    }
    protected override void Active()
    {
        base.Active();
        GameManager.Instance.player.AddXP(xpAmount);

    }
}
