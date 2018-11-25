using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sourav.Utilities.Scripts
{
    public static class RandomAlphaNumeric 
    {
        public static string GenerateRandomAlpha(int number)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[number];
            var random = new System.Random();
            
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            
            string finalString = new String(stringChars);
            return finalString;
        }
        
        public static string GenerateRandomNumeric(int number)
        {
            var chars = "0123456789";
            var stringChars = new char[number];
            var random = new System.Random();
            
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            
            string finalString = new String(stringChars);
            return finalString;
        }
    }
    
}