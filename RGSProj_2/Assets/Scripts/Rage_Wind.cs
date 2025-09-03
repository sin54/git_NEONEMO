using UnityEngine;

public class Rage_Wind : BaseRage
{
    public float RWSpeed;
    public float RWBounceSpeed;
    public float RWBounceDmg;
    public float RWDmgPerSec;
    public float RWAttractForce;
    public float RWreinforcedForce;
    public float RWreinforcedSize;
    public float RWSize;

    public float summonRad;

    [SerializeField] private GameObject WRparticle;
    [SerializeField] private GameObject WRrange;

    public override void RageStart()
    {
        base.RageStart();
        canAddGauge = false;
        GameObject windRageObj=Instantiate(WRparticle,GetSummonPos(),Quaternion.identity);
        Vector2 direc=windRageObj.GetComponent<WindRageParticle>().Init(this);
        SetWRpos(direc);
    }
    public void RageEnd()
    {
        canAddGauge = true;
        WRrange.SetActive(false);
    }
    private Vector2 GetSummonPos() {
        return Random.insideUnitCircle.normalized * summonRad+(Vector2)transform.position;
    }
    private void SetWRpos(Vector2 dir)
    {
        if (dir == Vector2.zero) return;
        WRrange.SetActive(true);
        WRrange.transform.position = transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WRrange.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
