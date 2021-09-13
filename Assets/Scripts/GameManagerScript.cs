using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;
using System.IO;


public class GameManagerScript : MonoBehaviour
{


    public UIManager UiManager;
    public FortuneMachine FortuneManager;
    // Start is called before the first frame update


    int PercentChance=0;
    int Iterations=0;
    int StartKredit=0;
    
    string content;//result 
    
    List<string> possibleBets = new List<string>();



    public void parseInputBets()
    {

        possibleBets.Clear();
        string[] inputBets;

        //split multipliers input field by ','
        inputBets = UiManager.Bets.text.Split(',');

        for (int i = 0; i < inputBets.Length; i++)
        {
            if (int.TryParse(inputBets[i], out int value))
            {
                int curNum = Int32.Parse(inputBets[i]);
                if (curNum >= 0)
                {
                    possibleBets.Add(inputBets[i]);
                }
            }
        }
        //replace input field with valid 
        string validNumString = string.Join(",", possibleBets);
        UiManager.Bets.text = validNumString;
        UiManager.Bet.ClearOptions();
        UiManager.Bet.AddOptions(possibleBets);

       


    }


    public void parseMultipliers()
    {
        string[] inputMultipliers;

        //split multipliers input field by ','
        inputMultipliers = UiManager.Multipliers.text.Split(',');

        List<string> validNumList = new List<string>();

        for (int i = 0; i < inputMultipliers.Length; i++)
        {
            if (int.TryParse(inputMultipliers[i], out int value))
            {
                int curNum = Int32.Parse(inputMultipliers[i]);
                if (curNum >= 0)
                {
                    validNumList.Add(inputMultipliers[i]);
                }
            }
        }
        //replace input field with valid 
        string validNumString = string.Join(",", validNumList);
        UiManager.Multipliers.text = validNumString;
        //set multipliers
        FortuneManager.nasobky = new int[validNumList.Count];
        for (int i = 0; i < FortuneManager.nasobky.Length; i++)
        {
            FortuneManager.nasobky[i] = Int32.Parse(validNumList[i]);
        }

    }

    public void PraseInputChances()
    {
        if (UiManager.Multipliers.text=="")
        {
            UiManager.Chances.text = "";
            return;
        }

        string[] inputChances;

        //split chances input field by ','
        inputChances = UiManager.Chances.text.Split(',');


        if (inputChances.Length != FortuneManager.nasobky.Length)
        {
            UiManager.Chances.text = "Chance Distribution array is smaller or bigger than bet array";
            return;
        }

        List<string> validChanceList = new List<string>();

        for (int i = 0; i < inputChances.Length; i++)
        {
            if (float.TryParse(inputChances[i], out float value))
            {
                float curNum = float.Parse(inputChances[i], CultureInfo.InvariantCulture.NumberFormat);
                if (curNum >= 0.0f)
                {
                    validChanceList.Add(inputChances[i]);
                }
            }
        }


        float sum = 0.0f;
        for (int i = 0; i < FortuneManager.nasobky.Length; i++)
        {
            sum+= float.Parse(validChanceList[i], CultureInfo.InvariantCulture.NumberFormat);
        }

        if (sum != 100.0)
        {
            UiManager.Chances.text = "Total chance sum must be 100";
            return;
        }

            //replace input field with valid 
            string validChanceString = string.Join(",", validChanceList);
        UiManager.Chances.text = validChanceString;
        //set chances
        FortuneManager.chanceDistribution = new float[validChanceList.Count];
        for (int i = 0; i < FortuneManager.nasobky.Length; i++)
        {
            FortuneManager.chanceDistribution[i] = float.Parse(validChanceList[i], CultureInfo.InvariantCulture.NumberFormat);
        }


    }

    //parse iterations input - only int is valid
    public void parseIterations()
    {
        string curIter = UiManager.MaxIterations.text;
        if (int.TryParse(curIter, out int value))
        {
            int curNum = Int32.Parse(curIter);
            if (curNum > 0)
            {
                Iterations = curNum;
            }
            else
            {
                Iterations = 0;
            }

        }
        else
        {
            Iterations = 0;
        }

        UiManager.MaxIterations.text=Iterations.ToString();
    }


    //parse chance input - only int is valid
    public void parsePercent()
    {
        string curPerc = UiManager.Percent.text;
        if (int.TryParse(curPerc, out int value))
        {
            int curNum = Int32.Parse(curPerc);
            if (curNum >= 0 && curNum<=100)
            {
                PercentChance = curNum;
            }
            else
            {
                PercentChance = 0;
            }

        }
        else
        {
            PercentChance = 0;
        }

        UiManager.Percent.text = PercentChance.ToString();
    }

    public void parseKredit()
    {
        string curKredit = UiManager.StartKredit.text;
        if (int.TryParse(curKredit, out int value))
        {
            int curNum = Int32.Parse(curKredit);
            if (curNum >= 0 )
            {
                StartKredit = curNum;
            }
            else
            {
                StartKredit = 0;
            }

        }
        else
        {
            StartKredit = 0;
        }

        UiManager.StartKredit.text = StartKredit.ToString();
        FortuneManager.startKredit = StartKredit;
        FortuneManager.NewSession();
    }


    //found nearest percent of bet in available bets
    int nearestPercent(int percent)
    {
        float value = (float)FortuneManager.PlayerCredit() / 100.0f * percent;
        int id=-1;
        float delta = float.MaxValue;

        for (int i = 0; i < possibleBets.Count; i++) {
            
            if (float.Parse(possibleBets[i], CultureInfo.InvariantCulture.NumberFormat) - value <= delta)
            {
                id = i;
                delta = Mathf.Abs(float.Parse(possibleBets[i], CultureInfo.InvariantCulture.NumberFormat) - value);
            }
        }

        return int.Parse(possibleBets[id]);
    }


    public void Export()
    {
        using (StreamWriter writer = new StreamWriter("result.txt", false))
        {
            writer.WriteLine(content);
        }
    }

    //get value of bet based on percent 
    int betValue()
    {
        int bet = 0;
        if (PercentChance == 0)
        {
            if (UiManager.Bet.options.Count > 0)
            {
                bet = int.Parse(UiManager.Bet.captionText.text);
            }
        }
        else
        {
            bet = nearestPercent(PercentChance);

        }
        return bet;
    }

    public void StartMachine()
    {

        
        parseMultipliers();
        parseIterations();
        parsePercent();
        parseKredit();
        PraseInputChances();

       


        string result = "";


        //go till end of iterations (even if kredit is zero)
        if (Iterations > 0)
        {
            for (int i = 0; i < Iterations; i++)
            {
                int bet = betValue();
                result += i +" ";
                result += FortuneManager.PlayerCredit() + " ";
                int nasobek = FortuneManager.CallSpin(bet);
                result += bet + " ";
                result += nasobek * bet +" ";
                result += nasobek + "\n";
               
            }

        }
        else //go till kredit >0
        {
            int nasobek=0;
            int i = 0;
            while (FortuneManager.PlayerCredit() > 0 )
            {
                int bet = betValue();
                string curLine="";
                curLine += i + " ";
                curLine += FortuneManager.PlayerCredit() + " ";
                nasobek = FortuneManager.CallSpin(bet);
                curLine += bet + " ";
                curLine += nasobek * bet + " ";
                curLine += nasobek + "\n";
                
                //check if player has enough money and bet is valid
                if (nasobek == -1)
                {
                    break;
                }
                result += curLine;
                i++;
            }
        }

        content = result;

        //show result in scroll view
        UiManager.log.text = result;
        
    }

}
