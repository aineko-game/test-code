using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : _Singleton<MainMenu>
{
    public static GameObject goTitle;
    public static GameObject goNewGame;
    public static GameObject goConfirmation;
    public static GameObject goBlackScreen;
    public static GameObject goInputProof;

    protected override void Awake()
    {
        base.Awake();

        goTitle = Instance.transform.Find("Title").gameObject;
        goNewGame = Instance.transform.Find("NewGame").gameObject;
        goConfirmation = Instance.transform.Find("Confirmation").gameObject;
        goBlackScreen = Instance.transform.Find("BlackScreen").gameObject;
        goInputProof = Instance.transform.Find("InputProof").gameObject;
    }

    private void OnEnable()
    {
        Instance.InitializeGameObjects();
        Instance.ConfigureMainMenu();
    }

    public void InitializeGameObjects()
    {
        goTitle.GetComponent<RectTransform>().StretchToThisAnchor(Vector2.zero, Vector2.zero + Vector2.one);
        goNewGame.GetComponent<RectTransform>().StretchToThisAnchor(Vector2.right, Vector2.right + Vector2.one);
        goBlackScreen.GetComponent<RectTransform>().StretchToThisAnchor(Vector2.one, Vector2.one + Vector2.one);
    }

    public void ConfigureMainMenu()
    {
        goTitle.Find("Option01_Continue").GetComponent<Button>().interactable = (SaveData.GetClass<Globals>("Globals", null) != null);
    }

    public void TryToOpenNewGame()
    {
        if (SaveData.GetClass<Globals>("Globals", null) == null)
        {
            OpenNewGame();
        }
        else
        {
            goConfirmation.SetActive(true);
        }
    }

    public void OpenNewGame()
    {
        General.Instance.StartCoroutine(SlideAndShow(goNewGame, Vector2.right, Vector2.zero));
    }
    public void CloseNewGame()
    {
        Instance.StartCoroutine(SlideAndShow(goNewGame, Vector2.zero, Vector2.right));
    }
    public void Continue()
    {
        Instance.gameObject.SetActive(false);
        Instance.transform.parent.Find("BlackScreen").gameObject.SetActive(true);
         General.Instance.StartCoroutine(Continue_Coroutine());

        IEnumerator Continue_Coroutine()
        {
            goInputProof.SetActive(true);
            yield return null;

            General.LoadDataAll();
            yield return new WaitForSeconds(0.5f);

            Image image_ = Instance.transform.parent.Find("BlackScreen").GetComponent<Image>();
            for (float timeSum_ = 0, timeMax_ = 1.2f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
            {
                float p_ = timeSum_ / timeMax_;
                image_.color = new Color(0, 0, 0, 1 - p_);

                yield return null;
            }
            image_.color = Color.black;
            Instance.transform.parent.Find("BlackScreen").gameObject.SetActive(false);
            goInputProof.SetActive(false);
        }
    }

    public void StartAdventure()
    {
        General.Instance.StartCoroutine(StartAdventure_Coroutine());

        IEnumerator StartAdventure_Coroutine()
        {
            yield return SlideAndShow(goBlackScreen, Vector2.right, Vector2.zero);

            General.StartAdventure();

            yield return new WaitForSeconds(0.5f);

            Instance.gameObject.SetActive(false);
            Instance.transform.parent.Find("BlackScreen").gameObject.SetActive(true);
            Image image_ = Instance.transform.parent.Find("BlackScreen").GetComponent<Image>();
            for (float timeSum_ = 0, timeMax_ = 1.2f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
            {
                float p_ = timeSum_ / timeMax_;
                image_.color = new Color(0, 0, 0, 1 - p_);

                yield return null;
            }
            image_.color = Color.black;
            Instance.transform.parent.Find("BlackScreen").gameObject.SetActive(false);
        }
    }

    public IEnumerator SlideAndShow(GameObject go_, Vector2 anchorStart_, Vector2 anchorEnd_, float timeMax_ = 0.4f)
    {
        goInputProof.SetActive(true);

        RectTransform rt_ = go_.GetComponent<RectTransform>();
        Vector2 anchorMinStart_ = new Vector2(anchorStart_.x + 0, anchorStart_.y + 0);
        Vector2 anchorMaxStart_ = new Vector2(anchorStart_.x + 1, anchorStart_.y + 1);
        Vector2 anchorMinEnd_ = new Vector2(anchorEnd_.x + 0, anchorEnd_.y + 0);
        Vector2 anchorMaxEnd_ = new Vector2(anchorEnd_.x + 1, anchorEnd_.y + 1);

        for (float timeSum_ = 0; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
        {
            float p_ = (timeSum_ / timeMax_).ToSigmoid();
            Vector2 anchorMin_ = Library.BezierLiner(anchorMinStart_, anchorMinEnd_, p_);
            Vector2 anchorMax_ = Library.BezierLiner(anchorMaxStart_, anchorMaxEnd_, p_);

            rt_.StretchToThisAnchor(anchorMin_, anchorMax_);

            yield return null;
        }
        rt_.StretchToThisAnchor(anchorMinEnd_, anchorMaxEnd_);
        yield return null;

        goInputProof.SetActive(false);
    }
}
