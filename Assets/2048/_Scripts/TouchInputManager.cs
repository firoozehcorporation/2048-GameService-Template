using UnityEngine;

namespace _2048._Scripts
{
    public class TouchInputManager : MonoBehaviour
    {
        // https://pfonseca.com/swipe-detection-on-unity

        private float _fingerStartTime  = 0.0f;
        private Vector2 _fingerStartPos = Vector2.zero;
	
        private bool _isSwipe = false;
        private float minSwipeDist  = 50.0f;
        private float maxSwipeTime = 1.5f;
	
        private GameManager _gm;

        private void Awake()
        {
            _gm = GameManager.Instance;
        }

        // Update is called once per frame
        private void Update () {
		
            if (_gm.State == GameState.Playing && Input.touchCount > 0){
			
                foreach (Touch touch in Input.touches)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began :
                            /* this is a new touch */
                            _isSwipe = true;
                            _fingerStartTime = Time.time;
                            _fingerStartPos = touch.position;
                            break;
					
                        case TouchPhase.Canceled :
                            /* The touch is being canceled */
                            _isSwipe = false;
                            break;
					
                        case TouchPhase.Ended :
					
                            float gestureTime = Time.time - _fingerStartTime;
                            float gestureDist = (touch.position - _fingerStartPos).magnitude;
					
                            if (_isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
                                Vector2 direction = touch.position - _fingerStartPos;
                                Vector2 swipeType;
						
                                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
                                    // the swipe is horizontal:
                                    swipeType = Vector2.right * Mathf.Sign(direction.x);
                                }else{
                                    // the swipe is vertical:
                                    swipeType = Vector2.up * Mathf.Sign(direction.y);
                                }
						
                                if(swipeType.x != 0.0f){
                                    if(swipeType.x > 0.0f){
                                        // MOVE RIGHT
                                        _gm.Move (MoveDirection.Right);
                                    }else{
                                        // MOVE LEFT
                                        _gm.Move (MoveDirection.Left);
                                    }
                                }
						
                                if(swipeType.y != 0.0f ){
                                    if(swipeType.y > 0.0f){
                                        // MOVE UP
                                        _gm.Move (MoveDirection.Up);
                                    }else{
                                        // MOVE DOWN
                                        _gm.Move (MoveDirection.Down);
                                    }
                                }
						
                            }
					
                            break;
                    }
                }
            }
        }
    }
}
