using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Table
{
    public static Dictionary<Tuple<int, string>, string[]> ObjectTable = new Dictionary<Tuple<int, string>, string[]>()
    {
        {
            new Tuple<int, string>(1, "Normal"),
            new string[] { "None", "None", "Rock x2", "Rock x2", "Rock x2", "Rock x2", "Rock x2", "Rock x3-a", "Rock x3-b", "Rock x3-c", }
        },
    };

    public static Dictionary<string, string[]> ObjectList = new Dictionary<string, string[]>()
    {
        {
            "Object",
            new string[]{ "Rock", "Healing Crystal", "Explosive Crystal" }
        },
    };

    public static Dictionary<string, string[][]> ObjectWaveData = new Dictionary<string, string[][]>()
    {
        {
            "None",
            new string[][]{ }
        },
        {
            "Rock x2",
            new string[][]
            {
                new string[]{ "Random Object", "Pos:(+00.0f, 0, +02.0f)", "PosDist:(+02.0f, 0, +02.0f)"},
                new string[]{ "Random Object", "Pos:(+00.0f, 0, -02.0f)", "PosDist:(+02.0f, 0, +02.0f)"},
            }
        },
        {
            "Rock x3-a",
            new string[][]
            {
                new string[]{ "Random Object", "Pos:(+00.0f, 0, +00.0f)", "PosDist:(+02.5f, 0, +02.0f)"},
                new string[]{ "Random Object", "Pos:(+00.0f, 0, +03.0f)", "PosDist:(+02.5f, 0, +01.5f)"},
                new string[]{ "Random Object", "Pos:(+00.0f, 0, -03.0f)", "PosDist:(+02.5f, 0, +01.5f)"},
            }
        },
        {
            "Rock x3-b",
            new string[][]
            {
                new string[]{ "Random Object", "Pos:(+00.0f, 0, +00.0f)", "PosDist:(+02.5f, 0, +02.0f)"},
                new string[]{ "Random Object", "Pos:(+01.5f, 0, +03.0f)", "PosDist:(+01.0f, 0, +01.0f)"},
                new string[]{ "Random Object", "Pos:(-01.5f, 0, -03.0f)", "PosDist:(+01.0f, 0, +01.0f)"},
            }
        },
        {
            "Rock x3-c",
            new string[][]
            {
                new string[]{ "Random Object", "Pos:(+00.0f, 0, +00.0f)", "PosDist:(+02.5f, 0, +02.0f)"},
                new string[]{ "Random Object", "Pos:(-01.5f, 0, +03.0f)", "PosDist:(+01.0f, 0, +01.0f)"},
                new string[]{ "Random Object", "Pos:(+01.5f, 0, -03.0f)", "PosDist:(+01.0f, 0, +01.0f)"},
            }
        },
        {
            "Rune Stone",
            new string[][]
            {
                new string[]{ "Rune Stone", "Pos:(+00.0f, 0, +00.0f)", "PosDist:(+00.0f, 0, +00.0f)"},
            }
        },
    };

    public static Dictionary<Tuple<int, string>, string[]> EnemyTable = new Dictionary<Tuple<int, string>, string[]>()
    {
        {
            new Tuple<int, string>(1, "EnemyWeak"),
            new string[] { "Slime x5", "Met x5", "Rat x5", "Slime&Wolf", "Slime&Met", "Met&Rat" }
        },
        {
            new Tuple<int, string>(1, "EnemyStrong"),
            new string[] { "Wolf x5", "Golem x3", "Wolf&Faerie", "Met&Faerie" }
        },
        {
            new Tuple<int, string>(1, "Boss"),
            new string[] { "Boss:Demon" }
        },
    };

    public static Dictionary<string, string[][]> EnemyWaveData = new Dictionary<string, string[][]>()
    {
        {
            "Boss:Demon",
            new string[][]
            {
                new string[]{ "Demon", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },

        {
            "Slime x5",
            new string[][]
            {
                new string[]{ "Slime", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Met x5",
            new string[][]
            {
                new string[]{ "Met", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Rat x5",
            new string[][]
            {
                new string[]{ "Rat", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Slime&Wolf",
            new string[][]
            {
                new string[]{ "Slime", "Pos:(+04.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf" , "Pos:(+07.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf" , "Pos:(+07.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Slime&Met",
            new string[][]
            {
                new string[]{ "Slime", "Pos:(+04.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Slime", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met" , "Pos:(+07.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met" , "Pos:(+07.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Met&Rat",
            new string[][]
            {
                new string[]{ "Met", "Pos:(+04.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Met", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat" , "Pos:(+07.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat" , "Pos:(+07.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Wolf x5",
            new string[][]
            {
                new string[]{ "Wolf", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf", "Pos:(+05.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf", "Pos:(+05.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Wolf", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Golem x3",
            new string[][]
            {
                new string[]{ "Golem", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Golem", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Golem", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Golem x5",
            new string[][]
            {
                new string[]{ "Golem", "Pos:(+09.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)", "Qrt:(0, -180, 0)", "Entrance:FromRight"},
                new string[]{ "Golem", "Pos:(+11.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)", "Qrt:(0, -180, 0)", "Entrance:FromRight"},
                new string[]{ "Golem", "Pos:(+11.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)", "Qrt:(0, -180, 0)", "Entrance:FromRight"},
                new string[]{ "Golem", "Pos:(-10.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)", "Qrt:(0, +180, 0)", "Entrance:FromLeft"},
                new string[]{ "Golem", "Pos:(-10.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)", "Qrt:(0, +180, 0)", "Entrance:FromLeft"},
            }
        },
        {
            "Wolf&Faerie",
            new string[][]
            {
                new string[]{ "Faerie", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Faerie", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Wolf", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Wolf", "Pos:(+06.0f, 0, +03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Wolf", "Pos:(+06.0f, 0, -03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
            }
        },
        {
            "Met&Faerie",
            new string[][]
            {
                new string[]{ "Faerie", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Faerie", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Met", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Met", "Pos:(+06.0f, 0, +03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Met", "Pos:(+06.0f, 0, -03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
            }
        },
        {
            "Explosive Bug x5",
            new string[][]
            {
                new string[]{ "Explosive Bug", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Explosive Bug", "Pos:(+06.0f, 0, +03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Explosive Bug", "Pos:(+06.0f, 0, -03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Explosive Bug", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Explosive Bug", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
            }
        },
        {
            "Frightfly x5",
            new string[][]
            {
                new string[]{ "Frightfly", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Frightfly", "Pos:(+06.0f, 0, +03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Frightfly", "Pos:(+06.0f, 0, -03.0f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Frightfly", "Pos:(+08.0f, 0, +01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
                new string[]{ "Frightfly", "Pos:(+08.0f, 0, -01.5f)", "PosDist:(+00.5f, 0, +00.5f)"},
            }
        },
        {
            "Fungusa x3",
            new string[][]
            {
                new string[]{ "Fungusa", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Fungusa", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Fungusa", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Treant x3",
            new string[][]
            {
                new string[]{ "Treant", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Treant", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Treant", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Snowman x3",
            new string[][]
            {
                new string[]{ "Snowman", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Snowman", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Snowman", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Longtail x3",
            new string[][]
            {
                new string[]{ "Longtail", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Longtail", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Longtail", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Stump x3",
            new string[][]
            {
                new string[]{ "Stump", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Stump", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Stump", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Scorpion x3",
            new string[][]
            {
                new string[]{ "Scorpion", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Scorpion", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Scorpion", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Leech x3",
            new string[][]
            {
                new string[]{ "Leech", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Leech", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Leech", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Caterpillar x3",
            new string[][]
            {
                new string[]{ "Caterpillar", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Caterpillar", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Caterpillar", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Venusa x3",
            new string[][]
            {
                new string[]{ "Venusa", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Venusa", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Venusa", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Egglet x3",
            new string[][]
            {
                new string[]{ "Egglet", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Egglet", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Egglet", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Sicklus x3",
            new string[][]
            {
                new string[]{ "Sicklus", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Sicklus", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Sicklus", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Serpent x3",
            new string[][]
            {
                new string[]{ "Serpent", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Serpent", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Serpent", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Crystal x3",
            new string[][]
            {
                new string[]{ "Crystal Guardian", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Crystal Guardian", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Crystal Guardian", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Rat x3",
            new string[][]
            {
                new string[]{ "Rat", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Rat", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Bee x3",
            new string[][]
            {
                new string[]{ "Bee", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Bee", "Pos:(+07.0f, 0, +03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
                new string[]{ "Bee", "Pos:(+07.0f, 0, -03.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
        {
            "Spider",
            new string[][]
            {
                new string[]{ "Spider", "Pos:(+05.0f, 0, +00.0f)", "PosDist:(+00.3f, 0, +00.3f)"},
            }
        },
    };

    public static Dictionary<string, int[]> ExpTable = new Dictionary<string, int[]>()
    {
        {
            "Warrior",
            new int[] { 0000, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 999999 }
        },
        {
            "Hunter",
            new int[] { 0000, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 999999 }
        },
        {
            "Mage",
            new int[] { 0000, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 999999 }
        },
    };

    public static Dictionary<string, string[]> ItemTable = new Dictionary<string, string[]>()
    {
        {
            "Common",
            new string[]{ "Healing Potion", "Explosive Potion", "Fire Potion", "Block Potion" }
        },
        {
            "Rare",
            new string[]{ "Scroll of Blessing", "Scroll of Fury", "Scroll of Protection" }
        },
        {
            "Epic",
            new string[]{ "Healing Potion", "Explosive Potion", "Fire Potion" }
        },
        {
            "Potion",
            new string[]{ "Healing Potion", "Explosive Potion", "Fire Potion", "Block Potion" }
        },
        {
            "Rune",
            new string[]{ "Rune of Strength", "Rune of Protection", "Rune of Magic", "Rune of Blessing", "Rune of Swift" }
        },
        {
            "Scroll",
            new string[]{ "Scroll of Blessing", "Scroll of Fury", "Scroll of Protection", }
        },
    };

    public static Dictionary<string, string[]> EquipTable = new Dictionary<string, string[]>()
    {
        //{
        //    "Shop",
        //    new string[]{ "Marisa's Witch Hat", "Moonlight Rod", "Mini Hakkero", "Eye of the Old God", "Staff of Sunfire", "Frozen Scepter", "Amulet of Wind", "Staff of Force", "Magic Broom", "Abyssal Staff",
        //                  "Bloody Cleaver", "Assassin Dagger", "Blade of Fury", "Soulcleaver", "Murasama", "Butcher Knife", "Titan Axe", "Lightning Spear", "Arcane Gauntlets" }
        //                  //"Berserker Sword", "Death Sword", "Twin Sword", "Damaging Robe", "Burning Blood", "Resurrect Armor", "Thorn Armor", "Healing Armor", "Speed Boots", "Sash Mantle",
        //                  //"Stealth Mantle", "Mox Almight", "Lucky Charm" ,"Scale Armor", "Heavy Armor", "Slow Armor", "Defence Armor", "Flying Boots" }
        //},
        {
            "Shop",
            new string[]{ "Bloody Cleaver", "Assassin Dagger", "Blade of Fury", "Soulcleaver", "Murasama", "Butcher Knife", "Titan Axe", "Lightning Spear", "Colossus Hammer", "Rune Blade", "Twin Force",
                          "Marisa's Witch Hat", "Moonlight Rod", "Mini Hakkero", "Eye of the Old God", "Staff of Sunfire", "Frozen Scepter", "Amulet of Wind", "Staff of Force", "Magic Broom", "Abyssal Staff",
                          "Aegis Shield", "Arcane Gauntlets", "Armor of Vitality", "Berserker Mail", "Spike Shield", "Heart of Iron", "Guardian Plate",
                          "Cloak of Shadows", "Necronomicon", "Pristine Talisman", "Twilight Cape", "Wind Cloak",
                          "Burning Blood", "Paladin's Necklace", "Mox Almight", "Talaria Shoes", }
        },
        {
            "Common",
            new string[]{ "Crystal", "Sword", "Shield", "Staff", "Robe", "Boots" }
        },
        {
            "Rare",
            new string[]{ "Mox Almight" }
        },
        {
            "Epic",
            new string[]{ "NULL" }
        },
    };

    public static Dictionary<int, _Event[]> EventTable = new Dictionary<int, _Event[]>()
    {
        {
            0,
            new _Event[]
            {
                _Event.EventDictionary["Elaborate Shrine"],
                _Event.EventDictionary["Fight for Money"],
                _Event.EventDictionary["Rockfall"],
                _Event.EventDictionary["Scroll Peddler"],
                _Event.EventDictionary["Golden Idol"],
                _Event.EventDictionary["Shaman in Cloak"],
                _Event.EventDictionary["Bloody Altar"],
                _Event.EventDictionary["Dead Adventurer"],
            }
        },
        {
            1,
            new _Event[]
            {
                _Event.EventDictionary["Weapon Merchant"],
                _Event.EventDictionary["Statue of the Old God"],
                _Event.EventDictionary["Whisper of the Old God"],
                _Event.EventDictionary["Old Beggar"],
                _Event.EventDictionary["Ancient Temple"],
                _Event.EventDictionary["Food Peddler"],
            }
        },
        {
            2,
            new _Event[]
            {
                _Event.EventDictionary["Fight for Money"],
            }
        },
        {
            3,
            new _Event[]
            {
                _Event.EventDictionary["Fight for Money"],
            }
        }
    };

    public static Dictionary<int, string[]> TreasureTable = new Dictionary<int, string[]>()
    {
        {
            0,
            new string[]{ "Artifact Wand", "Battle Frenzy", "Battle Rage", "Boon Reflection", "Demolisher", "Golden Armor", "First Blood", "Guardian Shield", "Intimidation",
                          "Mighty Guard", "Outrage", "Potion Mastery", "Predation", "Quick Move", "Sudden Attack", }
        },
        {
            1,
            new string[]{ "Amulet Coin", "Harvest", "Goldgrubber", "Discount Ticket", }
        },
        {
            2,
            new string[]{ "Big Game Hunter", "Binding Grasp", "Force of Nature", }
        },
        {
            3,
            new string[]{ "Berserker Soul", "Shattering Smash", "Stealth Strike",/*, "Carnage"*/ }
        },
    };

    public static Dictionary<Tuple<string, int>, string[]> HeroSkillTable = new Dictionary<Tuple<string, int>, string[]>()
    {
        {
            new Tuple<string, int>("Warrior", 1),
            new string[]{ "Whirlwind" }
        },
        {
            new Tuple<string, int>("Warrior", 2),
            new string[]{ "Charge", "Battlecry" }
        },
        {
            new Tuple<string, int>("Warrior", 3),
            new string[]{ "Execution", "Heavy Strike" }
        },
        {
            new Tuple<string, int>("Hunter", 1),
            new string[]{ "Tumble" }
        },
        {
            new Tuple<string, int>("Hunter", 2),
            new string[]{ "Piercing Shot", "On the Hunt" }
        },
        {
            new Tuple<string, int>("Hunter", 3),
            new string[]{ "Knockback Shot", "Raining Arrow" }
        },
        {
            new Tuple<string, int>("Mage", 1),
            new string[]{ "Fireball" }
        },
        {
            new Tuple<string, int>("Mage", 2),
            new string[]{ "Cone of Flame", "Chain Lightning" }
        },
        {
            new Tuple<string, int>("Mage", 3),
            new string[]{ "Thunder Strike", "Frost Nova" }
        },
    };



    public static Dictionary<Tuple<string, string>, string> ItemCombineTable = new Dictionary<Tuple<string, string>, string>()
    {
        { new Tuple<string, string>("Crystal", "Crystal"), "Burning Blood" },
        { new Tuple<string, string>("Crystal", "Sword"), "Murasama" },
        { new Tuple<string, string>("Crystal", "Shield"), "Arcane Gauntlets" },
        { new Tuple<string, string>("Crystal", "Staff"), "Vampiric Wand" },
        { new Tuple<string, string>("Crystal", "Robe"), "Paladin's Necklace" },
        { new Tuple<string, string>("Crystal", "Boots"), "Emerald Charm" },

        { new Tuple<string, string>("Sword", "Crystal"), "Murasama" },
        { new Tuple<string, string>("Sword", "Sword"), "Dragon Slayer" },
        { new Tuple<string, string>("Sword", "Shield"), "Bloody Cleaver" },
        { new Tuple<string, string>("Sword", "Staff"), "Twin Force" },
        { new Tuple<string, string>("Sword", "Robe"), "Rule Breaker" },
        { new Tuple<string, string>("Sword", "Boots"), "Youmu's Roukanken" },

        { new Tuple<string, string>("Shield", "Crystal"), "Arcane Gauntlets" },
        { new Tuple<string, string>("Shield", "Sword"), "Bloody Cleaver" },
        { new Tuple<string, string>("Shield", "Shield"), "Thorn Armor" },
        { new Tuple<string, string>("Shield", "Staff"), "Eye of the Old God" },
        { new Tuple<string, string>("Shield", "Robe"), "Guardian Plate" },
        { new Tuple<string, string>("Shield", "Boots"), "Greaves of Swift" },

        { new Tuple<string, string>("Staff", "Crystal"), "Vampiric Wand" },
        { new Tuple<string, string>("Staff", "Sword"), "Twin Force" },
        { new Tuple<string, string>("Staff", "Shield"), "Eye of the Old God" },
        { new Tuple<string, string>("Staff", "Staff"), "Marisa's Witch Hat" },
        { new Tuple<string, string>("Staff", "Robe"), "Necronomicon" },
        { new Tuple<string, string>("Staff", "Boots"), "Frozen Scepter" },

        { new Tuple<string, string>("Robe", "Crystal"), "Paladin's Necklace" },
        { new Tuple<string, string>("Robe", "Sword"), "Rule Breaker" },
        { new Tuple<string, string>("Robe", "Shield"), "Guardian Plate" },
        { new Tuple<string, string>("Robe", "Staff"), "Necronomicon" },
        { new Tuple<string, string>("Robe", "Robe"), "Inquisitor's Cloak" },
        { new Tuple<string, string>("Robe", "Boots"), "Gray Fox's Cowl" },

        { new Tuple<string, string>("Boots", "Crystal"), "Emerald Charm" },
        { new Tuple<string, string>("Boots", "Sword"), "Youmu's Roukanken" },
        { new Tuple<string, string>("Boots", "Shield"), "Greaves of Swift" },
        { new Tuple<string, string>("Boots", "Staff"), "Frozen Scepter" },
        { new Tuple<string, string>("Boots", "Robe"), "Gray Fox's Cowl" },
        { new Tuple<string, string>("Boots", "Boots"), "Talaria Shoes" },
    };

}
