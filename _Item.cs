using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class _Item
{
    public string _name;
    public string _descriptiveName;

    //public int _rank;
    public int _adDamage;
    public int _mdDamage;
    public int _sdDamage;
    public int _iValue00;
    public int _restoreValue;
    public List<string> _buffTypes = new List<string>();
    public List<int> _buffValues = new List<int>();
    //public int _useableStacks;
    public int _stackCount;
    public int _stackCountMax;
    public int _price;
    
    public float _hitRange;

    public string _castType = ""; //"Target", "AOE"
    public string _pathIcon;
    public string _effectColor;
    public float _effectSize = 1.0f;
    public List<string> _unitTypesTargetableList;

    public _Tooltip _tooltip;

    public delegate IEnumerator _IEnumerator(_Item item_, Vector3 posOnTarget_);
    public delegate void _FunctionVoid(_Item item_, Vector3 posOnTarget_);
    public delegate List<_Unit> _FunctionUnitList(_Item item_, Vector3 posOnTarget_);

    public _IEnumerator _Activate;
    public _FunctionVoid _SimulateEffect;
    public _FunctionVoid _DisplayArea;
    public _FunctionUnitList _DetectUnits;

    public static void Buy(int shopIndex_, int bagIndex_ = -1, bool isLoseGold_ = true)
    {
        if (shopIndex_.IsBetween(0, Globals.Instance.spotCurrent._shopItems.Length) == false) return;
        if (Globals.Instance.spotCurrent._shopItems[shopIndex_] == null || Globals.Instance.spotCurrent._shopItems[shopIndex_]._name.IsNullOrEmpty()) return;
        if (Globals.Instance.spotCurrent._shopItems[shopIndex_]._price > Globals.Instance.Gold && isLoseGold_ == true) return;

        _Item shopItem_ = Globals.Instance.spotCurrent._shopItems[shopIndex_];

        if (TryAndGetItem(shopItem_, bagIndex_) == true)
        {
            Globals.Instance.Gold -= Globals.Instance.spotCurrent._shopItems[shopIndex_]._price;
            Globals.Instance.spotCurrent._shopItems[shopIndex_] = null;
            UI.ConfigureItemsUI();
            UI.ConfigureShopUI();
        }
    }

    public static _Item CloneFromString(string itemName_)
    {
        if (itemName_.IsNullOrEmpty()) return default;

        _Item result_ = default;
        
        foreach (_Item item_i_ in OriginalItemList)
        {
            if (item_i_._name == itemName_)
            {
                return item_i_.DeepCopy();
            }
        }

        Debug.LogError("Invalid item name");
        return result_;
    }

    public static _Item DropItem(_Random random_)
    {
        float randomNum_ = random_.Range(0f, 100f);
        string rarity_ = (randomNum_ < 12.5f) ? "Rare" :
                         (randomNum_ < 25f) ? "Rare" :
                         /* else             */ "Common";

        return CloneFromString(Table.ItemTable[rarity_].GetRandom(random_));
    }

    public static bool IsGettableItem(_Item item_)
    {
        for (int i = 0; i < Globals.itemsInBagList.Count; i++)
        {
            if (Globals.itemsInBagList[i] != null && item_._name == Globals.itemsInBagList[i]._name && Globals.itemsInBagList[i]._stackCount < Globals.itemsInBagList[i]._stackCountMax)
            {
                return true;
            }
        }
        for (int i = 0; i < Globals.itemsInBagList.Count; i++)
        {
            if (Globals.itemsInBagList[i] == null || Globals.itemsInBagList[i]._name.IsNullOrEmpty())
            {
                return true;
            }
        }

        return false;
    }

    public static void PutItemsTogether(_Item item_i_, _Item item_j_)
    {
        if (item_i_ == item_j_) return;

        if (item_j_._stackCountMax >= item_i_._stackCount + item_j_._stackCount)
        {
            Globals.itemsInBagList[Globals.itemsInBagList.IndexOf(item_i_)] = null;
            Globals.itemsInBagList[Globals.itemsInBagList.IndexOf(item_j_)]._stackCount = item_i_._stackCount + item_j_._stackCount;
        }
        else
        {
            Globals.itemsInBagList[Globals.itemsInBagList.IndexOf(item_i_)]._stackCount = item_i_._stackCount + item_j_._stackCount - item_j_._stackCountMax;
            Globals.itemsInBagList[Globals.itemsInBagList.IndexOf(item_j_)]._stackCount = item_j_._stackCountMax;
        }
    }

    public static bool TryAndGetItem(_Item item_, int bagIndex_ = -1)
    {
        if (bagIndex_ == -1)
        {
            for (int i = 0; i < Globals.itemsInBagList.Count; i++)
            {
                if (Globals.itemsInBagList[i] != null && item_._name == Globals.itemsInBagList[i]._name && Globals.itemsInBagList[i]._stackCount < Globals.itemsInBagList[i]._stackCountMax)
                {
                    Globals.itemsInBagList[i]._stackCount += item_._stackCount;
                    UI.ConfigureItemsUI();
                    UI.ConfigureShopUI();
                    return true;
                }
            }
            for (int i = 0; i < Globals.itemsInBagList.Count; i++)
            {
                if (Globals.itemsInBagList[i] == null || Globals.itemsInBagList[i]._name.IsNullOrEmpty())
                {
                    Globals.itemsInBagList[i] = item_;
                    UI.ConfigureItemsUI();
                    UI.ConfigureShopUI();
                    return true;
                }
            }
        }
        else
        {
            _Item bagItem_ = Globals.itemsInBagList[bagIndex_];

            if (bagItem_ != null && item_._name == bagItem_._name && bagItem_._stackCount < bagItem_._stackCountMax)
            {
                Globals.itemsInBagList[bagIndex_]._stackCount += item_._stackCount;
                UI.ConfigureItemsUI();
                UI.ConfigureShopUI();
                return true;
            }
            else if (Globals.itemsInBagList[bagIndex_] == null || Globals.itemsInBagList[bagIndex_]._name.IsNullOrEmpty())
            {
                Globals.itemsInBagList[bagIndex_] = item_;
                UI.ConfigureItemsUI();
                UI.ConfigureShopUI();
                return true;
            }
        }
        return false;
    }

    public static readonly List<_Item> OriginalItemList = new List<_Item>()
    {
        new _Item //#Block Potion
        {
            _name = "Block Potion",
            _descriptiveName = "Block Potion",
            //_rank = 1,
            _iValue00 = 60,
            _stackCount = 1,
            _stackCountMax = 3,
            _price = 80,
            _castType = "Target",
            _pathIcon = "ItemIcon/Block Potion",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Block Potion").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Block Potion").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_BlockPotion,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalRestore,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Explosive Potion
        {
            _name = "Explosive Potion",
            _descriptiveName = "Explosive Potion",
            //_rank = 1,
            _sdDamage = 20,
            _stackCount = 1,
            _stackCountMax = 3,
            _price = 150,
            _hitRange = 2.5f,
            _castType = "AOE",
            _pathIcon = "ItemIcon/Explosive Potion",
            _unitTypesTargetableList = new List<string>{ "Enemy", "Object" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Explosive Potion").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Explosive Potion").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ExplosivePotion,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_MouseCircle,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Fire Potion
        {
            _name = "Fire Potion",
            _descriptiveName = "Fire Potion",
            //_rank = 1,
            _sdDamage = 60,
            _stackCount = 1,
            _stackCountMax = 3,
            _price = 120,
            _castType = "Target",
            _pathIcon = "ItemIcon/Fire Potion",
            _unitTypesTargetableList = new List<string>{ "Enemy" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Fire Potion").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Fire Potion").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_FirePotion,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Healing Potion
        {
            _name = "Healing Potion",
            _descriptiveName = "Healing Potion",
            //_rank = 1,
            _restoreValue = 60,
            _stackCount = 1,
            _stackCountMax = 3,
            _price = 120,
            _castType = "Target",
            _pathIcon = "ItemIcon/Healing Potion",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Healing Potion").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Healing Potion").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_HealingPotion,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalRestore,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Life
        {
            _name = "Rune of Life",
            _descriptiveName = "Rune of Life",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "HP" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Life",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Life").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Life").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Strength
        {
            _name = "Rune of Strength",
            _descriptiveName = "Rune of Strength",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "AD" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Strength",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Strength").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Strength").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Protection
        {
            _name = "Rune of Protection",
            _descriptiveName = "Rune of Protection",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "AR" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Protection",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Protection").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Protection").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Magic
        {
            _name = "Rune of Magic",
            _descriptiveName = "Rune of Magic",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "MD" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Magic",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Magic").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Magic").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Blessing
        {
            _name = "Rune of Blessing",
            _descriptiveName = "Rune of Blessing",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "MR" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Blessing",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Blessing").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Blessing").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Rune of Swift
        {
            _name = "Rune of Swift",
            _descriptiveName = "Rune of Swift",
            //_rank = 1,
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 120,
            _buffTypes = new List<string>{ "SP" },
            _buffValues = new List<int>{ 50 },
            _effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Rune of Swift",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Rune of Swift").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune of Swift").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ApplyBuff,
            _SimulateEffect = ItemFunction.SimulateEffect_NormalDamage,
            _DisplayArea = ItemFunction.DisplayArea_None,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Scroll of Blessing
        {
            _name = "Scroll of Blessing",
            _descriptiveName = "Scroll of Blessing",
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 240,
            //_effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Scroll of Blessing",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Scroll of Blessing").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Scroll of Blessing").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ScrollOfBlessing,
            _SimulateEffect = ItemFunction.SimulateEffect_None,
            _DisplayArea = ItemFunction.DisplayArea_AllHeroes,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Scroll of Fury
        {
            _name = "Scroll of Fury",
            _descriptiveName = "Scroll of Fury",
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 240,
            //_effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Scroll of Fury",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Scroll of Fury").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Scroll of Fury").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ScrollOfFury,
            _SimulateEffect = ItemFunction.SimulateEffect_None,
            _DisplayArea = ItemFunction.DisplayArea_AllHeroes,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
        new _Item //#Scroll of Protection
        {
            _name = "Scroll of Protection",
            _descriptiveName = "Scroll of Protection",
            _stackCount = 1,
            _stackCountMax = 1,
            _price = 240,
            //_effectColor = "Orange",
            _castType = "Target",
            _pathIcon = "ItemIcon/Scroll of Protection",
            _unitTypesTargetableList = new List<string>{ "Hero" },
            _tooltip = new _Tooltip
            {
                _type = "Item",
                _title = TextData.DescriptionText.GetValue("Scroll of Protection").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Scroll of Protection").GetIndexOf(1),
            },
            _Activate = ItemFunction.Activate_ScrollOfProtection,
            _SimulateEffect = ItemFunction.SimulateEffect_None,
            _DisplayArea = ItemFunction.DisplayArea_AllHeroes,
            _DetectUnits = ItemFunction.DetectUnits_MouseCircle,
        },
    };
}
