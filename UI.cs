using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI : _Singleton<UI>
{
    public static GameObject goGlobal;
    public static GameObject goBattle;
    public static GameObject goFieldMap;
    public static GameObject goMainMenu;

    public static GameObject goEvent;
    public static GameObject goItems;
    public static GameObject goOpenSettings;
    public static GameObject goGold;
    public static GameObject goOpenStatus;
    public static GameObject goMap;
    public static GameObject goOpenShop;
    public static GameObject goStatus;
    public static GameObject goShop;
    public static GameObject goTreasureChest;

    public static GameObject goCommand;
    public static GameObject goHeroes;
    public static GameObject goUnit;
    public static GameObject goUnitStats;
    public static GameObject goUnitSkills;
    public static GameObject goUnitEquips;
    public static GameObject goSuggest;
    public static GameObject goTooltip;
    public static GameObject goMiniTooltip;
    public static GameObject goInputProof;

    public static Image[] imItems;
    public static _Clickable[] clItems;
    public static TextMeshProUGUI[] txItemsStack;

    public static Image[] imHeroes;
    public static Image[] imHeroesFrame;
    public static Image[] imHeroesHp;
    public static GameObject[] goHeroesMove;
    public static GameObject[] goHeroesAttack;

    public static GameObject goHpBar;
    public static GameObject goExpBar;
    public static GameObject goLevel;

    public static GameObject[] goSkills = new GameObject[4];
    public static _Clickable[] clSkills = new _Clickable[4];
    public static _Clickable[] clEquips = new _Clickable[4];
    public static Image imHpSlider;
    public static Image imIconFace;
    public static Image imPassive;
    public static Image[] imSkillsIcon = new Image[4];
    public static Image[] imSkillsFocus = new Image[4];
    public static Image[] imSkillsCooldownOverlay = new Image[4];
    public static Image[] imEquipsButton = new Image[4];
    public static Image[] imEquipsIcon = new Image[4];
    public static List<Image> imStatusIconList = new List<Image>();
    public static TextMeshProUGUI txHpSlider;
    public static TextMeshProUGUI txLevelText;
    public static TextMeshProUGUI[] imSkillsCooldownCount = new TextMeshProUGUI[4];
    public static TextMeshProUGUI[] txStats;
    public static List<TextMeshProUGUI> txStatusCount = new List<TextMeshProUGUI>();
    public static Slider slHpSlider;
    public static Slider slExpSlider;
    public static Slider slSoundVolume;
    public static Slider slGameSpeed;

    public static TextMeshProUGUI txTooltipTitle;
    public static TextMeshProUGUI txTooltipEffect;
    public static TextMeshProUGUI txMiniTooltipTitle;
    public static TextMeshProUGUI txMiniTooltipEffect;
    public static ContentSizeFitter csfTooltipEffect;
    public static ContentSizeFitter csfMiniTooltipEffect;
    //public static _Tooltip _effectTextInfoOnTooltip;

    public static GameObject goChapter;
    public static Graphic[] grChapterArray;
    public static TextMeshProUGUI txChapter;

    public static GameObject goBossWarning;
    public static Graphic[] grBossWarningArray;

    public static GameObject goGameOver;
    public static Graphic[] graphicsGameOver;

    public static GameObject goWinBattle;
    public static Graphic[] graphicsWinBattle;

    public static TextMeshProUGUI txSuggest;

    public static Image imTransition;
    
    protected override void Awake()
    {
        base.Awake();

        goGlobal = transform.Find("Global").gameObject;
        goBattle = transform.Find("Battle").gameObject;
        goFieldMap = transform.Find("FieldMap").gameObject;
        goMainMenu = transform.Find("MainMenu").gameObject;

        goOpenSettings = goGlobal.transform.Find("OpenSettings").gameObject;
        goEvent = goGlobal.Find("Event").gameObject;
        goGold = goGlobal.transform.Find("Gold").gameObject;
        goOpenStatus = goGlobal.transform.Find("OpenStatus").gameObject;
        goMap = goGlobal.transform.Find("Map").gameObject;
        goOpenShop = goGlobal.transform.Find("OpenShop").gameObject;
        goItems = goGlobal.transform.Find("Items").gameObject;
        goStatus = goGlobal.transform.Find("Status").gameObject;
        goShop = goGlobal.transform.Find("Shop").gameObject;
        goHeroes = goGlobal.transform.Find("Heroes").gameObject;

        goTreasureChest = goBattle.transform.Find("TreasureChest").gameObject;
        goCommand = goBattle.transform.Find("Command").gameObject;
        goUnit = goBattle.transform.Find("Unit").gameObject;
        goUnitStats = goUnit.transform.Find("UnitStats").gameObject;
        goUnitSkills = goUnit.transform.Find("Skills").gameObject;
        goUnitEquips = goUnit.transform.Find("Equips").gameObject;
        goTooltip = transform.Find("Tooltip").gameObject;
        goMiniTooltip = transform.Find("MiniTooltip").gameObject;
        goSuggest = transform.Find("Suggest").gameObject;
        goInputProof = transform.Find("InputProof").gameObject;
        goChapter = goBattle.transform.Find("Chapter").gameObject;
        goBossWarning = goBattle.transform.Find("BossWarning").gameObject;
        goGameOver = goBattle.transform.Find("GameOver").gameObject;
        goWinBattle = goBattle.transform.Find("WinBattle").gameObject;

        imItems = new Image[goItems.transform.childCount - 1];
        clItems = new _Clickable[goItems.transform.childCount - 1];
        txItemsStack = new TextMeshProUGUI[goItems.transform.childCount];
        for (int i_ = 0; i_ < imItems.Length; i_++)
        {
            string index_ = i_.ToString("D2");
            imItems[i_] = goItems.transform.Find("Item" + index_).Find("IconImage").GetComponent<Image>();
            clItems[i_] = goItems.transform.Find("Item" + index_).GetComponent<_Clickable>();
            txItemsStack[i_] = goItems.transform.Find("Item" + index_).Find("Stack").GetComponent<TextMeshProUGUI>();
        }

        imHeroes = new Image[goHeroes.transform.childCount];
        imHeroesFrame = new Image[goHeroes.transform.childCount];
        imHeroesHp = new Image[goHeroes.transform.childCount];
        goHeroesMove = new GameObject[goHeroes.transform.childCount];
        goHeroesAttack = new GameObject[goHeroes.transform.childCount];
        for (int i_ = 0; i_ < imHeroes.Length; i_++)
        {
            string str_i_ = i_.ToString("D2");
            imHeroes[i_] = goHeroes.transform.Find("Hero" + str_i_).Find("FaceIcon").GetComponent<Image>();
            imHeroesFrame[i_] = goHeroes.transform.Find("Hero" + str_i_).Find("Frame").GetComponent<Image>();
            imHeroesHp[i_] = goHeroes.transform.Find("Hero" + str_i_).Find("HP").GetComponent<Image>();
            //goHeroesMove[i_] = goHeroes.transform.Find("Hero" + str_i_).Find("HeroMove" + str_i_).gameObject;
            //goHeroesAttack[i_] = goHeroes.transform.Find("Hero" + str_i_).Find("HeroAttack" + str_i_).gameObject;
        }

        goHpBar = goUnitStats.transform.Find("HpBar").gameObject;
        goExpBar = goUnitStats.transform.Find("ExpBar").gameObject;
        goLevel = goUnitStats.transform.Find("Level").gameObject;
        imHpSlider = goHpBar.transform.Find("HpImageFiller").GetComponent<Image>();
        imIconFace = goUnitStats.transform.Find("Icon").Find("IconFace").GetComponent<Image>();
        imPassive = goUnitSkills.transform.Find("Passive").Find("Icon").GetComponent<Image>();
        goSkills = new GameObject[] { goUnitSkills.transform.Find("Skill00").gameObject,
                                      goUnitSkills.transform.Find("Skill01").gameObject,
                                      goUnitSkills.transform.Find("Skill02").gameObject,
                                      goUnitSkills.transform.Find("Skill03").gameObject };
        for (int i_ = 0; i_ < goSkills.Length; i_++)
        {
            imSkillsIcon[i_] = goSkills[i_].transform.Find("Icon").GetComponent<Image>();
            //imSkillsFocus[i_] = goSkills[i_].transform.Find("Focus").GetComponent<Image>();
            imSkillsCooldownOverlay[i_] = goSkills[i_].transform.Find("CooldownOverlay").GetComponent<Image>();
            imSkillsCooldownCount[i_] = goSkills[i_].transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>();
            clSkills[i_] = goSkills[i_].GetComponent<_Clickable>();
        }

        clEquips = goUnitEquips.GetComponentsInChildren<_Clickable>();
        for (int i_ = 0; i_ < clEquips.Length; i_++)
        {
            imEquipsButton[i_] = clEquips[i_].gameObject.GetComponent<Image>();
            imEquipsIcon[i_] = clEquips[i_].transform.Find("IconImage").GetComponent<Image>();
        }

        txStats = goUnitStats.transform.Find("Stats").GetComponentsInChildren<TextMeshProUGUI>();
        txHpSlider = goHpBar.transform.Find("HpText").GetComponent<TextMeshProUGUI>();
        txLevelText = goLevel.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        slHpSlider = goHpBar.transform.Find("HpImageBack").GetComponent<Slider>();
        slExpSlider = goExpBar.transform.Find("ExpImageBack").GetComponent<Slider>();


        txTooltipTitle = goTooltip.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        txTooltipEffect = goTooltip.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        txMiniTooltipTitle = goMiniTooltip.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        txMiniTooltipEffect = goMiniTooltip.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        csfTooltipEffect = goTooltip.transform.Find("Description").GetComponent<ContentSizeFitter>();
        csfMiniTooltipEffect = goMiniTooltip.transform.Find("Description").GetComponent<ContentSizeFitter>();


        slSoundVolume = goGlobal.transform.Find("SoundVolume").GetComponent<Slider>();
        slGameSpeed = goGlobal.transform.Find("GameSpeed").GetComponent<Slider>();


        txSuggest = goSuggest.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        imTransition = transform.Find("Transition").GetComponent<Image>();


        grChapterArray = goChapter.transform.GetComponentsInChildren<Graphic>(true);
        txChapter = goChapter.transform.Find("Text").GetComponent<TextMeshProUGUI>();


        grBossWarningArray = goBossWarning.transform.GetComponentsInChildren<Graphic>(true);
        

        graphicsGameOver = goGameOver.GetComponentsInChildrenWithoutSelf<Graphic>(true);
        

        graphicsWinBattle = goWinBattle.GetComponentsInChildrenWithoutSelf<Graphic>(true);

        goEvent.SetActive(false);
        goShop.SetActive(false);
        goStatus.SetActive(false);

        goUnit.SetActive(false);
        goSuggest.SetActive(true);
        goTooltip.SetActive(false);
        goMiniTooltip.SetActive(false);
        goWinBattle.SetActive(false);
        goChapter.SetActive(false);
        goGameOver.SetActive(false);
        goTreasureChest.SetActive(false);
        goWinBattle.Find("Caution").gameObject.SetActive(false);
        goGlobal.Find("ComfirmDiscard").gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Globals._clOnActive is _Clickable && Globals._clOnActive.IsHaveContents() && Globals._clOnActive._isDragging == false)
        {
            Globals.timeCountToShowTooltip = (Globals.timeCountToShowTooltip + Time.deltaTime).Clamp(0, 2);
        }
        else
        {
            Globals.timeCountToShowTooltip = (Globals.timeCountToShowTooltip - Time.deltaTime * 5).Clamp(0, 2);
        }

        if (Globals.timeCountToShowTooltip > 1f && goTooltip.activeSelf == false)
        {
            goTooltip.SetActive(true);
            ConfigureTooltip(Globals._clOnActive);
        }
        else if (Globals.timeCountToShowTooltip < 1f && goTooltip.activeSelf == true)
        {
            goTooltip.SetActive(false);
        }
        if (Globals.timeCountToShowTooltip > 1f && goMiniTooltip.activeSelf == false)
        {
            goMiniTooltip.SetActive(true);
            ConfigureMiniTooltip(Globals._clOnActive);
        }
        else if (Globals.timeCountToShowTooltip < 1f && goMiniTooltip.activeSelf == true)
        {
            goMiniTooltip.SetActive(false);
        }
    }

    public static void ConfigureFieldMapUI()
    {
        string text_ = "<mspace=20>" + Globals.Instance.Food + "/" + Globals.Instance.FoodMax;
        goFieldMap.Find("Gold").Find("Text").GetComponent<TextMeshProUGUI>().text = text_;

        goFieldMap.Find("Gold").Find("Text").GetComponent<TextMeshProUGUI>().color =  (Globals.Instance.Food > 0) ? Color.white : Color.red;
    }

    public static void ConfigureNotification()
    {
        bool isShowNotification_OpenMap_ = false;

        if ((Globals.Instance.equipsToCombine[0] != null && Globals.Instance.equipsToCombine[0]._name.IsNullOrEmpty() == false) ||
            (Globals.Instance.equipsToCombine[1] != null && Globals.Instance.equipsToCombine[1]._name.IsNullOrEmpty() == false))
            isShowNotification_OpenMap_ = true;

        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            _Unit unit_i_ = Globals.heroList[i];

            if (unit_i_._CalclateSkillPointRemaining().IsContains(true))
                isShowNotification_OpenMap_ = true;

            foreach (_Skill skill_i_ in unit_i_._parameter._skillTree)
            {
                if (skill_i_._parameter._isActive && skill_i_._CalculateAbilityPointRemaining(unit_i_) > 0)
                    isShowNotification_OpenMap_ = true;
            }
        }

        goOpenStatus.Find("Notification").gameObject.SetActive(isShowNotification_OpenMap_);
        goMap.Find("Notification").gameObject.SetActive(Globals.Instance.sceneState == "CheckTheMap");
    }

    public static void ConfigureStatusUI()
    {
        goStatus.Find("Dimmed").gameObject.SetActive(Globals.Instance.sceneState == "Battle" || Globals.Instance.sceneState == "CheckTheMap");

        if ((Globals.Instance.equipsToCombine[0] == null || Globals.Instance.equipsToCombine[0]._name.IsNullOrEmpty()) &&
            (Globals.Instance.equipsToCombine[1] == null || Globals.Instance.equipsToCombine[1]._name.IsNullOrEmpty()))
        {
            goStatus.Find("Tabs").Find("StatusTab04").Find("Notification").gameObject.SetActive(false);
        }

        foreach (_Hero hero_i_ in Globals.heroList)
        {
            hero_i_._CopyStatusFromOriginal();
            hero_i_._ApplyLevel();
            hero_i_._ApplyEquips();
            //hero_i_._Skills_InitializeAndApplyAbilities();
            hero_i_._ApplyPassive_Static();
        }

        ConfigureStatusUI_All();

        for (int i = 1; i < 4; i++)
        {
            ConfigureStatusUI_IconAndStats(i);
            ConfigureStatusUI_Equips(i);
            ConfigureStatusUI_SkillTree(i);
        }
        ConfigureStatusUI_Combine();
        ConfigureStatusUI_Inventory();
        ConfigureStatusUI_Tabs();
        ConfigureNotification();

        void ConfigureStatusUI_All()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject go__i_ = goStatus.Find("ScrollView").Find("Contents").Find("Content00").Find("Hero" + i.ToString("D2")).gameObject;
                _Unit._Parameter p__ = Globals.heroList[i]._parameter;

                go__i_.Find("IconAndStats").Find("HeroIcon").Find("Image").GetComponent<Image>().sprite = Globals.heroList[i]._Sprite;
                
                string text__ = "<mspace=12>" + p__._hpMax.ToString().PadLeft(4) + p__._sp.ToString().PadLeft(7) + "\n" + p__._ad.ToString().PadLeft(4) +
                                p__._ar.ToString().PadLeft(7) + "\n" + p__._md.ToString().PadLeft(4) + p__._mr.ToString().PadLeft(7);
                go__i_.Find("IconAndStats").Find("HeroStatsValues").GetComponent<TextMeshProUGUI>().text = text__;
                go__i_.Find("IconAndStats").Find("HeroLevel").Find("LevelCount").GetComponent<TextMeshProUGUI>().text = p__._lv.ToString();
                go__i_.Find("IconAndStats").Find("HeroLevel").Find("ExpCount").GetComponent<TextMeshProUGUI>().text = p__._expFraction + " / " + Table.ExpTable.GetValue(p__._class)[p__._lv];
                go__i_.Find("IconAndStats").Find("HeroLevel").Find("ExpBarFiller").GetComponent<Image>().fillAmount = (float)p__._expFraction / Table.ExpTable.GetValue(p__._class)[p__._lv];

                for (int j = 0; j < p__._equips.Length; j++)
                {
                    GameObject go__i_j_ = go__i_.Find("Equips").Find("StatusAll_Equip" + j.ToString("D2")).gameObject;
                    _Clickable cl__i_j_ = go__i_j_.GetComponent<_Clickable>();
                    _Equip equip__i_j_ = p__._equips[j];
                    bool isEquipExist_ = (equip__i_j_ != null) && (equip__i_j_ != default) && (equip__i_j_._name.IsNullOrEmpty() == false);

                    cl__i_j_._tooltip = (isEquipExist_) ? equip__i_j_._tooltip.DeepCopy() : new _Tooltip();
                    cl__i_j_._tooltip._placeType = "Side";
                    cl__i_j_._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip__i_j_) : "";
                    go__i_j_.transform.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(p__._equips[j]._pathIcon) : Prefabs.Instance.spTransparent;
                }

                _Skill[] skills__ = new _Skill[4] { (p__._classPassives.Count > 0) ? p__._classPassives[0] : null , p__._skills[1], p__._skills[2], p__._skills[3], };
                for (int j = 0; j < p__._skills.Length; j++)
                {
                    GameObject go__i_j_ = go__i_.Find("PassiveAndSkills").Find("StatusAll_Skill" + j.ToString("D2")).gameObject;
                    _Clickable cl__i_j_ = go__i_j_.GetComponent<_Clickable>();
                    _Skill skill__i_j_ = skills__[j];
                    bool isSkillExist_ = (skill__i_j_ != null) && (skill__i_j_ != default) && (skill__i_j_._parameter._name.IsNullOrEmpty() == false);

                    cl__i_j_._tooltip = (isSkillExist_) ? skill__i_j_._tooltip.DeepCopy() : new _Tooltip();
                    cl__i_j_._tooltip._placeType = "Side";
                    cl__i_j_._tooltip._effectText = (isSkillExist_) ? _Tooltip.MakeTextFromSkill(skill__i_j_) : "";
                    go__i_j_.transform.Find("IconImage").GetComponent<Image>().sprite = (Resources.Load<Sprite>(skill__i_j_._parameter._pathIcon) is Sprite out_) ? out_ : Prefabs.Instance.spTransparent;
                    go__i_j_.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = (isSkillExist_) ? skill__i_j_._parameter._descriptiveName : "";
                }
            }
        }

        void ConfigureStatusUI_IconAndStats(int index__)
        {
            GameObject go__ = goStatus.transform.Find("ScrollView").Find("Contents").Find("Content" + index__.ToString("D2")).Find("IconAndStats").gameObject;
            _Unit unit__ = Globals.heroList[index__ - 1];
            _Unit._Parameter p__ = unit__._parameter;

            go__.Find("HeroIcon").Find("Image").GetComponent<Image>().sprite = unit__._Sprite;

            string text__ = "<mspace=15>" + p__._hpMax.ToString().PadLeft(4) + p__._sp.ToString().PadLeft(7) + "\n" + p__._ad.ToString().PadLeft(4) +
                            p__._ar.ToString().PadLeft(7) + "\n" + p__._md.ToString().PadLeft(4) + p__._mr.ToString().PadLeft(7);
            go__.Find("HeroStatsValues").GetComponent<TextMeshProUGUI>().text = text__;

            go__.Find("HeroLevel").Find("LevelCount").GetComponent<TextMeshProUGUI>().text = p__._lv.ToString();
            go__.Find("HeroLevel").Find("ExpCount").GetComponent<TextMeshProUGUI>().text = p__._expFraction + " / " + Table.ExpTable.GetValue(p__._class)[p__._lv];
            go__.Find("HeroLevel").Find("ExpBarFiller").GetComponent<Image>().fillAmount = (float)p__._expFraction / Table.ExpTable.GetValue(p__._class)[p__._lv];
        }

        void ConfigureStatusUI_Equips(int index__)
        {
            GameObject go__ = goStatus.transform.Find("ScrollView").Find("Contents").Find("Content" + index__.ToString("D2")).Find("Equips").gameObject;
            _Unit unit_i_ = Globals.heroList[index__ - 1];
            for (int j = 0; j < unit_i_._parameter._equips.Length; j++)
            {
                GameObject go__j_ = go__.transform.Find("StatusEquip" + j.ToString("D2")).gameObject;
                _Clickable cl__ = go__j_.GetComponent<_Clickable>();
                _Equip equip__j_ = unit_i_._parameter._equips[j];
                bool isEquipExist_ = (equip__j_ != null) && (equip__j_ != default) && (equip__j_._name.IsNullOrEmpty() == false);
                
                cl__._tooltip = (isEquipExist_) ? equip__j_._tooltip.DeepCopy() : new _Tooltip();
                cl__._tooltip._placeType = "StatusEquip";
                cl__._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip__j_) : "";
                go__j_.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? equip__j_._descriptiveName : "";
                go__j_.transform.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip__j_._pathIcon) : Prefabs.Instance.spTransparent;
                go__j_.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? MakeDescriptionText_StatusEquip(equip__j_, 12) : "";
            }
        }

        void ConfigureStatusUI_SkillTree(int index__)
        {
            GameObject go__ = goStatus.transform.Find("ScrollView").Find("Contents").Find("Content" + index__.ToString("D2")).Find("SkillTree").gameObject;
            _Unit unit__i_ = Globals.heroList[index__ - 1];
            _Skill[] skillTree_i_ = unit__i_._parameter._skillTree;
            //_Unit._Parameter._SkillTree skillTree__i_ = unit__i_._parameter._skillTrees;
            int unitRank_i_ = (unit__i_._parameter._lv + 2) / 3;
            bool[] isSkillInteractable_ = unit__i_._CalclateSkillPointRemaining(); 

            //go__.Find("Lock_Lv.4").gameObject.SetActive(unitRank_i_ < 2);
            //go__.Find("Lock_Lv.7").gameObject.SetActive(unitRank_i_ < 3);
            go__.Find("SkillTree_Undo").GetComponent<Button>().interactable = (skillTree_i_.IsEqual(unit__i_._parameter._skillTreeSave) == false);

            for (int j = 0; j < skillTree_i_.Length; j++)
            {
                GameObject go__j_ = go__.Find("StatusSkillTree" + j.ToString("D2")).gameObject;
                _Clickable cl__ = go__j_.Find("IconImage").GetComponent<_Clickable>();
                _Skill skill__j_ = skillTree_i_[j];
                int skillRank_j_ = (j + 3) / 2;
                int abilityPointRemaining_ = skill__j_._CalculateAbilityPointRemaining(unit__i_);
                bool isSkillActive_ = skill__j_._parameter._isActive;

                cl__._tooltip = skill__j_._tooltip.DeepCopy();
                cl__._tooltip._placeType = "Side";
                cl__._tooltip._effectText = _Tooltip.MakeTextFromSkill(skill__j_);
                go__j_.Find("IconImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(skill__j_._parameter._pathIcon);
                go__j_.Find("SkillTree_Dim").gameObject.SetActive(/*skillRank_j_ > unitRank_i_ && */isSkillActive_ == false);
                go__j_.Find("Ability_Token00").gameObject.SetActive(unit__i_._parameter._lv > skillRank_j_ * 3 - 2 && isSkillActive_);
                go__j_.Find("Ability_Token01").gameObject.SetActive(unit__i_._parameter._lv > skillRank_j_ * 3 - 1 && isSkillActive_);
                go__j_.Find("Ability_Token00").Find("Ability_TokenIsActive").gameObject.SetActive(abilityPointRemaining_ > 0 && isSkillActive_);
                go__j_.Find("Ability_Token01").Find("Ability_TokenIsActive").gameObject.SetActive(abilityPointRemaining_ > 1 && isSkillActive_);
                go__j_./*Find("SkillTreeFrame" + j.ToString("D2")).*/GetComponent<Button>().interactable = (isSkillInteractable_[skillRank_j_ - 1]);
                go__j_./*Find("SkillTreeFrame" + j.ToString("D2")).*/GetComponent<Button>().enabled = (isSkillActive_ == false);

                for (int k = 0; k < skill__j_._parameter._skillAbilities.Length; k++)
                {
                    GameObject go__j_k_ = go__j_.Find("Ability" + k.ToString("D2")).gameObject;
                    _SkillAbility ability__k_ = skill__j_._parameter._skillAbilities[k];

                    go__j_k_.Find("Ability_IconImage").GetComponent<Image>().sprite = ability__k_._Sprite;
                    go__j_k_.GetComponent<_Clickable>()._miniTooltip = ability__k_._miniTooltip.DeepCopy();
                    go__j_k_.GetComponent<_Clickable>()._miniTooltip._effectText = _Tooltip.ReplaceTag_SkillAbility(go__j_k_.GetComponent<_Clickable>()._miniTooltip._effectText, ability__k_);
                    go__j_k_.GetComponent<Button>().interactable = (isSkillActive_  && abilityPointRemaining_ > 0);
                    go__j_k_.Find("Ability_Dim").gameObject.SetActive(ability__k_._isActive == false);
                    go__j_k_.Find("Ability_Active").gameObject.SetActive(ability__k_._isActive);
                    go__j_k_.Find("Ability_Frame").GetComponent<Image>().color = (ability__k_._isActive) ? "#00ff00".ToColor32() : "#ffffff".ToColor32();
                }
            }
        }

        void ConfigureStatusUI_Combine()
        {
            GameObject go__ = goStatus.transform.Find("ScrollView").Find("Contents").Find("Content04").gameObject;
            GameObject goBase00__ = go__.Find("BasicEquip00").gameObject;
            GameObject goBase01__ = go__.Find("BasicEquip01").gameObject;
            GameObject goCombined__ = go__.Find("CombinedEquip").gameObject;
            _Equip base00_ = Globals.Instance.equipsToCombine[0];
            _Equip base01_ = Globals.Instance.equipsToCombine[1];
            bool isBase00Exist_ = (base00_ != null && base00_ != default && base00_._name.IsNullOrEmpty() == false);
            bool isBase01Exist_ = (base01_ != null && base01_ != default && base01_._name.IsNullOrEmpty() == false);
            _Equip combined_ = _Equip.CombineEquip(base00_, base01_);
            bool isCombinedExist__ = (combined_ != default);

            goBase00__.GetComponent<_Clickable>()._tooltip = (isBase00Exist_) ? base00_._tooltip.DeepCopy() : new _Tooltip();
            goBase00__.GetComponent<_Clickable>()._tooltip._placeType = "Side";
            goBase00__.GetComponent<_Clickable>()._tooltip._effectText = (isBase00Exist_) ? _Tooltip.MakeTextFromEquip(base00_) : "";
            goBase00__.Find("IconImage").GetComponent<Image>().sprite = (isBase00Exist_) ? Resources.Load<Sprite>(base00_._pathIcon) : Prefabs.Instance.spTransparent;

            goBase01__.GetComponent<_Clickable>()._tooltip = (isBase01Exist_) ? base01_._tooltip.DeepCopy() : new _Tooltip();
            goBase01__.GetComponent<_Clickable>()._tooltip._placeType = "Side";
            goBase01__.GetComponent<_Clickable>()._tooltip._effectText = (isBase01Exist_) ? _Tooltip.MakeTextFromEquip(base01_) : "";
            goBase01__.Find("IconImage").GetComponent<Image>().sprite = (isBase01Exist_) ? Resources.Load<Sprite>(base01_._pathIcon) : Prefabs.Instance.spTransparent;

            goCombined__.Find("Title").GetComponent<TextMeshProUGUI>().text = (isCombinedExist__) ? combined_._descriptiveName : "";
            goCombined__.Find("IconImage").GetComponent<Image>().sprite = (isCombinedExist__) ? Resources.Load<Sprite>(combined_._pathIcon) : Prefabs.Instance.spTransparent;
            goCombined__.Find("Stats").GetComponent<TextMeshProUGUI>().text = (isCombinedExist__) ? MakeDescriptionText_StatusEquip(combined_, 14) : "";
            goCombined__.Find("ScrollView").GetComponentInChildren<TextMeshProUGUI>().text = (isCombinedExist__) ? _Tooltip.RichDescriptionText(_Tooltip.ReplaceTag_Equip(combined_)) : "";
            goCombined__.Find("ScrollView").GetComponentInChildren<ContentSizeFitter>().SetLayoutVertical();
            Canvas.ForceUpdateCanvases();

            goCombined__.Find("Combine").GetComponent<Button>().interactable = (combined_ != default);

            for (int i = 0; i < Globals.heroList.Count; i++)
            {
                GameObject go__i_ = go__.Find("Combine_Hero" + i.ToString("D2")).gameObject;
                for (int j = 0; j < Globals.heroList[i]._parameter._equips.Length; j++)
                {
                    GameObject go__i_j_ = go__i_.Find("Combine_Equip" + j.ToString("D2")).gameObject;
                    _Equip equip_ = Globals.heroList[i]._parameter._equips[j];
                    bool isEquipIsExist_ = (equip_ != null && equip_ != default && equip_._name.IsNullOrEmpty() == false);

                    go__i_j_.GetComponent<_Clickable>()._tooltip = (isEquipIsExist_) ? equip_._tooltip.DeepCopy() : new _Tooltip();
                    go__i_j_.GetComponent<_Clickable>()._tooltip._placeType = "Side";
                    go__i_j_.GetComponent<_Clickable>()._tooltip._effectText = (isEquipIsExist_) ? _Tooltip.MakeTextFromEquip(equip_) : "";
                    Image image_ = go__i_j_.Find("IconImage").GetComponent<Image>();
                    image_.sprite = (equip_ != null && equip_ != default && equip_._name.IsNullOrEmpty() == false) ? Resources.Load<Sprite>(equip_._pathIcon) : Prefabs.Instance.spTransparent;
                }
            }
        }

        void ConfigureStatusUI_Inventory()
        {
            GameObject go__ = goStatus.transform.Find("Inventories").gameObject;

            for (int j = 0; j < Globals.inventoryList.Count; j++)
            {
                GameObject go__j_ = go__.Find("Inventory" + j.ToString("D2")).gameObject;
                _Clickable cl__ = go__j_.GetComponent<_Clickable>();
                _Equip equip__j_ = Globals.inventoryList[j];
                bool isEquipExist_ = (equip__j_ != null) && (equip__j_ != default) && (equip__j_._name.IsNullOrEmpty() == false);

                cl__._tooltip = (isEquipExist_) ? equip__j_._tooltip.DeepCopy() : new _Tooltip();
                cl__._tooltip._placeType = "Side";
                cl__._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip__j_) : "";
                go__j_.transform.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip__j_._pathIcon) : Prefabs.Instance.spTransparent;
                go__j_.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? equip__j_._descriptiveName : "";
                go__j_.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? MakeDescriptionText_StatusEquip(equip__j_, 10) : "";
            }
        }

        void ConfigureStatusUI_Tabs()
        {
            for (int i = 0; i < Globals.heroList.Count; i++)
            {
                _Unit unit_i_ = Globals.heroList[i];
                GameObject tab_i_ = goStatus.Find("Tabs").Find("StatusTab" + (i + 1).ToString("D2")).gameObject;
                bool isShowNotification_ = false;

                tab_i_.Find("IconNormal").GetComponent<Image>().sprite = unit_i_._Sprite;

                if (unit_i_._CalclateSkillPointRemaining().IsContains(true))
                    isShowNotification_ = true;

                foreach (_Skill skill_i_ in unit_i_._parameter._skillTree)
                {
                    if (skill_i_._parameter._isActive && skill_i_._CalculateAbilityPointRemaining(unit_i_) > 0)
                        isShowNotification_ = true;
                }

                tab_i_.Find("Notification").gameObject.SetActive(isShowNotification_);
            }
        }
    }

    public void SetActive_StatusCombine()
    {
        if (Globals.Instance.equipsToCombine[0] == null || Globals.Instance.equipsToCombine[0] == default) return;
        if (Globals.Instance.equipsToCombine[1] == null || Globals.Instance.equipsToCombine[1] == default) return;
        if (_Equip.CombineEquip(Globals.Instance.equipsToCombine[0], Globals.Instance.equipsToCombine[1]) == default) return;

        goStatus.Find("ComfirmCombine").gameObject.SetActive(true);

        GameObject go_ = goStatus.Find("ComfirmCombine").Find("Equip").gameObject;
        _Equip equip_ = _Equip.CombineEquip(Globals.Instance.equipsToCombine[0], Globals.Instance.equipsToCombine[1]);

        go_.Find("IconImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(equip_._pathIcon);
        go_.Find("Title").GetComponent<TextMeshProUGUI>().text = equip_._descriptiveName;
        go_.Find("Description").GetComponent<TextMeshProUGUI>().text = MakeDescriptionText_StatusEquip(equip_, 14);
    }

    public static void SetActive_StatusDiscard(_Equip equip_)
    {
        goStatus.Find("ComfirmDiscard").gameObject.SetActive(true);

        GameObject go_ = goStatus.Find("ComfirmDiscard").Find("Equip").gameObject;
        bool isEquipExist_ = (equip_ != null) && (equip_ != default) && (equip_._name.IsNullOrEmpty() == false);

        go_.transform.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip_._pathIcon) : Prefabs.Instance.spTransparent;
        go_.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? equip_._descriptiveName : "";
        go_.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? MakeDescriptionText_StatusEquip(equip_, 14) : "";
    }

    public static IEnumerator StatusUI_MoveTab(int index_)
    {
        if (Globals.inputStopperCount > 0) yield break;
        if (index_ == Globals.statusTabIndex) yield break;
        if (index_.IsBetween(0, 4) == false) yield break;

        Globals.inputStopperCount++;
        Globals.statusTabIndex = index_;

        if ((Globals.Instance.equipsToCombine[0] != null && Globals.Instance.equipsToCombine[0]._name.IsNullOrEmpty() ==false) ||
            (Globals.Instance.equipsToCombine[1] != null && Globals.Instance.equipsToCombine[1]._name.IsNullOrEmpty() == false))
        {
            if (index_ != 4)
                goStatus.Find("Tabs").Find("StatusTab04").Find("Notification").gameObject.SetActive(true);
        }

        RectTransform rtFocus_ = goStatus.Find("Tabs").Find("TabFocus").GetComponent<RectTransform>();
        Vector2 posFocusStart_ = rtFocus_.anchoredPosition;
        Vector2 posFocusEnd_ = goStatus.Find("Tabs").Find("StatusTab" + index_.ToString("D2")).GetComponent<RectTransform>().anchoredPosition * Vector2.right + posFocusStart_ * Vector2.up;

        RectTransform rtContent = goStatus.transform.Find("ScrollView").Find("Contents").GetComponent<RectTransform>();
        Vector2 posContentStart_ = rtContent.anchoredPosition;
        Vector2 posContentEnd_ = (-890 * index_) * Vector2.right + rtContent.anchoredPosition * Vector2.up;

        for (float timeSum_ = 0, timeMax_ = 0.3f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1).ToSigmoid();
            rtFocus_.anchoredPosition = Library.BezierLiner(posFocusStart_, posFocusEnd_, p_);
            rtContent.anchoredPosition = Library.BezierLiner(posContentStart_, posContentEnd_, p_);

            yield return null;
        }
        rtFocus_.anchoredPosition = posFocusEnd_;
        rtContent.anchoredPosition = posContentEnd_;

        Globals.inputStopperCount--;
    }

    public static void StatusUI_MoveTabImmediately(int index_)
    {
        RectTransform rtFocus_ = goStatus.Find("Tabs").Find("TabFocus").GetComponent<RectTransform>();
        Vector2 posFocusStart_ = rtFocus_.anchoredPosition;
        Vector2 posFocusEnd_ = goStatus.Find("Tabs").Find("StatusTab" + index_.ToString("D2")).GetComponent<RectTransform>().anchoredPosition * Vector2.right + posFocusStart_ * Vector2.up;

        RectTransform rtContent = goStatus.transform.Find("ScrollView").Find("Contents").GetComponent<RectTransform>();
        Vector2 posContentEnd_ = (-890 * index_) * Vector2.right + rtContent.anchoredPosition * Vector2.up;

        rtFocus_.anchoredPosition = posFocusEnd_;
        rtContent.anchoredPosition = posContentEnd_;
    }

    public static string MakeDescriptionText_StatusEquip(_Equip equip_, float size_ = 12)
    {
        string result = "";
        string text_ = ((equip_._hpPlus != 0) ? "<sprite name=HP><mspace=" + size_ + ">" + equip_._hpPlus.ToString("+00;-00") + "</mspace>\n" : "") +
                       ((equip_._adPlus != 0) ? "<sprite name=AD><mspace=" + size_ + ">" + equip_._adPlus.ToString("+00;-00") + "</mspace>\n" : "") +
                       ((equip_._arPlus != 0) ? "<sprite name=AR><mspace=" + size_ + ">" + equip_._arPlus.ToString("+00;-00") + "</mspace>\n" : "") +
                       ((equip_._mdPlus != 0) ? "<sprite name=MD><mspace=" + size_ + ">" + equip_._mdPlus.ToString("+00;-00") + "</mspace>\n" : "") +
                       ((equip_._mrPlus != 0) ? "<sprite name=MR><mspace=" + size_ + ">" + equip_._mrPlus.ToString("+00;-00") + "</mspace>\n" : "") +
                       ((equip_._spPlus != 0) ? "<sprite name=SP><mspace=" + size_ + ">" + equip_._spPlus.ToString("+00;-00") + "</mspace>\n" : "");

        string[] parts_ = text_.TrimEnd('\n').Split('\n');
        for (int i = 0; i < parts_.Length; i++)
        {
            if (i > 2)
            {
                result += "<line-height=12>\n</line-height><align=right>...</align>";
                break;
            }
            result += "\n" + parts_[i];
        }
        result = result.TrimStart('\n');

        return result;
    }

    public static void ConfigureGoldUI(int gold_ = -1)
    {
        if (gold_ == -1)
            gold_ = Globals.Instance.Gold;

        if (gold_ < 1000)
        {
            goGold.Find("Text").GetComponent<TextMeshProUGUI>().text = "<mspace=22>" + gold_ + "</mspace>";
            goShop.Find("Gold").Find("Text").GetComponent<TextMeshProUGUI>().text = "<mspace=22>" + gold_ + "</mspace>";
        }
        else
        {
            goGold.Find("Text").GetComponent<TextMeshProUGUI>().text = "<mspace=22>" + gold_ / 1000 + "</mspace>,<mspace=22>" + (gold_ % 1000).ToString("D3") + "</mspace>";
            goShop.Find("Gold").Find("Text").GetComponent<TextMeshProUGUI>().text = "<mspace=22>" + gold_ / 1000 + "</mspace>,<mspace=22>" + (gold_ % 1000).ToString("D3") + "</mspace>";
        }
    }

    public static void ConfigureGlobalEffectUI()
    {
        List<GameObject> Icons = goGlobal.Find("GlobalEffect").GetChildrenGameObject().ToList();
        int index_ = 0;

        foreach (_Skill skill_i_ in Globals.Instance.globalEffectList)
        {
            ConfigureGlobalEffectUI_SetActiveOrCreateObject();
            Icons[index_].Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(skill_i_._parameter._pathIcon);
            Icons[index_].Find("Count").GetComponent<TextMeshProUGUI>().text = (skill_i_._parameter._isShowICount) ? skill_i_._parameter._iCount.ToString() :"";
            Icons[index_].GetComponent<_Clickable>()._miniTooltip = skill_i_._miniTooltip.DeepCopy();
            Icons[index_].GetComponent<_Clickable>()._miniTooltip._effectText = _Tooltip.ReplaceTag_GlobalEffect(skill_i_._miniTooltip._effectText, skill_i_);
            index_++;
        }

        for (int i = index_; i < Icons.Count; i++)
        {
            Icons[i].SetActive(false);
        }

        void ConfigureGlobalEffectUI_SetActiveOrCreateObject()
        {
            if (index_ < Icons.Count)
            {
                Icons[index_].SetActive(true);
            }
            else
            {
                GameObject goNew_ = Instantiate(Prefabs.goInstances.Find(m => m.name == "GlobalEffect"), goGlobal.Find("GlobalEffect"));
                goNew_.name = "GlobalEffect" + index_.ToString("D2");
                goNew_.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 * (index_ % 8), 60 * (index_ / 8));
                Icons.Add(goNew_);
            }
        }
    }

    public static IEnumerator LerpGoldValue()
    {
        int goldStart_ = int.Parse(goGold.Find("Text").GetComponent<TextMeshProUGUI>().text.Replace("<mspace=22>", "").Replace("</mspace>", "").Replace(",", ""));
        int goldEnd_ = Globals.Instance.Gold;
        
        for (float timeSum_ = 0, timeMax_ = 0.6f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);
            int gold_ = Mathf.Lerp(goldStart_, goldEnd_, p_).ToInt();

            ConfigureGoldUI(gold_);

            yield return null;
        }

        ConfigureGoldUI(goldEnd_);
    }

    public static void ConfigureShopUI()
    {
        Map._Spot spot_ = Globals.Instance.spotCurrent;
        goShop.Find("Shop_Undo").GetComponent<Button>().interactable = (Globals.Instance.Gold != Globals.Instance.shopUndoSave._gold);

        ConfigureShopUI_Items();
        ConfigureShopUI_Equips();
        //ConfigureShopUI_Skills();

        void ConfigureShopUI_Items()
        {
            //GameObject go_ = goShop.Find("Shop_Items").gameObject;

            for (int i = 0; i < spot_._shopItems.Length; i++)
            {
                GameObject go_i_ = goShop.Find("Shop_Items").Find("Shop_Item" + i.ToString("D2")).gameObject;
                _Clickable cl_i_ = go_i_.GetComponent<_Clickable>();
                _Item item_i_ = spot_._shopItems[i];
                bool isItemExist_ = (item_i_ != null && item_i_._name.IsNullOrEmpty() == false);

                cl_i_._tooltip = (isItemExist_) ? item_i_._tooltip.DeepCopy() : new _Tooltip();
                cl_i_._tooltip._effectText = (isItemExist_) ? _Tooltip.ReplaceTag_Item(cl_i_._tooltip._effectText, item_i_) : "";
                cl_i_._tooltip._placeType = "Side";
                go_i_.Find("IconImage").GetComponent<Image>().sprite = (isItemExist_) ? Resources.Load<Sprite>(item_i_._pathIcon) : Resources.Load<Sprite>("EquipIcon/SoldOut");
                go_i_.Find("PriceTag").GetComponent<TextMeshProUGUI>().text = (isItemExist_) ? "<mspace=16.5><sprite name=Gold>" + item_i_._price + "</mspace>" : "";
            }

            for (int i = 0; i < Globals.itemsInBagList.Count; i++)
            {
                GameObject go_i_ = goShop.Find("Items").Find("Item" + i.ToString("D2")).gameObject;
                _Clickable cl_i_ = go_i_.GetComponent<_Clickable>();
                _Item item_i_ = Globals.itemsInBagList[i];
                bool isItemExist_ = (item_i_ != null && item_i_._name.IsNullOrEmpty() == false);

                cl_i_._tooltip = (isItemExist_) ? item_i_._tooltip.DeepCopy() : new _Tooltip();
                cl_i_._tooltip._effectText = (isItemExist_) ? _Tooltip.ReplaceTag_Item(cl_i_._tooltip._effectText, item_i_) : "";
                cl_i_._tooltip._placeType = "Side";
                go_i_.Find("IconImage").GetComponent<Image>().sprite = (isItemExist_) ? Resources.Load<Sprite>(item_i_._pathIcon) : Prefabs.Instance.spTransparent;
                go_i_.Find("Stack").GetComponent<TextMeshProUGUI>().text = (isItemExist_ && item_i_._stackCount > 1) ? item_i_._stackCount.ToString() : "";
            }
        }

        void ConfigureShopUI_Equips()
        {
            //GameObject go_ = goShop.Find("Shop_Equips").gameObject;

            for (int i = 0; i < spot_._shopEquips.Length; i++)
            {
                GameObject go_i_ = goShop.Find("Shop_Equips").Find("Shop_Equip" + i.ToString("D2")).gameObject;
                _Clickable cl_i_ = go_i_.GetComponent<_Clickable>();
                _Equip equip_i_ = spot_._shopEquips[i];
                bool isEquipExist_ = (equip_i_ != null && equip_i_._name.IsNullOrEmpty() == false);

                cl_i_._tooltip = (isEquipExist_) ? equip_i_._tooltip.DeepCopy() : new _Tooltip();
                cl_i_._tooltip._placeType = "Side";
                cl_i_._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip_i_) : "";
                go_i_.Find("Title").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? equip_i_._descriptiveName : "";
                go_i_.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip_i_._pathIcon) : Resources.Load<Sprite>("EquipIcon/SoldOut");
                go_i_.Find("Description").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? MakeDescriptionText_StatusEquip(equip_i_, 12) : "";
                go_i_.Find("PriceTag").GetComponent<TextMeshProUGUI>().text = (isEquipExist_) ? "<sprite name=Gold><mspace=18>" + equip_i_._price + "</mspace>" : "";
            }
            for (int i = 0; i < Globals.heroList.Count; i++)
            {
                GameObject go_i_ = goShop.Find("Hero" + i.ToString("D2")).gameObject;
                _Hero hero_i_ = Globals.heroList[i];

                go_i_.Find("IconImage").GetComponent<Image>().sprite = hero_i_._Sprite;

                for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
                {
                    GameObject go_i_j_ = go_i_.Find("Equips").Find("StatusAll_Equip" + j.ToString("D2")).gameObject;
                    _Clickable cl_i_j_ = go_i_j_.GetComponent<_Clickable>();
                    _Equip equip_i_j_ = hero_i_._parameter._equips[j];
                    bool isEquipExist_ = (equip_i_j_ != null && equip_i_j_._name.IsNullOrEmpty() == false);

                    cl_i_j_._tooltip = (isEquipExist_) ? equip_i_j_._tooltip.DeepCopy() : new _Tooltip();
                    cl_i_j_._tooltip._placeType = "Side";
                    cl_i_j_._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip_i_j_) : "";
                    go_i_j_.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip_i_j_._pathIcon) : Prefabs.Instance.spTransparent;
                }
            }
            for (int i = 0; i < Globals.inventoryList.Count; i++)
            {
                GameObject go_i_ =goShop.Find("Inventories").Find("Inventory" + i.ToString("D2")).gameObject;
                _Clickable cl_i_ = go_i_.GetComponent<_Clickable>();
                _Equip equip_i_ = Globals.inventoryList[i];
                bool isEquipExist_ = (equip_i_ != null && equip_i_._name.IsNullOrEmpty() == false);

                cl_i_._tooltip = (isEquipExist_) ? equip_i_._tooltip.DeepCopy() : new _Tooltip();
                cl_i_._tooltip._placeType = "Side";
                cl_i_._tooltip._effectText = (isEquipExist_) ? _Tooltip.MakeTextFromEquip(equip_i_) : "";
                go_i_.Find("IconImage").GetComponent<Image>().sprite = (isEquipExist_) ? Resources.Load<Sprite>(equip_i_._pathIcon) : Prefabs.Instance.spTransparent;
            }
        }

        //void ConfigureShopUI_Skills()
        //{
        //    GameObject go_ = goShop.Find("ScrollView").Find("Contents").Find("Content02").gameObject;

        //    for (int i = 0; i < spot_._shopSkills.Length; i++)
        //    {
        //        GameObject go_i_ = go_.Find("Shop_Skills").Find("Shop_Skill" + i.ToString("D2")).gameObject;
        //        _Clickable cl_i_ = go_i_.GetComponent<_Clickable>();
        //        _Skill skill_i_ = spot_._shopSkills[i];
        //        bool isSkillExist_ = (skill_i_ != null && skill_i_._parameter._name.IsNullOrEmpty() == false);

        //        cl_i_._tooltip = (isSkillExist_) ? skill_i_._tooltip.DeepCopy() : new _Tooltip();
        //        cl_i_._tooltip._placeType = "Side";
        //        cl_i_._tooltip._effectText = (isSkillExist_) ? _Tooltip.MakeTextFromSkill(skill_i_) : "";
        //        go_i_.Find("Title").GetComponent<TextMeshProUGUI>().text = (isSkillExist_) ? skill_i_._parameter._descriptiveName : "";
        //        go_i_.Find("IconImage").GetComponent<Image>().sprite = (isSkillExist_) ? Resources.Load<Sprite>(skill_i_._parameter._pathIcon) : Resources.Load<Sprite>("EquipIcon/SoldOut");
        //        go_i_.Find("Description").GetComponent<TextMeshProUGUI>().text = (isSkillExist_) ? _Tooltip.RichDescriptionText(_Tooltip.ReplaceTag_Skill(skill_i_._tooltip._effectText, skill_i_)) : "";
        //        //go_i_.Find("PriceTag").GetComponent<TextMeshProUGUI>().text = (isSkillExist_) ? "<sprite name=Gold><mspace=18>" + skill_i_._parameter._price + "</mspace>" : "";
        //    }

        //    for (int i = 0; i < Globals.heroList.Count; i++)
        //    {
        //        GameObject go_i_ = go_.Find("Hero" + i.ToString("D2")).gameObject;
        //        _Hero hero_i_ = Globals.heroList[i];

        //        for (int j = 1; j < hero_i_._parameter._skills.Length; j++)
        //        {
        //            GameObject go_i_j_ = go_i_.Find("Skills").Find("StatusAll_Skill" + j.ToString("D2")).gameObject;
        //            _Clickable cl_i_j_ = go_i_j_.GetComponent<_Clickable>();
        //            _Skill skill_i_j_ = hero_i_._parameter._skills[j];
        //            bool isSkillExist_ = (skill_i_j_ != null && skill_i_j_._parameter._name.IsNullOrEmpty() == false);

        //            cl_i_j_._tooltip = (isSkillExist_) ? skill_i_j_._tooltip.DeepCopy() : new _Tooltip();
        //            cl_i_j_._tooltip._placeType = "Side";
        //            cl_i_j_._tooltip._effectText = (isSkillExist_) ? _Tooltip.MakeTextFromSkill(skill_i_j_) : "";
        //            go_i_j_.Find("IconImage").GetComponent<Image>().sprite = (isSkillExist_) ? Resources.Load<Sprite>(skill_i_j_._parameter._pathIcon) : Prefabs.Instance.spTransparent;
        //        }
        //    }
        //}
    }

    public static void ConfigureTreasureChestUI()
    {
        for (int i = 0; i < 3; i++)
        {
            _Skill skill_i_ = _Skill.CloneFromString(Globals.Instance.spotCurrent._treasureItems[i]);
            GameObject go_i_ = goTreasureChest.Find("Treasure" + i.ToString("D2")).gameObject;

            go_i_.Find("Item").Find("Image").GetComponent<Image>().sprite = (Resources.Load<Sprite>(skill_i_._parameter._pathIcon) is Sprite sprite_) ? sprite_ : Prefabs.Instance.spDummy;
            go_i_.Find("ItemName").GetComponent<TextMeshProUGUI>().text = TextData.DescriptionText[skill_i_._parameter._descriptiveName].GetIndexOf(0);
            go_i_.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text = TextData.DescriptionText[skill_i_._parameter._descriptiveName].GetIndexOf(1);
        }
    }

    public static IEnumerator MoveTab_ShopUI(int index_)
    {
        if (Globals.inputStopperCount > 0) yield break;
        if (index_ == Globals.shopTabIndex) yield break;
        if (index_.IsBetween(0, 2) == false) yield break;

        Globals.inputStopperCount++;
        Globals.shopTabIndex = index_;
        float width_ = goShop.Find("ScrollView").Find("Contents").GetComponent<RectTransform>().sizeDelta.x / 3;

        RectTransform rtFocus_ = goShop.Find("Tabs").Find("TabFocus").GetComponent<RectTransform>();
        Vector2 posFocusStart_ = rtFocus_.anchoredPosition;
        Vector2 posFocusEnd_ = goShop.Find("Tabs").Find("ShopTab" + index_.ToString("D2")).GetComponent<RectTransform>().anchoredPosition * Vector2.right + posFocusStart_ * Vector2.up;

        RectTransform rtContent = goShop.transform.Find("ScrollView").Find("Contents").GetComponent<RectTransform>();
        Vector2 posContentStart_ = rtContent.anchoredPosition;
        Vector2 posContentEnd_ = (-width_ * index_) * Vector2.right + rtContent.anchoredPosition * Vector2.up;

        for (float timeSum_ = 0, timeMax_ = 0.3f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1).ToSigmoid();
            rtFocus_.anchoredPosition = Library.BezierLiner(posFocusStart_, posFocusEnd_, p_);
            rtContent.anchoredPosition = Library.BezierLiner(posContentStart_, posContentEnd_, p_);

            yield return null;
        }
        rtFocus_.anchoredPosition = posFocusEnd_;
        rtContent.anchoredPosition = posContentEnd_;

        Globals.inputStopperCount--;
    }

    public static void ConfigureItemsUI()
    {
        for (int i_ = 0; i_ < imItems.Length; i_++)
        {
            if (Globals.itemsInBagList[i_] is _Item item_i_ && item_i_._name.IsNullOrEmpty() == false)
            {
                imItems[i_].sprite = (Resources.Load<Sprite>(item_i_._pathIcon) is Sprite) ? Resources.Load<Sprite>(item_i_._pathIcon) : Prefabs.Instance.spDummy;
                clItems[i_]._tooltip = item_i_._tooltip.DeepCopy();
                clItems[i_]._tooltip._effectText = _Tooltip.ReplaceTag_Item(clItems[i_]._tooltip._effectText, item_i_);
                txItemsStack[i_].text = (item_i_._stackCount > 1) ? item_i_._stackCount.ToString() : "";
            }
            else
            {
                imItems[i_].sprite = Prefabs.Instance.spTransparent;
                clItems[i_]._tooltip = default;
                txItemsStack[i_].text = "";
            }
        }
    }

    public static void ConfigureHeroesUI()
    {
        for (int i_ = 0; i_ < imHeroes.Length; i_++)
        {
            if (i_ > Globals.heroList.Count - 1) break;
            if (Globals.heroList[i_] == null) break;

            _Hero hero_i_ = Globals.heroList[i_];
            GameObject goHero_i_ = imHeroes[i_].transform.parent.gameObject;

            goHero_i_.Find("HP").GetComponent<Slider>().value = (float)hero_i_._parameter._hp / hero_i_._parameter._hpMax;
            goHero_i_.Find("Exp").GetComponent<Slider>().maxValue = Table.ExpTable.GetValue(hero_i_._parameter._class)[hero_i_._parameter._lv];
            goHero_i_.Find("Exp").GetComponent<Slider>().value = hero_i_._parameter._expFraction;
            goHero_i_.Find("Lv.").Find("Text").GetComponent<TextMeshProUGUI>().text = hero_i_._parameter._lv.ToString();
            imHeroes[i_].sprite = hero_i_._Sprite;

            bool isShowNorification_ = false;

            if (hero_i_._CalclateSkillPointRemaining().IsContains(true))
                isShowNorification_ = true;

            foreach (_Skill skill_i_ in hero_i_._parameter._skillTree)
            {
                if (skill_i_._parameter._isActive && skill_i_._CalculateAbilityPointRemaining(hero_i_) > 0)
                    isShowNorification_ = true;
            }

            goHero_i_.Find("Notification").gameObject.SetActive(isShowNorification_);

            bool isShowMoveIcon_ = (Globals.Instance.sceneState == "Battle" && hero_i_._parameter._movableCount > 0);
            bool isShowAttackIcon_ = (Globals.Instance.sceneState == "Battle" && hero_i_._parameter._actableCount > 0);

            goHero_i_.Find("HeroMove" + i_.ToString("D2")).gameObject.SetActive(isShowMoveIcon_);
            goHero_i_.Find("HeroAttack" + i_.ToString("D2")).gameObject.SetActive(isShowAttackIcon_);
        }
    }

    public static void ConfigureUnitUI(_Unit unit_)
    {
        if (unit_ == null) return;

        goUnitEquips.SetActive(unit_ is _Hero);

        int[] stats_ = new int[5] { unit_._parameter._ad, unit_._parameter._ar, unit_._parameter._md, unit_._parameter._mr, unit_._parameter._sp };
        int[] statsApplied_ = new int[5] { unit_._parameter._adApplied, unit_._parameter._arApplied, unit_._parameter._mdApplied, unit_._parameter._mrApplied, unit_._parameter._spApplied };
        //int[] buff_ = new int[5] { unit_._parameter._adBuff, unit_._parameter._arBuff, unit_._parameter._mdBuff, unit_._parameter._mrBuff, unit_._parameter._spBuff };
        string[] spNames_ = new string[5] { "<sprite name=AD>", "<sprite name=AR>", "<sprite name=MD>", "<sprite name=MR>", "<sprite name=SP>" };
        for (int i_ = 0; i_ < txStats.Length; i_++)
        {
            txStats[i_].text = spNames_[i_]  + statsApplied_[i_];
            if (statsApplied_[i_] == stats_[i_]) txStats[i_].color = new Color32(255, 255, 255, 255);
            if (statsApplied_[i_] > stats_[i_]) txStats[i_].color = new Color32(000, 255, 000, 255);
            if (statsApplied_[i_] < stats_[i_]) txStats[i_].color = new Color32(255, 000, 000, 255);
        }

        imIconFace.sprite = unit_._Sprite;
        goExpBar.SetActive(unit_ is _Hero);
        goLevel.SetActive(unit_ is _Hero);
        slHpSlider.value = (float)unit_._parameter._hp / unit_._parameter._hpMax;
        imHpSlider.color = (unit_._parameter._hp > unit_._parameter._hpMax / 2) ? new Color32(000, 255, 000, 255) :
                         /*(unit_._parameter._hp < unit_._parameter._hpMax / 2)*/ new Color32(255, 255, 000, 255);
        txHpSlider.text = unit_._parameter._hp + "/" + unit_._parameter._hpMax;
        slExpSlider.value = (float)unit_._parameter._expFraction / 100;
        txLevelText.text = unit_._parameter._lv.ToString();

        ConfigureSkillUI(unit_);
        ConfigureEquipsUI(unit_);
        ConfigureStatusIconsUI(unit_);
    }

    public static void ConfigureSkillUI(_Unit unit_)
    {
        if (unit_ == null) return;

        if (unit_._parameter._classPassives.Count > 0 && unit_._parameter._classPassives?[0]?._parameter._name.IsNullOrEmpty() == false)
        {
            _Skill passive_ = unit_._parameter._classPassives[0];
            goUnitSkills.Find("Passive").Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(passive_._parameter._pathIcon);
            goUnitSkills.Find("Passive").Find("CooldownOverlay").gameObject.SetActive(passive_._parameter._cooldownRemaining > 0);
            //goUnitSkills.Find("Passive").Find("CooldownCount").gameObject.SetActive(passive_._parameter._cooldownRemaining > 0);
            goUnitSkills.Find("Passive").Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = (passive_._parameter._cooldownRemaining > 0) ? passive_._parameter._cooldownRemaining.ToString() : "";
            goUnitSkills.Find("Passive").Find("StackCount").GetComponent<TextMeshProUGUI>().text = (passive_._parameter._stack != 0) ? passive_._parameter._stack.ToString() : "";
            goUnitSkills.Find("Passive").GetComponent<_Clickable>()._tooltip = passive_._tooltip.DeepCopy();
            goUnitSkills.Find("Passive").GetComponent<_Clickable>()._tooltip._effectText = _Tooltip.MakeTextFromSkill(passive_);
        }
        else
        {
            goUnitSkills.Find("Passive").Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Square_Black");
            goUnitSkills.Find("Passive").Find("CooldownOverlay").gameObject.SetActive(false);
            goUnitSkills.Find("Passive").Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = "";
            goUnitSkills.Find("Passive").Find("StackCount").GetComponent<TextMeshProUGUI>().text = "";
            goUnitSkills.Find("Passive").GetComponent<_Clickable>()._tooltip = new _Tooltip();
        }
        for (int i_ = 0; i_ < goSkills.Length; i_++)
        {
            if (unit_._parameter._skills[i_] is _Skill skill_i_ && skill_i_._parameter._name.IsNullOrEmpty() == false)
            {
                if (Resources.Load<Sprite>(skill_i_._parameter._pathIcon) is Sprite sprite_)
                    imSkillsIcon[i_].sprite = sprite_;
                else
                    imSkillsIcon[i_].sprite = Prefabs.Instance.spDummy;

                //imSkillsFocus[i_].gameObject.SetActive(skill_i_ == unit_._skillOnActive);
                imSkillsCooldownOverlay[i_].gameObject.SetActive(skill_i_._IsCastable() == false);
                imSkillsCooldownCount[i_].gameObject.SetActive(skill_i_._IsCastable() == false);
                imSkillsCooldownCount[i_].text = skill_i_._parameter._cooldownRemaining.ToString();
                clSkills[i_]._tooltip = skill_i_._tooltip.DeepCopy();
                clSkills[i_]._tooltip._effectText = _Tooltip.MakeTextFromSkill(skill_i_);
                //clSkills[i_]._skillRef = skill_i_;
            }
            else
            {
                if (unit_._parameter._unitType == "Hero")
                {
                    if (i_ == 2)
                        imSkillsIcon[i_].sprite = Resources.Load<Sprite>("SkillIcon/Locked_Lv.4");
                    else if (i_ == 3)
                        imSkillsIcon[i_].sprite = Resources.Load<Sprite>("SkillIcon/Locked_Lv.7");
                    else
                        imSkillsIcon[i_].sprite = Resources.Load<Sprite>("SkillIcon/Locked");
                }
                else
                {
                    imSkillsIcon[i_].sprite = Resources.Load<Sprite>("SkillIcon/Square_Black");
                }
                //imSkillsFocus[i_].gameObject.SetActive(false);
                imSkillsCooldownOverlay[i_].gameObject.SetActive(false);
                imSkillsCooldownCount[i_].gameObject.SetActive(false);
                clSkills[i_]._tooltip = default;
                //clSkills[i_]._skillRef = default;
            }
        }
    }

    public static void ConfigureEquipsUI(_Unit unit_)
    {
        if (unit_ == null) return;

        for (int i_ = 0; i_ < unit_._parameter._equips.Length; i_++)
        {
            if (unit_._parameter._equips[i_] is _Equip equip_i_ && equip_i_._name.IsNullOrEmpty() == false)
            {
                imEquipsButton[i_].color = (equip_i_._castableLimitCount > 0 && equip_i_._castableStacks > 0) ? "ffff00".ToColor32() :"ffffff".ToColor32();
                imEquipsIcon[i_].sprite = (Resources.Load<Sprite>(equip_i_._pathIcon) is Sprite) ? Resources.Load<Sprite>(equip_i_._pathIcon) : Prefabs.Instance.spDummy;
                clEquips[i_]._tooltip = equip_i_._tooltip.DeepCopy();
                clEquips[i_]._tooltip._effectText = _Tooltip.MakeTextFromEquip(equip_i_);
                if (equip_i_._active is _Skill active_i_ && active_i_._parameter._name.IsNullOrEmpty() == false)
                {
                    int cd_ =equip_i_._cooldownRemaining;
                    clEquips[i_].transform.Find("CooldownOverlay").gameObject.SetActive(cd_ > 0);
                    clEquips[i_].transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = (cd_ > 0) ? cd_.ToString() : "";
                }
                else
                {
                    clEquips[i_].transform.Find("CooldownOverlay").gameObject.SetActive(false);
                    clEquips[i_].transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            else
            {
                imEquipsButton[i_].color = "ffffff".ToColor32();
                clEquips[i_]._tooltip = default;
                imEquipsIcon[i_].sprite = Prefabs.Instance.spTransparent;
                clEquips[i_].transform.Find("CooldownOverlay").gameObject.SetActive(false);
                clEquips[i_].transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public static void ConfigureStatusIconsUI(_Unit unit_)
    {
        if (unit_ == null) return;

        int index_ = 0;
        int indexMax = imStatusIconList.Count;

        foreach (_Skill skill_i_ in unit_._parameter._classPassives)
        {
            if (skill_i_._parameter._isShowAsIcon == false) continue;
            if (skill_i_ == unit_._parameter._classPassives.First()) continue;
            SetActiveOrCreateIconObject();
            string stack_ = (skill_i_._parameter._isShowICount) ? skill_i_._parameter._iCount.ToString() : "";
            ConfigureIcon(stack_, skill_i_._parameter._pathIcon, skill_i_._parameter._name);
            imStatusIconList[index_ - 1].transform.parent.GetComponent<_Clickable>()._miniTooltip._effectText = _Tooltip.MakeTextFromSkill(skill_i_);
        }

        foreach (_Skill skill_i_ in unit_._parameter._additionalPassives)
        {
            if (skill_i_._parameter._isShowAsIcon == false) continue;
            SetActiveOrCreateIconObject();
            string stack_ = (skill_i_._parameter._isShowICount) ? skill_i_._parameter._iCount.ToString() : "";
            ConfigureIcon(stack_, skill_i_._parameter._pathIcon, skill_i_._parameter._name);
            imStatusIconList[index_ - 1].transform.parent.GetComponent<_Clickable>()._miniTooltip._effectText = _Tooltip.MakeTextFromSkill(skill_i_);
        }
        if (unit_._BarrierValue > 0)
        {
            SetActiveOrCreateIconObject();
            ConfigureIcon(unit_._BarrierValue.ToString(), "StatusIcon/Barrier", "Barrier");
        }
        foreach (_Unit._Parameter._StatusCondition statusCondition_i_ in unit_._parameter._statusConditions)
        {
            if (statusCondition_i_._count > 0)
            {
                SetActiveOrCreateIconObject();
                ConfigureIcon(statusCondition_i_._count.ToString(), "StatusIcon/" + statusCondition_i_._name, statusCondition_i_._name);
            }
        }
        //if (unit_._parameter._stealthCount > 0)
        //{
        //    SetActiveOrCreateIconObject();
        //    ConfigureIcon("", "StatusIcon/Stealth", "Stealth");
        //}
        //if (unit_._parameter._stunCount > 0)
        //{
        //    SetActiveOrCreateIconObject();
        //    ConfigureIcon("", "StatusIcon/Stun", "Stun");
        //}

        List<int> buffValuesList_ = new List<int>() { unit_._parameter._hpBuff, unit_._parameter._adBuff, unit_._parameter._arBuff,
                                                      unit_._parameter._mdBuff, unit_._parameter._mrBuff, unit_._parameter._spBuff };
        List<string> buffNamesList_ = new List<string>() { "HP", "AD", "AR", "MD", "MR", "SP" };
        for (int i = 0; i < buffNamesList_.Count; i++)
        {
            if (buffValuesList_[i] == 0) continue;

            string title_ = buffNamesList_[i] + ((buffValuesList_[i] > 0) ? " Buff" : " Debuff");
            SetActiveOrCreateIconObject();
            ConfigureIcon(buffValuesList_[i].Abs().ToString(), "StatusIcon/" + title_, title_);
        }

        for (int i = index_; i < indexMax; i++)
        {
            imStatusIconList[i].transform.parent.gameObject.SetActive(false);
        }

        void SetActiveOrCreateIconObject()
        {
            if (index_ < indexMax)
            {
                imStatusIconList[index_].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                GameObject goNew = Instantiate(Prefabs.goInstances.Find(m => m.name == "StatusIcon"), goBattle.transform.Find("Unit").Find("StatusIcons"));
                goNew.name = "StatusIcon" + index_.ToString("D2");
                imStatusIconList.Add(goNew.transform.Find("Icon").GetComponent<Image>());
                txStatusCount.Add(goNew.transform.Find("Count").GetComponent<TextMeshProUGUI>());
                goNew.GetComponent<RectTransform>().anchoredPosition = new Vector2(55 * (index_ % 6), 55 * (index_ / 6));
            }
        }

        void ConfigureIcon(string text_, string path_, string title_)
        {
            txStatusCount[index_].text = text_;
            imStatusIconList[index_].sprite = Resources.Load<Sprite>(path_);
            imStatusIconList[index_].transform.parent.GetComponent<_Clickable>()._miniTooltip._title = TextData.DescriptionText[title_].GetIndexOf(0);
            imStatusIconList[index_].transform.parent.GetComponent<_Clickable>()._miniTooltip._effectText = TextData.DescriptionText[title_].GetIndexOf(1);
            index_++;
        }
    }

    public static void ConfigureTooltip(_Clickable clickable_)
    {
        if (clickable_ == null) { goTooltip.SetActive(false); return; }
        if (clickable_._tooltip == null) { goTooltip.SetActive(false); return; }
        if (clickable_._tooltip._title.IsNullOrEmpty()) { goTooltip.SetActive(false); return; }

        Vector2 scale_ = new Vector2((float)Screen.width / 1600, (float)Screen.height / 900);
        txTooltipTitle.text = clickable_._tooltip._title;
        txTooltipEffect.text = _Tooltip.RichDescriptionText(clickable_._tooltip._effectText);

        csfTooltipEffect.SetLayoutVertical();
        txTooltipEffect.transform.parent.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        Canvas.ForceUpdateCanvases();

        if (clickable_.gameObject.GetComponent<RectTransform>() is RectTransform rt_ && rt_ != null)
        {
            if (clickable_._tooltip._placeType == "Side")
            {
                if (rt_.transform.position.x < Screen.width / 2)
                {
                    Vector2 posConer_ = rt_.GetWorldCornersPos()[2];
                    Vector2 posMin_ = new Vector2(20 * scale_.x, (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);
                    Vector2 posMax_ = new Vector2(Screen.width - (450 + 20) * scale_.x, Screen.height - (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);

                    goTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                    goTooltip.GetComponent<RectTransform>().position = (posConer_ + new Vector2(+10 * scale_.x, -20 * scale_.y)).ClampByVector2(posMin_, posMax_);
                }
                else
                {
                    Vector2 posConer_ = rt_.GetWorldCornersPos()[1];
                    Vector2 posMin_ = new Vector2(20 * scale_.x, (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);
                    Vector2 posMax_ = new Vector2(Screen.width - 20 * scale_.x, Screen.height - (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);

                    goTooltip.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                    goTooltip.GetComponent<RectTransform>().position = (posConer_ + new Vector2(-10 * scale_.x, -20 * scale_.y)).ClampByVector2(posMin_, posMax_);
                }
            }
            else if (clickable_._tooltip._placeType == "StatusEquip")
            {
                Vector2 posConer_ = rt_.GetWorldCornersPos()[2];
                Vector2 posMin_ = new Vector2(Screen.width / 2 - 155 * scale_.y, (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);
                Vector2 posMax_ = new Vector2(Screen.width - (450 + 20) * scale_.x, Screen.height - (goTooltip.GetComponent<RectTransform>().sizeDelta.y / 2 + 20) * scale_.y);

                goTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                goTooltip.GetComponent<RectTransform>().position = (posConer_ + new Vector2(+10 * scale_.x, -20 * scale_.y)).ClampByVector2(posMin_, posMax_);
            }
            else
            {
                if (rt_.transform.position.y < Screen.height / 2)
                {
                    Vector2 posConer_ = rt_.GetWorldCornersPos()[1];
                    Vector2 posMin_ = new Vector2(20 * scale_.x, 200 * scale_.y);
                    Vector2 posMax_ = new Vector2(Screen.width - (450 + 20) * scale_.x, Screen.height - 20 * scale_.y);

                    goTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                    goTooltip.transform.position = (posConer_ + new Vector2(+00, +15 * scale_.y)).ClampByVector2(posMin_, posMax_);
                }
                else
                {
                    Vector2 posConer_ = rt_.GetWorldCornersPos()[0];
                    Vector2 posMin_ = new Vector2(20 * scale_.x, 200 * scale_.y);
                    Vector2 posMax_ = new Vector2(Screen.width - (450 + 20) * scale_.x, Screen.height - 20 * scale_.y);

                    goTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                    goTooltip.transform.position = (posConer_ + new Vector2(+00, -15 * scale_.y)).ClampByVector2(posMin_, posMax_);
                }
            }
        }
        else
        {
            Vector2 posOnScreen = Camera.main.WorldToScreenPoint(clickable_.gameObject.transform.position);

            if (posOnScreen.x < Screen.width / 2)
            {
                goTooltip.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.5f);
                goTooltip.transform.position = (posOnScreen + new Vector2(+100 * scale_.x, +40 * scale_.y));
            }
            else
            {
                goTooltip.GetComponent<RectTransform>().pivot = new Vector2(1.0f, 0.5f);
                goTooltip.transform.position = (posOnScreen + new Vector2(-100 * scale_.x, +40 * scale_.y));
            }
        }
    }

    public static void ConfigureMiniTooltip(_Clickable clickable_)
    {
        if (clickable_ == null) { goMiniTooltip.SetActive(false); return; }
        if (clickable_._miniTooltip == null) { goMiniTooltip.SetActive(false); return; }
        if (clickable_._miniTooltip._title.IsNullOrEmpty()) { goMiniTooltip.SetActive(false); return; }

        _Tooltip tooltip_ = clickable_._miniTooltip.DeepCopy();

        Vector2 scale_ = new Vector2((float)Screen.width / 1600, (float)Screen.height / 900);
        txMiniTooltipTitle.text = (TextData.DescriptionText.ContainsKey(tooltip_._title)) ? TextData.DescriptionText.GetValue(tooltip_._title).GetIndexOf(0) : tooltip_._title;
        txMiniTooltipEffect.text = _Tooltip.RichDescriptionText(TextData.DescriptionText.GetValue(tooltip_._title).GetIndexOf(1));
        if (tooltip_._effectText.IsNullOrEmpty())
            txMiniTooltipEffect.text = _Tooltip.RichDescriptionText(TextData.DescriptionText.GetValue(tooltip_._title).GetIndexOf(1));
        else
            txMiniTooltipEffect.text = _Tooltip.RichDescriptionText(tooltip_._effectText);

        csfMiniTooltipEffect.SetLayoutVertical();
        txMiniTooltipEffect.transform.parent.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        Canvas.ForceUpdateCanvases();

        if (clickable_.gameObject.GetComponent<RectTransform>() is RectTransform rt_ && rt_ != null)
        {
            if (rt_.transform.position.y < Screen.height / 2)
            {
                Vector2 posConer_ = rt_.GetWorldCornersPos()[1];
                float posLeftEdge_ = Screen.width - (280 + 20) * scale_.x;
                goMiniTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                goMiniTooltip.transform.position = (posConer_ + new Vector2(+00, +15 * scale_.x)).ClampByVector2(Vector2.zero, new Vector2(posLeftEdge_, Screen.height));
            }
            else
            {
                Vector2 posConer_ = rt_.GetWorldCornersPos()[0];
                float posLeftEdge_ = Screen.width - (280 + 20) * scale_.x;
                goMiniTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                goMiniTooltip.transform.position = (posConer_ + new Vector2(+00, -15 * scale_.y)).ClampByVector2(Vector2.zero, new Vector2(posLeftEdge_, Screen.height));
            }
        }
        else
        {
            Vector2 posOnScreen = Camera.main.WorldToScreenPoint(clickable_.gameObject.transform.position);

            if (posOnScreen.x < Screen.width / 2)
            {
                goMiniTooltip.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.5f);
                goMiniTooltip.transform.position = (posOnScreen + new Vector2(+100 * scale_.x, +40 * scale_.y));
            }
            else
            {
                goMiniTooltip.GetComponent<RectTransform>().pivot = new Vector2(1.0f, 0.5f);
                goMiniTooltip.transform.position = (posOnScreen + new Vector2(-100 * scale_.x, +40 * scale_.y));
            }
        }
    }

    public static void ConfigureWinBattle(bool isConfigureExp_ = true)
    {
        ConfigureWinBattle_Loot();
        ConfigureWinBattle_Hero();

        void ConfigureWinBattle_Loot()
        {
            List<Map._Spot._Loot> lootsList_ = Globals.Instance.spotCurrent._lootsList;
            GameObject[] goLootsList_ = goWinBattle.Find("Loot").Find("ScrollView").Find("Viewport").Find("Contents").GetChildrenGameObject();

            for (int i = 0; i < goLootsList_.Length; i++)
            {
                goLootsList_[i].gameObject.SetActive(false);
            }

            if (Globals.Instance.globalEffectList.Find(m => m._parameter._tags.Contains("Hunger")) != null)
            {
                goLootsList_.ToList().Find(m => m.name == "Text_Can'tGetLoots").gameObject.SetActive(true);
                return;
            }

            for (int i = 0; i < lootsList_.Count; i++)
            {
                goLootsList_[i].gameObject.SetActive(true);

                if (lootsList_[i]._type == "Gold")
                {
                    goLootsList_[i].gameObject.Find("Text").GetComponent<TextMeshProUGUI>().text = "<sprite name=Gold> " + lootsList_[i]._value + " Gold";
                }
                else if (lootsList_[i]._type == "Exp")
                {
                    goLootsList_[i].gameObject.Find("Text").GetComponent<TextMeshProUGUI>().text = "<sprite name=Exp> " + lootsList_[i]._value + " Exp";
                }
            }
        }

        void ConfigureWinBattle_Hero()
        {
            GameObject[] goHeroesList_ = goHeroes.transform.GetChildrenGameObject();

            for (int i = 0; i < goHeroesList_.Length; i++)
            {
                if (isConfigureExp_ == false) break;
                if (Globals.heroList[i] == null) continue;
                _Unit hero_i_ = Globals.heroList[i];

                goHeroesList_[i].Find("Lv.").Find("Text").GetComponent<TextMeshProUGUI>().text = hero_i_._parameter._lv.ToString();
                goHeroesList_[i].Find("Exp").GetComponent<Slider>().maxValue = Table.ExpTable[hero_i_._parameter._class][hero_i_._parameter._lv];
                goHeroesList_[i].Find("Exp").GetComponent<Slider>().value = hero_i_._parameter._expFraction;
            }
        }
    }

    public static void WinBattle_AnimateExpBar(int expDelta_, List<int> levelAtStartList_)
    {
        GameObject[] goHeroesList_ = goHeroes.transform.GetChildrenGameObject();

        for (int i = 0; i < goHeroesList_.Length; i++)
        {
            if (Globals.heroList[i] == null) continue;

            _Unit hero_i_ = Globals.heroList[i];
            int expStart_ = goHeroesList_[i].Find("Exp").GetComponent<Slider>().value.ToInt();

            General.Instance.StartCoroutine(WinBattle_AnimateExpBar_Slide(goHeroesList_[i].Find("Exp").gameObject, hero_i_, expStart_, levelAtStartList_[i]));
        }

        IEnumerator WinBattle_AnimateExpBar_Slide(GameObject Slider_i_, _Unit hero_i_,  int expStart_i_, int levelAtStart_i_)
        {
            Slider slider_i_ = Slider_i_.GetComponent<Slider>();
            int expCurrent_i_ = expStart_i_;
            int expDeltaSum_i_ = 0;
            int levelCurrent_i_ = levelAtStart_i_;
            float timeMax_ = 0.5f;

            for (int i = 0; i < 9999; i++)
            {
                int expDeltaDelta_ = (expDelta_ * Time.deltaTime / timeMax_).ToInt().Clamp(1, expDelta_ - expDeltaSum_i_);
                expCurrent_i_ = expCurrent_i_ + expDeltaDelta_;
                expDeltaSum_i_ = expDeltaSum_i_ + expDeltaDelta_;

                if (expCurrent_i_ > Table.ExpTable[hero_i_._parameter._class][levelCurrent_i_])
                {
                    Instance.StartCoroutine(WinBattle_AnimateExpBar_LevelUp(Slider_i_, hero_i_, levelCurrent_i_));
                    expCurrent_i_ -= Table.ExpTable[hero_i_._parameter._class][levelCurrent_i_++];
                }

                slider_i_.maxValue = Table.ExpTable[hero_i_._parameter._class][levelCurrent_i_];
                slider_i_.value = expCurrent_i_;

                if (expDeltaSum_i_ >= expDelta_)
                    break;

                yield return null;
            }
        }

        IEnumerator WinBattle_AnimateExpBar_LevelUp(GameObject Slider_i_, _Unit unit_i_, int levelCurrent_)
        {
            GameObject Level_i_ = Slider_i_.transform.parent.Find("Lv.").gameObject;
            Level_i_.Find("Text").GetComponent<TextMeshProUGUI>().text = (levelCurrent_ + 1).ToString();

            GameObject LevelClone_i_ = Instantiate(Level_i_, Level_i_.transform.parent);
            Graphic[] graphics_i_ = LevelClone_i_.GetComponentsInChildren<Graphic>();

            for (float timeSum_ = 0, timeMax_ = 0.6f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
            {
                float p = timeSum_ / timeMax_;
                LevelClone_i_.transform.localScale = (1 + p * 0.8f)*Level_i_.transform.localScale;
                ConfigureGraphicsAlpha(graphics_i_, 1 - p);

                yield return null;
            }

            Destroy(LevelClone_i_);
        }
    }

    public static void ConfigureSuggest(string inputState_)
    {
        if (inputState_ == "StartGame" || inputState_ == "ExecutingSBA" || inputState_ == "AnimatingUI" || inputState_ == "WinBattle")
        {
            txSuggest.text = "";
        }
        else if (inputState_ == "EnemyMain")
        {
            txSuggest.text = "Enemy Turn";
        }
        else if (inputState_ == "GameOver")
        {
            txSuggest.text = "";
        }
        else if (inputState_ == "HeroMain")
        {
            txSuggest.text = "<sprite name=ClickLeft>,<sprite name=ClickRight>:Select Unit";
        }
        else if (inputState_ == "HeroOnActive")
        {
            txSuggest.text = "<sprite name=ClickLeft>:Select Unit / <sprite name=ClickRight>:Move";
        }
        else if (inputState_ == "HeroOnMove")
        {
            txSuggest.text = "<sprite name=ClickLeft>:Cancel / <sprite name=ClickRight>:Move";
        }
        else if (inputState_ == "HeroOnCastSkill")
        {
            txSuggest.text = "<sprite name=ClickLeft>:Cast Skill / <sprite name=ClickRight>:Cancel";
        }
        else if (inputState_ == "HeroOnUseItem")
        {
            txSuggest.text = "<sprite name=ClickLeft>:Use Item / <sprite name=ClickRight>:Cancel";
        }
        else if (inputState_ == "Swap")
        {
            txSuggest.text = "";
        }
        else if (inputState_ == "GoToNextSpot")
        {
            txSuggest.text = "<sprite name=ClickLeft>:Select Next Spot";
        }
        else if (inputState_ == "CheckTheMap")
        {
            txSuggest.text = "Click the Map icon to back to battle.";
        }
        else if (inputState_ == "Treasure")
        {

        }
        else
        {
            Debug.Log("Invalid inputState " + inputState_);
        }
    }

    public static void ConfigureGraphicsAlpha(Graphic[] graphicsList_, float a_)
    {
        foreach (Graphic graphic_ in graphicsList_)
        {
            graphic_.color = new Color(graphic_.color.r, graphic_.color.g, graphic_.color.b, a_);
        }
    }

    public static void HideWinBattle()
    {
        foreach (Graphic graphic_i_ in graphicsWinBattle)
        {
            graphic_i_.gameObject.SetActive(false);
        }
    }

    public static void HideGameOver()
    {
        foreach (Graphic graphic_i_ in graphicsGameOver)
        {
            graphic_i_.gameObject.SetActive(false);
        }
    }

    public static IEnumerator ShowChapter(string chapterType_)
    {
        Globals.InputState = "AnimatingUI";
        float timeMax00_ = 0.8f;
        float timeMax01_ = 0.4f;

        if (chapterType_ == "Hero")
        {
            grChapterArray[0].color = new Color32(000, 255, 000, 000);
            grChapterArray[2].color = new Color32(000, 255, 000, 000);
            txChapter.text = "HERO TURN";
        }
        else if (chapterType_ == "Enemy")
        {
            grChapterArray[0].color = new Color32(255, 000, 000, 000);
            grChapterArray[2].color = new Color32(255, 000, 000, 000);
            txChapter.text = "ENEMY TURN";
        }
        else if (chapterType_ == "StartBattle")
        {
            grChapterArray[0].color = new Color32(255, 255, 000, 000);
            grChapterArray[2].color = new Color32(255, 255, 000, 000);
            txChapter.text = "START BATTLE";
        }

        goChapter.SetActive(true);

        for (float timeSum = 0; timeSum < timeMax00_ + Globals.timeDeltaFixed; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax00_).Clamp(0, 1);
            ConfigureGraphicsAlpha(grChapterArray, p_);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f / Globals.Instance.gameSpeed);

        for (float timeSum = 0; timeSum < timeMax01_ + Globals.timeDeltaFixed; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = 1 - (timeSum / timeMax01_).Clamp(0, 1);
            ConfigureGraphicsAlpha(grChapterArray, p_);
            yield return null;
        }
        goChapter.SetActive(false);
    }

    public static IEnumerator ShowBossWarning()
    {
        Globals.InputState = "AnimatingUI";
        float timeMax00_ = 0.4f;
        float timeMax01_ = 0.4f;

        goBossWarning.SetActive(true);
        Instance.StartCoroutine(ShowBossWarning_Scroll(3.0f));

        for (float timeSum_ = 0; timeSum_ < timeMax00_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax00_).Clamp(0, 1);
            ConfigureGraphicsAlpha(grBossWarningArray, p_);
            yield return null;
        }

        yield return new WaitForSeconds(2.2f / Globals.Instance.gameSpeed);

        for (float timeSum_ = 0; timeSum_ < timeMax01_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = 1 - (timeSum_ / timeMax01_).Clamp(0, 1);
            ConfigureGraphicsAlpha(grBossWarningArray, p_);
            yield return null;
        }

        goBossWarning.SetActive(false);
    }
    public static IEnumerator ShowBossWarning_Scroll(float timeMax_)
    {
        float moveLength_ = 500;

        GameObject WarningUpper = goBossWarning.transform.Find("WarningUpper").gameObject;
        GameObject WarningLower = goBossWarning.transform.Find("WarningLower").gameObject;
        WarningUpper.GetComponent<RectTransform>().anchoredPosition = new Vector3(-moveLength_ / 2, -57.5f, 0);
        WarningLower.GetComponent<RectTransform>().anchoredPosition = new Vector3(+moveLength_ / 2, +57.5f, 0);

        for (float timeSum_ = 0; timeSum_ < timeMax_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float d_ = Globals.timeDeltaFixed / timeMax_;

            WarningUpper.transform.position += Vector3.right * moveLength_ * d_;
            WarningLower.transform.position -= Vector3.right * moveLength_ * d_;

            yield return null;
        }
    }

    public static void ShowWinBattle()
    {
        //foreach (Graphic graphic_i_ in graphicsWinBattle)
        //{
        //    graphic_i_.gameObject.SetActive(true);
        //}
    }

    public static void ShowGameOver()
    {
        foreach (Graphic graphic_i_ in graphicsGameOver)
        {
            graphic_i_.gameObject.SetActive(true);
        }
    }
}
