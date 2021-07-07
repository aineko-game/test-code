using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefabs : _Singleton<Prefabs>
{
    public static GameObject goOriginalUnits;
    public static GameObject goFields;
    //public static GameObject goInstances;

    public static GameObject goPlane_Y5;
    public static GameObject goMoveSuggestion;
    public static GameObject goSkillSuggestion;

    //public static GameObject goArrow;

    public static List<GameObject> goInstances = new List<GameObject>();
    public static List<GameObject> goParticles = new List<GameObject>();
    
    public static LineRenderer lrFieldOutline;

    public static Image imMoveArrow;
    public static Image imColliderArea;
    public static Image imTargetAreaCircle;
    public static Image imHitAreaCircle;
    public static Image imHitAreaArc;
    public static Image[] imHitAreaLines;
    public static Image imMoveAreaCircle;
    public static RectTransform rtMoveArrow;
    public static RectTransform[] rtHitAreaLines;

    public static Sprite[] spStatusIcons;

    public static Image imCursor;

    public static GameObject goSwapImage;
    public static RectTransform rtSwapImageBack;
    public static RectTransform rtSwapImageImage;

    public Material transitionIn;
    public Material transitionOut;
    public List<Sprite> transitionSprites;

    public List<GameObject> UnitComponents;

    public Sprite spDummy;
    public Sprite spTransparent;
    public Sprite spMoveArrowGreen;
    public Sprite spMoveArrowRed;

    protected override void Awake()
    {
        base.Awake();

        goOriginalUnits = Instance.transform.Find("OriginalUnits").gameObject;
        goFields = Instance.transform.Find("Fields").gameObject;
        //goInstances = Instance.transform.Find("Instances").gameObject;

        goPlane_Y5 = goFields.transform.Find("Plane_Y5").gameObject;
        goMoveSuggestion = goFields.transform.Find("MoveSuggestion").gameObject;
        goSkillSuggestion = goFields.transform.Find("SkillSuggestion").gameObject;

        //goArrow = goInstances.transform.Find("Arrow").gameObject;

        lrFieldOutline = goFields.transform.Find("FieldOutline").GetComponent<LineRenderer>();

        foreach (Transform child_ in Instance.transform.Find("Instances"))
        {
            goInstances.Add(child_.gameObject);
        }

        foreach (Transform child_ in Instance.transform.Find("Particles"))
        {
            goParticles.Add(child_.gameObject);
        }

        imMoveArrow = goMoveSuggestion.transform.Find("MoveArrow").GetComponent<Image>();
        imColliderArea = goMoveSuggestion.transform.transform.Find("ColliderArea").GetComponent<Image>();
        imTargetAreaCircle = goSkillSuggestion.transform.Find("TargetAreaCircle").GetComponent<Image>();
        imHitAreaCircle = goSkillSuggestion.transform.Find("HitAreaCircle").GetComponent<Image>();
        imHitAreaArc = goSkillSuggestion.transform.Find("HitAreaArc").GetComponent<Image>();
        //imHitAreaLine = goSkillSuggestion.transform.Find("HitAreaLine").GetComponent<Image>();
        rtMoveArrow = goMoveSuggestion.transform.Find("MoveArrow").GetComponent<RectTransform>();
        //rtHitAreaLine = goSkillSuggestion.transform.Find("HitAreaLine").GetComponent<RectTransform>();

        imHitAreaLines = new Image[5];
        rtHitAreaLines = new RectTransform[5];
        for (int i = 0; i < imHitAreaLines.Length; i++)
        {
            imHitAreaLines[i] = goSkillSuggestion.transform.Find("HitAreaLine" + i.ToString("D2")).GetComponent<Image>();
            rtHitAreaLines[i] = goSkillSuggestion.transform.Find("HitAreaLine" + i.ToString("D2")).GetComponent<RectTransform>();
        }

        spStatusIcons = Resources.LoadAll<Sprite>("StatusIcon/StatusIcons");

        goOriginalUnits.SetActive(false);
        goFields.SetActive(true);
        //goInstances.SetActive(false);

        goMoveSuggestion.SetActive(false);
        goSkillSuggestion.SetActive(false);
        
        lrFieldOutline.gameObject.SetActive(true);

        imCursor = Instance.transform.Find("CursorCanvas").Find("Cursor").GetComponent<Image>();

        goSwapImage = Instance.transform.Find("SwapImage").gameObject;
        rtSwapImageBack = Instance.transform.Find("SwapImage").Find("IconBack").GetComponent<RectTransform>();
        rtSwapImageImage = Instance.transform.Find("SwapImage").Find("IconImage").GetComponent<RectTransform>();


        if (spMoveArrowGreen == null) Debug.Log("Object is not assigned.");
        if (spMoveArrowRed == null) Debug.Log("Object is not assigned.");
    }

    public void LateUpdate()
    {
        if (imCursor.gameObject.activeSelf == true)
        {
            imCursor.transform.position = Globals.posOnCursorAtScreen;
        }
        //else if(goSwapImage.activeSelf == true)
        //{
        //    rtSwapImageBack.position = Globals.posOnCursorAtScreen;
        //    rtSwapImageImage.position = Globals.posOnCursorAtScreen;
        //}
    }

    public static void SetCursorActive(Sprite sprite_)
    {
        if (sprite_ == null) return;

        //Cursor.visible = false;
        imCursor.gameObject.SetActive(true);
        imCursor.sprite = sprite_;
    }

    public static void SetCursorInactive()
    {
        //Cursor.visible = true;
        imCursor.gameObject.SetActive(false);
    }
}
