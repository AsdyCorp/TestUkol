using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneMachine : MonoBehaviour
{
    PlayerEntity player;
    public int startKredit = 100;
    public int[] nasobky;
    public float[] chanceDistribution;

    //current chance distribution postion in array  
    //uses in wheel animation visualizttion
    int curID=0;

    
    public void NewSession()
    {
        //create new player
        player = new PlayerEntity(startKredit);

        //check array
        checkChanceArray();
       
        
    }


    public int GetID()
    {
        return curID;
    }


    //in editor checks
    void checkChanceArray() {
        //Check if Chance distribution array is valid
        if (chanceDistribution.Length != nasobky.Length)
        {
            Debug.LogError("Chance Distribution array is smalleror bigger than multiples array");
            Debug.Break();
        }
        for (int i = 0; i < chanceDistribution.Length; i++)
        {
            if (chanceDistribution[i] > 100.0f)
            {
                Debug.LogError("Chance Distribution single value cant be more than 100%");
                Debug.Break();
            }
        }
        float totalChance = 0.0f;
        for (int i = 0; i < chanceDistribution.Length; i++)
        {
            totalChance += chanceDistribution[i];
        }

        if ((totalChance - 100.0f) > 0.0001f)
        {
            Debug.LogError("Chance Distribution total value cant be more than 100%, Current value:" +totalChance);
            Debug.Break();
        }
        if ((totalChance - 100.0f) < -0.0001f)
        {
            Debug.LogError("Chance Distribution total value cant be less than 100%, Current value:" + totalChance);
            Debug.Break();
        }
    }


    int GetSpinResult()
    {
        //check array
        checkChanceArray();
        

        int[] nasobkyCur = new int[nasobky.Length];
        float[] chanceDistributionCur= new float[chanceDistribution.Length];


        //for random result with chance distribution we will need sorted array based by distribution
        //manipulate only with copies 
        Array.Copy(nasobky, nasobkyCur,nasobky.Length);
        Array.Copy(chanceDistribution, chanceDistributionCur, chanceDistributionCur.Length);
        Array.Sort(chanceDistributionCur, nasobkyCur);


        //distribution based random
        float randRes = UnityEngine.Random.Range(0.0f, 1.0f);
       
  
        float curChance = 0.0f;
        for(int i= chanceDistributionCur.Length-1; i>=0; i--)
        {
            
            if (curChance<=randRes && randRes < curChance+chanceDistributionCur[i]/100.0f)
            {
                curID = i;
                return nasobkyCur[i];
            }
            else
            {
                curChance += chanceDistributionCur[i] / 100.0f;
            }
        }

        return 0;

    }

    int Spin(int bet)
    {
        //check if player can set bet 
        if (player.SetBet(bet) >= 0)
        {
            
            
            int curResult = GetSpinResult();
            
            player.SetKredit(player.GetKredit() + curResult * bet);
            return curResult;

        }
        return 0;
        //Debug.Log(curResult + " "+player.GetKredit());
    }

    public int PlayerCredit()
    {
        return player.GetKredit();
    }
    //call spin from game manager
    public int CallSpin(int bet)
    {
        if (bet <= 0 || player.GetKredit()-bet<0)
        {
            return -1;
        }
        else
        {
            return Spin(bet);
        }
    }


}
