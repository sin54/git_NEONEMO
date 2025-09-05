using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

namespace UI.LevelUp
{
    /// <summary>
    /// 레벨업 및 스킬·어빌리티 업그레이드 UI 흐름을 관리합니다.
    /// 레벨업 패널 → 스킬/어빌리티 선택 → 최종 업그레이드 패널 순으로 
    /// 화면 전환과 입력 처리, 선택 결과 반영, TimeScale 제어를 수행합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class LevelManager : MonoBehaviour
    {
        #region Fields

        /// <summary>업그레이드 옵션 클릭 대기 가능 여부</summary>
        private bool canClick = false;

        /// <summary>현재 레벨업 프로세스 중인지 여부</summary>
        public bool isUpgrading { get; private set; }

        [Header("Panels")]
        [SerializeField] private GameObject totalPanel;           // 전체 레벨업 UI
        [SerializeField] private GameObject levelUpPanel;         // 레벨업 선택 패널
        [SerializeField] private GameObject skillUpgradePanel;    // 최종 스킬/어빌리티 업그레이드 패널

        [Tooltip("레벨업 옵션 표시용 패널(3개)")]
        [SerializeField] private GameObject[] panels = new GameObject[3];
        [Tooltip("최종 업그레이드 옵션 표시용 패널(3개)")]
        [SerializeField] private GameObject[] upgradePanels = new GameObject[3];

        [Header("Skill & Ability Pools")]
        [Tooltip("기본 스킬 풀")]
        [SerializeField] private BaseSkill[] skills;
        [Tooltip("불 속성 스킬 풀")]
        [SerializeField] private BaseSkill[] fireSkills;
        [Tooltip("바람 속성 스킬 풀")]
        [SerializeField] private BaseSkill[] windSkills;
        [Tooltip("빛 속성 스킬 풀")]
        [SerializeField] private BaseSkill[] lightSkills;
        [Tooltip("합본 스킬 풀(초기 스킬)")]
        [SerializeField] private BaseSkill[] totalSkills;
        [Tooltip("어빌리티 풀")]
        [SerializeField] private BaseType[] typeClasses;

        /// <summary>최종 레벨 업그레이드 데이터(3개)</summary>
        private MaxLevelData[] maxLevelDatas = new MaxLevelData[3];

        /// <summary>현재 화면에 뿌릴 스킬 옵션 배열</summary>
        private BaseSkill[] selectedSkills;
        /// <summary>현재 화면에 뿌릴 어빌리티 옵션 배열</summary>
        private BaseType[] selectedAbilities;
        /// <summary>각 패널이 스킬(true) 또는 어빌리티(false) 선택인지</summary>
        private bool[] isSkill = new bool[3];

        [Header("Configuration")]
        [Tooltip("레벨업 옵션 중 현재 선택된 인덱스")]
        [SerializeField] private int selectedNum = -1;
        [Tooltip("최종 업그레이드 옵션 중 선택된 인덱스")]
        [SerializeField] private int maxSkillSelectedNum = -1;
        [Tooltip("어빌리티 확률(0~1)")]
        [SerializeField] private float abilityPercent;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            // 처음엔 모두 비활성화
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
                            if (selectedAbilities[selectedNum].typePassiveLevel<0)
                            {
                                selectedAbilities[selectedNum].gameObject.SetActive(true);
                            }
                            selectedAbilities[selectedNum].GetComponent<IUpgradable>().Upgrade();

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

        #endregion

        #region Public Methods
        /// <summary>
        /// 레벨업 프로세스를 시작합니다.
        /// 화면 전환, 랜덤 옵션 생성, TimeScale 정지를 처리합니다.
        /// </summary>
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
            selectedAbilities = GetRandomAbillity(3);
            for (int i = 0; i < selectedSkills.Length; i++) {
                if (UtilClass.GetPercent(abilityPercent))
                {
                    panels[i].GetComponent<hoveringUI>().SetPanel(selectedAbilities[i]);
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

        /// <summary>불 속성 스킬을 풀에 추가합니다.</summary>
        public void AddFireSkill() 
        {
            skills = skills.Concat(fireSkills).ToArray();
        }
        /// <summary>바람 속성 스킬을 풀에 추가합니다.</summary>
        public void AddWindSkill()
        {
            skills = skills.Concat(windSkills).ToArray();
        }
        /// <summary>빛 속성 스킬을 풀에 추가합니다.</summary>
        public void AddLightSkill()
        {
            skills = skills.Concat(lightSkills).ToArray();
        }

        #endregion

        // 아직 이후 내용 region 블록처리 X

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
                    selectedAbilities[selectedNum].GetComponent<IUpgradable>().Upgrade();
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
}