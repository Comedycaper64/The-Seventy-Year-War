using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseStats", menuName = "The Seventy Year War/BaseStats", order = 0)]
public class BaseStats : ScriptableObject
{
    [Header("Main Stats")]
    [SerializeField]
    private int strength = 10;

    [SerializeField]
    private int dexterity = 10;

    [SerializeField]
    private int constitution = 10;

    [SerializeField]
    private int intelligence = 10;

    [SerializeField]
    private int wisdom = 10;

    [SerializeField]
    private int charisma = 10;

    [Header("Other")]
    [SerializeField]
    private int proficiencyBonus = 2;

    [SerializeField]
    private HitDiceType hitDice;

    public int GetStrength()
    {
        return strength;
    }

    public int GetDexterity()
    {
        return dexterity;
    }

    public int GetConstitution()
    {
        return constitution;
    }

    public int GetIntelligence()
    {
        return intelligence;
    }

    public int GetWisdom()
    {
        return wisdom;
    }

    public int GetCharisma()
    {
        return charisma;
    }

    public int GetProficiencyBonus()
    {
        return proficiencyBonus;
    }

    public HitDiceType GetHitDiceType()
    {
        return hitDice;
    }

    public void SetStrength(int newStrength)
    {
        strength = newStrength;
    }
}
