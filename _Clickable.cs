using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class _Clickable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public _Skill _skillRef;
    public bool _isClickable = true;
    public bool _isDoubleClickable = false;
    public bool _isDraggable = false;
    public bool _isDisplayIconWhileDragging = false;

    public bool _isDragging = false;

    public float _lastTimeClicked = 0;
    public float _doubleClickInterval;

    public _Tooltip _tooltip;
    public _Tooltip _miniTooltip;
    
    private void Awake()
    {
        if (GetComponent<Button>())
        {
            tag = "Button";
        }
        else if (GetComponent<_Unit>())
        {
            tag = "Unit";
        }
        else if (name.Slice(0, 4) == "Spot")
        {
            tag = "Spot";
        }
        else if (name.Slice(0, 5) == "Token")
        {
            tag = "Token";
        }
    }

    private void Update()
    {
        if (_isDragging == false) return;

        if (Prefabs.goSwapImage.activeSelf == true)
        {
            Prefabs.rtSwapImageBack.position = Globals.posOnCursorAtScreen;
            Prefabs.rtSwapImageImage.position = Globals.posOnCursorAtScreen;
        }

        if (gameObject.name.Slice(00, 11) == "StatusSkill" && int.TryParse(gameObject.name.Slice(11, 13), out int out_) && out_.IsBetween(2, 4))
        {
            RectTransform rt_ = gameObject.GetComponent<RectTransform>();

            gameObject.transform.position = gameObject.transform.position * Vector2.right + Globals.posOnCursorAtScreen * Vector2.up;
            rt_.anchoredPosition = rt_.anchoredPosition.ClampByVector2(new Vector2(0, -480), new Vector2(Screen.width, -250));
            gameObject.transform.SetAsLastSibling();

            List<GameObject> goList_ = new List<GameObject> { gameObject.transform.parent.Find("StatusSkill02").gameObject, gameObject.transform.parent.Find("StatusSkill03").gameObject,
                                                              gameObject.transform.parent.Find("StatusSkill04").gameObject };
            goList_ = goList_.OrderBy(m => Vector3.Distance(m.transform.position, new Vector2(905, 500))).ToList();
            List<Vector2> posList_ = new List<Vector2> { new Vector2(0, -260), new Vector2(0, -365), new Vector2(0, -470) };

            for (int i = 0; i < goList_.Count; i++)
            {
                goList_[i].GetComponent<RectTransform>().anchoredPosition = Library.BezierLiner(goList_[i].GetComponent<RectTransform>().anchoredPosition, posList_[i], 0.15f);
            }
        }
        else if (gameObject.name.Slice(00, 15) == "StatusAll_Skill" && int.TryParse(gameObject.name.Slice(15, 17), out out_) && out_.IsBetween(1, 3))
        {
            RectTransform rt_ = gameObject.GetComponent<RectTransform>();

            gameObject.transform.position = gameObject.transform.position * Vector2.right + Globals.posOnCursorAtScreen * Vector2.up;
            rt_.anchoredPosition = rt_.anchoredPosition.ClampByVector2(new Vector2(0, -220), new Vector2(Screen.width, -105));
            gameObject.transform.SetAsLastSibling();

            List<GameObject> goList_ = new List<GameObject> { gameObject.transform.parent.Find("StatusAll_Skill01").gameObject, gameObject.transform.parent.Find("StatusAll_Skill02").gameObject,
                                                              gameObject.transform.parent.Find("StatusAll_Skill03").gameObject };
            goList_ = goList_.OrderBy(m => Vector3.Distance(m.transform.position, new Vector2(905, 500))).ToList();

            List<Vector2> posList_ = new List<Vector2> { new Vector2(0, -107.5f), new Vector2(0, -162.5f), new Vector2(0, -217.5f) };
            for (int i = 0; i < goList_.Count; i++)
            {
                goList_[i].GetComponent<RectTransform>().anchoredPosition = Library.BezierLiner(goList_[i].GetComponent<RectTransform>().anchoredPosition, posList_[i], 0.2f);
            }
        }
    }

    public bool IsHaveContents()
    {
        if (_tooltip is _Tooltip && _tooltip._title.IsNullOrEmpty() == false) return true;
        if (_miniTooltip is _Tooltip && _miniTooltip._title.IsNullOrEmpty() == false) return true;

        return false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData_)
    {
        if (_tooltip is _Tooltip)
        {
            UI.ConfigureTooltip(this);
            UI.ConfigureMiniTooltip(this);
        }
        Globals._clOnActive = this;

        if (tag == "Unit" && GetComponent<_Unit>() is _Unit unit_)
        {
            Globals.unitOnMouseover = unit_;
        }
        else if (tag == "Spot" && int.TryParse(name.Slice(4, 6), out _) && int.Parse(name.Slice(4, 6)).IsBetween(0, Globals.Instance.spotsArray.Length))
        {
            Globals.spotOnMouseover = Globals.Instance.spotsArray[int.Parse(name.Slice(4, 6))];
            Map.DrawMap();
        }
    }

    public void OnPointerExit(PointerEventData pointerData)
    {
        if (Globals._clOnActive == this)
            Globals._clOnActive = default;

        if (tag == "Unit" && GetComponent<_Unit>() is _Unit unit_)
        {
            if (Globals.unitOnMouseover == unit_)
                Globals.unitOnMouseover = default;
        }
        else if (tag == "Spot" && int.TryParse(name.Slice(4, 6), out _) && int.Parse(name.Slice(4, 6)).IsBetween(0, Globals.Instance.spotsArray.Length))
        {
            if (Globals.spotOnMouseover == Globals.Instance.spotsArray[int.Parse(name.Slice(4, 6))])
                Globals.spotOnMouseover = default;
            Map.DrawMap();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData_)
    {
        if (tag == "Button")
        {
            ButtonOnPointerClick(pointerEventData_);
        }
        else if (tag == "Field")
        {
            FieldOnPointerClick(pointerEventData_);
        }
        else if (tag == "Unit")
        {
            UnitOnPointerClick(pointerEventData_);
        }
        else if (tag == "Spot")
        {
            StartCoroutine(Map.MoveTokenToSpot(gameObject.name));
        }
        else if (tag == "Token")
        {
            Map.SwitchHeroToken(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData_)
    {
        if (_isDraggable == false) return;
        if (_tooltip == null || _tooltip._title.IsNullOrEmpty()) return;
        _isDragging = true;

        if (_isDisplayIconWhileDragging)
        {
            Prefabs.goSwapImage.SetActive(true);
            Prefabs.goSwapImage.transform.Find("IconBack").GetComponent<RectTransform>().sizeDelta = gameObject.Find("IconBack").GetComponent<RectTransform>().sizeDelta;
            Prefabs.goSwapImage.transform.Find("IconBack").GetComponent<Image>().sprite = gameObject.Find("IconBack").GetComponent<Image>().sprite;
            Prefabs.goSwapImage.transform.Find("IconBack").GetComponent<Image>().color = gameObject.Find("IconBack").GetComponent<Image>().color;
            Prefabs.goSwapImage.transform.Find("IconImage").GetComponent<RectTransform>().sizeDelta = gameObject.Find("IconImage").GetComponent<RectTransform>().sizeDelta;
            Prefabs.goSwapImage.transform.Find("IconImage").GetComponent<Image>().sprite = gameObject.Find("IconImage").GetComponent<Image>().sprite;
            Prefabs.goSwapImage.transform.Find("IconImage").GetComponent<Image>().color = gameObject.Find("IconImage").GetComponent<Image>().color;
        }
    }

    public void OnDrag(PointerEventData eventData_)
    {
    }

    public void OnEndDrag(PointerEventData eventData_)
    {
        _isDragging = false;
        Prefabs.SetCursorInactive();
        Prefabs.goSwapImage.SetActive(false);
        if (Globals.InputState == "HeroOnUseItem")
            Battle.SetInactiveItemSuggestion();

        if (_isDraggable == false) return;
        if (_tooltip == null || _tooltip._title.IsNullOrEmpty()) return;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData_, hits);

        if (gameObject.name.Slice(0, 4) == "Item")
        {
            if (int.TryParse(gameObject.name.Slice(4, 6), out int index_i_) == false) return;
            if (index_i_.IsBetween(0, Globals.itemsInBagList.Count - 1) == false) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(0, 4) == "Item" && int.TryParse(hit.gameObject.name.Slice(4, 6), out int index_j_) && index_j_.IsBetween(0, Globals.itemsInBagList.Count - 1))
                {
                    if (index_i_ == index_j_)
                    {
                        Battle.SetActiveItemSuggestion(index_i_);
                    }
                    if (Globals.itemsInBagList[index_i_] != null && Globals.itemsInBagList[index_j_] != null && Globals.itemsInBagList[index_i_]._name == Globals.itemsInBagList[index_j_]._name)
                    {
                        _Item.PutItemsTogether(Globals.itemsInBagList[index_i_], Globals.itemsInBagList[index_j_]);
                    }
                    else
                    {
                        _Item temp_ = Globals.itemsInBagList[index_i_];
                        Globals.itemsInBagList[index_i_] = Globals.itemsInBagList[index_j_];
                        Globals.itemsInBagList[index_j_] = temp_;

                        Battle.SetInactiveItemSuggestion();
                    }
                }
                else if (hit.gameObject.name == "ItemDiscard")
                {
                    Globals.itemToDiscard = Globals.itemsInBagList[index_i_];
                    UI.goGlobal.Find("ComfirmDiscard").gameObject.SetActive(true);
                }
            }
        }
        else if (gameObject.name.Slice(0, 5) == "Equip")
        {
            if (Globals.unitOnActive == null) return;
            if (int.TryParse(gameObject.name.Slice(5, 7), out int index_i_) == false) return;
            if (index_i_.IsBetween(0, Globals.unitOnActive._parameter._equips.Length) == false) return;

            _Unit unitSelf_ = Globals.unitOnActive;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(0, 5) == "Equip" && int.TryParse(hit.gameObject.name.Slice(5, 7), out int index_j_) && index_j_.IsBetween(0, Globals.itemsInBagList.Count - 1))
                {
                    _Equip temp_ = unitSelf_._parameter._equips[index_i_];
                    unitSelf_._parameter._equips[index_i_] = unitSelf_._parameter._equips[index_j_];
                    unitSelf_._parameter._equips[index_j_] = temp_;

                    UI.ConfigureEquipsUI(unitSelf_);
                }
            }
        }
        else if (gameObject.name.Slice(00, 15) == "StatusAll_Equip")
        {
            if (int.TryParse(gameObject.transform.parent.parent.name.Slice(04, 06), out int heroIndex_i_) == false) return;
            if (int.TryParse(gameObject.name.Slice(15, 17), out int equipIndex_j_) == false) return;

            _Hero hero_i_ = Globals.heroList[heroIndex_i_];

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 15) == "StatusAll_Equip")
                {
                    if (int.TryParse(hit.gameObject.transform.parent.parent.name.Slice(04, 06), out int heroIndex_k_) == false) continue;
                    if (int.TryParse(hit.gameObject.name.Slice(15, 17), out int equipIndex_l_) == false) continue;

                    _Hero hero_k_ = Globals.heroList[heroIndex_k_];
                    _Equip temp_ = hero_i_._parameter._equips[equipIndex_j_];
                    hero_i_._parameter._equips[equipIndex_j_] = hero_k_._parameter._equips[equipIndex_l_];
                    hero_k_._parameter._equips[equipIndex_l_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(09, 11), out int inventoryIndex_l_) == false) continue;

                    _Equip temp_ = hero_i_._parameter._equips[equipIndex_j_];
                    hero_i_._parameter._equips[equipIndex_j_] = Globals.inventoryList[inventoryIndex_l_];
                    Globals.inventoryList[inventoryIndex_l_] = temp_;
                }
                else if (hit.gameObject.name == "StatusDiscard")
                {
                    Globals.equipToDiscard = hero_i_._parameter._equips[equipIndex_j_];
                    UI.SetActive_StatusDiscard(hero_i_._parameter._equips[equipIndex_j_]);
                }
            }
        }
        else if (gameObject.name.Slice(00, 11) == "StatusEquip")
        {
            if (int.TryParse(gameObject.name.Slice(11, 13), out int index_i_) == false) return;
            if (index_i_.IsBetween(0, Globals.heroList[Globals.statusTabIndex - 1]._parameter._equips.Length) == false) return;

            _Hero hero_ = Globals.heroList[Globals.statusTabIndex - 1];
            
            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 11) == "StatusEquip" && int.TryParse(hit.gameObject.name.Slice(11, 13), out int index_j_) && index_j_.IsBetween(0, hero_._parameter._equips.Length))
                {
                    _Equip temp_ = hero_._parameter._equips[index_i_];
                    hero_._parameter._equips[index_i_] = hero_._parameter._equips[index_j_];
                    hero_._parameter._equips[index_j_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory" && int.TryParse(hit.gameObject.name.Slice(09, 11),out index_j_) && index_j_.IsBetween(0, Globals.inventoryList.Count))
                {
                    _Equip temp_ = hero_._parameter._equips[index_i_];
                    hero_._parameter._equips[index_i_] = Globals.inventoryList[index_j_];
                    Globals.inventoryList[index_j_] = temp_;
                }
                else if (hit.gameObject.name == "StatusDiscard")
                {
                    Globals.equipToDiscard = hero_._parameter._equips[index_i_];
                    UI.SetActive_StatusDiscard(hero_._parameter._equips[index_i_]);
                }
            }
        }
        else if (gameObject.name.Slice(00, 10) == "BasicEquip")
        {
            if (int.TryParse(gameObject.name.Slice(10, 12), out int index_i_) == false) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 10) == "BasicEquip")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(10, 12), out int index_k_) == false) continue;

                    _Equip temp_ = Globals.Instance.equipsToCombine[index_i_];
                    Globals.Instance.equipsToCombine[index_i_] = Globals.Instance.equipsToCombine[index_k_];
                    Globals.Instance.equipsToCombine[index_k_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 13) == "Combine_Equip")
                {
                    if (int.TryParse(hit.gameObject.transform.parent.name.Slice(12, 14), out int heroIndex_k_) == false) continue;
                    if (int.TryParse(hit.gameObject.name.Slice(13, 15), out int equipIndex_l_) == false) continue;

                    _Equip temp_ = Globals.Instance.equipsToCombine[index_i_];
                    Globals.Instance.equipsToCombine[index_i_] = Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_];
                    Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(09, 11), out int inventoryIndex_k_) == false) continue;

                    _Equip temp_ = Globals.Instance.equipsToCombine[index_i_];
                    Globals.Instance.equipsToCombine[index_i_] = Globals.inventoryList[inventoryIndex_k_];
                    Globals.inventoryList[inventoryIndex_k_] = temp_;
                }
                else if (hit.gameObject.name == "StatusDiscard")
                {
                    Globals.equipToDiscard = Globals.Instance.equipsToCombine[index_i_];
                    UI.SetActive_StatusDiscard(Globals.Instance.equipsToCombine[index_i_]);
                }
            }
        }
        else if (gameObject.name.Slice(00, 13) == "Combine_Equip")
        {
            if (int.TryParse(gameObject.transform.parent.name.Slice(12, 14), out int heroIndex_i_) == false) return;
            if (int.TryParse(gameObject.name.Slice(13, 15), out int equipIndex_j_) == false) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 13) == "Combine_Equip")
                {
                    if (int.TryParse(hit.gameObject.transform.parent.name.Slice(12, 14), out int heroIndex_k_) == false) return;
                    if (int.TryParse(hit.gameObject.name.Slice(13, 15), out int equipIndex_l_) == false) return;

                    _Equip temp_ = Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_];
                    Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_] = Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_];
                    Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 10) == "BasicEquip")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(10, 12), out int index_k_) == false) continue;
                    if (Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_]._type != "Basic") continue;

                    _Equip temp_ = Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_];
                    Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_] = Globals.Instance.equipsToCombine[index_k_];
                    Globals.Instance.equipsToCombine[index_k_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(09, 11), out int index_k_) == false) continue;

                    _Equip temp_ = Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_];
                    Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_] = Globals.inventoryList[index_k_];
                    Globals.inventoryList[index_k_] = temp_;
                }
                else if (hit.gameObject.name == "StatusDiscard")
                {
                    Globals.equipToDiscard = Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_];
                    UI.SetActive_StatusDiscard(Globals.heroList[heroIndex_i_]._parameter._equips[equipIndex_j_]);
                }
            }
        }
        else if (gameObject.name.Slice(00, 09) == "Inventory")
        {
            if (int.TryParse(gameObject.name.Slice(09, 11), out int index_i_) == false) return;
            if (index_i_.IsBetween(0, Globals.inventoryList.Count) == false) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 11) == "StatusEquip" && int.TryParse(hit.gameObject.name.Slice(11, 13), out int index_j_))
                {
                    if (index_j_.IsBetween(0, Globals.heroList[Globals.statusTabIndex - 1]._parameter._equips.Length) == false) continue;

                    _Hero hero_ = Globals.heroList[Globals.statusTabIndex - 1];
                    _Equip temp_ = Globals.inventoryList[index_i_];
                    Globals.inventoryList[index_i_] = hero_._parameter._equips[index_j_];
                    hero_._parameter._equips[index_j_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory" && int.TryParse(hit.gameObject.name.Slice(09, 11), out index_j_) && index_j_.IsBetween(0, Globals.inventoryList.Count))
                {
                    _Equip temp_ = Globals.inventoryList[index_i_];
                    Globals.inventoryList[index_i_] = Globals.inventoryList[index_j_];
                    Globals.inventoryList[index_j_] = temp_;
                }
                else if (hit.gameObject.name.Slice(0, 15) == "StatusAll_Equip")
                {
                    if (int.TryParse(hit.gameObject.transform.parent.parent.name.Slice(04, 06), out int heroIndex_k_) == false) continue;
                    if (int.TryParse(hit.gameObject.name.Slice(15, 17), out int equipIndex_l_) == false) continue;

                    _Equip temp_ = Globals.inventoryList[index_i_];
                    Globals.inventoryList[index_i_] = Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_];
                    Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 10) == "BasicEquip")
                {
                    if (int.TryParse(hit.gameObject.name.Slice(10, 12), out int basicIndex_k_) == false) continue;
                    if (Globals.inventoryList[index_i_]._type != "Basic") continue;

                    _Equip temp_ = Globals.inventoryList[index_i_];
                    Globals.inventoryList[index_i_] = Globals.Instance.equipsToCombine[basicIndex_k_];
                    Globals.Instance.equipsToCombine[basicIndex_k_] = temp_;
                }
                else if (hit.gameObject.name.Slice(00, 13) == "Combine_Equip")
                {
                    if (int.TryParse(hit.gameObject.transform.parent.name.Slice(12, 14), out int heroIndex_k_) == false) continue;
                    if (int.TryParse(hit.gameObject.name.Slice(13, 15), out int equipIndex_l_) == false) continue;

                    _Equip temp_ = Globals.inventoryList[index_i_];
                    Globals.inventoryList[index_i_] = Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_];
                    Globals.heroList[heroIndex_k_]._parameter._equips[equipIndex_l_] = temp_;
                }
                else if (hit.gameObject.name == "StatusDiscard")
                {
                    Globals.equipToDiscard = Globals.inventoryList[index_i_];
                    UI.SetActive_StatusDiscard(Globals.inventoryList[index_i_]);
                }
            }
        }
        else if (gameObject.name.Slice(00, 11) == "StatusSkill" && int.TryParse(gameObject.name.Slice(11, 13), out int out_) && out_.IsBetween(2, 4))
        {
            List<GameObject> goList_ = new List<GameObject> { gameObject.transform.parent.Find("StatusSkill02").gameObject, gameObject.transform.parent.Find("StatusSkill03").gameObject,
                                                              gameObject.transform.parent.Find("StatusSkill04").gameObject };
            List<GameObject> goListOrder_ = goList_.OrderBy(m => Vector3.Distance(m.transform.position, new Vector2(Screen.width / 2, Screen.height))).ToList();
            List<Vector2> posList_ = new List<Vector2> { new Vector2(0, -260), new Vector2(0, -365), new Vector2(0, -470) };
            _Unit unit_ = Globals.heroList[Globals.statusTabIndex - 1];
            List<_Skill> temp_ = unit_._parameter._skills.Skip(1).ToList();

            for (int i = 0; i < goList_.Count; i++)
            {
                goList_[i].transform.SetAsLastSibling();
                goList_[i].GetComponent<RectTransform>().anchoredPosition = posList_[i];
                unit_._parameter._skills[i + 1] = temp_[goList_.IndexOf(goListOrder_[i])];
            }
        }
        else if (gameObject.name.Slice(00, 15) == "StatusAll_Skill" && int.TryParse(gameObject.name.Slice(15, 17), out out_) && out_.IsBetween(1, 3))
        {
            List<GameObject> goList_ = new List<GameObject> { gameObject.transform.parent.Find("StatusAll_Skill01").gameObject, gameObject.transform.parent.Find("StatusAll_Skill02").gameObject,
                                                              gameObject.transform.parent.Find("StatusAll_Skill03").gameObject };
            List<GameObject> goListOrder_ = goList_.OrderBy(m => Vector3.Distance(m.transform.position, new Vector2(Screen.width / 2, Screen.height))).ToList();
            _Unit unit_ = Globals.heroList[int.Parse(gameObject.transform.parent.parent.name.Slice(04, 06))];
            List<_Skill> temp_ = unit_._parameter._skills.Skip(1).ToList();

            List<Vector2> posList_ = new List<Vector2> { new Vector2(0, -107.5f), new Vector2(0, -162.5f), new Vector2(0, -217.5f) };
            for (int i = 0; i < goList_.Count; i++)
            {
                goList_[i].transform.SetAsLastSibling();
                goList_[i].GetComponent<RectTransform>().anchoredPosition = posList_[i];
                unit_._parameter._skills[i + 1] = temp_[goList_.IndexOf(goListOrder_[i])];
            }
        }
        else if (gameObject.name.Slice(00, 09) == "Shop_Item" && int.TryParse(gameObject.name.Slice(09, 11), out int shopIndex_))
        {
            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 04) == "Item" && int.TryParse(hit.gameObject.name.Slice(04, 06), out int itemIndex_))
                {
                    _Item.Buy(shopIndex_, itemIndex_, true);
                }
            }
        }
        else if (gameObject.name.Slice(00, 10) == "Shop_Equip" && int.TryParse(gameObject.name.Slice(10, 12), out shopIndex_))
        {
            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.name.Slice(00, 15) == "StatusAll_Equip" && int.TryParse(hit.gameObject.name.Slice(15, 17), out int equipIndex_) &&
                    hit.gameObject.transform.parent.parent.name.Slice(00, 04) == "Hero" && int.TryParse(hit.gameObject.transform.parent.parent.name.Slice(04, 06), out int heroIndex_))
                {
                    _Equip.Buy(shopIndex_, heroIndex_, equipIndex_);
                }
                else if (hit.gameObject.name.Slice(00, 09) == "Inventory" && int.TryParse(hit.gameObject.name.Slice(09, 11), out int inventoryIndex_))
                {
                    _Equip.Buy(shopIndex_, -1, inventoryIndex_);
                }
            }
        }
        //else if (gameObject.name.Slice(00, 10) == "Shop_Skill" && int.TryParse(gameObject.name.Slice(10, 12), out shopIndex_))
        //{
        //    foreach (RaycastResult hit in hits)
        //    {
        //        if (hit.gameObject.name.Slice(00, 15) == "StatusAll_Skill" && int.TryParse(hit.gameObject.name.Slice(15, 17), out int skillIndex_) &&
        //            hit.gameObject.transform.parent.parent.name.Slice(00, 04) == "Hero" && int.TryParse(hit.gameObject.transform.parent.parent.name.Slice(04, 06), out int heroIndex_))
        //        {
        //            _Skill.Buy(shopIndex_, heroIndex_, skillIndex_);
        //        }
        //    }
        //}

        UI.ConfigureItemsUI();
        if (UI.goStatus.activeSelf)
            UI.ConfigureStatusUI();
        if (UI.goShop.activeSelf)
            UI.ConfigureShopUI();
    }

    public void ClickButton_OK(string type_)
    {
        if (type_ == "DiscardEquip")
        {
            for (int i = 0; i < Globals.inventoryList.Count; i++)
            {
                _Equip equip_i_ = Globals.inventoryList[i];

                if (equip_i_ == Globals.equipToDiscard)
                {
                    Globals.equipToDiscard = default;
                    Globals.inventoryList[i] = default;
                    UI.ConfigureStatusUI();
                    return;
                }
            }
            foreach (_Hero hero_i_ in Globals.heroList)
            {
                for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
                {
                    _Equip equip_i_j_ = hero_i_._parameter._equips[j];

                    if (equip_i_j_ == Globals.equipToDiscard)
                    {
                        Globals.equipToDiscard = default;
                        hero_i_._parameter._equips[j] = default;
                        UI.ConfigureStatusUI();
                        return;
                    }
                }
            }
            for (int i = 0; i < Globals.Instance.equipsToCombine.Length; i++)
            {
                _Equip equip_i_ = Globals.Instance.equipsToCombine[i];

                if (equip_i_ == Globals.equipToDiscard)
                {
                    Globals.equipToDiscard = default;
                    Globals.Instance.equipsToCombine[i] = default;
                    UI.ConfigureStatusUI();
                    return;
                }
            }
        }
        if (type_ == "DiscardItem")
        {
            for (int i = 0; i < Globals.itemsInBagList.Count; i++)
            {
                _Item item_i_ = Globals.itemsInBagList[i];

                if (item_i_ == Globals.itemToDiscard)
                {
                    Globals.itemToDiscard = default;
                    Globals.itemOnActive = null;
                    Globals.itemsInBagList[i] = default;
                    UI.ConfigureItemsUI();
                    if (UI.goShop.activeSelf)
                        UI.ConfigureShopUI();
                    ClickButton_OK_ReconfigureEventChoices();
                    return;
                }
            }
        }
        else if (type_ == "CombineEquips")
        {
            if (Globals.Instance.equipsToCombine[0] == null || Globals.Instance.equipsToCombine[0] == default) return;
            if (Globals.Instance.equipsToCombine[1] == null || Globals.Instance.equipsToCombine[1] == default) return;
            if (_Equip.CombineEquip(Globals.Instance.equipsToCombine[0], Globals.Instance.equipsToCombine[1]) == default) return;

            _Equip newEquip_ = _Equip.CombineEquip(Globals.Instance.equipsToCombine[0], Globals.Instance.equipsToCombine[1]);
            Globals.Instance.equipsToCombine[0] = default;
            Globals.Instance.equipsToCombine[1] = default;

            for (int i = 0; i < Globals.inventoryList.Count; i++)
            {
                if (Globals.inventoryList[i] == null || Globals.inventoryList[i] == default || Globals.inventoryList[i]._name.IsNullOrEmpty())
                {
                    Globals.inventoryList[i] = newEquip_;
                    UI.ConfigureStatusUI();
                    return;
                }
            }
            foreach (_Hero hero_i_ in Globals.heroList)
            {
                for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
                {
                    _Equip equip_i_j_ = hero_i_._parameter._equips[j];
                    if (equip_i_j_ == null || equip_i_j_ == default || equip_i_j_._name.IsNullOrEmpty())
                    {
                        hero_i_._parameter._equips[j] = newEquip_;
                        UI.ConfigureStatusUI();
                        return;
                    }
                }
            }
            UI.ConfigureStatusUI();
        }

        void ClickButton_OK_ReconfigureEventChoices()
        {
            Map._Spot spot_ = Globals.Instance.spotCurrent;
            GameObject[] goChoices_ = UI.goEvent.Find("Choices").GetChildrenGameObject();

            for (int i = 0; i < spot_._choicesList.Count; i++)
            {
                goChoices_[i].GetComponent<Button>().interactable = spot_._choicesList[i]._functionIsSelectable(spot_._choicesList[i]);
                goChoices_[i].Find("Dim").gameObject.SetActive(spot_._choicesList[i]._functionIsSelectable(spot_._choicesList[i]) == false);
            }
        }
    }

    public void ButtonOnPointerClick(PointerEventData pointerEventData_)
    {
        if (_isDoubleClickable && pointerEventData_.clickCount == 2)
        {
            ButtonOnPointerDoubleClick(pointerEventData_);
        }

        if (_isClickable == false) return;

        if (name == "EndTurn")
        {
            StartCoroutine(Battle.EndHeroTurn());
        }
        else if (name == "UndoMove")
        {
            Battle.TryUndoUnitMove();
        }
        else if (name == "ResetTurn")
        {
            Battle.TryResetTurn();
        }
        else if (name == "OpenSettings")
        {
            Setting.Instance.ConfigureSettingUI();
            Setting.goSettings.SetActive(true);
        }
        else if (name == "CloseSettings")
        {
            Setting.goSettings.SetActive(false);
        }
        else if (name == "OpenStatus")
        {
            UI.goStatus.SetActive(true);
            UI.ConfigureStatusUI();
            UI.ConfigureHeroesUI();
        }
        else if (name == "CloseStatus")
        {
            UI.goStatus.SetActive(false);
            UI.ConfigureHeroesUI();
        }
        else if (name.Slice(00, 09) == "StatusTab" && int.TryParse(name.Slice(09, 11), out int _))
        {
            General.Instance.StartCoroutine(UI.StatusUI_MoveTab(int.Parse(name.Slice(09, 11))));
        }
        else if (name.Slice(00, 07) == "ShopTab" && int.TryParse(name.Slice(07, 09), out int _))
        {
            General.Instance.StartCoroutine(UI.MoveTab_ShopUI(int.Parse(name.Slice(07, 09))));
        }
        else if (name == "Map")
        {
            if (Globals.inputStopperCount > 0) return;
            if (Globals.Instance.sceneState == "Battle" || Globals.Instance.sceneState == "Treasure")
                General.Instance.StartCoroutine(General.ChangeSceneWithFade("CheckTheMap", "BattleToMap"));
            else if(Globals.Instance.sceneState == "CheckTheMap")
            {
                if (Globals.Instance.turnState == "Treasure")
                    General.Instance.StartCoroutine(General.ChangeSceneWithFade("Treasure", "MapToBattle"));
                else
                    General.Instance.StartCoroutine(General.ChangeSceneWithFade("Battle", "MapToBattle"));
            }
        }
        else if (name == "OpenShop")
        {
            if (Globals.inputStopperCount < 1 && Globals.Instance.sceneState == "GoToNextSpot")
            {
                //Globals.shopUndoSave = new Globals.ShopUndoSave(Globals.Instance.Gold, Globals.itemsInBagList.DeepCopy(), Globals.Instance.spotCurrent._shopItems.DeepCopy());
                //Globals.shopUndoSave.Save();
                UI.ConfigureShopUI();
                UI.goShop.SetActive(true);
            }
        }
        else if (name == "CloseShop")
        {
            //Globals.shopUndoSave = default;
            UI.goShop.SetActive(false);
        }
        else if (name == "Shop_Undo")
        {
            Globals.Instance.shopUndoSave._Load();
            UI.ConfigureShopUI();
            UI.ConfigureItemsUI();
        }
        else if (name == "SkillTree_Undo")
        {
            Globals.heroList[Globals.statusTabIndex - 1]._SkillTree_Load();
            //Globals.Instance.skillTreesList[Globals.statusTabIndex - 1]._Load();
            UI.ConfigureStatusUI();
        }
        else if (name.Slice(00, 08) == "Treasure" && int.TryParse(name.Slice(08, 10), out _))
        {
            Map.SelectTreasure(int.Parse(name.Slice(08, 10)));
        }
        else if (name.Slice(0, 4) == "Hero" && int.TryParse(name.Slice(4, 6), out _) && int.Parse(name.Slice(4, 6)) < Globals.heroList.Count)
        {
            if (Globals.Instance.sceneState == "GoToNextSpot")
            {
                UI.goStatus.SetActive(true);
                UI.ConfigureStatusUI();
                UI.StatusUI_MoveTabImmediately(int.Parse(name.Slice(04, 06)) + 1);
            }
            else if (Globals.Instance.sceneState == "Battle")
            {
                Battle.SetUnitOnActive(Globals.heroList[int.Parse(name.Slice(4, 6))]);
            }
        }
        else if (name.Slice(0, 8) == "HeroMove" && int.TryParse(name.Slice(8, 10), out _) && int.Parse(name.Slice(8, 10)) < Globals.heroList.Count)
        {
            Battle.SetUnitOnActive(Globals.heroList[int.Parse(name.Slice(8, 10))]);
            Battle.SetActiveMoveSuggestion(Globals.heroList[int.Parse(name.Slice(8, 10))]);
        }
        else if (name.Slice(0, 10) == "HeroAttack" && int.TryParse(name.Slice(10, 12), out _) && int.Parse(name.Slice(10, 12)) < Globals.heroList.Count)
        {
            Battle.SetUnitOnActive(Globals.heroList[int.Parse(name.Slice(10, 12))]);
            Battle.SetActiveSkillSuggestion(Globals.heroList[int.Parse(name.Slice(10, 12))], 0);
        }
        else if (name.Slice(0, 4) == "Item" && int.TryParse(name.Slice(4, 6), out _))
        {
            Battle.SetActiveItemSuggestion(int.Parse(name.Slice(4, 6)));
        }
        else if (name.Slice(0, 5) == "Skill" && int.TryParse(name.Slice(5, 7), out _))
        {
            Battle.SetActiveSkillSuggestion(Globals.unitOnActive, int.Parse(name.Slice(5, 7)));
        }
        else if (name.Slice(0, 5) == "Equip" && int.TryParse(name.Slice(5, 7), out _))
        {
            Battle.SetActiveEquipSuggestion(Globals.unitOnActive, int.Parse(name.Slice(5, 7)));
        }
        else if (name.Slice(0, 4) == "Loot" && int.TryParse(name.Slice(4, 6), out _))
        {
            Battle.WinBattle_TakeLoot(int.Parse(name.Slice(4, 6)));
        }
        else if (name.Slice(0, 6) == "Choice" && int.TryParse(name.Slice(06, 08), out _))
        {
            int index_ = int.Parse(name.Slice(06, 08));
            if (index_ >= Globals.Instance.spotCurrent._choicesList.Count) return;

            Globals.Instance.spotCurrent._choicesList[index_]._functionExecuteEvent(Globals.Instance.spotCurrent._choicesList[index_], out bool isExecutable_);
            if (isExecutable_)
            {
                Globals.Instance.spotCurrent._isActive = false;
            }
        }
        else if (name.Slice(0, 7) == "Ability" && int.TryParse(name.Slice(7, 9), out _) && int.TryParse(transform.parent.name.Slice(15, 17), out _))
        {
            _Unit unit_ = Globals.heroList[Globals.statusTabIndex - 1];
            _Skill skill_ = unit_._parameter._skillTree[int.Parse(transform.parent.name.Slice(15, 17))];
            _SkillAbility ability_ = skill_._parameter._skillAbilities[int.Parse(name.Slice(07, 09))];

            if (skill_._parameter._isActive == true)
                _SkillAbility.ActivateAbility(unit_, skill_, ability_);
            else
                _Skill.ActivateSkill(unit_, skill_);
            UI.ConfigureStatusUI();
        }
        else if (name == "IconImage" && transform.parent.name.Slice(00, 15) == "StatusSkillTree" && int.TryParse(transform.parent.name.Slice(15, 17), out _))
        {
            _Unit unit_ = Globals.heroList[Globals.statusTabIndex - 1];
            _Skill skill_ = unit_._parameter._skillTree[int.Parse(transform.parent.name.Slice(15, 17))];
            //_SkillAbility ability_ = skill_._parameter._skillAbilities[int.Parse(transform.parent.name.Slice(07, 09))];

            _Skill.ActivateSkill(unit_, skill_);
            UI.ConfigureStatusUI();
        }
        else if (name.Slice(00, 15) == "StatusSkillTree" && int.TryParse(name.Slice(15, 17), out _))
        {
            _Unit unit_i_ = Globals.heroList[Globals.statusTabIndex - 1];

            _Skill.ActivateSkill(unit_i_, unit_i_._parameter._skillTree[int.Parse(name.Slice(15, 17))]);
            UI.ConfigureStatusUI();
        }
        else if (name == "GameOver")
        {
            UI.ShowGameOver();
        }
        else if (name == "NewAdventrue")
        {
            General.StartAdventure();
        }
        else if (name == "HideGameOver")
        {
            UI.HideGameOver();
        }
        else if (name == "WinBattle")
        {
            UI.ShowWinBattle();
        }
        else if (name == "HideWinBattle")
        {
            UI.HideWinBattle();
        }
        else if (name == "GoNext")
        {
            if (Globals.Instance.spotCurrent._lootsList.Count == 0)
                General.Instance.StartCoroutine(General.ChangeSceneWithFade("GoToNextSpot", "Normal"));
            else
                UI.goWinBattle.Find("Caution").gameObject.SetActive(true);
        }
        else if (name == "SkipLoots")
        {
            General.Instance.StartCoroutine(General.ChangeSceneWithFade("GoToNextSpot", "Normal"));
        }
        else if (name == "OK" || name == "NO")
        {

        }
        else if (name == "TreasureChest")
        {
            General.Instance.StartCoroutine(Map.OpenTreasureChest(gameObject));
        }
        else
        {
            Debug.LogError("Invalid button name " + name);
        }
    }

    public void ButtonOnPointerDoubleClick(PointerEventData pointerEventData_)
    {
        if (name.Slice(00, 09) == "Shop_Item" && int.TryParse(name.Slice(09, 11), out _))
        {
            _Item.Buy(int.Parse(name.Slice(09, 11)));
        }
        else if (name.Slice(00, 10) == "Shop_Equip" && int.TryParse(name.Slice(10, 12), out _))
        {
            _Equip.Buy(int.Parse(name.Slice(10, 12)));
        }
    }

    public void FieldOnPointerClick(PointerEventData pointerEventData_)
    {
        if (Globals.InputState == "HeroMain")
        {

        }
        else if (Globals.InputState == "HeroOnActive")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                Battle.SetUnitOnInactive();
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetActiveMoveSuggestion(Globals.unitOnActive);
            }
        }
        else if (Globals.InputState == "HeroOnMove")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                Battle.SetUnitOnInactive();
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.TryMove(Globals.unitOnActive);
            }
        }
        else if (Globals.InputState == "HeroOnCastSkill")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                if (Battle.TryCastSkill(Globals.unitOnActive, default, Globals.unitOnActive._skillOnActive) == true)
                {

                }
                else
                {
                    Battle.SetInactiveSkillSuggestion(Globals.unitOnActive);
                }
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetInactiveSkillSuggestion(Globals.unitOnActive);
            }
        }
        else if (Globals.InputState == "HeroOnUseItem")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                if (Battle.TryUseItem(default))
                {

                }
                else
                {
                    Battle.SetInactiveItemSuggestion();
                }
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetInactiveItemSuggestion();
            }
        }
    }

    public void UnitOnPointerClick(PointerEventData pointerEventData_)
    {
        if (GetComponent<_Unit>() == null) return;

        _Unit unitOnPointer_ = GetComponent<_Unit>();

        if (Globals.InputState == "HeroMain")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
                Battle.SetActiveMoveSuggestion(unitOnPointer_);
            }
        }
        else if (Globals.InputState == "HeroOnActive")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
                Battle.SetActiveMoveSuggestion(unitOnPointer_);
            }
        }
        else if (Globals.InputState == "HeroOnMove")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                if (Battle.TryMove(Globals.unitOnActive) == true)
                {

                }
                else
                {
                    Battle.SetUnitOnActive(unitOnPointer_);
                    Battle.SetActiveMoveSuggestion(unitOnPointer_);
                }
            }
        }
        else if (Globals.InputState == "HeroOnCastSkill")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                if (Battle.TryCastSkill(Globals.unitOnActive, unitOnPointer_, Globals.unitOnActive._skillOnActive) == true)
                {

                }
                else
                {
                    Battle.SetInactiveSkillSuggestion(Globals.unitOnActive);
                }
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetUnitOnActive(unitOnPointer_);
                Battle.SetActiveMoveSuggestion(unitOnPointer_);
            }
        }
        else if (Globals.InputState == "HeroOnUseItem")
        {
            if (pointerEventData_.button == PointerEventData.InputButton.Left)
            {
                if (Battle.TryUseItem(unitOnPointer_))
                {

                }
                else
                {
                    Battle.SetInactiveItemSuggestion();
                }
            }
            else if (pointerEventData_.button == PointerEventData.InputButton.Right)
            {
                Battle.SetInactiveItemSuggestion();
            }
        }
    }
}
