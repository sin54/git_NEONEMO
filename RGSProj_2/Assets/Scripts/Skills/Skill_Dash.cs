using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Core;

public class Skill_Dash : BaseSkill
{
    private Player player;
    private PlayerTrail playerTrail;
    private SpriteRenderer SR;
    private SO_DashData skillData;
    private PlayerTypeManager PT;

    [SerializeField] private GameObject teleportParticle;
    [SerializeField] private Image dashCoolImg;
    [SerializeField] private TMP_Text dashCoolTxt;
    [SerializeField] private TMP_Text dashLvlTxt;
    public bool isPlayerDashing { get; private set; }
    private float lastPlayerDashStartTime;
    public Color dashColor;
    public Color playerColor;
    private void Awake()
    {
        player=GetComponent<Player>();
        playerTrail = GetComponent<PlayerTrail>();
        SR = GetComponent<SpriteRenderer>();
        PT = GameManager.Instance.playerTypeManager;
        if (baseSkillData.GetType() == typeof(SO_DashData))
        {
            skillData = (SO_DashData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }

    private void Update()
    {
        // 쿨타임 UI 업데이트
        float cooldown = skillData.dashCool[itemLevel] * GameManager.Instance.SM.GetFinalValue("CoolReduce");
        float remaining = Mathf.Max(0, (lastPlayerDashStartTime + cooldown) - Time.time);
        dashLvlTxt.text="LV"+(1+itemLevel).ToString();
        dashCoolImg.fillAmount = 1.0f-(remaining / cooldown);
        dashCoolTxt.text = remaining > 0 ? remaining.ToString("F1") : "RM";
        dashCoolImg.color = remaining > 0 ? new Color(1, 1, 1, 0.42f) : Color.white;

        if (isPlayerDashing)
        {
            if (PT.BT.typeCode == 2)
            {
                player.SetVelocity(30f, player.direction);
            }
            if (Time.time > lastPlayerDashStartTime + skillData.dashTime)
            {
                isPlayerDashing = false;
                playerTrail.makeGhost = false;
                SetColor(playerColor);
                gameObject.layer = 7;
                if (PT.BT.typeCode == 3)
                {
                    GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.5f, duration: 0.5f);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && CanDash())
            {
                isPlayerDashing = true;
                lastPlayerDashStartTime = Time.time;
                playerTrail.ghostDestroyTime = 0.5f;
                if (PT.BT.typeCode == 2)
                {
                    StartCoroutine(PlayerBlink());
                }
                else
                {
                    PlayerDash();
                }
            }
        }
    }

    private bool CanDash()
    {
        return Time.time > lastPlayerDashStartTime + skillData.dashCool[itemLevel]*GameManager.Instance.SM.GetFinalValue("CoolReduce")&& player.direction != Vector2.zero && player.canDash;
    }
    private void SetColor(Color color)
    {
        SR.material.color = color;
    }
    private void PlayerDash()
    {
        SetColor(dashColor);
        playerTrail.makeGhost = true;
        player.SetVelocity(skillData.dashSpeed[itemLevel], player.direction);
        gameObject.layer = 9;
    }
    private IEnumerator PlayerBlink()
    {
        isPlayerDashing = true;
        gameObject.layer = 9;
        player.SetVelocity(skillData.lightDashSpeed[itemLevel], player.direction);
        Instantiate(teleportParticle,transform.position,Quaternion.identity);
        yield return new WaitForFixedUpdate();
        gameObject.layer = 7;
        isPlayerDashing = false;
    }
}
