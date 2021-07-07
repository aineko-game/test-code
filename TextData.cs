using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextData
{
    public static Dictionary<string, string> Keywords = new Dictionary<string, string>()
    {
        { "After Cast Skill", "After Cast Skill" },
        { "Angle", "Angle" },
        { "Cooldown", "Cooldown" },
        { "Can cast once", "<color=yellow>Can cast once per battle.</color>" },
        { "Can cast while disable", "<color=green>Can cast while disabled.</color>" },
        { "Chain range", "Chain range" },
        { "End of Enemy Turn", "End of Enemy Turn" },
        { "End of Your Turn", "End of Your Turn" },
        { "End of Turn", "End of Turn" },
        { "Hit range", "Hit range" },
        { "Knockback range", "Knockback range" },
        { "Lv.", "Lv." },
        { "magic damage", "magic damage" },
        { "On Cast Skill", "On Cast Skill" },
        { "On Deal Damage", "On Deal Damage" },
        { "On Deal Magic Damage", "On Deal Magic Damage" },
        { "On Deal Physical Damage", "On Deal Physical Damage" },
        { "On Death", "On Death" },
        { "On Destroy Object", "On Destroy Object" },
        { "On Destroyed", "On Destroyed" },
        { "On Kill Enemy", "On Kill Enemy" },
        { "On Taken Basic Attack", "On Taken Basic Attack" },
        { "On Taken Damage", "On Taken Damage" },
        { "On Taken Physical Damage", "On Taken Physical Damage" },
        { "On Taken Magic Damage", "On Taken Magic Damage" },
        { "physical damage", "physical damage" },
        { "protection", "protection" },
        { "Quick cast", "Quick cast (This isn't counted as an action.)" },
        { "static damage", "static damage" },
        { "Start of Turn", "Start of Turn" },
        { "Start of Battle", "Start of Battle" },
        { "stun", "stun" },
        { "Target range", "Target range" },
        { "UNIQUE Active", "UNIQUE Active" },
        { "UNIQUE Passive", "UNIQUE Passive" },
    };

    public static Dictionary<string, string> EventText = new Dictionary<string, string>()
    {
        {
            "Ancient Temple",
            "You find the remains of an ancient temple.\n" +
            "Most of them are broken, but you find a glowing statue in the wreckage."
        },
        {
            "Ancient Temple-0",
            "As <Hero> touched it, magic power flowed from it.\n"
        },
        {
            "Bloody Altar",
            "You find an horrible looking bloody altar.\n" +
            "You try to leave there, but you start to think that you have to offer blood to that altar."
        },
        {
            "Bloody Altar-0",
            "As you offered your blood on the altar, you suddenly had a headache.\n" +
            "When you awoke, the gold was in your hands and you left the place dazed."
        },
        {
            "Bloody Altar-1",
            "You left the bloody altar."
        },
        {
            "Dead Adventurer",
            "You find a dead adventurer on the path.\n" +
            "The body is still fresh, and you feel the presence of the enemy nearby, but a quick scavenging would go unnoticed."
        },
        {
            "Dead Adventurer-0a",
            "You find <Gold> gold!\n" +
            "Continue scavenging?"
        },
        {
            "Dead Adventurer-0b",
            "You find some items!\n" +
            "Continue scavenging?"
        },
        {
            "Dead Adventurer-0c",
            "Before you find loot, the enemy returned!"
        },
        {
            "Dead Adventurer-0d",
            "Fortunately, you scavenged all of his possessions.\n" +
            "The enemy seems to have gone away."
        },
        {
            "Dead Adventurer-1",
            "You left there quietly."
        },
        {
            "Elaborate Shrine",
            "You find an elaborate shrine at the edge of the cliff.\n" +
            "You sense a mystical power from the shrine, and it makes you decide to wish for something."
        },
        {
            "Elaborate Shrine-0",
            "The shrine's energy flows into you."
        },
        {
            "Elaborate Shrine-1",
            "The shrine's energy flows into you"
        },
        {
            "Elaborate Shrine-2",
            "The shrine's energy flows into you"
        },
        {
            "Elaborate Shrine-3",
            "You ignore the shrine."
        },
        {
            "Fight for Money",
            "You have dropped a bag of money down the hillside. \n" +
            "When you try to get it back, you notice that there is a monster."
        },
        {
            "Fight for Money-1",
            "You've given up on retrieving it. It's not worth more than life."
        },
        {
            "Food Peddler",
            "You come across food peddler.\n" +
            "He offer you to buy some foods."
        },
        {
            "Food Peddler-0",
            "You pay money and get <Food Count> food."
        },
        {
            "Food Peddler-3",
            "You decline the offer."
        },
        {
            "Golden Idol",
            "Among the stone and boulders, you find a shiny little golden idol.\n" +
            "It is full of arcane power, and you wonder if you could somehow harness that power."
        },
        {
            "Golden Idol-0",
            "As you destroyed the idol, you feel the arcane power flowing through you."
        },
        {
            "Golden Idol-1",
            "As you prayed to the idol, you feel the arcane power flowing through you."
        },
        {
            "Golden Idol-2",
            "As you took the idol, you feel the arcane power flowing through you."
        },
        {
            "Merchant in trouble",
            "You have found a merchant who is in trouble because his carriage has broken down.\n" +
            "After you help him, he thanks you and offers to give you one of any potions."
        },
        {
            "Old Beggar",
            "You come across an old beggar cloaked in fur.\n" +
            "<i>\"Spare some food, adventurer?\"</i>"
        },
        {
            "Old Beggar-0",
            "<i>\"Thanks friend. Here, have this jewel as a payment.\"</i>"
        },
        {
            "Old Beggar-1",
            "You left the place with a sense of guilt."
        },
        {
            "Potion peddler",
            "You encounter a potion peddler.\n" +
            "He offer you to buy potion."
        },
        {
            "Rockfall",
            "Suddenly, a large rock come tumbling down from the top of the cliff.\n" +
            "There's no time to run."
        },
        {
            "Rockfall-0",
            "<Hero> caught the rock and managed to get out of the way, but he get injured."
        },
        {
            "Rune Device",
            "You find an ancient rune device.\n" +
            "Activating it may gives you some power, but doing so may attract nearby enemies."
        },
        {
            "Rune Device-0",
            "As you activated it, the rune stone began to glow.\n" +
            "A nearby enemy reacts to the light and approaches you."
        },
        {
            "Rune Device-1",
            "You broke the device and took it away.\n" +
            "It could be useful for something."
        },
        {
            "Rune Device-2",
            "You leave that place."
        },
        {
            "Scroll Peddler",
            "You encounter a scroll peddler.\n" +
            "He offer you to buy potion."
        },
        {
            "Scroll Peddler-0",
            "The peddler thanked you and left for the next town."
        },
        {
            "Scroll Peddler-3",
            "You just decline his offer."
        },
        {
            "Shaman in Cloak",
            "You encounter a shaman in a worn-out cloak.\n" +
            "He wished good luck in your adventure and offer to cast blessing of might to one of you."
        },
        {
            "Shaman in Cloak-0",
            "The shaman cast a spell and the hero was filled with power."
        },
        {
            "Statue of the Old God",
            "You find a half-destroyed statue of an old god.\n" +
            "You notice that the statue is decorated with jewels, and you think about destroying it and scavenging."
        },
        {
            "Statue of the Old God-0",
            "You destroyed the statue and took the jewel in your hand.\n" +
            "As you pocket it, something weighs heavily on you."
        },
        {
            "Statue of the Old God-1",
            "You left there."
        },
        {
            "Veteran warrior",
            "You have met a veteran warrior on the way.\n" +
            "He offers to give one of you a lesson."
        },
        {
            "Weapon Merchant",
            "You come across a traveling weapon merchant.\n" +
            "He offers you a deal on weapons."
        },
        {
            "Weapon Merchant-0",
            "You pay money and get <Equip Name>."
        },
        {
            "Weapon Merchant-1",
            "You decline the deal."
        },
        {
            "Whisper of the Old God",
            "You heard a whispering voice calling you from altar.\n" +
            "<i>\"Make an offering to me...\"</i>\n" +
            "That whisper is so attracting."
        },
        {
            "Whisper of the Old God-0",
            "<i>\"Praise the god of death...\"</i>.\n" +
            "You took the sacrament with a burning pain."
        },
        {
            "Whisper of the Old God-1",
            "<i>\"Praise the god of death...\"</i>.\n" +
            "You took the sacrament with a burning pain."
        },
        {
            "Whisper of the Old God-2",
            "<i>\"You have already... lost.\"</i>.\n" +
            "The whisper accuse you."
        },
    };

    public static Dictionary<string, string[]> DescriptionText = new Dictionary<string, string[]>()
    {
        {
            "Abyss",
            new string[]
            {
                "Abyss",
                "This unit's attack ignore 50% of target's <sprite name=MR>."
            }
        },
        {
            "Abyssal Staff",
            new string[]
            {
                "Abyssal Staff",
                "<Unique Passive>Abyss</Unique Passive><Description>Abyss</Description>"
            }
        },
        {
            "Adrenaline",
            new string[]
            {
                "Adrenaline",
                "If this unit's HP is 50% or below, gain +20<sprite name=AR> and +20<sprite name=MR>."
            }
        },
        {
            "Aegis Shield",
            new string[]
            {
                "Aegis Shield",
                "<Unique Passive>Aegis</Unique Passive><Description>Aegis</Description>"
            }
        },
        {
            "Aegis",
            new string[]
            {
                "Aegis",
                "Start of Battle : Gain 1 Protection."
            }
        },
        {
            "AD Buff",
            new string[]
            {
                "AD Buff",
                "Increase <sprite name=AD> in proportion to value."
            }
        },
        {
            "AD Debuff",
            new string[]
            {
                "AD Debuff",
                "Decrease <sprite name=AD> in proportion to value."
            }
        },
        {
            "AD Ratio+",
            new string[]
            {
                "Damage Ratio+",
                "Deal <sprite name=AD>x<fValue00></nobr> additional damage."
            }
        },
        {
            "Amulet Coin",
            new string[]
            {
                "Amulet Coin",
                "Enemies drop 25% more Gold."
            }
        },
        {
            "Angle+",
            new string[]
            {
                "Angle+",
                "Gain <nobr>+<iValue00></nobr> angle."
            }
        },
        {
            "AR Buff",
            new string[]
            {
                "AD Buff",
                "Increase <sprite name=AR> in proportion to value."
            }
        },
        {
            "AR Debuff",
            new string[]
            {
                "AD Debuff",
                "Decrease <sprite name=AR> in proportion to value."
            }
        },
        {
            "Archmage Staff",
            new string[]
            {
                "Archmage Staff",
                "<Unique Passive>Mastery of Magic</Unique Passive><Description>Mastery of Magic</Description>"
            }
        },
        {
            "Arcane Gauntlets",
            new string[]
            {
                "Arcane Gauntlet",
                "<Unique Active>Divine Barrier</Unique Active><Description>Divine Barrier</Description>"
            }
        },
        {
            "Armor Break",
            new string[]
            {
                "Armor Break",
                "Apply <nobr>-<iValue00>%</nobr> <sprite name=AR> debuff to hit units."
            }
        },
        {
            "Armor of Vitality",
            new string[]
            {
                "Armor of Vitality",
                "<Unique Passive>Vitality</Unique Passive><Description>Vitality</Description>"
            }
        },
        {
            "Artifact",
            new string[]
            {
                "Artifact",
                "Negate next debuff."
            }
        },
        {
            "Artifact Ward",
            new string[]
            {
                "Artifact Ward",
                "Start of Battle : All heroes gain 1 Artifact."
            }
        },
        {
            "Assassinate",
            new string[]
            {
                "Assassinate",
                "On Kill Enemy : Gain 1 Stealth."
            }
        },
        {
            "Assassin Dagger",
            new string[]
            {
                "Assassin Dagger",
                "<Unique Passive>Assassinate</Unique Passive><Description>Assassinate</Description>"
            }
        },
        {
            "Barrier",
            new string[]
            {
                "Shield",
                "Prevent damage until the start of this unit's next turn."
            }
        },
        {
            "Base Damage+",
            new string[]
            {
                "Base Damage+",
                "Deal <nobr>+<iValue00></nobr> additional damage."
            }
        },
        {
            "Basic Attack",
            new string[]
            {
                "Basic Attack",
                "Deal <sprite name=AD>x<AD Ratio> physical damage to target enemy or object."
            }
        },
        {
            "Basic Attack Bonus",
            new string[]
            {
                "Basic Attack Bonus",
                "Next basic attack deals <Base Value>% damage."
            }
        },
        {
            "Basic Attack Bonus Battlecry",
            new string[]
            {
                "Basic Attack Bonus",
                "Next basic attack deals <Base Value>% damage."
            }
        },
        {
            "Basic Attack Bonus Spellblade",
            new string[]
            {
                "Basic Attack Bonus",
                "Next basic attack deals <Base Value>% damage."
            }
        },
        {
            "Basic Attack Bonus Tumble",
            new string[]
            {
                "Basic Attack Bonus",
                "Next basic attack deals <Base Value>% damage."
            }
        },
        {
            "Battle Frenzy",
            new string[]
            {
                "Battle Frenzy",
                "All unit gain +30<sprite name=AD>."
            }
        },
        {
            "Battlecry",
            new string[]
            {
                "Battlecry",
                "Gain <Buff Value00>%<sprite name=AD> buff and <Buff Value01>%<sprite name=AR> buff."
            }
        },
        {
            "Berserker Soul",
            new string[]
            {
                "Berserker Soul",
                "Every hero's 5th attack deals x1.5 damage."
            }
        },
        {
            "Berserker",
            new string[]
            {
                "Berserker",
                "On Taken Damage : Gain 10% <sprite name=AD> buff."
            }
        },
        {
            "Berserker Mail",
            new string[]
            {
                "Berserker Mail",
                "<Unique Passive>Berserker</Unique Passive><Description>Berserker</Description>"
            }
        },
        {
            "Big Game Hunter",
            new string[]
            {
                "Big Game Hunter",
                "While boss battle, you gain +30<sprite name=AD> and +30<sprite name=MD>."
            }
        },
        {
            "Binding Grasp",
            new string[]
            {
                "Binding Grasp",
                "If enemies gain debuff, they gain -20% debuff instead."
            }
        },
        {
            "Bloody Cleaver",
            new string[]
            {
                "Bloody Cleaver",
                "<Unique Passive>Carve</Unique Passive><Description>Carve</Description>"
            }
        },
        {
            "Blazing Star",
            new string[]
            {
                "Blazing Star",
                "Gain <sprite name=SP> equal to 10% of <sprite name=MD>."
            }
        },
        {
            "Bleeding",
            new string[]
            {
                "Bleeding",
                "All heroes lose 30% of <sprite name=HP> at the start of next battle."
            }
        },
        {
            "Blessing",
            new string[]
            {
                "Blessing",
                "<Target Hero> gain <iValue>% status buff."
            }
        },
        {
            "Blind",
            new string[]
            {
                "Blind",
                "This unit's basic attack don't deal damage."
            }
        },
        {
            "Blind Spore",
            new string[]
            {
                "Blind Spore",
                "Deal <sprite name=MD>x<MD Ratio> magic damage and blind target enemy."
            }
        },
        {
            "Block Potion",
            new string[]
            {
                "Block Potion",
                "Apply <iValue00> Barrier to target hero."
            }
        },
        {
            "Blood Thirst",
            new string[]
            {
                "Blood Thirst",
                "On Deal Physical Damage : Restore 10% of damage dealt."
            }
        },
        {
            "Bloodlust",
            new string[]
            {
                "Bloodlust",
                "On Kill Enemy : Gain <nobr>+<iValue00>%</nobr> <sprite name=AD> buff."
            }
        },
        {
            "Boon Reflection",
            new string[]
            {
                "Boon Reflection",
                "If hero would restore HP, it restore x1.5 HP instead."
            }
        },
        {
            "Boots",
            new string[]
            {
                "Boots",
                "<flavor>Run like the wind blows.</flavor>"
            }
        },
        {
            "Buff+",
            new string[]
            {
                "Buff+",
                "Gain <nobr>+<iValue00>%</nobr> additional <sprite name=<string00>> buff."
            }
        },
        {
            "Staff of Sunfire",
            new string[]
            {
                "Staff of Sunfire",
                "<Unique Active>Offering</Unique Active><Description>Offering</Description>"
            }
        },
        {
            "Burning Blood",
            new string[]
            {
                "Burning Blood",
                "<Unique Passive>Undying Rage</Unique Passive><Description>Undying Rage</Description>"
            }
        },
        {
            "Butcher Knife",
            new string[]
            {
                "Butcher Knife",
                "<Unique Passive>Dismantle</Unique Passive><Description>Dismantle</Description>"
            }
        },
        {
            "Call Wolves",
            new string[]
            {
                "Call Wolves",
                "Summon 3 wolves."
            }
        },
        {
            "Carnage",
            new string[]
            {
                "Carnage",
                "On Deal Physical Damage : Also apply <nobr>-20%</nobr> <sprite name=AR> debuff."
            }
        },
        {
            "Carve",
            new string[]
            {
                "Carve",
                "On Deal Physical Damage : Also apply <nobr>-20%</nobr> <sprite name=AR> debuff."
            }
        },
        {
            "Chain Lightning",
            new string[]
            {
                "Chain Lightning",
                "Deal (<sprite name=MD>x<MD Ratio>+<MD Base>) magic damage to target enemy and chain damage through nearby enemies."
            }
        },
        {
            "Chain Range+",
            new string[]
            {
                "Chain Range+",
                "Gain <nobr>+<iValue00></nobr> chain range."
            }
        },
        {
            "Charge",
            new string[]
            {
                "Charge",
                "Dash <Move Range> distance and deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage to hit enemies and objects nearby."
            }
        },
        {
            "Mini Hakkero",
            new string[]
            {
                "Mini Hakkero",
                "<Unique Passive>Spark!</Unique Passive><Description>Spark!</Description>"
            }
        },
        {
            "Circle of Healing",
            new string[]
            {
                "Circle of Healing",
                "Restore (<sprite name=HP>x0.3+20) HP to all heroes around this unit."
            }
        },
        {
            "Cleave",
            new string[]
            {
                "Cleave",
                "Basic attack deals 10% of target's current HP as bonus physical damage.."
            }
        },
        {
            "Cloak of Shadows",
            new string[]
            {
                "Cloak of Shadows",
                "<Unique Active>Shadow Hide You</Unique Active><Description>Shadow Hide You</Description>"
            }
        },
        {
            "Colossus Hammer",
            new string[]
            {
                "Colossus Hammer",
                "<Unique Passive>Skullcrusher</Unique Passive><Description>Skullcrusher</Description>"
            }
        },
        {
            "Cone of Flame",
            new string[]
            {
                "Cone of Flame",
                "Fire a cone of flame in the target direction and deal (<sprite name=MD>x<MD Ratio>+<MD Base>) magic damage <Hit Count> times to all enemies and objects."
            }
        },
        {
            "Cooldown-",
            new string[]
            {
                "Cooldown-",
                "Reduce cooldown by <iValue00>."
            }
        },
        {
            "Cosmic Plasma",
            new string[]
            {
                "Cosmic Plasma",
                "Fire a projectile and deal <sprite name=MD>x1.0 magic damage to hit enemy."
            }
        },
        {
            "Crystal",
            new string[]
            {
                "Crystal",
                "<flavor>This is filled with the power of life."
            }
        },
        {
            "Curse",
            new string[]
            {
                "Curse",
                "All heroes lose 10% of current <sprite name=HP> at end of turn.\n" +
                "Last for next <iCount> battle."
            }
        },
        {
            "Damage Ratio+",
            new string[]
            {
                "Damage Ratio+",
                "Deal <sprite name=AD>x<fValue00></nobr> additional damage."
            }
        },
        {
            "Debuff+",
            new string[]
            {
                "Debuff+",
                "Gain <nobr>-<iValue00>%</nobr> additional <sprite name=<string00>> debuff."
            }
        },
        {
            "Demolisher",
            new string[]
            {
                "Demolisher",
                "On Destroy Object : Gain +20% <sprite name=AD> buff."
            }
        },
        {
            "Demonic Frenzy",
            new string[]
            {
                "Demonic Frenzy",
                "Gain 50% <sprite name=AD> buff."
            }
        },
        {
            "Discount Ticket",
            new string[]
            {
                "Discount Ticket",
                "20% discount on products in the store."
            }
        },
        {
            "Dismantle",
            new string[]
            {
                "Dismantle",
                "If target's HP is 50% or below, deal x1.2 damage."
            }
        },
        {
            "Divine Barrier",
            new string[]
            {
                "Divine Barrier",
                "Gain (<sprite name=AR>x1.0 + 40) shield."
            }
        },
        {
            "Double Punch",
            new string[]
            {
                "Double Punch",
                "Deal <sprite name=AD>x<AD Ratio> physical damage 2 times to target hero or object."
            }
        },
        {
            "Dragon Slayer",
            new string[]
            {
                "Dragon Slayer",
                "<Unique Passive>Berserkr</Unique Passive><Description>Berserkr</Description>"
            }
        },
        {
            "Emerald Charm",
            new string[]
            {
                "Emerald Charm",
                "<Unique Active>Haste</Unique Active><Description>Haste</Description>"
            }
        },
        {
            "Enemy Attack",
            new string[]
            {
                "Basic Attack",
                "Deal <sprite name=AD>x<AD Ratio> physical damage to target hero or object."
            }
        },
        {
            "Enlightenment",
            new string[]
            {
                "Enlightenment",
                "End of Your Turn : Deal (<sprite name=MD>x0.5 + 20) magic damage to all units nearby."
            }
        },
        {
            "Execution",
            new string[]
            {
                "Execution",
                "Deal (<sprite name=AD>x<AD Ratio>+<iValue>% of target's missing <sprite name=HP>) physical damage to target enemy."
            }
        },
        {
            "Execution+",
            new string[]
            {
                "Execution+",
                "Deal <nobr>+<iValue00>%</nobr> of target's missing <sprite name=HP> additional damage."
            }
        },
        {
            "Explosive Bug",
            new string[]
            {
                "Explosive Bug",
                "On Death : Explode and deal 40 magic damage to units and objects around this unit."
            }
        },
        {
            "Explosive Potion",
            new string[]
            {
                "Explosive Potion",
                "Deal <SD Damage> static damage to all enemies and objects in area.\n\n" +
                "Hit range:<Hit Range>"
            }
        },
        {
            "Eye of the Old God",
            new string[]
            {
                "Eye of the Old God",
                "<Unique Active>Cosmic Plasma</Unique Active><Description>Cosmic Plasma</Description>"
            }
        },
        {
            "Fasting",
            new string[]
            {
                "Fasting",
                "Lose 1 maximum food value. Gain 1000 Gold."
            }
        },
        {
            "Fire Potion",
            new string[]
            {
                "Fire Potion",
                "Deal <SD Damage> static damage to target enemy."
            }
        },
        {
            "Fireball",
            new string[]
            {
                "Fireball",
                "Fire a explosive fireball and deal (<sprite name=MD>x<MD Ratio>+<MD Base>) magic damage to hit enemies and objects."
            }
        },
        {
            "First Blood",
            new string[]
            {
                "First Blood",
                "The first hero to kill enemy gain 30% <sprite name=AD> buff."
            }
        },
        {
            "Flying",
            new string[]
            {
                "Flying",
                "This unit can move through any type of units and objects."
            }
        },
        {
            "Foods",
            new string[]
            {
                "Foods",
                "Consume 1 food per moves.\n" +
                "If running out, you get hungry."
            }
        },
        {
            "Force",
            new string[]
            {
                "Force",
                "This unit's basic attack deals 0.15x<sprite name=MD> bonus damage."
            }
        },
        {
            "Force of Nature",
            new string[]
            {
                "Force of Nature",
                "If heroes gain buff, they gain +20% buff instead."
            }
        },
        {
            "Frozen Scepter",
            new string[]
            {
                "Frozen Scepter",
                "<Unique Passive>Rimefrost</Unique Passive><Description>Rimefrost</Description>"
            }
        },
        {
            "Frost Nova",
            new string[]
            {
                "Frost Nova",
                "Make a frost explosion and deal (<sprite name=MD>x<MD Ratio>+<MD Base>) magic damage and apply <nobr><Buff Value00>%</nobr> <sprite name=SP> debuff to hit enemies."
            }
        },
        {
            "Fury",
            new string[]
            {
                "Fury",
                "Deal 50% additional damage."
            }
        },
        {
            "Gain Barrier",
            new string[]
            {
                "Gain Barrier",
                "Gain <iValue00> Barrier."
            }
        },
        {
            "Gain Buff",
            new string[]
            {
                "Gain Buff",
                "Gain <nobr>+<iValue00>%</nobr> <sprite name=<string00>> buff."
            }
        },
        {
            "Game Speed",
            new string[]
            {
                "Game Speed",
                "Change the game speed to <nobr>x1 ~ x3</nobr>."
            }
        },
        {
            "Gray Fox's Cowl",
            new string[]
            {
                "Gray Fox's Cowl",
                "<Unique Active>Shadow Hide You</Unique Passive><Description>Shadow Hide You</Description>"
            }
        },
        {
            "Ghost Step",
            new string[]
            {
                "Ghost Step",
                "Leap (<sprite name=SP>x0.4 + 10) distance and deal <sprite name=AD>x1.0 physical damage to closest enemy."
            }
        },
        {
            "Giant Slayer",
            new string[]
            {
                "Giant Slayer",
                "If target's HP is greater than this unit's HP, deal x1.2 damage."
            }
        },
        {
            "Greaves of Swift",
            new string[]
            {
                "Greaves of Swift",
                "<Unique Passive>Swiftness</Unique Passive><Description>Swiftness</Description>"
            }
        },
        {
            "Guardian Plate",
            new string[]
            {
                "Guardian Plate",
                "<Unique Passive>Resurrection</Unique Passive><Description>Resurrection</Description>"
            }
        },
        {
            "Haste",
            new string[]
            {
                "Hate",
                "Apply 30% <sprite name=SP> buff to target hero."
            }
        },
        {
            "Heart of Iron",
            new string[]
            {
                "Heart of Iron",
                "<Unique Passive>Adrenaline</Unique Passive><Description>Adrenaline</Description>"
            }
        },
        {
            "Heal",
            new string[]
            {
                "Heal",
                "Restore <fValue00>x<sprite name=HP> HP."
            }
        },
        {
            "Healing Potion",
            new string[]
            {
                "Healing Potion",
                "Restore <Restore Value> HP to target hero."
            }
        },
        {
            "Heavy Strike",
            new string[]
            {
                "Heavy Strike",
                "Deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage to target enemy and stun it."
            }
        },
        {
            "Harvest",
            new string[]
            {
                "Harvest",
                "Gain 1 maximum food value."
            }
        },
        {
            "Hit Count+",
            new string[]
            {
                "Hit Count+",
                "Gain <nobr>+<iValue00></nobr> hit count."
            }
        },
        {
            "Hit Range+",
            new string[]
            {
                "Hit Range+",
                "Gain <nobr>+<iValue00></nobr> hit range."
            }
        },
        {
            "Howling",
            new string[]
            {
                "Howling",
                "Gain <Buff Value00>%<sprite name=AD> buff."
            }
        },
        {
            "HP Buff",
            new string[]
            {
                "HP Buff",
                "Increase <sprite name=HP> in proportion to value."
            }
        },
        {
            "HP Debuff",
            new string[]
            {
                "HP Debuff",
                "Decrease <sprite name=HP> in proportion to value."
            }
        },
        {
            "Hunger",
            new string[]
            {
                "Hunger",
                "Status is reduced by half.\n" +
                "You can't get loots."
            }
        },
        {
            "Ice Shard",
            new string[]
            {
                "Ice Shard",
                "Throw an ice sphere and deal (<sprite name=MD>x<MD Ratio>+<MD Base>) magic damage to hit enemy. " +
                "If this hit enemy, it shatter and deal same damage to hit enemies."
            }
        },
        {
            "Inflame",
            new string[]
            {
                "Inflame",
                "Gain 1% bonus <sprite name=AD> buff for each 1% of <nobr>missing <sprite name=HP></nobr>."
            }
        },
        {
            "Inquisitor's Cloak",
            new string[]
            {
                "Inquisitor's Cloak",
                "<Unique Active>Quicksilver</Unique Active><Description>Quicksilver</Description>"
            }
        },
        {
            "Injured",
            new string[]
            {
                "Injured",
                "<Target Hero> take 40 damage at the start of battle."
            }
        },
        {
            "Inspire",
            new string[]
            {
                "Inspire",
                "Apply <Buff Value00>% all-type buff to target ally."
            }
        },
        {
            "Intimidation",
            new string[]
            {
                "Intimidation",
                "Every 4th battle, all enemies have 25% less HP at the start of battle."
            }
        },
        {
            "Killing Spree",
            new string[]
            {
                "Killing Spree",
                "On Kill Enemy : Reduce cooldown by <iValue00>."
            }
        },
        {
            "Knockback Shot",
            new string[]
            {
                "Knockback Shot",
                "Deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage and knockback target enemy. If target collide something, stun it."
            }
        },
        {
            "Goldgrubber",
            new string[]
            {
                "Goldgrubber",
                "When you open Treasure, gain 200 Gold."
            }
        },
        {
            "Lightning Spear",
            new string[]
            {
                "Lightning Spear",
                "<Unique Passive>Pierce</Unique Passive><Description>Pierce</Description>"
            }
        },
        {
            "Magic Broom",
            new string[]
            {
                "Magic Broom",
                "<Unique Passive>Blazing Star</Unique Passive><Description>Blazing Star</Description>"
            }
        },
        {
            "Magic Burst",
            new string[]
            {
                "Magic Burst",
                "On Kill Enemy : Gain <iValue00> <sprite name=MD> buff."
            }
        },
        {
            "Magic Shield",
            new string[]
            {
                "Magic Shield",
                "Apply (<Barrier Value>+<sprite name=MD>x<MD Ratio>) shield to target hero."
            }
        },
        {
            "Marisa's Witch Hat",
            new string[]
            {
                "Marisa's Witch Hat",
                "<Unique Passive>Phantasmagoria</Unique Passive><Description>Phantasmagoria</Description>"
            }
        },
        {
            "Mastery of Magic",
            new string[]
            {
                "Mastery of Magic",
                "Start of Battle : If this unit have 100 or more <sprite name=MD>, reduce the cooldown of all skills by 1."
            }
        },
        {
            "MD Buff",
            new string[]
            {
                "MD Buff",
                "Increase <sprite name=MD> in proportion to value."
            }
        },
        {
            "MD Debuff",
            new string[]
            {
                "MD Debuff",
                "Decrease <sprite name=MD> in proportion to value."
            }
        },
        {
            "MD Ratio+",
            new string[]
            {
                "Damage Ratio+",
                "Deal <sprite name=MD>x<fValue00></nobr> additional damage."
            }
        },
        {
            "Mighty Guard",
            new string[]
            {
                "Mighty Guard",
                "At the start of hero's 3rd turn, all heroes gain +50 Shield."
            }
        },
        {
            "Move Range+",
            new string[]
            {
                "Move Range+",
                "Gain <nobr>+<iValue00></nobr> move range."
            }
        },
        {
            "Mox Almight",
            new string[]
            {
                "Mox Almight",
                "<flavor>The \" Vintage\" item.</flavor>"
            }
        },
        {
            "MR Buff",
            new string[]
            {
                "MR Buff",
                "Increase <sprite name=MR> in proportion to value."
            }
        },
        {
            "MR Debuff",
            new string[]
            {
                "MR Debuff",
                "Decrease <sprite name=MR> in proportion to value."
            }
        },
        {
            "Murasama",
            new string[]
            {
                "Murasama",
                "<Unique Passive>Inflame</Unique Passive><Description>Inflame</Description>"
            }
        },
        {
            "Necronomicon",
            new string[]
            {
                "Necronomicon",
                "<Unique Passive>Enlightenment</Unique Passive><Description>Enlightenment</Description>"
            }
        },
        {
            "NULL",
            new string[]
            {
                "NULL",
                "This is a bug. Report me!"
            }
        },
        {
            "On the Hunt",
            new string[]
            {
                "On the Hunt",
                "Gain <Buff Value00>%<sprite name=AD> buff and <Buff Value01>%<sprite name=SP> buff."
            }
        },
        {
            "Offering",
            new string[]
            {
                "Offering",
                "Lose 30% of current HP, gain 40% <sprite name=MD> buff."
            }
        },
        {
            "Outrage",
            new string[]
            {
                "Outrage",
                "Heroes with HP is at or below 50% gain +30 <sprite name=AD>."
            }
        },
        {
            "Paladin's Necklace",
            new string[]
            {
                "Paladin's Necklace",
                "<Unique Active>Circle of Healing</Unique Active><Description>Circle of Healing</Description>"
            }
        },
        {
            "Passive_Warrior",
            new string[]
            {
                "Blood Thirst",
                "Restore 30% of damage dealt to enemies."
            }
        },
        {
            "Passive_Hunter",
            new string[]
            {
                "Back Attack",
                "Deal 50% additional damage when this unit attack enemy from behind."
            }
        },
        //{
        //    "Passive_Hunter",
        //    new string[]
        //    {
        //        "Soul Collector",
        //        "On Kill Enemy : Gain 1 stack.\n" +
        //        "Basic Attack and skills deal 10% bonus damage for each stack."
        //    }
        //},
        {
            "Passive_Mage",
            new string[]
            {
                "Magic Burst",
                "On Cast Skill : Deal 30% additional damage."
            }
        },
        {
            "Passive_Demon",
            new string[]
            {
                "Inflame",
                "Gain 1% bonus <sprite name=AD> buff for each 1% of <nobr>missing <sprite name=HP></nobr>."
            }
        },
        {
            "Passive_Slime",
            new string[]
            {
                "Weakness",
                "If this unit's <sprite name=HP> is below 50%, it takes 30% additional damage."
            }
        },
        {
            "Passive_Met",
            new string[]
            {
                "Thorn",
                "On Taken Physical Damage : Reflects 20% of taken damage as static damage."
            }
        },
        {
            "Passive_Rat",
            new string[]
            {
                "Revenge",
                "When other rats died, gain 20% <sprite name=AD> buff."
            }
        },
        {
            "Passive_Wolf",
            new string[]
            {
                "Group Hunting",
                "Gain bonus 10% <sprite name=AD> buff for each other wolves."
            }
        },
        {
            "Passive_Golem",
            new string[]
            {
                "Stone Skin",
                "End of Turn : Gain 30 shield."
            }
        },
        {
            "Passive_Explosive Bug",
            new string[]
            {
                "Explosion",
                "On Death : Explode and deal 40 magic damage to units and objects around this unit."
            }
        },
        {
            "Passive_Faerie",
            new string[]
            {
                "Devotion",
                "End of Turn : Resotre 40<sprite name=HP> to other nearby allies."
            }
        },
        {
            "Passive_Frightfly",
            new string[]
            {
                "Windfury",
                "This unit can take action twice."
            }
        },
        {
            "Passive_Fungusa",
            new string[]
            {
                "Spawn Sapling",
                "On Taken Physical Damage : Spawn Fungee."
            }
        },
        {
            "Passive_Fungee",
            new string[]
            {
                "Mortal",
                "End of Turn : Lose 20% of Max <sprite name=HP>."
            }
        },
        {
            "Passive_Treant",
            new string[]
            {
                "Nature's Blessing",
                "Gain 1% bonus <sprite name=AR> and <sprite name=MR> buff for each 1% of <nobr>missing <sprite name=HP></nobr>."
            }
        },
        {
            "Passive_Explosive Crystal",
            new string[]
            {
                "Explosion",
                "On Destroyed : Explode and deal 40 magic damage to units and objects around this object."
            }
        },
        {
            "Passive_Healing Crystal",
            new string[]
            {
                "Healing",
                "On Destroyed : Restore 40<sprite name=HP> to units around this object."
            }
        },
        {
            "Passive_Rune Stone",
            new string[]
            {
                "Rune Magic",
                "End of Turn : Restore 40<sprite name=HP> to all heroes in area."
            }
        },
        {
            "Phantasmagoria",
            new string[]
            {
                "Phantasmagoria",
                "Increase <sprite name=MD> by 30%."
            }
        },
        {
            "Photosynthesis",
            new string[]
            {
                "Photosynthesis",
                "Restore <Restore Value> HP and remove all debuff."
            }
        },
        {
            "Pierce",
            new string[]
            {
                "Piercing Shot",
                "This unit's basic attack ignore 50% of target's <sprite name=AR>."
            }
        },
        {
            "Piercing Shot",
            new string[]
            {
                "Piercing Shot",
                "Fire a piercing arrow in the target direction and deal (<sprite name=AD>x<AD Ratio>+<AD Base>) magic damage to all enemies and objects."
            }
        },
        {
            "Potion Mastery",
            new string[]
            {
                "Potion Mastery",
                "Double the effectiveness of potions."
            }
        },
        {
            "Predation",
            new string[]
            {
                "Predation",
                "On Kill Enemy : Restore 20 HP."
            }
        },
        {
            "Protect",
            new string[]
            {
                "Protect",
                "Apply <Barrier Value> shield to target enemy."
            }
        },
        {
            "Protection",
            new string[]
            {
                "Protection",
                "Negate next damage."
            }
        },
        {
            "Purify",
            new string[]
            {
                "Purify",
                "Remove all debuff and disable from self."
            }
        },
        {
            "Pristine Talisman",
            new string[]
            {
                "Pristine Talisman",
                "<Unique Active>Purify</Unique Active><Description>Purify</Description>"
            }
        },
        {
            "Blade of Fury",
            new string[]
            {
                "Blade of Fury",
                "<Unique Passive>Rage</Unique Passive><Description>Rage</Description>"
            }
        },
        {
            "Rage",
            new string[]
            {
                "Rage",
                "Start of Battle : Gain 1 Fury."
            }
        },
        {
            "Raining Arrow",
            new string[]
            {
                "Raining Arrow",
                "Fire an arrows to target point and deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage and apply <nobr><Base Value>%</nobr><sprite name=SP> debuff."
            }
        },
        {
            "Rampage",
            new string[]
            {
                "Rampage",
                "Deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage to target enemy. Increase damage per 1% of this unit's <nobr>missing <sprite name=HP></nobr> (Max 200%)."
            }
        },
        {
            "Resurrection",
            new string[]
            {
                "Resurrection",
                "On Death : Ressurect with 50% of Max <sprite name=HP>."
            }
        },
        {
            "Return to Origin",
            new string[]
            {
                "Return to Origin",
                "Remove target unit's all buff, debuff, disable and passive."
            }
        },
        {
            "Rimefrost",
            new string[]
            {
                "Rimefrost",
                "On Deal Magic Damage : Also apply <nobr>-20</nobr> <sprite name=SP> debuff."
            }
        },
        {
            "Robe",
            new string[]
            {
                "Robe",
                "<flavor>This protects you from basic spells.</flavor>"
            }
        },
        {
            "Rule Breaker",
            new string[]
            {
                "Rule Breaker",
                "<Unique Active>Return to Origin</Unique Active><Description>Return to Origin</Description>"
            }
        },
        {
            "Rune Blade",
            new string[]
            {
                "Rune Blade",
                "<flavor>A sword with magic rune.</flavor>"
            }
        },
        {
            "Rune of Life",
            new string[]
            {
                "Rune of Life",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Rune of Strength",
            new string[]
            {
                "Rune of Strength",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Rune of Protection",
            new string[]
            {
                "Rune of Protection",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Rune of Magic",
            new string[]
            {
                "Rune of Magic",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Rune of Blessing",
            new string[]
            {
                "Rune of Blessing",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Rune of Swift",
            new string[]
            {
                "Rune of Swift",
                "Apply <nobr><Buff Value00>%</nobr> <sprite name=<Buff Type00>> buff to target hero."
            }
        },
        {
            "Savage Smash",
            new string[]
            {
                "Savage Smash",
                "Deal <sprite name=AD>x<AD Ratio> physical damage to target hero and stun it."
            }
        },
        {
            "Scroll of Blessing",
            new string[]
            {
                "Scroll of Blessing",
                "All heroes gain 80 shield."
            }
        },
        {
            "Scroll of Fury",
            new string[]
            {
                "Scroll of Fury",
                "All heroes gain <color=yellow>Fury</color>."
            }
        },
        {
            "Scroll of Protection",
            new string[]
            {
                "Scroll of Protection",
                "All heroes gain <color=yellow>Protection</color>."
            }
        },
        {
            "Shadow Hide You",
            new string[]
            {
                "Shadow Hide You",
                "Gain stealth 1 turn."
            }
        },
        {
            "Shattering Smash",
            new string[]
            {
                "Shattering Smash",
                "Deal x1.5 damage to stunned enemy."
            }
        },
        {
            "Shell Attack",
            new string[]
            {
                "Shell Attack",
                "Deal <AD Base> physical damage to target enemy."
            }
        },
        {
            "Shield",
            new string[]
            {
                "Shield",
                "<flavor>An explorer's essentials in a wild world.</flavor>\n"
            }
        },
        {
            "Moonlight Rod",
            new string[]
            {
                "Moonlight Rod",
                "<Unique Active>Magic Shield</Unique Active><Description>Magic Shield</Description>"
            }
        },
        {
            "Siphon Soul",
            new string[]
            {
                "Siphon Soul",
                "On Deal Magic Damage : Restore 10% of damage dealt."
            }
        },
        {
            "Skullcrusher",
            new string[]
            {
                "Skullcrusher",
                "This unit's first basic attack each battle also apply stun."
            }
        },
        {
            "Soulcleaver",
            new string[]
            {
                "Soulcleaver",
                "<Unique Passive>Cleave</Unique Passive><Description>Cleave</Description>"
            }
        },
        {
            "SP Buff",
            new string[]
            {
                "SP Buff",
                "Increase <sprite name=SP> in proportion to value."
            }
        },
        {
            "SP Debuff",
            new string[]
            {
                "SP Debuff",
                "Decrease <sprite name=SP> in proportion to value."
            }
        },
        {
            "Spark!",
            new string[]
            {
                "Spark!",
                "End of Turn : Gain 1 stack.\n" +
                "On Cast Skill : Consume all stacks and deal 10% bonus damage for each stacks."
            }
        },
        {
            "Spellblade",
            new string[]
            {
                "Spellblade",
                "After Cast Skill : next basic attack deal 130% damage."
            }
        },
        {
            "Spellblade On",
            new string[]
            {
                "Spellblade On",
                "Next basic attack deals 150% damage."
            }
        },
        {
            "Spike Shield",
            new string[]
            {
                "Spike Shield",
                "<Unique Passive>Thorn</Unique Passive><Description>Thorn</Description>"
            }
        },
        {
            "Staff",
            new string[]
            {
                "Staff",
                "<flavor>A symbol of wisdom in adventuring times.</flavor>."
            }
        },
        {
            "Staff of Force",
            new string[]
            {
                "Staff of Force",
                "<Unique Passive>Force</Unique Passive><Description>Force</Description>"
            }
        },
        {
            "Start of Battle - Buff Heroes",
            new string[]
            {
                "<String> Buff",
                "<color=yellow>At the Start of Battle</color> Gain <Buff Value00>% <sprite name=<String>> buff."
            }
        },
        {
            "Stealth",
            new string[]
            {
                "Stealth",
                "Can't be targeted by enemy until this unit's next turn or this unit's next action. "
            }
        },
        {
            "Stealth Strike",
            new string[]
            {
                "Stealth Strike",
                "If attacking hero is stealth, deal x1.5 damage to enemy. "
            }
        },
        {
            "Stigma",
            new string[]
            {
                "Stigma",
                "On Kill Enemy : Gain 40 Shield."
            }
        },
        {
            "Sting",
            new string[]
            {
                "Sting",
                "On Kill Enemy : Gain 30% status buff."
            }
        },
        {
            "Stun",
            new string[]
            {
                "Stun",
                "Can't take action this turn."
            }
        },
        {
            "Sudden Attack",
            new string[]
            {
                "Sudden Attack",
                "Hero's first attack each battle deals x1.5 damage."
            }
        },
        {
            "Swift",
            new string[]
            {
                "Swift",
                "Gain +15<sprite name=SP> until this unit take first damage each battle."
            }
        },
        {
            "Amulet of Wind",
            new string[]
            {
                "Amulet of Wind",
                "<Unique Passive>Wind Blast</Unique Passive><Description>Wind Blast</Description>"
            }
        },
        {
            "Sword",
            new string[]
            {
                "Sword",
                "<flavor>Let's get started an adventure!</flavor>"
            }
        },
        {
            "Sword Break",
            new string[]
            {
                "Sword Break",
                "Apply <nobr>-<iValue00></nobr>% <sprite name=AD> debuff to hit units."
            }
        },
        {
            "Talaria Shoes",
            new string[]
            {
                "Talaria Shoes",
                "<Unique Passive>Flying</Unique Passive><Description>Flying</Description>"
            }
        },
        {
            "Titan Axe",
            new string[]
            {
                "Titan Axe",
                "<Unique Passive>Giant Slayer</Unique Passive><Description>Giant Slayer</Description>"
            }
        },
        {
            "Thorn",
            new string[]
            {
                "Thorn",
                "On Taken Physical Damage : Reflects 20% of taken damage as static damage."
            }
        },
        {
            "Thorn Armor",
            new string[]
            {
                "Thorn Armor",
                "<Unique Passive>Thorn</Unique Passive><Description>Thorn</Description>"
            }
        },
        {
            "Tumble",
            new string[]
            {
                "Tumble",
                "Leap (<sprite name=SP>x<SP Ratio>+<Move Range>) distance. Next basic attack deals <Base Value>% damage."
            }
        },
        {
            "Tumble+",
            new string[]
            {
                "Tumble+",
                "Next basic attack deals <nobr>+<iValue00>%</nobr> additional damage."
            }
        },
        {
            "Golden Armor",
            new string[]
            {
                "Golden Armor",
                "Prevent the first damage a hero takes."
            }
        },
        {
            "Battle Rage",
            new string[]
            {
                "Battle Rage",
                "Every 4th battle, heroes gain 40% <sprite name=AD> and <sprite name=MD> buff at the start of battle."
            }
        },
        {
            "Guardian Shield",
            new string[]
            {
                "Guardian Shield",
                "Heroes don't lose shield at start of turn."
            }
        },
        {
            "Meal Ticket",
            new string[]
            {
                "Meal Ticket",
                "Whenever you go shop, heroes resotre full HP."
            }
        },
        {
            "Thunder Strike",
            new string[]
            {
                "Thunder Strike",
                "Deal (<sprite name=MD>x<MD Ratio>) magic damage and stun it."
            }
        },
        {
            "Twin Force",
            new string[]
            {
                "Twin Force",
                "<Unique Passive>Spellblade</Unique Passive><Description>Spellblade</Description>"
            }
        },
        {
            "Undying Rage",
            new string[]
            {
                "Undying Rage",
                "End of Your Turn : Restore 5% of Max <sprite name=HP>."
            }
        },
        {
            "Volley",
            new string[]
            {
                "Volley",
                "Shoot arrows in five directions, each deal <sprite name=AD>x<AD Ratio> physical damage to first hit enemy. Enemies don't take extra damage, even if they hit multipile arrows."
            }
        },
        {
            "Vampiric Wand",
            new string[]
            {
                "Vampiric Wand",
                "<Unique Passive>Siphon Soul</Unique Passive><Description>Siphon Soul</Description>"
            }
        },
        {
            "Youmu's Roukanken",
            new string[]
            {
                "Youmu's Roukanken",
                "<Unique Active>Ghost Step</Unique Active><Description>Ghost Step</Description>"
            }
        },
        {
            "Vitality",
            new string[]
            {
                "Vitality",
                "Negate snare and stun."
            }
        },
        {
            "Wind Blast",
            new string[]
            {
                "Wind Blast",
                "Start of Battle : Gain 40% <sprite name=SP> buff."
            }
        },
        {
            "Wind Cloak",
            new string[]
            {
                "Wind Cloak",
                "<Unique Passive>Swift</Unique Passive><Description>Swift</Description>"
            }
        },
        {
            "Whirlwind",
            new string[]
            {
                "Whirlwind",
                "Deal (<sprite name=AD>x<AD Ratio>+<AD Base>) physical damage to all enemies and objects around this unit."
            }
        },
    };
}
