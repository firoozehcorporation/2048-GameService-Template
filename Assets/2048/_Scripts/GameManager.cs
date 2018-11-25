using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _2048._Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public TileStyleHolder TileStyleHolder;
        
        private Tile[,] _allTiles = new Tile[4,4];
        private List<Tile[]> _columns = new List<Tile[]>();
        private List<Tile[]> _rows = new List<Tile[]>();
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
            
            _columns.Add(new Tile[]{_allTiles[0, 0], _allTiles[1, 0], _allTiles[2, 0], _allTiles[3, 0]});
            _columns.Add(new Tile[]{_allTiles[0, 1], _allTiles[1, 1], _allTiles[2, 1], _allTiles[3, 1]});
            _columns.Add(new Tile[]{_allTiles[0, 2], _allTiles[1, 2], _allTiles[2, 2], _allTiles[3, 2]});
            _columns.Add(new Tile[]{_allTiles[0, 3], _allTiles[1, 3], _allTiles[2, 3], _allTiles[3, 3]});
            
            _rows.Add(new Tile[]{_allTiles[0, 0], _allTiles[0, 1], _allTiles[0, 2], _allTiles[0, 3]});
            _rows.Add(new Tile[]{_allTiles[1, 0], _allTiles[1, 1], _allTiles[1, 2], _allTiles[1, 3]});
            _rows.Add(new Tile[]{_allTiles[2, 0], _allTiles[2, 1], _allTiles[2, 2], _allTiles[2, 3]});
            _rows.Add(new Tile[]{_allTiles[3, 0], _allTiles[3, 1], _allTiles[3, 2], _allTiles[3, 3]});
        }

        private bool MakeOneMoveDownIndex(Tile[] lineOfTiles)
        {
            for (int i = 0; i < lineOfTiles.Length - 1; i++)
            {
                //Move block 
                if (lineOfTiles[i].Number == 0 && lineOfTiles[i + 1].Number != 0)
                {
                    Swap(lineOfTiles[i], lineOfTiles[i+1]);
                    return true;
                }
                
                //Merge block
                if (lineOfTiles[i].Number == lineOfTiles[i + 1].Number &&
                    !lineOfTiles[i].MergedThisTurn && !lineOfTiles[i + 1].MergedThisTurn && 
                    lineOfTiles[i].Number != 0)
                {
                    lineOfTiles[i].Number *= 2;
                    lineOfTiles[i + 1].Number = 0;

                    lineOfTiles[i].MergedThisTurn = true;
                    lineOfTiles[i + 1].MergedThisTurn = true;

                    return true;
                }
            }

            return false;
        }

        private bool MakeOneMoveUpIndex(Tile[] lineOfTiles)
        {
            for (int i = lineOfTiles.Length - 1; i > 0; i--)
            {
                //Move block 
                if (lineOfTiles[i].Number == 0 && lineOfTiles[i - 1].Number != 0)
                {
                    Swap(lineOfTiles[i], lineOfTiles[i-1]);
                    return true;
                }
                
                //Merge block
                if (lineOfTiles[i].Number == lineOfTiles[i - 1].Number &&
                    !lineOfTiles[i].MergedThisTurn && !lineOfTiles[i - 1] && 
                    lineOfTiles[i].Number != 0)
                {
                    lineOfTiles[i].Number *= 2;
                    lineOfTiles[i - 1].Number = 0;

                    lineOfTiles[i].MergedThisTurn = true;
                    lineOfTiles[i - 1].MergedThisTurn = true;

                    return true;
                }
            }

            return false;
        }

        private void ResetMergedFlags()
        {
            foreach (var tile in _allTiles)
            {
                tile.MergedThisTurn = false;
            }
        }
        
        private void Swap(Tile i, Tile j)
        {
            i.Number += j.Number;
            j.Number = i.Number - j.Number;
            i.Number = i.Number - j.Number;
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

        public void Move(MoveDirection moveDirection)
        {
            ResetMergedFlags();
            
            for (int i = 0; i < _rows.Count; i++)
            {
                switch (moveDirection)
                {
                    case MoveDirection.Left:
                        while (MakeOneMoveDownIndex(_rows[i])){}
                        break;
                    case MoveDirection.Right:
                        while (MakeOneMoveUpIndex(_rows[i])){}
                        break;
                    case MoveDirection.Up:
                        while (MakeOneMoveDownIndex(_columns[i])){}
                        break;
                    case MoveDirection.Down:
                        while (MakeOneMoveUpIndex(_columns[i])){}
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
                }
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
