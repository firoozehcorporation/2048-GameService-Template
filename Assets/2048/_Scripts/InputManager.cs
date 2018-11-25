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
                GameManager.Instance.Move(MoveDirection.Up);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                GameManager.Instance.Move(MoveDirection.Down);
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GameManager.Instance.Move(MoveDirection.Left);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.Instance.Move(MoveDirection.Right);
            }
        }
    }
}

