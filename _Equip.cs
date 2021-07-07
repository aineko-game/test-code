using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class _Equip
{
    public string _name;
    public string _descriptiveName;
    public string _type;
    public string _rarity;

    public int _price;
    public int _hpPlus;
    public int _adPlus;
    public int _arPlus;
    public int _mdPlus;
    public int _mrPlus;
    public int _spPlus;
    public int _castableLimitCount;
    public int _castableStacks;
    public int _cooldownRemaining;
    public int _cooldownDuration;

    public string _pathIcon;

    public _Skill _active;
    public _Skill _passive;
    public _Tooltip _tooltip;

    public static void Buy(int shopIndex_, int heroIndex_ = -2, int equipIndex_ = -2)
    {
        if (shopIndex_.IsBetween(0, Globals.Instance.spotCurrent._shopEquips.Length) == false) return;
        if (Globals.Instance.spotCurrent._shopEquips[shopIndex_] == null || Globals.Instance.spotCurrent._shopEquips[shopIndex_]._name.IsNullOrEmpty()) return;
        if (Globals.Instance.spotCurrent._shopEquips[shopIndex_]._price > Globals.Instance.Gold) return;

        _Equip shopEquip_ = Globals.Instance.spotCurrent._shopEquips[shopIndex_];

        GetEquip(shopEquip_, out bool isGettable_, heroIndex_, equipIndex_);

        if (isGettable_)
        {
            Globals.Instance.Gold -= Globals.Instance.spotCurrent._shopEquips[shopIndex_]._price;
            Globals.Instance.spotCurrent._shopEquips[shopIndex_] = null;
            UI.ConfigureItemsUI();
            UI.ConfigureShopUI();
        }

        //if (heroIndex_ < -1)
        //{
        //    for (int i = 0; i < Globals.inventoryList.Count; i++)
        //    {
        //        if (Globals.inventoryList[i] == null || Globals.inventoryList[i]._name.IsNullOrEmpty())
        //        {
        //            Globals.inventoryList[i] = shopEquip_;
        //            Deal();
        //            return;
        //        }
        //    }
        //    foreach (_Hero hero_i_ in Globals.heroList)
        //    {
        //        for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
        //        {
        //            if (hero_i_._parameter._equips[j] == null || hero_i_._parameter._equips[j]._name.IsNullOrEmpty())
        //            {
        //                hero_i_._parameter._equips[j] = shopEquip_;
        //                Deal();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else if (heroIndex_ == -1)
        //{
        //    if (Globals.inventoryList[equipIndex_] == null || Globals.inventoryList[equipIndex_]._name.IsNullOrEmpty())
        //    {
        //        Globals.inventoryList[equipIndex_] = shopEquip_;
        //        Deal();
        //    }
        //}
        //else
        //{
        //    if (Globals.heroList[heroIndex_]._parameter._equips[equipIndex_] == null || Globals.heroList[heroIndex_]._parameter._equips[equipIndex_]._name.IsNullOrEmpty())
        //    {
        //        Globals.heroList[heroIndex_]._parameter._equips[equipIndex_] = shopEquip_;
        //        Deal();
        //    }
        //}

        //void Deal()
        //{
        //    Globals.Instance.Gold -= Globals.Instance.spotCurrent._shopEquips[shopIndex_]._price;
        //    Globals.Instance.spotCurrent._shopEquips[shopIndex_] = null;
        //    UI.ConfigureItemsUI();
        //    UI.ConfigureShopUI();
        //}
    }

    public static _Equip CloneFromString(string equipName_)
    {
        if (equipName_.IsNullOrEmpty()) return default;

        _Equip result_ = default;

        foreach (_Equip equip_i_ in OriginalEquipList)
        {
            if (equip_i_._name == equipName_)
            {
                return equip_i_.DeepCopy();
            }
        }

        Debug.LogError("Invalid equip name : " + equipName_);
        return result_;
    }

    public static _Equip CombineEquip(_Equip base00_, _Equip base01_)
    {
        if (base00_ == null || base00_ == default) return default;
        if (base01_ == null || base01_ == default) return default;

        return CloneFromString(Table.ItemCombineTable.GetValue(new Tuple<string, string>(base00_._name, base01_._name)));
    }

    public static _Equip DropEquip(Map._Spot spot_, _Random random_)
    {
        float randomNum_ = random_.Range(0f, 100f);
        string rarity_ = /*(randomNum_ < 0f) ?*/ "Shop";
        string equipName_ = "";

        for (int iLoop_ = 0; iLoop_ < 99; iLoop_++)
        {
            equipName_ = Table.EquipTable[rarity_].GetRandom(random_);

            foreach (_Equip equip_j_ in spot_._shopEquips)
            {
                if (equip_j_ == null || equip_j_._name.IsNullOrEmpty())
                    return CloneFromString(equipName_);

                if (equip_j_._name == equipName_) break;

                if (equip_j_ == spot_._shopEquips.Last())
                    return CloneFromString(equipName_);
            }
        }
        Debug.LogError("Invalid drop!");
        return CloneFromString(Table.EquipTable[rarity_].GetRandom(random_));
    }

    public static void GetEquip(_Equip equip_, out bool isGettable_, int heroIndex_, int equipIndex_)
    {
        isGettable_ = true;

        if (heroIndex_ < -1)
        {
            for (int i = 0; i < Globals.inventoryList.Count; i++)
            {
                if (Globals.inventoryList[i] == null || Globals.inventoryList[i]._name.IsNullOrEmpty())
                {
                    Globals.inventoryList[i] = equip_;
                    return;
                }
            }
            foreach (_Hero hero_i_ in Globals.heroList)
            {
                for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
                {
                    if (hero_i_._parameter._equips[j] == null || hero_i_._parameter._equips[j]._name.IsNullOrEmpty())
                    {
                        hero_i_._parameter._equips[j] = equip_;
                        return;
                    }
                }
            }
        }
        else if (heroIndex_ == -1)
        {
            if (Globals.inventoryList[equipIndex_] == null || Globals.inventoryList[equipIndex_]._name.IsNullOrEmpty())
            {
                Globals.inventoryList[equipIndex_] = equip_;
                return;
            }
        }
        else
        {
            if (Globals.heroList[heroIndex_]._parameter._equips[equipIndex_] == null || Globals.heroList[heroIndex_]._parameter._equips[equipIndex_]._name.IsNullOrEmpty())
            {
                Globals.heroList[heroIndex_]._parameter._equips[equipIndex_] = equip_;
                return;
            }
        }

        isGettable_ = false;
    }

    public static bool IsGettableEquip(_Equip equip_)
    {
        for (int i = 0; i < Globals.inventoryList.Count; i++)
        {
            if (Globals.inventoryList[i] == null || Globals.inventoryList[i]._name.IsNullOrEmpty())
            {
                return true;
            }
        }
        foreach (_Hero hero_i_ in Globals.heroList)
        {
            for (int j = 0; j < hero_i_._parameter._equips.Length; j++)
            {
                if (hero_i_._parameter._equips[j] == null || hero_i_._parameter._equips[j]._name.IsNullOrEmpty())
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static readonly List<_Equip> OriginalEquipList = new List<_Equip>
    {
        new _Equip //#Abyssal Staff
        {
            _name = "Abyssal Staff",
            _descriptiveName = "Abyssal Staff",
            _type = "Combined",
            _rarity = "Common",
            _price = 1700,
            _mdPlus = 20,
            _mrPlus = 20,
            _pathIcon = "EquipIcon/Abyssal Staff",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Abyss"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Abyssal Staff").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Abyssal Staff").GetIndexOf(1),
            },
        },
        new _Equip //#Aegis Shield
        {
            _name = "Aegis Shield",
            _descriptiveName = "Aegis Shield",
            _type = "Combined",
            _rarity = "Common",
            _price = 1800,
            _adPlus = 30,
            _mrPlus = 30,
            _pathIcon = "EquipIcon/Aegis Shield",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Aegis"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Aegis Shield").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Aegis Shield").GetIndexOf(1),
            },
        },
        new _Equip //#Arcane Gauntlets
        {
            _name = "Arcane Gauntlets",
            _descriptiveName = "Arcane Gauntlets",
            _type = "Combined",
            _rarity = "Common",
            _price = 1600,
            _adPlus = 30,
            _arPlus = 20,
            _castableLimitCount = 99,
            _castableStacks = 99,
            _cooldownDuration = 6,
            _pathIcon = "EquipIcon/Arcane Gauntlets",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Divine Barrier"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Arcane Gauntlets").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Arcane Gauntlets").GetIndexOf(1),
            },
        },
        new _Equip //#Archmage Staff
        {
            _name = "Archmage Staff",
            _descriptiveName = "Archmage Staff",
            _type = "Shop",
            _rarity = "Common",
            _price = 2000,
            _mdPlus = 20,
            _pathIcon = "EquipIcon/Archmage Staff",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Mastery of Magic"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Archmage Staff").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Archmage Staff").GetIndexOf(1),
            },
        },
        new _Equip //#Armor of Vitality
        {
            _name = "Armor of Vitality",
            _descriptiveName = "Armor of Vitality",
            _type = "Combined",
            _rarity = "Common",
            _price = 1500,
            _arPlus = 20,
            _mrPlus = 20,
            _spPlus = 10,
            _pathIcon = "EquipIcon/Armor of Vitality",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Vitality"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Armor of Vitality").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Armor of Vitality").GetIndexOf(1),
            }
        },
        new _Equip //#Assassin Dagger
        {
            _name = "Assassin Dagger",
            _descriptiveName = "Assassin Dagger",
            _type = "Shop",
            _rarity = "Common",
            _price = 1700,
            _adPlus = 20,
            _pathIcon = "EquipIcon/Assassin Dagger",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Assassinate"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Assassin Dagger").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Assassin Dagger").GetIndexOf(1),
            },
        },
        new _Equip //#Berserker Mail
        {
            _name = "Berserker Mail",
            _descriptiveName = "Berserker Mail",
            _type = "Combined",
            _rarity = "Common",
            _price = 1600,
            _adPlus = 10,
            _arPlus = 30,
            _pathIcon = "EquipIcon/Berserker Mail",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Berserker"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Berserker Mail").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Berserker Mail").GetIndexOf(1),
            }
        },
        new _Equip //#Bloody Cleaver
        {
            _name = "Bloody Cleaver",
            _descriptiveName = "Bloody Cleaver",
            _type = "Combined",
            _rarity = "Common",
            _price = 1700,
            _adPlus = 20,
            _arPlus = 20,
            _pathIcon = "EquipIcon/Bloody Cleaver",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Carve"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Bloody Cleaver").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Bloody Cleaver").GetIndexOf(1),
            }
        },
        new _Equip //#Boots
        {
            _name = "Boots",
            _descriptiveName = "Boots",
            _type = "Basic",
            _rarity = "Common",
            _price = 800,
            _spPlus = 20,
            _pathIcon = "EquipIcon/Boots",
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Boots").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Boots").GetIndexOf(1),
            },
        },
        new _Equip //#Butcher Knife
        {
            _name = "Butcher Knife",
            _descriptiveName = "Butcher Knife",
            _type = "Shop",
            _rarity = "Common",
            _price = 1500,
            _adPlus = 30,
            _arPlus = 20,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Butcher Knife",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Dismantle"),
            _tooltip = new _Tooltip
            {
                _type = "_Equip",
                _title = TextData.DescriptionText.GetValue("Butcher Knife").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Butcher Knife").GetIndexOf(1),
            }
        },
        new _Equip //#Staff of Sunfire
        {
            _name = "Staff of Sunfire",
            _descriptiveName = "Staff of Sunfire",
            _type = "Shop",
            _rarity = "Common",
            _price = 1500,
            _mdPlus = 20,
            _arPlus = 20,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Staff of Sunfire",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Offering"),
            _tooltip = new _Tooltip
            {
                _type = "_Equip",
                _title = TextData.DescriptionText.GetValue("Staff of Sunfire").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Staff of Sunfire").GetIndexOf(1),
            }
        },
        new _Equip //#Burning Blood
        {
            _name = "Burning Blood",
            _descriptiveName = "Burning Blood",
            _type = "Combined",
            _rarity = "Common",
            _price = 1300,
            _hpPlus = 100,
            _pathIcon = "EquipIcon/Burning Blood",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Undying Rage"),
            _tooltip = new _Tooltip
            {
                _type = "_Equip",
                _title = TextData.DescriptionText.GetValue("Burning Blood").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Burning Blood").GetIndexOf(1),
            }
        },
        new _Equip //#Mini Hakkero
        {
            _name = "Mini Hakkero",
            _descriptiveName = "Mini Hakkero",
            _type = "Shop",
            _rarity = "Common",
            _price = 1600,
            _mdPlus = 20,
            _mrPlus = 10,
            _pathIcon = "EquipIcon/Mini Hakkero",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Spark!"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Mini Hakkero").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Mini Hakkero").GetIndexOf(1),
            },
        },
        new _Equip //#Cloak of Shadows
        {
            _name = "Cloak of Shadows",
            _descriptiveName = "Cloak of Shadows",
            _type = "Shop",
            _rarity = "Common",
            _price = 1500,
            _mrPlus = 30,
            _spPlus = 10,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Cloak of Shadows",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Shadow Hide You"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Cloak of Shadows").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Cloak of Shadows").GetIndexOf(1),
            },
        },
        new _Equip //#Colossus Hammer
        {
            _name = "Colossus Hammer",
            _descriptiveName = "Colossus Hammer",
            _type = "Shop",
            _rarity = "Common",
            _price = 1600,
            _adPlus = 40,
            _pathIcon = "EquipIcon/Colossus Hammer",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Skullcrusher"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Colossus Hammer").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Colossus Hammer").GetIndexOf(1),
            },
        },
        new _Equip //#Crystal
        {
            _name = "Crystal",
            _descriptiveName = "Crystal",
            _type = "Basic",
            _rarity = "Common",
            _price = 600,
            _hpPlus = 20,
            _pathIcon = "EquipIcon/Crystal",
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Crystal").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Crystal").GetIndexOf(1),
            },
        },
        new _Equip //#Dragon Slayer
        {
            _name = "Dragon Slayer",
            _descriptiveName = "Dragon Slayer",
            _type = "Combined",
            _rarity = "Common",
            _price = 2000,
            _adPlus = 40,
            _pathIcon = "EquipIcon/Dragon Slayer",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Giantkilling"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Dragon Slayer").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Dragon Slayer").GetIndexOf(1),
            }
        },
        new _Equip //#Emerald Charm
        {
            _name = "Emerald Charm",
            _descriptiveName = "Emerald Charm",
            _type = "Combined",
            _rarity = "Common",
            _price = 1200,
            _hpPlus = 50,
            _spPlus = 20,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Emerald Charm",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Haste"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Emerald Charm").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Emerald Charm").GetIndexOf(1),
            }
        },
        new _Equip //#Eye of the Old God
        {
            _name = "Eye of the Old God",
            _descriptiveName = "Eye of the Old God",
            _type = "Shop",
            _rarity = "Common",
            _price = 1400,
            _mdPlus = 20,
            _mrPlus = 20,
            _castableLimitCount = 99,
            _castableStacks = 99,
            _cooldownDuration = 5,
            _pathIcon = "EquipIcon/Eye of the Old God",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Cosmic Plasma"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Eye of the Old God").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Eye of the Old God").GetIndexOf(1),
            }
        },
        new _Equip //#Frozen Scepter
        {
            _name = "Frozen Scepter",
            _descriptiveName = "Frozen Scepter",
            _type = "Combined",
            _rarity = "Common",
            _price = 1150,
            _mdPlus = 30,
            _spPlus = 10,
            _pathIcon = "EquipIcon/Frozen Scepter",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Rimefrost"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Frozen Scepter").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Frozen Scepter").GetIndexOf(1),
            }
        },
        new _Equip //#Guardian Plate
        {
            _name = "Guardian Plate",
            _descriptiveName = "Guardian Plate",
            _type = "Shop",
            _rarity = "Common",
            _price = 1600,
            _arPlus = 30,
            _mrPlus = 30,
            _pathIcon = "EquipIcon/Guardian Plate",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Resurrection"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Guardian Plate").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Guardian Plate").GetIndexOf(1),
            }
        },
        new _Equip //#Heart of Iron
        {
            _name = "Heart of Iron",
            _descriptiveName = "Heart of Iron",
            _type = "Shop",
            _rarity = "Common",
            _price = 1600,
            _arPlus = 20,
            _mrPlus = 20,
            _pathIcon = "EquipIcon/Heart of Iron",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Adrenaline"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Heart of Iron").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Heart of Iron").GetIndexOf(1),
            }
        },
        new _Equip //#Inquisitor's Cloak
        {
            _name = "Inquisitor's Cloak",
            _descriptiveName = "Inquisitor's Cloak",
            _type = "Combined",
            _rarity = "Common",
            _price = 2000,
            _mrPlus = 40,
            _castableLimitCount = 99,
            _castableStacks = 99,
            _cooldownDuration = 4,
            _pathIcon = "EquipIcon/Inquisitor's Cloak",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Quicksilver"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Inquisitor's Cloak").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Inquisitor's Cloak").GetIndexOf(1),
            }
        },
        new _Equip //#Lightning Spear
        {
            _name = "Lightning Spear",
            _descriptiveName = "Lightning Spear",
            _type = "Shop",
            _rarity = "Common",
            _price = 1550,
            _adPlus = 30,
            _spPlus = 10,
            _pathIcon = "EquipIcon/Lightning Spear",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Pierce"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Lightning Spear").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Lightning Spear").GetIndexOf(1),
            },
        },
        new _Equip //#Magic Broom
        {
            _name = "Magic Broom",
            _descriptiveName = "Magic Broom",
            _type = "Shop",
            _rarity = "Common",
            _price = 1550,
            _mdPlus = 30,
            _pathIcon = "EquipIcon/Magic Broom",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Blazing Star"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Magic Broom").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Magic Broom").GetIndexOf(1),
            },
        },
        new _Equip //#Marisa's Witch Hat
        {
            _name = "Marisa's Witch Hat",
            _descriptiveName = "Marisa's Witch Hat",
            _type = "Shop",
            _rarity = "Common",
            _price = 1800,
            _mdPlus = 30,
            _pathIcon = "EquipIcon/Marisa's Witch Hat",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Phantasmagoria"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Marisa's Witch Hat").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Marisa's Witch Hat").GetIndexOf(1),
            },
        },
        new _Equip //#Mox Almight
        {
            _name = "Mox Almight",
            _descriptiveName = "Mox Almight",
            _type = "Unique",
            _rarity = "Epic",
            _price = 2000,
            _hpPlus = 50,
            _adPlus = 20,
            _arPlus = 20,
            _mdPlus = 20,
            _mrPlus = 20,
            _spPlus = 10,
            _pathIcon = "EquipIcon/Mox Almight",
            _active = null,
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Mox Almight").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Mox Almight").GetIndexOf(1),
            },
        },
        new _Equip //#Murasama
        {
            _name = "Murasama",
            _descriptiveName = "Murasama",
            _type = "Combined",
            _rarity = "Common",
            _price = 1800,
            _hpPlus = 50,
            _adPlus = 20,
            _pathIcon = "EquipIcon/Murasama",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Inflame"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Murasama").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Murasama").GetIndexOf(1),
            }
        },
        new _Equip //#Necronomicon
        {
            _name = "Necronomicon",
            _descriptiveName = "Necronomicon",
            _type = "Combined",
            _rarity = "Common",
            _price = 1300,
            _hpPlus = 50,
            _mdPlus = 20,
            _mrPlus = 20,
            _pathIcon = "EquipIcon/Necronomicon",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Enlightenment"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Necronomicon").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Necronomicon").GetIndexOf(1),
            },
        },
        new _Equip //#NULL
        {
            _name = "NULL",
            _descriptiveName = "NULL",
            _type = "NULL",
            _rarity = "NULL",
            _price = 1000,
            _pathIcon = "EquipIcon/NULL",
            _active = null,
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("NULL").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("NULL").GetIndexOf(1),
            },
        },
        new _Equip //#Paladin's Necklace
        {
            _name = "Paladin's Necklace",
            _descriptiveName = "Paladin's Necklace",
            _type = "Combined",
            _rarity = "Common",
            _price = 2000,
            _hpPlus = 50,
            _mrPlus = 20,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Paladin's Necklace",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Circle of Healing"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Paladin's Necklace").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Paladin's Necklace").GetIndexOf(1),
            },
        },
        new _Equip //#Pristine Talisman
        {
            _name = "Pristine Talisman",
            _descriptiveName = "Pristine Talisman",
            _type = "Shop",
            _rarity = "Common",
            _price = 1400,
            _hpPlus = 80,
            _mrPlus = 30,
            _pathIcon = "EquipIcon/Pristine Talisman",
            _castableLimitCount = 1,
            _castableStacks = 99,
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Purify"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Pristine Talisman").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Pristine Talisman").GetIndexOf(1),
            },
        },
        new _Equip //#Blade of Fury
        {
            _name = "Blade of Fury",
            _descriptiveName = "Blade of Fury",
            _type = "Shop",
            _rarity = "Common",
            _price = 1400,
            _adPlus = 30,
            _pathIcon = "EquipIcon/Blade of Fury",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Rage"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Blade of Fury").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Blade of Fury").GetIndexOf(1),
            },
        },
        new _Equip //#Robe
        {
            _name = "Robe",
            _descriptiveName = "Robe",
            _type = "Basic",
            _rarity = "Common",
            _price = 600,
            _mrPlus = 20,
            _pathIcon = "EquipIcon/Robe",
            _active = null,
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Robe").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Robe").GetIndexOf(1),
            },
        },
        new _Equip //#Rule Breaker
        {
            _name = "Rule Breaker",
            _descriptiveName = "Rule Breaker",
            _type = "Combined",
            _rarity = "Common",
            _price = 2000,
            _adPlus = 20,
            _mrPlus = 20,
            _castableLimitCount = 1,
            _castableStacks = 99,
            _pathIcon = "EquipIcon/Rule Breaker",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Return to Origin"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Rule Breaker").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rule Breaker").GetIndexOf(1),
            }
        },
        new _Equip //#Rune Blade
        {
            _name = "Rune Blade",
            _descriptiveName = "Rune Blade",
            _type = "Shop",
            _rarity = "Common",
            _price = 1200,
            _adPlus = 30,
            _mdPlus = 30,
            _pathIcon = "EquipIcon/Rune Blade",
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Rune Blade").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rune Blade").GetIndexOf(1),
            }
        },
        new _Equip //#Shield
        {
            _name = "Shield",
            _descriptiveName = "Shield",
            _type = "Basic",
            _rarity = "Common",
            _price = 1000,
            _arPlus = 20,
            _pathIcon = "EquipIcon/Shield",
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Shield").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Shield").GetIndexOf(1),
            }
        },
        new _Equip //#Moonlight Rod
        {
            _name = "Moonlight Rod",
            _descriptiveName = "Moonlight Rod",
            _type = "Shop",
            _rarity = "Common",
            _price = 1200,
            _hpPlus = 50,
            _mdPlus = 20,
            _castableLimitCount = 99,
            _castableStacks = 99,
            _cooldownDuration = 6,
            _pathIcon = "EquipIcon/Moonlight Rod",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Magic Shield"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Moonlight Rod").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Moonlight Rod").GetIndexOf(1),
            }
        },
        new _Equip //#Soulcleaver
        {
            _name = "Soulcleaver",
            _descriptiveName = "Soulcleaver",
            _type = "Shop",
            _rarity = "Common",
            _price = 1600,
            _adPlus = 30,
            _mrPlus = 20,
            _pathIcon = "EquipIcon/Soulcleaver",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Cleave"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Soulcleaver").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Soulcleaver").GetIndexOf(1),
            },
        },
        new _Equip //#Spike Shield
        {
            _name = "Spike Shield",
            _descriptiveName = "Spike Shield",
            _type = "Shop",
            _rarity = "Common",
            _price = 1400,
            _adPlus = 20,
            _arPlus = 30,
            _pathIcon = "EquipIcon/Spike Shield",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Thorn"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Spike Shield").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Spike Shield").GetIndexOf(1),
            },
        },
        new _Equip //#Staff
        {
            _name = "Staff",
            _descriptiveName = "Staf",
            _type = "Basic",
            _rarity = "Common",
            _price = 800,
            _mdPlus = 20,
            _pathIcon = "EquipIcon/Staff",
            _active = null,
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Staff").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Staff").GetIndexOf(1),
            },
        },
        new _Equip //#Staff of Force
        {
            _name = "Staff of Force",
            _descriptiveName = "Staff of Force",
            _type = "Shop",
            _rarity = "Common",
            _price = 1550,
            _mdPlus = 30,
            _pathIcon = "EquipIcon/Staff of Force",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Force"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Staff of Force").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Staff of Force").GetIndexOf(1),
            },
        },
        new _Equip //#Amulet of Wind
        {
            _name = "Amulet of Wind",
            _descriptiveName = "Amulet of Wind",
            _type = "Shop",
            _rarity = "Common",
            _price = 1450,
            _mdPlus = 20,
            _spPlus = 15,
            _pathIcon = "EquipIcon/Amulet of Wind",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Wind Blast"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Amulet of Wind").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Amulet of Wind").GetIndexOf(1),
            },
        },
        new _Equip //#Sword
        {
            _name = "Sword",
            _descriptiveName = "Sword",
            _type = "Basic",
            _rarity = "Common",
            _price = 800,
            _adPlus = 20,
            _pathIcon = "EquipIcon/Sword",
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Sword").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Sword").GetIndexOf(1),
            },
        },
        new _Equip //#Talaria Shoes
        {
            _name = "Talaria Shoes",
            _descriptiveName = "Talaria Shoes",
            _type = "Combined",
            _rarity = "Common",
            _price = 1300,
            _spPlus = 40,
            _pathIcon = "EquipIcon/Talaria Shoes",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Flying"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Talaria Shoes").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Talaria Shoes").GetIndexOf(1),
            }
        },
        new _Equip //#Thorn Armor
        {
            _name = "Thorn Armor",
            _descriptiveName = "Thorn Armor",
            _type = "Combined",
            _rarity = "Common",
            _price = 1500,
            _arPlus = 40,
            _pathIcon = "EquipIcon/Thorn Armor",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Thorn"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Thorn Armor").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Thorn Armor").GetIndexOf(1),
            }
        },
        new _Equip //#Titan Axe
        {
            _name = "Titan Axe",
            _descriptiveName = "Titan Axe",
            _type = "Combined",
            _rarity = "Common",
            _price = 1800,
            _adPlus = 30,
            _pathIcon = "EquipIcon/Titan Axe",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Giant Slayer"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Titan Axe").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Titan Axe").GetIndexOf(1),
            }
        },
        new _Equip //#Twilight Cape
        {
            _name = "Twilight Cape",
            _descriptiveName = "Twilight Cape",
            _type = "Shop",
            _rarity = "Common",
            _price = 1200,
            _mdPlus = 20,
            _mrPlus = 30,
            _pathIcon = "EquipIcon/Twilight Cape",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Twilight"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Twilight Cape").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Twilight Cape").GetIndexOf(1),
            }
        },
        new _Equip //#Twin Force
        {
            _name = "Twin Force",
            _descriptiveName = "Twin Force",
            _type = "Combined",
            _rarity = "Common",
            _price = 1500,
            _adPlus = 20,
            _mdPlus = 20,
            _pathIcon = "EquipIcon/Twin Force",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Spellblade"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Twin Force").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Twin Force").GetIndexOf(1),
            }
        },
        new _Equip //#Vampiric Wand
        {
            _name = "Vampiric Wand",
            _descriptiveName = "Vampiric Wand",
            _type = "Combined",
            _rarity = "Common",
            _price = 2000,
            _hpPlus = 50,
            _mdPlus = 20,
            _pathIcon = "EquipIcon/Vampiric Wand",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Siphon Soul"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Vampiric Wand").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Vampiric Wand").GetIndexOf(1),
            }
        },
        new _Equip //#Wind Cloak
        {
            _name = "Wind Cloak",
            _descriptiveName = "Wind Cloak",
            _type = "Shop",
            _rarity = "Common",
            _price = 1200,
            _mrPlus = 30,
            _spPlus = 10,
            _pathIcon = "EquipIcon/Wind Cloak",
            _passive = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Swift"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Wind Cloak").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Wind Cloak").GetIndexOf(1),
            }
        },
        new _Equip //#Youmu's Roukanken
        {
            _name = "Youmu's Roukanken",
            _descriptiveName = "Youmu's Roukanken",
            _type = "Combined",
            _rarity = "Common",
            _price = 1700,
            _adPlus = 20,
            _spPlus = 20,
            _castableLimitCount = 99,
            _castableStacks = 99,
            _cooldownDuration = 6,
            _pathIcon = "EquipIcon/Youmu's Roukanken",
            _active = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Ghost Step"),
            _tooltip = new _Tooltip
            {
                _type = "Equip",
                _title = TextData.DescriptionText.GetValue("Youmu's Roukanken").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Youmu's Roukanken").GetIndexOf(1),
            }
        },
    };
}
