using Sourav.Utilities.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecimalToBinary : MonoBehaviour
{
    public long Decimal;
    public string Binary;

    [Button()]
    public void Convert()
    {
        Binary = "";

        long dec = Decimal;
        long remainder = 0;
        long Base = 1;

        while(dec > 0)
        {
            remainder = dec % 2;
            Binary += remainder;
            dec = dec / 2;
        }

        Debug.Log(Binary);
        BinaryGap(Binary);
    }

    private void BinaryGap(string s)
    {
        int maxZeros = 0;
        int numberOfZeros = 0;
        bool startCounting = false;

        for(int i = s.Length - 1; i>=0; i--)
        {
            if(s[i] == '1')
            {
                startCounting = true;
            }

            if(startCounting)
            {
                if(s[i] == '0')
                {
                    numberOfZeros++;
                }
                else
                {
                    maxZeros = (maxZeros >= numberOfZeros) ? maxZeros : numberOfZeros;
                    numberOfZeros = 0;
                }
            }
        }

        Debug.Log("Max Binary Gap = "+maxZeros);
    }
}
