using UnityEngine;

public class XPOrb : BaseItem
{
    public int xpAmount;
    protected override void Active()
    {
        base.Active();
        GameManager.instance.player.AddXP(xpAmount);

    }
}
