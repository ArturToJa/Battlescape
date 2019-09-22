using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HitChancer
{
    int TestNumber;
    DamageCalculator dc;
    public UnitScript Attacker;
    public UnitScript Defender;

    public HitChancer(UnitScript attacker, UnitScript defender, int testNumber)
    {
        dc = new DamageCalculator();
        Attacker = attacker;
        Defender = defender;
        TestNumber = testNumber;
    }

    public float MissChance(bool badRange)
    {
        int misses = GetAmountOfResult(badRange,0);
        return ((float)misses / (float)TestNumber) * 100;
    }
    public float HitNoDamageChance(bool badRange)
    {
        int hits = GetAmountOfResult(badRange, 1);
        return ((float)hits / (float)TestNumber) * 100;
    }

    private int GetAmountOfResult(bool badRange, int resultWanted)
    {
        int result = 0;
        int treshold = dc.CalculateTreshold(Attacker, Defender);
        int diceNumber = dc.CalculateDiceNumber(Attacker, Defender, badRange);
        for (int i = 0; i < TestNumber; i++)
        {
            if (dc.ThrowDice(Attacker, Defender, badRange, treshold, diceNumber) == resultWanted)
            {
                result++;
            }
        }

        return result;
    }

    public int[] GetMinMaxDmg()
    {
        int[] results = new int[2];
        results[0] = dc.GetMiniDmg(Attacker, Defender);
        results[1] = dc.GetMaxiDmg(Attacker, Defender);
        return results;
    }

    /*public float AverageDamage(bool badRange)
    {
        int treshold = dc.CalculateTreshold(Attacker, Defender);
        int diceNumber = dc.CalculateDiceNumber(Attacker, Defender, badRange);
        if (Application.isEditor && Input.GetKey(KeyCode.LeftControl))
        {
            Debug.LogError("Treshold: " + treshold);
            Debug.LogError("Dice: " + diceNumber);
        }
        int[] results = new int[TestNumber];
        List<int> hittingResults = new List<int>();

        for (int i = 0; i < TestNumber; i++)
        {
            results[i] = dc.CalculateDamageForCalculations(Attacker, Defender, badRange, treshold, diceNumber );
            if (results[i] != 0)
            {
                hittingResults.Add(results[i]);
            }
        }

        int[] hittingResArray = hittingResults.ToArray();
        float average = (float)hittingResArray/ hittingResArray.Length;
        return average;
    }*/
}

