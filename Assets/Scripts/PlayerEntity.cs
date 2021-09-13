using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity 
{

    int Kredit=0;

    int currentBet=0;
    //Constructor 
    public PlayerEntity(int startKredit){
        if (startKredit < 0)
        {
            Kredit = 0;
        }
        else
        {
            Kredit = startKredit;
        }
    }

    public int SetBet (int bet)
    {

        //bet cant be negative
        if(bet < 0)
        {
            return -1;
        }

        //check ballance after spin - can't be negative 
        if (Kredit - bet >= 0)
        {
            currentBet = bet;
            Kredit -= bet;
            return currentBet;
        }
        else
        {
            return -1;
        }
    }

    public void SetKredit(int newKredit)
    {
        if (newKredit < 0)
        {
            Kredit = 0;
            return;
        }
        Kredit = newKredit;
    }
    public int GetKredit()
    {
        return Kredit;
    }
   
}
