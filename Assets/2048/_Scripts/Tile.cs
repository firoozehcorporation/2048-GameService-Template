using UnityEngine;
using UnityEngine.UI;

namespace _2048._Scripts
{
    public class Tile : MonoBehaviour
    {
        private int _number;
        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                if (_number == 0)
                {
                    SetEmpty();
                }
                else
                {
                    ApplyStyle(_number);
                    SetVisible();
                }
            }
        }
        private Text _tileText;
        private Image _tileImage;

        public int IndexForRow;
        public int IndexForColumn;
        
        private void Awake()
        {
            _tileText = GetComponentInChildren<Text>();
            _tileImage = transform.Find("NumberedCell").GetComponent<Image>();
        }

        private void ApplyStyleFromHolder(int index)
        {
            _tileText.text = GameManager.Instance.TileStyleHolder.TileStyles[index].Number.ToString();
            _tileText.color = GameManager.Instance.TileStyleHolder.TileStyles[index].TextColor;
            _tileImage.color = GameManager.Instance.TileStyleHolder.TileStyles[index].TileColor;
        }

        private void ApplyStyle(int num)
        {
            int tileIndex = GameManager.Instance.TileStyleHolder.GetIndexOfNumber(num);
            if(num >= 0)
            {
                ApplyStyleFromHolder(tileIndex);
            }
            else
            {
                Debug.LogError("Invalid tile number "+num+". Cannot apply style as this tile is not contained in tile list");
            }
        }

        private void SetVisible()
        {
            _tileImage.enabled = true;
            _tileText.enabled = true;
        }

        private void SetEmpty()
        {
            _tileImage.enabled = false;
            _tileText.enabled = false;
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
