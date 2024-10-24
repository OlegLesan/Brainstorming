using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public void Awake()
    {
        instance = this;
    }

    public int currentMoney;

    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void GiveMoney (int amountToGive)
    {
        currentMoney += amountToGive;
    }
}
