using UnityEngine;
using Core;

public class XPOrb : BaseItem
{
    public float xpAmount;
    private bool isHalf = false;
    protected override void EndDay()
    {
        base.EndDay();
        isHalf = true;
        isCollected = true;
    }
    protected override void Active()
    {
        base.Active();
        GameManager.Instance.player.AddXP(isHalf?xpAmount/2:xpAmount);

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isHalf = false;
    }
}
