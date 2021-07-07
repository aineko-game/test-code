using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class _Event
{
    public string _name;
    public int choiceNum;

    public List<_choice> _choices;
    public List<_choice> _choiceTable;
    public bool _isDeleteOnSelected = true;

    [Serializable]
    public class _choice
    {
        public string _text;
        public List<string> _tags = new List<string>();
        public string _key_ExecuteEvent = "Normal";
        public int _index;
        public int _heroIndex;
        public string _globalEffectName;
        public string[] _itemNames = new string[0];
        public string _itemToLose;
        public string _equipName;
        public string _enemyName;
        public string _heroEntranceType;
        public string _triggeredTextKey;
        public string _triggeredEventKey;
        public int _itemCount;
        public int _goldValue;
        public int _foodCount;
        public int[] _expValues = new int[3];

        public _Tooltip _tooltip = new _Tooltip();

        public delegate void _FunctionExecuteEvent(_choice choice_, out bool isExecutable_);
        public _FunctionExecuteEvent _functionExecuteEvent
        {
            get { return Function_ExecuteEventList[_key_ExecuteEvent]; }
        }

        public delegate bool _FunctionIsSelectable(_choice choice_);
        public _FunctionIsSelectable _functionIsSelectable
        {
            get { return Function_IsSelectableList[_key_ExecuteEvent]; }
        }
    }

    public delegate void _FunctionMakeEvent (Map._Spot spot_, _Event event_, _Random random_);
    public _FunctionMakeEvent _functionMakeEvent
    {
        get
        {
            if (Function_MakeEventList.ContainsKey(_name))
                return Function_MakeEventList[_name];
            else
            {
                Debug.LogError("Invalid name : " + _name);
                return null;
            }
        }
    }

    public static _Event SelectEvent(_Random random_)
    {
        _Event result;
        int rn_ = random_.Range(0, 100);

        List<_Event> eventList_ = (Globals.Instance.stageCount == 1) ? Globals.Instance.eventList01_ :
                                  (Globals.Instance.stageCount == 2) ? Globals.Instance.eventList02_ :
                                /*(Globals.Instance.stageCount == 3)*/ Globals.Instance.eventList03_;

        if (rn_ < 33 || eventList_.Count == 0)
        {
            do
            {
                result = Globals.Instance.eventList00_.GetRandom();
            } while (false);

            if (result._isDeleteOnSelected)
            {
                Globals.Instance.eventList00_.Remove(result);
            }
        }
        else
        {
            do
            {
                result = eventList_.GetRandom();
            } while (false);

            if (result._isDeleteOnSelected)
            {
                eventList_.Remove(result);
            }
        }

        return result;
    }

    public static IEnumerator ShowEvent(string text_, _Event event_, _choice choice_)
    {
        Map._Spot spot_ = Globals.Instance.spotCurrent;
        _Random random_ = new _Random((uint)(Globals.Instance.seedOnAdventure + 1000 * Globals.Instance.stageCount + 10 * spot_._index + spot_._visitCount));
        GameObject[] goChoices_ = UI.goEvent.Find("Choices").GetChildrenGameObject();
        TextMeshProUGUI tmpro_ = UI.goEvent.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>();
        string eventText_ = _Tooltip.ReplaceTag_EventText(text_, choice_);
        event_?._functionMakeEvent(spot_, event_, random_);
        spot_._choicesList = event_._choices.DeepCopy();
        tmpro_.text = "";

        Globals.Instance.currentEvent = event_;
        Globals.Instance.currentEventText = eventText_;
        Globals.Instance.currentEventChoice = choice_;

        for (int i = 0; i < goChoices_.Length; i++)
        {
            GameObject go_i_ = goChoices_[i];
            go_i_.GetComponent<Button>().interactable = false;
            go_i_.SetActive(false);
            if (i < spot_._choicesList.Count)
            {
                go_i_.Find("Text").GetComponent<TextMeshProUGUI>().text = spot_._choicesList[i]._text;
                go_i_.Find("Dim").gameObject.SetActive(spot_._choicesList[i]._functionIsSelectable(spot_._choicesList[i]) == false);

                spot_._choicesList[i]._heroEntranceType = choice_?._heroEntranceType;
                spot_._choicesList[i]._tags.AddRange(choice_?._tags);

                if (spot_._choicesList[i]._tooltip._title.IsNullOrEmpty() == false)
                {
                    _Tooltip tooltip_i_ = go_i_.GetComponent<_Clickable>()._tooltip;
                    tooltip_i_._title = spot_._choicesList[i]._tooltip._title;
                    tooltip_i_._effectText = spot_._choicesList[i]._tooltip._effectText;
                }
                else
                {
                    go_i_.GetComponent<_Clickable>()._tooltip = new _Tooltip();
                }
            }
        }

        yield return General.Typewriter(tmpro_, eventText_, 0.01f / Globals.Instance.gameSpeed);

        for (int i = 0; i < spot_._choicesList.Count; i++)
        {
            General.Instance.StartCoroutine(Event_Event_SlideAndShow(goChoices_[i]));
            yield return new WaitForSeconds(0.3f);
        }

        IEnumerator Event_Event_SlideAndShow(GameObject go_i_)
        {
            Graphic[] graphics_i_ = go_i_.GetComponentsInChildren<Graphic>();
            RectTransform rectTransform_ = go_i_.GetComponent<RectTransform>();
            Vector3 posStart_ = rectTransform_.localPosition.MulVector(Vector3.up) + Vector3.right * 100;
            Vector3 posEnd_ = rectTransform_.localPosition.MulVector(Vector3.up);
            go_i_.SetActive(true);

            for (float timeSum_ = 0, timeMax_ = 0.4f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
            {
                float p_ = timeSum_ / timeMax_;
                UI.ConfigureGraphicsAlpha(graphics_i_, p_);
                rectTransform_.localPosition = Library.BezierLiner(posStart_, posEnd_, p_);

                yield return null;
            }
            UI.ConfigureGraphicsAlpha(graphics_i_, 1);
            rectTransform_.localPosition = posEnd_;

            go_i_.GetComponent<Button>().interactable = true;
        }
    }

    //public static bool TryAndTakeChoice(int index_)
    //{
    //    if (index_ >= Globals.Instance.spotCurrent._choicesList.Count) return false;

    //    _choice choice_ = Globals.Instance.spotCurrent._choicesList[index_];

    //    if (choice_._itemName.IsNullOrEmpty() == false)
    //    {
    //        _Item item = _Item.CloneFromString(choice_._itemName);
    //        if (_Item.TryAndGetItem(item, -1) == false)
    //        {
    //            return false;
    //        }
    //    }
    //    Globals.Instance.Gold += choice_._goldValue;

    //    return true;
    //}

    public static readonly Dictionary<string, _Event> EventDictionary = new Dictionary<string, _Event>()
    {
        {
            "Ancient Temple",
            new _Event
            {
                _name = "Ancient Temple",
                _choices = new List<_choice>{ new _choice(), new _choice(), new _choice(), }
            }
        },
        {
            "Bloody Altar",
            new _Event
            {
                _name = "Bloody Altar",
                _choices = new List<_choice>{ new _choice(), new _choice(), }
            }
        },
        {
            "Dead Adventurer",
            new _Event
            {
                _name = "Dead Adventurer",
                _choices = new List<_choice>{ new _choice(), new _choice(), }
            }
        },
        {
            "Dead Adventurer-1",
            new _Event
            {
                _name = "Dead Adventurer-1",
                _choices = new List<_choice>{ new _choice(), new _choice(), }
            }
        },
        {
            "Dead Adventurer-2",
            new _Event
            {
                _name = "Dead Adventurer-2",
                _choices = new List<_choice>{ new _choice(), new _choice(), }
            }
        },
        //{
        //    "Dead Adventurer-3",
        //    new _Event
        //    {
        //        _name = "Dead Adventurer-3",
        //        _choices = new List<_choice>{ new _choice(), new _choice() }
        //    }
        //},
        {
            "Elaborate Shrine",
            new _Event
            {
                _name = "Elaborate Shrine",
                _choices = new List<_choice>{ new _choice(), new _choice(), new _choice(), new _choice(), }
            }
        },
        {
            "Fight",
            new _Event
            {
                _name = "Fight",
                _choices = new List<_choice> { new _choice { _text = "<color=yellow>[Fight]</color> Start battle.", _key_ExecuteEvent = "StartBattle" }, },
            }
        },
        {
            "Fight for Money",
            new _Event
            {
                _name = "Fight for Money",
                _choices = new List<_choice>{ new _choice(), new _choice(), }
            }
        },
        {
            "Food Peddler",
            new _Event
            {
                _name = "Food Peddler",
                _choices = new List<_choice>{ new _choice(), new _choice(), new _choice(), new _choice(), }
            }
        },
        {
            "Go Next",
            new _Event
            {
                _name = "Go Next",
                _choices = new List<_choice> { new _choice { _text = "<color=yellow>[Go Next]</color>" }, },
            }
        },
        {
            "Golden Idol",
            new _Event
            {
                _name = "Golden Idol",
                _choices = new List<_choice> { new _choice(), new _choice(), new _choice(), },
            }
        },
        {
            "Old Beggar",
            new _Event
            {
                _name = "Old Beggar",
                _choices = new List<_choice> { new _choice { }, new _choice { }, },
            }
        },
        {
            "Rockfall",
            new _Event
            {
                _name = "Rockfall",
                _choices = new List<_choice> { new _choice { }, new _choice { }, new _choice { }, },
            }
        },
        {
            "Rune Device",
            new _Event
            {
                _name = "Rune Device",
                _choices = new List<_choice> { new _choice { }, new _choice { }, new _choice { }, },
            }
        },
        {
            "Scroll Peddler",
            new _Event
            {
                _name = "Scroll Peddler",
                _choices = new List<_choice> { new _choice { }, new _choice { }, new _choice { }, new _choice { }, },
            }
        },
        {
            "Shaman in Cloak",
            new _Event
            {
                _name = "Shaman in Cloak",
                _choices = new List<_choice> { new _choice { }, new _choice { }, new _choice { }, },
            }
        },
        {
            "Statue of the Old God",
            new _Event
            {
                _name = "Statue of the Old God",
                _choices = new List<_choice> { new _choice { }, new _choice { }, },
            }
        },
        {
            "Weapon Merchant",
            new _Event
            {
                _name = "Weapon Merchant",
                _choices = new List<_choice> { new _choice { }, new _choice { }, },
            }
        },
        {
            "Whisper of the Old God",
            new _Event
            {
                _name = "Whisper of the Old God",
                _choices = new List<_choice> { new _choice { }, new _choice { }, new _choice { }, },
            }
        },
    };

    public static readonly Dictionary<string, _FunctionMakeEvent> Function_MakeEventList = new Dictionary<string, _FunctionMakeEvent>()
    {
        { "Ancient Temple", MakeEvent_AncientTemple },
        { "Bloody Altar", MakeEvent_BloodyAltar },
        { "Dead Adventurer", MakeEvent_DeadAdventurer },
        { "Dead Adventurer-1", MakeEvent_DeadAdventurer_1 },
        { "Dead Adventurer-2", MakeEvent_DeadAdventurer_2 },
        //{ "Dead Adventurer-3", MakeEvent_DeadAdventurer_3 },
        { "Elaborate Shrine", MakeEvent_ElaborateShrine },
        { "Fight", MakeEvent_None },
        { "Fight for Money", MakeEvent_FightForMoney },
        { "Food Peddler", MakeEvent_FoodPeddler },
        { "Golden Idol", MakeEvent_GoldenIdol },
        { "Go Next", MakeEvent_None },
        { "Old Beggar", MakeEvent_OldBeggar },
        { "Rockfall", MakeEvent_Rockfall },
        { "Rune Device", MakeEvent_RuneDevice },
        { "Scroll Peddler", MakeEvent_ScrollPeddler },
        { "Shaman in Cloak", MakeEvent_ShamanInCloak },
        { "Statue of the Old God", MakeEvent_StatueOfTheOldGod },
        { "Weapon Merchant", MakeEvent_WeaponMerchant },
        { "Whisper of the Old God", MakeEvent_WhisperOfTheOldGod },
    };

    public static void MakeEvent_AncientTemple(Map._Spot spot_, _Event event_, _Random random_)
    {
        int expValue_ = random_.Range(600, 800);
        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            string heroName_i_ = Globals.heroList[i]._parameter._class;
            event_._choices[i] = new _choice { _heroIndex = i, _triggeredEventKey = "Go Next", _triggeredTextKey = "Ancient Temple-0" };
            event_._choices[i]._text = "<color=yellow>[" + heroName_i_ + " Touch It]</color> <color=green>" + heroName_i_ + " get " + expValue_ + " exp</color>.";
            event_._choices[i]._expValues[i] = expValue_;
        }
    }
    public static void MakeEvent_BloodyAltar(Map._Spot spot_, _Event event_, _Random random_)
    {
        int goldValue_ = random_.Range(400, 600);

        event_._choices[0] = new _choice { _goldValue = goldValue_, _triggeredEventKey = "Go Next", _triggeredTextKey = "Bloody Altar-0", _globalEffectName = "Bleeding" };
        event_._choices[0]._text = "<color=yellow>[Offer Blood]</color> Gain <sprite name=Gold><color=green>" + goldValue_ + "</color> and gain <color=red>Bleeding</color>.";
        event_._choices[0]._tooltip._title = TextData.DescriptionText["Bleeding"].GetIndexOf(0);
        event_._choices[0]._tooltip._effectText = TextData.DescriptionText["Bleeding"].GetIndexOf(1);
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Bloody Altar-1" };
    }
    public static void MakeEvent_BlackMerket(Map._Spot spot_, _Event event_, _Random random_)
    {
        int goldValue_ = random_.Range(600, 800);
        string equipName_ = Table.EquipTable["Common"].GetRandom(random_);

        event_._choices[0]._goldValue = -goldValue_;
        event_._choices[0]._equipName = equipName_;
        event_._choices[0]._text = "[Deal] : Pay <sprite name=Gold><color=red>" + goldValue_ + "</color> gold and get a random basic equip.";
    }
    public static void MakeEvent_DeadAdventurer(Map._Spot spot_, _Event event_, _Random random_)
    {
        if (random_.Range(0, 100) < 025)
        {
            event_._choices[0] = new _choice { _triggeredEventKey = "Fight", _triggeredTextKey = "Dead Adventurer-0c" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 25% of chance of enemy return.";
        }
        else
        {
            event_._choices[0] = new _choice { _goldValue = random_.Range(60, 80), _triggeredEventKey = "Dead Adventurer-1", _triggeredTextKey = "Dead Adventurer-0a" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 25% of chance of enemy return.";
        }
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Dead Adventurer-1" };
    }
    public static void MakeEvent_DeadAdventurer_1(Map._Spot spot_, _Event event_, _Random random_)
    {
        if (random_.Range(0, 100) < 050)
        {
            event_._choices[0] = new _choice { _triggeredEventKey = "Fight", _triggeredTextKey = "Dead Adventurer-0c" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 50% of chance of enemy return.";
        }
        else
        {
            event_._choices[0] = new _choice { _goldValue = random_.Range(80, 100), _triggeredEventKey = "Dead Adventurer-2", _triggeredTextKey = "Dead Adventurer-0a" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 50% of chance of enemy return.";
        }
        //event_._choices[0] = new _choice { _triggeredEventKey = "Dead Adventurer-2", _triggeredTextKey = "Dead Adventurer-0a" };
        //event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 50% of chance of enemy return.";
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Dead Adventurer-1" };
    }
    public static void MakeEvent_DeadAdventurer_2(Map._Spot spot_, _Event event_, _Random random_)
    {
        random_.Range(0, 100);
        if (random_.Range(0, 100) < 075)
        {
            event_._choices[0] = new _choice { _triggeredEventKey = "Fight", _triggeredTextKey = "Dead Adventurer-0c" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 75% of chance of enemy return.";
        }
        else
        {
            event_._choices[0] = new _choice { _goldValue = random_.Range(100, 120), _triggeredEventKey = "Go Next", _triggeredTextKey = "Dead Adventurer-0d" };
            event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 75% of chance of enemy return.";
        }
        //event_._choices[0] = new _choice { _triggeredEventKey = "Dead Adventurer-3", _triggeredTextKey = "Dead Adventurer-0a" };
        //event_._choices[0]._text = "<color=yellow>[Scavenge]</color> Find loot. 75% of chance of enemy return.";
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Dead Adventurer-1" };
    }
    //public static void MakeEvent_DeadAdventurer_3(Map._Spot spot_, _Event event_, _Random random_)
    //{
    //    event_._choices[0] = new _choice { _key_ExecuteEvent = "StartBattle" };
    //    event_._choices[0]._text = "<color=yellow>[Fight]</color> Start battle.";
    //    event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Dead Adventurer-1" };
    //}
    public static void MakeEvent_ElaborateShrine(Map._Spot spot_, _Event event_, _Random random_)
    {
        int goldValue_ = random_.Range(200, 300);
        int expValue_ = random_.Range(200, 300);
        int heroIndex_ = random_.Range(0, 2);
        _Item item_ = _Item.CloneFromString(Table.ItemTable["Potion"].GetRandom(random_));
        event_._choices[0] = new _choice { _goldValue = goldValue_, _triggeredEventKey = "Go Next", _triggeredTextKey = "Elaborate Shrine-0" };
        event_._choices[0]._text = "<color=yellow>[Wish money]</color> Get <sprite name=Gold><color=green>" + event_._choices[0]._goldValue + "</color> gold.";

        event_._choices[1] = new _choice { _itemNames = new string[] { item_._name }, _itemCount = 1, _triggeredEventKey = "Go Next", _triggeredTextKey = "Elaborate Shrine-1" };
        event_._choices[1]._text = "<color=yellow>[Wish item]</color> Get <color=green>" + item_._descriptiveName + "</color>.";
        event_._choices[1]._tooltip._title = TextData.DescriptionText[item_._descriptiveName].GetIndexOf(0);
        event_._choices[1]._tooltip._effectText = _Tooltip.ReplaceTag_Item(TextData.DescriptionText[item_._descriptiveName].GetIndexOf(1), item_);

        event_._choices[2] = new _choice { _triggeredEventKey = "Go Next", _triggeredTextKey = "Elaborate Shrine-2" };
        event_._choices[2]._expValues[heroIndex_] = expValue_;
        event_._choices[2]._text = "<color=yellow>[Wish power]</color> <color=green>" + Globals.heroList[heroIndex_]._parameter._class + "</color> gets <color=green>" + expValue_ + "</color> exp.";

        event_._choices[3] = new _choice { _text = "<color=yellow>[Leave]</color> Get nothing.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Elaborate Shrine-3" };
    }
    public static void MakeEvent_FightForMoney(Map._Spot spot_, _Event event_, _Random random_)
    {
        event_._choices[0] = new _choice { _text = "<color=yellow>[Fight]</color> Begin the battle.", _key_ExecuteEvent = "StartBattle" };

        event_._choices[1] = new _choice { _goldValue = -(Globals.Instance.Gold / 2), _triggeredEventKey = "Go Next", _triggeredTextKey = "Fight for Money-1" };
        event_._choices[1]._text = "<color=yellow>[Pay]</color> Lose <sprite name=Gold><color=red>" + (Globals.Instance.Gold / 2) + "</color> gold.";
    }
    public static void MakeEvent_FoodPeddler(Map._Spot spot_, _Event event_, _Random random_)
    {
        event_._choices[0] = new _choice { _goldValue = -200, _foodCount = 1, _triggeredEventKey = "Go Next", _triggeredTextKey = "Food Peddler-0" };
        event_._choices[0]._text = "<color=yellow>[Buy 1 Food]</color> <color=red>Lose <sprite name=Gold>200</color> and <color=green> get 1 food</color>.";
        event_._choices[1] = new _choice { _goldValue = -400, _foodCount = 2, _triggeredEventKey = "Go Next", _triggeredTextKey = "Food Peddler-0" };
        event_._choices[1]._text = "<color=yellow>[Buy 2 Food]</color> <color=red>Lose <sprite name=Gold>400</color> and <color=green> get 2 foods</color>.";
        event_._choices[2] = new _choice { _goldValue = -600, _foodCount = 3, _triggeredEventKey = "Go Next", _triggeredTextKey = "Food Peddler-0" };
        event_._choices[2]._text = "<color=yellow>[Buy 3 Food]</color> <color=red>Lose <sprite name=Gold>600</color> and <color=green> get 3 foods</color>.";
        event_._choices[3] = new _choice { _text = "<color=yellow>[Decline]</color>", _triggeredEventKey = "Go Next", _triggeredTextKey = "Food Peddler-3" };

        //event_._choices[1] = new _choice { _goldValue = -(Globals.Instance.Gold / 2), _triggeredEventKey = "Go Next", _triggeredTextKey = "Fight for Money-1" };
        //event_._choices[1]._text = "<color=yellow>[Pay]</color> Lose <sprite name=Gold><color=red>" + (Globals.Instance.Gold / 2) + "</color> gold.";
        //event_._choices[1]._goldValue = -(Globals.Instance.Gold / 2);
        //event_._choices[1]._triggeredTextKey = "Fight for Money-1";
    }
    public static void MakeEvent_GoldenIdol(Map._Spot spot_, _Event event_, _Random random_)
    {
        event_._choices[0] = new _choice { _index = 0, _key_ExecuteEvent = "GoldenIdol", _triggeredEventKey = "Go Next", _triggeredTextKey = "Golden Idol-0" };
        event_._choices[0]._text = "<color=yellow>[Destroy]</color> Gain 30% status buff for 1 battle.";
        event_._choices[1] = new _choice { _index = 1, _key_ExecuteEvent = "GoldenIdol", _triggeredEventKey = "Go Next", _triggeredTextKey = "Golden Idol-1" };
        event_._choices[1]._text = "<color=yellow>[Pray]</color> Gain 20% status buff for 3 battle.";
        event_._choices[2] = new _choice { _index = 2, _key_ExecuteEvent = "GoldenIdol", _triggeredEventKey = "Go Next", _triggeredTextKey = "Golden Idol-2" };
        event_._choices[2]._text = "<color=yellow>[Take Away]</color> Gain 10% status buff this stage.";
    }
    public static void MakeEvent_MerchantInTrouble(Map._Spot spot_, _Event event_, _Random random_)
    {
        string[] itemNameArray_ = Table.ItemTable["Potion"].Shuffle(random_);

        for (int i = 0; i < 3; i++)
        {
            _Item item_i_ = _Item.CloneFromString(itemNameArray_[i]);
            event_._choices[i]._itemNames = new string[] { itemNameArray_[i] };
            event_._choices[i]._itemCount = 1;
            event_._choices[i]._text = "[Accept] : Get <color=green>" + itemNameArray_[i] + "</color>.";
            event_._choices[i]._tooltip._title = TextData.DescriptionText[itemNameArray_[i]].GetIndexOf(0);
            event_._choices[i]._tooltip._effectText = _Tooltip.ReplaceTag_Item(TextData.DescriptionText[itemNameArray_[i]].GetIndexOf(1), item_i_);
        }
    }
    public static void MakeEvent_None(Map._Spot spot_, _Event event_, _Random random_)
    {

    }
    public static void MakeEvent_OldBeggar(Map._Spot spot_, _Event event_, _Random random_)
    {
        int goldValue_ = random_.Range(500, 800);
        event_._choices[0] = new _choice { _goldValue = goldValue_, _foodCount = -1, _triggeredEventKey = "Go Next", _triggeredTextKey = "Old Beggar-0" };
        event_._choices[0]._text = "<color=yellow>[Offer food]</color> <color=red>Lose 1 food</color>, and <color=green>get <sprite name=Gold>" + goldValue_ + "</color>.";
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens." , _triggeredEventKey = "Go Next", _triggeredTextKey = "Old Beggar-1" };
    }
    public static void MakeEvent_PotionPeddler(Map._Spot spot_, _Event event_, _Random random_)
    {

        event_._choices[0]._text = "<color=yellow>[Pay 60 Gold]</color>  <color=red>Lose 60<sprite name=Gold></color> and get 1 random potion.";
        event_._choices[0]._foodCount = -1;
        event_._choices[0]._goldValue = -60;
    }
    public static void MakeEvent_Rockfall(Map._Spot spot_, _Event event_, _Random random_)
    {
        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            event_._choices[i] = new _choice { _heroIndex = i, _globalEffectName = "Injured", _triggeredTextKey = "Rockfall-0", _triggeredEventKey = "Go Next" };
            event_._choices[i]._text = "<color=yellow>[" + Globals.heroList[i]._parameter._class + " catches] </color> " + Globals.heroList[i]._parameter._class + " get injured.";
            event_._choices[i]._tooltip._title = TextData.DescriptionText["Injured"].GetIndexOf(0);
            event_._choices[i]._tooltip._effectText = TextData.DescriptionText["Injured"].GetIndexOf(1).Replace("<Target Hero>", Globals.heroList[i]._parameter._class);
        }
    }
    public static void MakeEvent_RuneDevice(Map._Spot spot_, _Event event_, _Random random_)
    {
        string[] itemNames_ = new string[] { Table.ItemTable["Rune"].GetRandom(random_) };

        event_._choices[0] = new _choice { _heroEntranceType = "Stay", _triggeredTextKey = "Rune Device-0", _triggeredEventKey = "Fight" };
        event_._choices[0]._text = "<color=yellow>[Activate It]</color> Ready for battle.";
        event_._choices[0]._tags.Add("Rune Device");
        event_._choices[1] = new _choice { _itemNames = itemNames_, _triggeredTextKey = "Rune Device-1", _triggeredEventKey = "Go Next" };
        event_._choices[1]._text = "<color=yellow>[Take It Away]</color> <color=green>Get random rune-item</color>.";
        event_._choices[2] = new _choice { _triggeredTextKey = "Rune Device-2", _triggeredEventKey = "Go Next" };
        event_._choices[2]._text = "<color=yellow>[Leave]</color>";
    }
    public static void MakeEvent_ScrollPeddler(Map._Spot spot_, _Event event_, _Random random_)
    {
        string[] itemNames_ = Table.ItemTable["Scroll"].Shuffle();
        event_._choices[0] = new _choice { _goldValue = -100, _itemNames = itemNames_.Take(1).ToArray(), _triggeredEventKey = "Go Next", _triggeredTextKey = "Scroll Peddler-0" };
        event_._choices[0]._text = "<color=yellow>[Pay 100 Gold]</color>  <color=green>Get 1 random scroll</color>.";
        event_._choices[1] = new _choice { _goldValue = -200, _itemNames = itemNames_.Take(2).ToArray(), _triggeredEventKey = "Go Next", _triggeredTextKey = "Scroll Peddler-0" };
        event_._choices[1]._text = "<color=yellow>[Pay 200 Gold]</color>  <color=green>Get 2 random scroll</color>.";
        event_._choices[2] = new _choice { _goldValue = -300, _itemNames = itemNames_.Take(3).ToArray(), _triggeredEventKey = "Go Next", _triggeredTextKey = "Scroll Peddler-0" };
        event_._choices[2]._text = "<color=yellow>[Pay 300 Gold]</color>  <color=green>Get 3 random scroll</color>.";
        event_._choices[3] = new _choice { _text = "<color=yellow>[Decline]</color> Get nothing.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Scroll Peddler-3" };
    }
    public static void MakeEvent_ShamanInCloak(Map._Spot spot_, _Event event_, _Random random_)
    {
        string[] itemNames_ = Table.ItemTable["Scroll"].Shuffle();
        for (int i = 0; i < event_._choices.Count; i++)
        {
            string heroName_i_ = Globals.heroList[i]._parameter._class;
            event_._choices[i] = new _choice { _index = i, _key_ExecuteEvent = "ShamanInCloak", _triggeredEventKey = "Go Next", _triggeredTextKey = "Shaman in Cloak-0" };
            event_._choices[i]._text = "<color=yellow>[Bless " + heroName_i_ + "]</color> " + heroName_i_ + " gain 20% buff this stage.";
        }
    }
    public static void MakeEvent_StatueOfTheOldGod(Map._Spot spot_, _Event event_, _Random random_)
    {
        int gold_ = random_.Range(500, 800);
        event_._choices[0] = new _choice { _goldValue = gold_, _globalEffectName = "Curse", _triggeredEventKey = "Go Next", _triggeredTextKey = "Statue of the Old God-0" };
        event_._choices[0]._text = "<color=yellow>[Destroy]</color> Get <sprite name=Gold><color=green>" + gold_ + "</color> and <color=red>Curse</color>.";
        event_._choices[0]._tooltip._title = TextData.DescriptionText["Curse"].GetIndexOf(0);
        event_._choices[0]._tooltip._effectText = TextData.DescriptionText["Curse"].GetIndexOf(1).Replace("<iCount>", "3");
        event_._choices[1] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Statue of the Old God-1" };
    }
    public static void MakeEvent_VeteranWarrior(Map._Spot spot_, _Event event_, _Random random_)
    {
        int expValue_ = random_.Range(200, 300);
        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            event_._choices[i]._text = "[Accept] : <color=green>" + Globals.heroList[i]._parameter._class + "</color> get <color=green>" + expValue_ + "</color> exp.";
            event_._choices[i]._expValues[i] = expValue_;
        }
    }
    public static void MakeEvent_WeaponMerchant(Map._Spot spot_, _Event event_, _Random random_)
    {
        int gold_ = random_.Range(600, 800);
        string equipName_ = Table.EquipTable["Common"].GetRandom(random_);

        event_._choices[0] = new _choice { _goldValue = -gold_, _equipName = equipName_,  _triggeredEventKey = "Go Next", _triggeredTextKey = "Weapon Merchant-0" };
        event_._choices[0]._text = "<color=yellow>[Deal]</color> Pay <sprite name=Gold><color=red>" + gold_ + "</color> and <color=green>get a random basic equip</color>." ;
        event_._choices[1] = new _choice { _text = "<color=yellow>[Decline]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Weapon Merchant-1" };
    }
    public static void MakeEvent_WhisperOfTheOldGod(Map._Spot spot_, _Event event_, _Random random_)
    {
        int gold_ = random_.Range(400, 600);

        event_._choices[0] = new _choice { _goldValue = -gold_, _globalEffectName = "Stigma", _triggeredEventKey = "Go Next", _triggeredTextKey = "Whisper of the Old God-0" };
        event_._choices[0]._text = "<color=yellow>[Offer Gold]</color> <color=red>Lose <sprite name=Gold>" + gold_ + "</color> and <color=green>get Stigma</color> this stage.";
        event_._choices[0]._tooltip._title = TextData.DescriptionText["Stigma"].GetIndexOf(0);
        event_._choices[0]._tooltip._effectText = TextData.DescriptionText["Stigma"].GetIndexOf(1);
        event_._choices[1] = new _choice { _foodCount = -1, _globalEffectName = "Stigma", _triggeredEventKey = "Go Next", _triggeredTextKey = "Whisper of the Old God-1" };
        event_._choices[1]._text = "<color=yellow>[Offer Food]</color> <color=red>Lose 1 Food</color> and <color=green> get Stigma</color> this stage.";
        event_._choices[1]._tooltip._title = TextData.DescriptionText["Stigma"].GetIndexOf(0);
        event_._choices[1]._tooltip._effectText = TextData.DescriptionText["Stigma"].GetIndexOf(1);
        event_._choices[2] = new _choice { _text = "<color=yellow>[Leave]</color> Nothing happens.", _triggeredEventKey = "Go Next", _triggeredTextKey = "Whisper of the Old God-2" };
    }



    public static readonly Dictionary<string, _choice._FunctionExecuteEvent> Function_ExecuteEventList = new Dictionary<string, _choice._FunctionExecuteEvent>()
    {
        { "Normal", ExecuteEvent_Normal },
        { "StartBattle", ExecuteEvent_StartBattle },
        { "GoldenIdol", ExecuteEvent_GoldenIdol },
        { "ShamanInCloak", ExecuteEvent_ShamanInCloak },
    };

    public static void ExecuteEvent_Normal(_choice choice_, out bool isExecutable_)
    {
        isExecutable_ = true;

        if (Globals.Instance.Gold + choice_._goldValue < 0)
        {
            isExecutable_ = false;
            return;
        }
        if (Globals.Instance.Food + choice_._foodCount < 0)
        {
            isExecutable_ = false;
            return;
        }

        foreach (string itemName_i_ in choice_._itemNames)
        {
            _Item item_i_ = _Item.CloneFromString(itemName_i_);
            if (_Item.TryAndGetItem(item_i_, -1) == false)
            {
                isExecutable_ = false;
                return;
            }
        }

        if (choice_._equipName.IsNullOrEmpty() == false)
        {
            _Equip equip_ = _Equip.CloneFromString(choice_._equipName);
            _Equip.GetEquip(equip_, out bool isGettable_, -2, -2);
            if (isGettable_ == false)
            {
                isExecutable_ = false;
                return;
            }
        }

        if (choice_._globalEffectName.IsNullOrEmpty() == false)
        {
            _Skill skill_ = _Skill.CloneFromString(choice_._globalEffectName);
            skill_._parameter._targetHeroIndex = new List<int> { choice_._heroIndex };
            //skill_._parameter._buffTarget_ = new List<string> { Globals.heroList[choice_._heroIndex]._parameter._class };
            Globals.Instance.globalEffectList.Add(skill_);
            UI.ConfigureGlobalEffectUI();
        }

        Globals.Instance.Gold += choice_._goldValue;
        Globals.Instance.Food += choice_._foodCount;

        for (int i = 0; i < choice_._expValues.Length; i++)
        {
            Globals.heroList[i]._GainExp(choice_._expValues[i]);
        }

        UI.ConfigureNotification();

        if (choice_._triggeredTextKey.IsNullOrEmpty() == false)
        {
            General.Instance.StartCoroutine(ShowEvent(TextData.EventText[choice_._triggeredTextKey], EventDictionary[choice_._triggeredEventKey], choice_));
        }
        else
        {
            UI.goEvent.SetActive(false);
        }
    }

    public static void ExecuteEvent_StartBattle(_choice choice_, out bool isExecutable_)
    {
        isExecutable_ = true;
        string objectName_ = "";

        UI.goEvent.SetActive(false);

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            if (choice_._heroEntranceType.IsNullOrEmpty() || choice_._heroEntranceType == "FromLeft")
                unit_i_._parameter._entranceType = "FromLeft";
            else if (choice_._heroEntranceType == "FromRight")
                unit_i_._parameter._entranceType = "FromRight";
            else if (choice_._heroEntranceType == "Stay")
                unit_i_._parameter._entranceType = "Stay";
        }

        if (choice_._tags.Contains("Rune Device"))
        {
            for (int i = 0; i < Globals.heroList.Count; i++)
            {
                _Unit unit_i_ = Globals.heroList[i];

                unit_i_._qrtSOB = (i == 0) ? Quaternion.Euler(0, +90, 0) : Quaternion.Euler(0, -90, 0);
                unit_i_._posSOB = (i == 0) ? new Vector3(+2, +0, +0) : (i== 1) ? new Vector3(-2, +0, +2) : new Vector3(-2, +0, -2);
                unit_i_._posDistSOB = new Vector3(0.5f, 0, 1);
                objectName_ = "Rune Stone";
                choice_._enemyName = "Golem x5";
            }
        }

        General.Instance.StartCoroutine(Battle.StartBattle(Globals.Instance.seedOnAdventure * Globals.Instance.spotCurrent._index, "Battle", choice_._enemyName, objectName_));
    }

    public static void ExecuteEvent_GoldenIdol(_choice choice_, out bool isExecutable_)
    {
        isExecutable_ = true;

        _Skill skill_ = _Skill.CloneFromString("Blessing");
        skill_._parameter._targetHeroIndex = new List<int> { 0, 1, 2 };
        skill_._parameter._iValue = (choice_._index == 0) ? 30 :
                                    (choice_._index == 1) ? 20 :
                                  /*(choice_._index == 2)*/ 10;
        skill_._parameter._iCount = (choice_._index == 0) ? 01 :
                                    (choice_._index == 1) ? 03 :
                                  /*(choice_._index == 2)*/ 99;

        Globals.Instance.globalEffectList.Add(skill_);
        UI.ConfigureGlobalEffectUI();

        if (choice_._triggeredTextKey.IsNullOrEmpty() == false)
        {
            General.Instance.StartCoroutine(ShowEvent(TextData.EventText[choice_._triggeredTextKey], EventDictionary[choice_._triggeredEventKey], choice_));
        }
        else
        {
            UI.goEvent.SetActive(false);
        }
    }

    public static void ExecuteEvent_ShamanInCloak(_choice choice_, out bool isExecutable_)
    {
        isExecutable_ = true;

        _Skill skill_ = _Skill.CloneFromString("Blessing");
        skill_._parameter._targetHeroIndex = new List<int> { choice_._index };
        skill_._parameter._iValue = 20;
        skill_._parameter._iCount = 99;

        Globals.Instance.globalEffectList.Add(skill_);
        UI.ConfigureGlobalEffectUI();

        if (choice_._triggeredTextKey.IsNullOrEmpty() == false)
        {
            General.Instance.StartCoroutine(ShowEvent(TextData.EventText[choice_._triggeredTextKey], EventDictionary[choice_._triggeredEventKey], choice_));
        }
        else
        {
            UI.goEvent.SetActive(false);
        }
    }



    public static readonly Dictionary<string, _choice._FunctionIsSelectable> Function_IsSelectableList = new Dictionary<string, _choice._FunctionIsSelectable>()
    {
        { "Normal", IsSelectable_Normal },
        { "StartBattle", IsSelectable_Normal },
        { "GoldenIdol", IsSelectable_Normal },
        { "ShamanInCloak", IsSelectable_Normal },
    };
    public static bool IsSelectable_Normal(_choice choice_)
    {
        if (Globals.Instance.Gold + choice_._goldValue < 0)
            return false;

        if (Globals.Instance.Food + choice_._foodCount < 0)
            return false;

        int openCount_ = 0;
        foreach (_Item item_i_ in Globals.itemsInBagList)
        {
            if (item_i_ == null || item_i_._name.IsNullOrEmpty())
                openCount_++;
        }

        if (choice_._itemNames?.Length > openCount_)
            return false;

        //if (choice_._itemName.IsNullOrEmpty() == false)
        //{
        //    _Item item_ = _Item.CloneFromString(choice_._itemName);

        //    if (_Item.IsGettableItem(item_) == false)
        //        return false;
        //}

        if (choice_._equipName.IsNullOrEmpty() == false)
        {
            _Equip equip_ = _Equip.CloneFromString(choice_._equipName);
            //_Equip.GetEquip(equip_, out bool isGettable_, -2, -2);

            if (_Equip.IsGettableEquip(equip_) == false)
                return false;
        }

        return true;
    }
}
