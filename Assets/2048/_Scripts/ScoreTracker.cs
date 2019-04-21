using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace _2048._Scripts
{
    public class ScoreTracker : MonoBehaviour
    {
        public Text ScoreText;
        public Text HighScoreText;

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                ScoreText.text = _score.ToString();
                if (PlayerPrefs.GetInt("HighScore") >= _score) return;
                PlayerPrefs.SetInt("HighScore", _score);
                HighScoreText.text = _score.ToString();
            }
        }
        
        private void Awake()
        {
            if(!PlayerPrefs.HasKey("HighScore")){ PlayerPrefs.SetInt("HighScore", 0);}

            ScoreText.text = "0";
            HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    }
}
