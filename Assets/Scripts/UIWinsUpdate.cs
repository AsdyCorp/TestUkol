using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinsUpdate : MonoBehaviour
{

    public int[] bets;
    int betId = 0;

    int currentBet = 100;
    //UI
    public Text winsTable;
    public Text dollarTable;
    public Text betUI;
    public Text kreditUI;
    public Image wheel;
    
    //fortune manager
    public FortuneMachine FortuneManager;
    
    //text of possible wins table
    string winsTableText;
    //                                1  2    3   4      5    6    7  8      9  10
    int[] wheelAngles = new int[10] { 0, 288, 180, 72, 324, 144, 108, 252, 216, 36 };


    //block new spin after spin start till spin end
    bool blockSpin=false;
    float ticks = 0;
    //needed angle index from wheelAngles
    int rotationAngle;

    //needed rotation steps(1 per angle) to rotate wheel to start + rotate it 10 times + rotate it 1 rotation ro last point(slowly) 
    int angleSteps;
    //change bet size with buttons 
    public void changeBet(int step)
    {
        if (blockSpin != true)
        {
            betId += step;
            betId = (betId % bets.Length + bets.Length) % bets.Length;
            betUI.text = bets[betId] + "$";
            currentBet = bets[betId];
            CountTable();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
     
        //start new session and set default values
        FortuneManager.NewSession();
        //set ui kredit
        kreditUI.text = "SCORE " + FortuneManager.PlayerCredit() + " $";
        if (bets.Length == 0)
        {
            bets = new int[1] {100};
        }
        betUI.text = bets[betId] +"$";
        currentBet = bets[betId];
        CountTable();

    }

    // Update is called once per frame
    public void CountTable ()
    {

        winsTableText = "";
        for(int i=FortuneManager.nasobky.Length-1; i>=0; i--)
        {
            winsTableText +=" $"+currentBet * FortuneManager.nasobky[i]+"\n";
        }
        winsTable.text = winsTableText;
        winsTableText = "";
        for (int i = 0; i < FortuneManager.nasobky.Length; i++)
        {
            winsTableText += i+1+ " "+  "\n";
        }
        
        dollarTable.text = winsTableText;
    }


    ///rotate fastly at start but slowly later 
    ///50 ticks in 1 second
    void FixedUpdate()
    {
       
        if (blockSpin == true) {
            if (ticks < 360 - (int)wheel.transform.eulerAngles.z + wheelAngles[rotationAngle])
            {
                ticks += 10;
                wheel.transform.Rotate(new Vector3(0, 0, 10));
            }
            else
            {
                float step = (angleSteps - ticks) / 36.0f;
                if (step <= 1.0f)
                {
                    step = 1.0f;
                }
                ticks += step;
                wheel.transform.Rotate(new Vector3(0, 0,step));
            }
            if (ticks >= angleSteps)
            {
                wheel.transform.eulerAngles=new Vector3(0, 0, wheelAngles[rotationAngle]);
                blockSpin = false;
                ticks = 0;
                //increase kredit after rotation 
                kreditUI.text = "SCORE " + FortuneManager.PlayerCredit() + " $";
            }
        }
    }

    ///spin 
    public void StartSpin()
    {
        if (FortuneManager.PlayerCredit()-currentBet>=0) {
             
           
            if (blockSpin != true)
            {
                //decrease kredit before rotation 
                kreditUI.text = "SCORE " + (FortuneManager.PlayerCredit() - currentBet) + " $";
                FortuneManager.CallSpin(currentBet);
                
                rotationAngle = FortuneManager.GetID();
                angleSteps = 360 - (int)wheel.transform.eulerAngles.z + 360 + wheelAngles[rotationAngle];
                blockSpin = true;
            }
            
            
        }
    }



}
