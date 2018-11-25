using System;
using UnityEngine;

namespace _2048._Scripts
{
    public enum MoveDirection { Left, Right, Up, Down }
    public class InputManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(MoveDirection.Up);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(MoveDirection.Down);
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(MoveDirection.Left);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(MoveDirection.Right);
            }
        }

        private void Move(MoveDirection direction)
        {
            switch(direction)
            {
                case MoveDirection.Left:
                    Debug.Log("Move Left");
                    break;

                case MoveDirection.Right:
                    Debug.Log("Move Right");
                    break;

                case MoveDirection.Up:
                    Debug.Log("Move Up");
                    break;

                case MoveDirection.Down:
                    Debug.Log("Move Down");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}

