using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _2048._Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public TileStyleHolder TileStyleHolder;
        
        private Tile[,] _allTiles = new Tile[4,4];
        private List<Tile> _emptyTiles = new List<Tile>();
        
        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Clear the gamefield
            Tile[] allTilesOneDim = FindObjectsOfType<Tile>();
            for (int i = 0; i < allTilesOneDim.Length; i++)
            {
                allTilesOneDim[i].Number = 0;
                _allTiles[allTilesOneDim[i].IndexForRow, allTilesOneDim[i].IndexForColumn] = allTilesOneDim[i];
                _emptyTiles.Add(allTilesOneDim[i]);
            }
        }

        private void Generate()
        {
            if (_emptyTiles.Count > 0)
            {
                int indexForNewNumber = Random.Range(0, _emptyTiles.Count);
                int randomNumber = Random.Range(0, 10);
                if (randomNumber == 0)
                {
                    //FIXME remove the hardcoded number
                    _emptyTiles[indexForNewNumber].Number = 4;
                }
                else
                {
                    //FIXME remove the hardcoded number
                    _emptyTiles[indexForNewNumber].Number = 2;
                }
                _emptyTiles.RemoveAt(indexForNewNumber);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Generate();
            }
        }
    }
}
