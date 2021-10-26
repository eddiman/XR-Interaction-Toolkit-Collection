using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class TimeClock : MonoBehaviour {
    public TextMeshProUGUI textClock;

    private void Update (){
        DateTime time = DateTime.Now;
        string hour = LeadingZero( time.Hour );
        string minute = LeadingZero( time.Minute );
        string second = LeadingZero( time.Second );
        textClock.text = hour + ":" + minute + ":" + second;
    }
    private string LeadingZero (int n){
        return n.ToString().PadLeft(2, '0');
    }
}
