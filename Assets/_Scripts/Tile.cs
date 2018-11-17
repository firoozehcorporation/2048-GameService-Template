using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private Text TileText;
    private Image TileImage;

    private void Awake()
    {
        TileText = GetComponentInChildren<Text>();
        TileImage = transform.Find("NumberedCell").GetComponent<Image>();
    }

    private void ApplyStyleFromHolder(int index)
    {
        TileText.text = GameManager.Instance.tileStyleHolder.TileStyles[index].Number.ToString();
        TileText.color = GameManager.Instance.tileStyleHolder.TileStyles[index].TextColor;
        TileImage.color = GameManager.Instance.tileStyleHolder.TileStyles[index].TileColor;
    }

    private void ApplyStyle(int num)
    {
        int tileIndex = GameManager.Instance.tileStyleHolder.GetIndexOfNumber(num);
        if(num >= 0)
        {
            ApplyStyleFromHolder(tileIndex);
        }
        else
        {
            Debug.LogError("Invalid tile number "+num+". Cannot apply style as this tile is not contained in tile list");
        }
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
