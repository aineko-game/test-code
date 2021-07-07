using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public class Setting : _Singleton<Setting>
{
    public static GameObject goSettings;
    
        //public string _seedPredefined;
    public _Parameter _parameter = new _Parameter();
    [Serializable]
    public class _Parameter
    {
        public int _optionValue00 = 0;
        public int _optionValue01 = 0;
        public int _optionValue02 = 0;
        public int _optionValue03 = 0;
        public bool _optionBool10 = true;
        public bool _optionBool11 = true;

        public string _seedPredefined;
    }

    protected override void Awake()
    {
        base.Awake();

        goSettings = transform.Find("Settings").gameObject;
        goSettings.SetActive(false);

        LoadSettingData();
    }

    private void OnDisable()
    {
        SaveSettingData();
    }

    public void SaveSettingData()
    {
        GameObject goLeft_ = Instance.transform.Find("Settings").Find("OptionsLeft").gameObject;
        GameObject goRight_ = Instance.transform.Find("Settings").Find("OptionsRight").gameObject;

        _parameter._optionValue00 = goLeft_.Find("Option00").GetComponentInChildren<TMP_Dropdown>().value;
        _parameter._optionValue01 = goLeft_.Find("Option01").GetComponentInChildren<TMP_Dropdown>().value;
        _parameter._optionValue02 = goLeft_.Find("Option02").GetComponentInChildren<Slider>().value.ToInt();
        _parameter._optionValue03 = goLeft_.Find("Option03").GetComponentInChildren<Slider>().value.ToInt();

        _parameter._optionBool10 = goRight_.Find("Option10").GetComponentInChildren<Toggle>().isOn;
        _parameter._optionBool11 = goRight_.Find("Option11").GetComponentInChildren<Toggle>().isOn;

        SaveData.SetClass("Setting", Instance._parameter);
        SaveData.Save();
    }

    public void LoadSettingData()
    {
        _parameter = SaveData.GetClass("Setting", _parameter);

        GameObject goLeft_ = Instance.transform.Find("Settings").Find("OptionsLeft").gameObject;
        GameObject goRight_ = Instance.transform.Find("Settings").Find("OptionsRight").gameObject;
        Vector2[] resoArray_ = new Vector2[] { new Vector2(1920, 1080), new Vector2(1768, 0992), new Vector2(1680, 1050), new Vector2(1600, 1024), new Vector2(1600, 0900),
                                               new Vector2(1440, 0900), new Vector2(1366, 0768), new Vector2(1360, 0768), new Vector2(1280, 0960), new Vector2(1280, 0800),
                                               new Vector2(1280, 0768), new Vector2(1280, 0720), new Vector2(1152, 0864), new Vector2(1024, 0768)};

        goLeft_.Find("Option00").GetComponentInChildren<TMP_Dropdown>().value = _parameter._optionValue00;
        goLeft_.Find("Option01").GetComponentInChildren<TMP_Dropdown>().value = _parameter._optionValue01;
        goLeft_.Find("Option02").GetComponentInChildren<Slider>().value = _parameter._optionValue02;
        goLeft_.Find("Option02").Find("SliderVolume").Find("Text_Volume").GetComponent<TextMeshProUGUI>().text = _parameter._optionValue02.ToString();
        goLeft_.Find("Option03").GetComponentInChildren<Slider>().value = _parameter._optionValue03;
        goLeft_.Find("Option03").Find("SliderVolume").Find("Text_Volume").GetComponent<TextMeshProUGUI>().text = _parameter._optionValue03.ToString();

        goRight_.Find("Option10").GetComponentInChildren<Toggle>().isOn = _parameter._optionBool10;
        goRight_.Find("Option11").GetComponentInChildren<Toggle>().isOn = _parameter._optionBool11;

        bool isFullScreen_ = _parameter._optionValue00 == 0;
        Vector2Int reso_ = resoArray_[_parameter._optionValue01].ToVector2Int();
        //int volume02_ = goLeft_.Find("Option02").Find("SliderVolume").GetComponent<Slider>().value.ToInt();
        //int volume03_ = goLeft_.Find("Option03").Find("SliderVolume").GetComponent<Slider>().value.ToInt();

        #if UNITY_WEBGL
                /*Skip*/;
        #else
                Screen.SetResolution(reso_.x, reso_.y, isFullScreen_);
        #endif

        //ApplySetting();
        ApplyGameSpeed();
    }

    public void BackToMainMenu()
    {
        MainMenu.Instance.gameObject.SetActive(true);
        goSettings.SetActive(false);

        if (Battle.IsGameOver() == false)
            General.SaveDataAll();

        General.Instance.StartCoroutine(MainMenu.Instance.SlideAndShow(MainMenu.goTitle, Vector2.up, Vector2.zero));
    }

    public static void ApplyGameSpeed()
    {
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._animator.speed = Globals.Instance.gameSpeed;
        }

        foreach (_Particles particle_i_ in Globals.particlesList)
        {
            foreach (ParticleSystem psChild_ in particle_i_._psChildrenList)
            {
                ParticleSystem.MainModule psMain_ = psChild_.main;
                psMain_.simulationSpeed = Globals.Instance.gameSpeed;
            }
        }

        foreach (GameObject go_ in Map.UnitTokenList)
        {
            if (go_ != null && go_.GetComponent<Animator>())
            {
                go_.GetComponent<Animator>().speed = Globals.Instance.gameSpeed;
            }
        }

        //UI.slGameSpeed.value = (Globals.Instance.gameSpeed - 1) / 0.5f;
    }

    //public void ConfigureGameSpeed(Slider slider_)
    //{
    //    Globals.Instance.gameSpeed = 1 + slider_.value / 2f;

    //    ApplySetting();
    //}

    public void ConfigureSettingUI()
    {
        GameObject goLeft_ = Instance.transform.Find("Settings").Find("OptionsLeft").gameObject;
        bool isFullScreen_ = Screen.fullScreen;
        Vector2[] resoArray_ = new Vector2[] { new Vector2(1920, 1080), new Vector2(1768, 0992), new Vector2(1680, 1050), new Vector2(1600, 1024), new Vector2(1600, 0900),
                                               new Vector2(1440, 0900), new Vector2(1440, 0810), new Vector2(1366, 0768), new Vector2(1360, 0768), new Vector2(1280, 0960), 
                                               new Vector2(1280, 0800), new Vector2(1280, 0768), new Vector2(1280, 0720), new Vector2(1152, 0864), new Vector2(1024, 0768) };
        goLeft_.Find("Option00").Find("Dropdown").GetComponent<TMP_Dropdown>().value = (isFullScreen_) ? 0 : 1;

        for (int i = 0; i < resoArray_.Length; i++)
        {
            if (resoArray_[i] == new Vector2(Screen.width, Screen.height))
            {
                goLeft_.Find("Option01").Find("Dropdown").GetComponent<TMP_Dropdown>().value = i;
                break;
            }
        }

        goLeft_.Find("Option02").Find("SliderVolume").GetComponentInChildren<TextMeshProUGUI>().text = goLeft_.Find("Option02").Find("SliderVolume").GetComponent<Slider>().value.ToString();
        goLeft_.Find("Option03").Find("SliderVolume").GetComponentInChildren<TextMeshProUGUI>().text = goLeft_.Find("Option03").Find("SliderVolume").GetComponent<Slider>().value.ToString();

        goSettings.Find("MainMenu").GetComponent<Button>().interactable = (MainMenu.Instance.gameObject.activeSelf == false);

        #if UNITY_WEBGL
            goLeft_.Find("Option00").Find("Text_Alert").gameObject.SetActive(true);
            goLeft_.Find("Option01").Find("Text_Alert").gameObject.SetActive(true);
        #else
            goLeft_.Find("Option00").Find("Text_Alert").gameObject.SetActive(false);
            goLeft_.Find("Option01").Find("Text_Alert").gameObject.SetActive(false);
        #endif
    }

    public void ConfigureGameSetting()
    {
        GameObject goLeft_ = Instance.transform.Find("Settings").Find("OptionsLeft").gameObject;
        float gameSpeed_ = UI.Instance.transform.Find("Global").Find("GameSpeed").GetComponent<Slider>().value / 2f + 1;
        bool isFullScreen_ = goLeft_.Find("Option00").Find("Dropdown").GetComponent<TMP_Dropdown>().value == 0;
        Vector2[] resoArray_ = new Vector2[] { new Vector2(1920, 1080), new Vector2(1768, 0992), new Vector2(1680, 1050), new Vector2(1600, 1024), new Vector2(1600, 0900),
                                               new Vector2(1440, 0900), new Vector2(1366, 0768), new Vector2(1360, 0768), new Vector2(1280, 0960), new Vector2(1280, 0800),
                                               new Vector2(1280, 0768), new Vector2(1280, 0720), new Vector2(1152, 0864), new Vector2(1024, 0768)};
        Vector2Int reso_ = resoArray_[goLeft_.Find("Option01").Find("Dropdown").GetComponent<TMP_Dropdown>().value].ToVector2Int();
        int volume02_ = goLeft_.Find("Option02").Find("SliderVolume").GetComponent<Slider>().value.ToInt();
        int volume03_ = goLeft_.Find("Option03").Find("SliderVolume").GetComponent<Slider>().value.ToInt();

        Globals.Instance.gameSpeed = gameSpeed_;

        #if UNITY_WEBGL
            /*Skip*/;
        #else
            Screen.SetResolution(reso_.x, reso_.y, isFullScreen_);
        #endif

        goLeft_.Find("Option02").Find("SliderVolume").Find("Text_Volume").GetComponent<TextMeshProUGUI>().text = volume02_.ToString();
        goLeft_.Find("Option03").Find("SliderVolume").Find("Text_Volume").GetComponent<TextMeshProUGUI>().text = volume03_.ToString();

        //ApplySetting();
        ApplyGameSpeed();
    }

    public void ConfigureSoundVolume(Slider slider_)
    {

    }

    public void OnValueChanged_SliderToggle(GameObject go_)
    {
        Slider slider_ = go_.GetComponent<Slider>();
        GameObject Bg_On = go_.transform.Find("Bg").Find("Bg_On").gameObject;
        GameObject Text_Off = go_.transform.Find("Text").Find("Text_Off").gameObject;
        GameObject Text_On = go_.transform.Find("Text").Find("Text_On").gameObject;
        GameObject Handle = go_.transform.Find("Handle").gameObject;
        GameObject Handle_On = go_.transform.Find("Handle").Find("Handle_On").gameObject;

        Bg_On.SetActive(slider_.value == 1);
        Text_Off.SetActive(slider_.value == 0);
        Text_On.SetActive(slider_.value == 1);
        Handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-45 + 90 * slider_.value, 0);
        Handle_On.SetActive(slider_.value == 1);
    }

    public void OnValueChanged_SliderVolume(GameObject go_)
    {
        Slider slider_ = go_.GetComponent<Slider>();
        TextMeshProUGUI tmpro_ = go_.GetComponentInChildren<TextMeshProUGUI>();

        tmpro_.text = slider_.value.ToString();
    }
}
