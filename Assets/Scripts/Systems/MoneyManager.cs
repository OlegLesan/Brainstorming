using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int currentMoney;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIController.instance.goldText.text = currentMoney.ToString();
    }

    void Update()
    {
    }

    public void GiveMoney(int amountToGive)
    {
        currentMoney += amountToGive;
        UIController.instance.goldText.text = currentMoney.ToString();
    }

    public bool SpendMoney(int amountToSpend)
    {
        bool canSpend = false;

        if (amountToSpend <= currentMoney)
        {
            canSpend = true;
            currentMoney -= amountToSpend;
        }
        UIController.instance.goldText.text = currentMoney.ToString();

        return canSpend;
    }
}
