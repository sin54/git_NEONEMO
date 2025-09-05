using UnityEngine;
using Core;

public class XPOrb : BaseItem
{
    public int xpAmount;
    protected override void Active()
    {
        base.Active();
        GameManager.Instance.player.AddXP(xpAmount);

    }
}
