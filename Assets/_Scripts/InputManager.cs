using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum MoveDirection { Left, Right, Up, Down }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}

