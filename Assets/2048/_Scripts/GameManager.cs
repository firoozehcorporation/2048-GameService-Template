using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FiroozehGameServiceAndroid;
using FiroozehGameServiceAndroid.Builders;
using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _2048._Scripts
{
    public enum GameState
    {
        Playing,
        GameOver,
        WaitingForMoveToEnd
    }
    
    public class GameManager : MonoBehaviour
    {
        public static FiroozehGameService GameService;
        public GameState State;
        [Range(0, 2f)]
        public float Delay;
        [SerializeField]private bool _moveMade;
        [SerializeField]private bool[] _lineMoveComplete = new bool[4]{ true, true, true, true };
        
        public Text GameOverScoreText;
        public GameObject GameOverPanel;

        public GameObject GameWonPanel;
        
        public static GameManager Instance;

        public TileStyleHolder TileStyleHolder;
        public ScoreTracker ScoreTracker;
        
        private Tile[,] _allTiles;
        private List<Tile[]> _columns;
        private List<Tile[]> _rows;
        private List<Tile> _emptyTiles;

        private bool _disableInput;
        private bool _has2048Reached;
        
        
        private void Awake()
        {
            Instance = this;
            
        
            
        }

        private void Start()
        {
            if (GameService == null)
            {

                FiroozehGameServiceInitializer
                    .With("Your clientId", "Your clientSecret")
                    .IsNotificationEnable(true)
                    .CheckGameServiceInstallStatus(true)
                    .CheckGameServiceOptionalUpdate(true)
                    .Init(g => { GameService = g; },
                        e => { Debug.Log("FiroozehGameServiceInitializerError: " + e); });
            }

            StartNewGame();
        }

        public void StartNewGame()
        {
            State = GameState.Playing;
            _has2048Reached = false;
            GameOverPanel.Hide();
            GameWonPanel.Hide();
            _allTiles = new Tile[4,4];
            _columns = new List<Tile[]>();
            _rows = new List<Tile[]>();
            _emptyTiles = new List<Tile>();
            ScoreTracker.Score = 0;
            Tile[] allTilesOneDim = FindObjectsOfType<Tile>();
            for (int i = 0; i < allTilesOneDim.Length; i++)
            {
                allTilesOneDim[i].Number = 0;
                _allTiles[allTilesOneDim[i].IndexForRow, allTilesOneDim[i].IndexForColumn] = allTilesOneDim[i];
                _emptyTiles.Add(allTilesOneDim[i]);
            }

            _columns.Add(new Tile[] {_allTiles[0, 0], _allTiles[1, 0], _allTiles[2, 0], _allTiles[3, 0]});
            _columns.Add(new Tile[] {_allTiles[0, 1], _allTiles[1, 1], _allTiles[2, 1], _allTiles[3, 1]});
            _columns.Add(new Tile[] {_allTiles[0, 2], _allTiles[1, 2], _allTiles[2, 2], _allTiles[3, 2]});
            _columns.Add(new Tile[] {_allTiles[0, 3], _allTiles[1, 3], _allTiles[2, 3], _allTiles[3, 3]});

            _rows.Add(new Tile[] {_allTiles[0, 0], _allTiles[0, 1], _allTiles[0, 2], _allTiles[0, 3]});
            _rows.Add(new Tile[] {_allTiles[1, 0], _allTiles[1, 1], _allTiles[1, 2], _allTiles[1, 3]});
            _rows.Add(new Tile[] {_allTiles[2, 0], _allTiles[2, 1], _allTiles[2, 2], _allTiles[2, 3]});
            _rows.Add(new Tile[] {_allTiles[3, 0], _allTiles[3, 1], _allTiles[3, 2], _allTiles[3, 3]});

            Generate();
            Generate();
        }

        public void ContinuePlayingUponWinning()
        {
            State = GameState.Playing;
            GameWonPanel.Hide();
        }

        private void YouWon()
        {
            if (!_has2048Reached)
            {
                GameWonPanel.Show();
                _has2048Reached = true;
                State = GameState.GameOver;
            }
        }
        
        private void GameOver()
        {
            GameOverScoreText.text = ScoreTracker.Score.ToString();
            GameOverPanel.Show();
            State = GameState.GameOver;
        }

        private bool CanMove()
        {
            if (_emptyTiles.Count > 0)
                return true;
            else
            {
                //check columns
                for (int i = 0; i < _columns.Count; i++)
                {
                    for (int j = 0; j < _rows.Count - 1; j++)
                    {
                        if (_allTiles[j, i].Number == _allTiles[j + 1, i].Number)
                            return true;
                    }
                }
                
                //check rows
                for (int i = 0; i < _rows.Count; i++)
                {
                    for (int j = 0; j < _columns.Count - 1; j++)
                    {
                        if (_allTiles[i, j].Number == _allTiles[i, j + 1].Number)
                            return true;
                    }
                }
                
                return false;
            }
        }
        
        private bool MakeOneMoveDownIndex(IReadOnlyList<Tile> lineOfTiles)
        {
            for (int i = 0; i < lineOfTiles.Count - 1; i++)
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
                    lineOfTiles[i].Play_MergedAnimation();
                    ScoreTracker.Score += lineOfTiles[i].Number;

                    if (lineOfTiles[i].Number == 2048)
                    {
                        
                        YouWon();
                    }
                    
                    return true;
                }
            }

            return false;
        }

        private bool MakeOneMoveUpIndex(IReadOnlyList<Tile> lineOfTiles)
        {
            for (int i = lineOfTiles.Count - 1; i > 0; i--)
            {
                //Move block 
                if (lineOfTiles[i].Number == 0 && lineOfTiles[i - 1].Number != 0)
                {
                    Swap(lineOfTiles[i], lineOfTiles[i-1]);
                    return true;
                }
                
                //Merge block
                if (lineOfTiles[i].Number == lineOfTiles[i - 1].Number &&
                    !lineOfTiles[i].MergedThisTurn && !lineOfTiles[i - 1].MergedThisTurn && 
                    lineOfTiles[i].Number != 0)
                {
                    lineOfTiles[i].Number *= 2;
                    lineOfTiles[i - 1].Number = 0;

                    lineOfTiles[i].MergedThisTurn = true;
                    lineOfTiles[i - 1].MergedThisTurn = true;
                    lineOfTiles[i].Play_MergedAnimation();
                    ScoreTracker.Score += lineOfTiles[i].Number;

                    if (lineOfTiles[i].Number == 2048)
                    {
                        YouWon();
                    }
                    
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
                _emptyTiles[indexForNewNumber].Play_AppearAnimation();
                _emptyTiles.RemoveAt(indexForNewNumber);
            }
        }

        private void UpdateEmptyTiles()
        {
            _emptyTiles.Clear();
            foreach (var tile in _allTiles)
            {
                if (tile.Number == 0)
                {
                    _emptyTiles.Add(tile);
                }
            }
        }

        public void Move(MoveDirection moveDirection)
        {
            if (!_moveMade && State == GameState.WaitingForMoveToEnd)
                State = GameState.Playing;
            
            if (State != GameState.Playing)
                return;
            
            ResetMergedFlags();

            if (Delay > 0)
            {
                //Start coroutine to simulate delay
                StartCoroutine(MoveCoroutine(moveDirection));
            }
            else
            {
                for (int i = 0; i < _rows.Count; i++)
                {
                    switch (moveDirection)
                    {
                        case MoveDirection.Left:
                            while (MakeOneMoveDownIndex(_rows[i]))
                            {
                                _moveMade = true;
                            }
                            break;
                        case MoveDirection.Right:
                            while (MakeOneMoveUpIndex(_rows[i]))
                            {
                                _moveMade = true;
                            }
                            break;
                        case MoveDirection.Up:
                            while (MakeOneMoveDownIndex(_columns[i]))
                            {
                                _moveMade = true;
                            }
                            break;
                        case MoveDirection.Down:
                            while (MakeOneMoveUpIndex(_columns[i]))
                            {
                                _moveMade = true;
                            }
                            break;
                    }
                }
                if (_moveMade)
                {
                    UpdateEmptyTiles();
                    Generate();
        
                    if (!CanMove())
                    {
                        GameOver();
                    }
                }
                _moveMade = false;
                State = GameState.Playing;
            }
        }

        private IEnumerator MoveCoroutine(MoveDirection moveDirection)
        {
            State = GameState.WaitingForMoveToEnd;

            switch (moveDirection)
            {
                case MoveDirection.Down:
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        StartCoroutine(MoveOneLineUpIndexCoroutine(_columns[i], i));
                    }
                    break;
                case MoveDirection.Left:
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        StartCoroutine(MoveOneLineDownIndexCoroutine(_rows[i], i));
                    }
                    break;
                case MoveDirection.Right:
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        StartCoroutine(MoveOneLineUpIndexCoroutine(_rows[i], i));
                    }
                    break;
                case MoveDirection.Up:
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        StartCoroutine(MoveOneLineDownIndexCoroutine(_columns[i], i));
                    }
                    break;
            }

            while (!(_lineMoveComplete[0] && _lineMoveComplete[1] && _lineMoveComplete[2] && _lineMoveComplete[3]))
                yield return null;

            if (_moveMade)
            {
                UpdateEmptyTiles();
                Generate();

                if (!CanMove())
                {
                    GameOver();
                }
            }
            _moveMade = false;
            State = GameState.Playing;
        }

        private IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index)
        {
            _lineMoveComplete[index] = false;
            while (MakeOneMoveUpIndex(line))
            {
                _moveMade = true;
                yield return new WaitForSeconds(Delay);
            }

            _lineMoveComplete[index] = true;
        }

        private IEnumerator MoveOneLineDownIndexCoroutine(Tile[] line, int index)
        {
            _lineMoveComplete[index] = false;
            while (MakeOneMoveDownIndex(line))
            {
                _moveMade = true;
                yield return new WaitForSeconds(Delay);
            }

            _lineMoveComplete[index] = true;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Generate();
            }
        }
    }
}
