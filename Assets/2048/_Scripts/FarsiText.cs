using System;
using FiroozehGameServiceAndroid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _2048._Scripts
{
    public enum TextType
    {
        Name
        ,des
        ,Score
        ,HighScore
        ,NewGame
        ,GameOver
        ,Won
        ,Restart
        ,Continue
      
    }
    
    public class FarsiText : MonoBehaviour
    {
        public TextType Type;
        public Text Text;

        
        private void Start()
        {
            Text = GetComponent<Text>();
            switch (Type)
            {
                case TextType.Name:
                    Text.text = FarsiTextUtil.FixText("بازی ۲۰۴۸");
                    break;
                case TextType.des:
                    Text.text = FarsiTextUtil.FixText("قدرت گرفته از");
                    break;
                case TextType.Score:
                    Text.text = FarsiTextUtil.FixText("امتیاز");
                    break;
                case TextType.HighScore:
                    Text.text = FarsiTextUtil.FixText("بیشترین امتیاز");
                    break;
                case TextType.NewGame:
                    Text.text = FarsiTextUtil.FixText("بازی جدید");
                    break;
                case TextType.GameOver:
                    Text.text = FarsiTextUtil.FixText("باختی! امتیاز شما : ");
                    break;
                case TextType.Won:
                    Text.text = FarsiTextUtil.FixText("بردی!آفرین");
                    break;
                case TextType.Restart:
                    Text.text = FarsiTextUtil.FixText("دوباره");
                    break;
                case TextType.Continue:
                    Text.text = FarsiTextUtil.FixText("ادامه");
                    break;
                default:
                   break;
            }
        }
    }
}