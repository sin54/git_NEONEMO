using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

public class LevelManager : MonoBehaviour
{
    private bool canClick = false;
    public bool isUpgrading {  get; private set; }

    [SerializeField] private GameObject totalPanel;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private GameObject skillUpgradePanel;
    [SerializeField] private GameObject[] panels = new GameObject[3];
    [SerializeField] private GameObject[] upgradePanels=new GameObject[3];
    [SerializeField] private BaseSkill[] skills;
    [SerializeField] private BaseSkill[] fireSkills;
    [SerializeField] private BaseSkill[] windSkills;
    [SerializeField] private BaseSkill[] lightSkills;
    [SerializeField] private BaseSkill[] totalSkills;
    [SerializeField] private BaseType[] typeClasses;
    private MaxLevelData[] maxLevelDatas=new MaxLevelData[3];
    private BaseSkill[] selectedSkills;
    private BaseType[] selectedAbillities;
    private bool[] isSkill=new bool[3];
    [SerializeField]private int selectedNum;
    [SerializeField]private int maxSkillSelectedNum;
    [SerializeField] private float abillityPercent;
    private bool isSkillSelected;
    private void Start()
    {
        totalPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        skillUpgradePanel.SetActive(false);
        skills = skills.Concat(totalSkills).ToArray();
        foreach (GameObject panel in upgradePanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        selectedNum = -1;
        maxSkillSelectedNum = -1;
        for(int i = 0; i < totalSkills.Length; i++)
        {
            GameManager.Instance.UIM.AddSkill(totalSkills[i]);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&selectedNum==-1)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].GetComponent<hoveringUI>().isHovering)
                {
                    selectedNum = i;
                    break;
                }
            }
            if (selectedNum >= 0)
            {
                if (isSkill[selectedNum])
                {
                    if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
                    {
                        //마지막 업그레이드라면
                    }
                    else
                    {
                        if (selectedSkills[selectedNum].itemLevel < 0)
                        {
                            selectedSkills[selectedNum].gameObject.SetActive(true);
                            GameManager.Instance.UIM.AddSkill(selectedSkills[selectedNum]);
                        }
                        selectedSkills[selectedNum].GetComponent<IUpgradable>().Upgrade();
                    }
                }
                else
                {
                    if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
                    {

                    }
                    else
                    {
                        if (selectedAbillities[selectedNum].typePassiveLevel<0)
                        {
                            selectedAbillities[selectedNum].gameObject.SetActive(true);
                        }
                        selectedAbillities[selectedNum].GetComponent<IUpgradable>().Upgrade();

                    }

                }


                StartCoroutine(DisappearPanel());
            }
        }

        if (Input.GetMouseButtonDown(0) && maxSkillSelectedNum == -1 && canClick)
        {
            for (int i = 0; i < 3; i++)
            {
                if (upgradePanels[i].GetComponent<hoveringUI>().isHovering)
                {
                    maxSkillSelectedNum = i;
                    break;
                }
            }

            if (maxSkillSelectedNum >= 0)
            {
                canClick = false; // 입력 방지
                selectedSkills[selectedNum].reinforcedNum = maxSkillSelectedNum + 1;
                selectedSkills[selectedNum].GetComponent<IUpgradable>().Upgrade();
                StartCoroutine(DisappearUpgradePanel());
            }
        }
    }
    public void LevelUp()
    {
        Debug.Log("PLUSED");
        TimeScaleManager.Instance.TimeStopStackPlus();
        isUpgrading = true;
        totalPanel.SetActive(true);
        levelUpPanel.SetActive(true);
        skillUpgradePanel.SetActive(false);
        selectedNum = -1;
        maxSkillSelectedNum = -1;
        selectedSkills = GetRandomSkills(3);
        selectedAbillities = GetRandomAbillity(3);
        for (int i = 0; i < selectedSkills.Length; i++) {
            if (UtilClass.GetPercent(abillityPercent))
            {
                panels[i].GetComponent<hoveringUI>().SetPanel(selectedAbillities[i]);
                isSkill[i] = false;
            }
            else
            {
                panels[i].GetComponent<hoveringUI>().SetPanel(selectedSkills[i]);
                isSkill[i] = true;
            }
        }
        StartCoroutine(ShowPanel());
    }

    private IEnumerator ShowPanel()
    {
        panels[0].SetActive(true);
        yield return new WaitForSecondsRealtime(0.22f);
        panels[2].SetActive(true);
        yield return new WaitForSecondsRealtime(0.22f);
        panels[1].SetActive(true);
    }

    private IEnumerator ShowUpgradedPanel()
    {
        upgradePanels[0].SetActive(true);
        yield return new WaitForSecondsRealtime(0.22f);
        upgradePanels[2].SetActive(true);
        yield return new WaitForSecondsRealtime(0.22f);
        upgradePanels[1].SetActive(true);

        yield return new WaitForSecondsRealtime(0.1f); // 추가 지연 (optional)
        canClick = true;
    }

    private IEnumerator DisappearPanel()
    {
        if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
        {

            if (isSkill[selectedNum])
            {
                foreach (GameObject panel in panels)
                {
                    panel.SetActive(false);
                }
                levelUpPanel.SetActive(false);
                maxLevelDatas = selectedSkills[selectedNum].baseSkillData.levelDatas;
                skillUpgradePanel.SetActive(true);

                for (int i = 0; i < 3; i++)
                {
                    upgradePanels[i].GetComponent<hoveringUI>().SetPanel(maxLevelDatas[i]);
                }
                StartCoroutine(ShowUpgradedPanel());
            }
            else
            {
                selectedAbillities[selectedNum].GetComponent<IUpgradable>().Upgrade();
                panels[selectedNum].GetComponent<Animator>().SetTrigger("Disappear");
                yield return new WaitForSecondsRealtime(0.4f);
                levelUpPanel.SetActive(false);
                totalPanel.SetActive(false);

                EndUpgrade();
            }

        }
        else
        {
            panels[selectedNum].GetComponent<Animator>().SetTrigger("Disappear");
            yield return new WaitForSecondsRealtime(0.4f);
            levelUpPanel.SetActive(false);
            totalPanel.SetActive(false);

            EndUpgrade();
        }
    }
    private IEnumerator DisappearUpgradePanel()
    {
        upgradePanels[maxSkillSelectedNum].GetComponent<Animator>().SetTrigger("Disappear");
        RemoveSkill(selectedSkills[selectedNum]);
        yield return new WaitForSecondsRealtime(0.4f);
        skillUpgradePanel.SetActive(false);
        levelUpPanel.SetActive(false);
        totalPanel.SetActive(false);

        foreach (GameObject panel in upgradePanels)
        {
            panel.SetActive(false);
        }

        canClick = false; // 다시 막아줌
        EndUpgrade();
    }
    private BaseSkill[] GetRandomSkills(int count)
    {
        var availableSkills = skills
            .Where(skill => skill != null && !skill.isMaxLevel) // null 체크도 함께!
            .OrderBy(x => Random.value)
            .Take(count)
            .ToArray();
        return availableSkills;
    }
    private BaseType[] GetRandomAbillity(int count)
    {
        var availableSkills = typeClasses
            .Where(skill => skill != null) // null 체크도 함께!
            .OrderBy(x => Random.value)
            .Take(count)
            .ToArray();
        return availableSkills;
    }
    public void RemoveSkill(BaseSkill skillToRemove)
    {
        skills = skills.Where(skill => skill != skillToRemove).ToArray();
    }

    public void RemoveAbility(BaseType typeToRemove)
    {

        typeClasses=typeClasses.Where(type=>type!=typeToRemove).ToArray();
        Debug.Log("removed");
    }

    public void AddFireSkill()
    {
        skills = skills.Concat(fireSkills).ToArray();
    }
    public void AddWindSkill()
    {
        skills = skills.Concat(windSkills).ToArray();
    }
    public void AddLightSkill()
    {
        skills = skills.Concat(lightSkills).ToArray();
    }

    private void EndUpgrade()
    {
        if (!isUpgrading) return;
        isUpgrading = false;
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        GameManager.Instance.UIM.UpdateAll();
        TimeScaleManager.Instance.TimeStopStackMinus();
    }
}
