using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPanZoomEdge : MonoBehaviour
{
    public float PanSpeed;
    public float ScreenPadding;

    public float ScrollSpeed;
    public Vector2 MinMaxX;
    public Vector2 MinMaxY;
    public Vector2 MinMaxZ;

    public Vector2 MinMaxExtentsX;
    public Vector2 MinMaxExtentsY;

    public float defaultCameraSize;

    public bool ShouldMove;

    private Camera camera;

    [SerializeField]
    float leftExtent;
    [SerializeField]
    float rightExtent;
    [SerializeField]
    float topExtent;
    [SerializeField]
    float bottomExtent;

    private void Awake()
    {
        camera = GetComponent<Camera> ( );
    }

	void Update ()
    {
        //TODO think about who can move camera and who can't, and about timescale.
        //if ( !GameManager.Instance.CanMoveCamera ( player ) )
        //{
        //    return;
        //}

        Vector3 pos = transform.localPosition;
        float cameraSize = camera.orthographicSize;


        if(ShouldMove)
        {
            if ( Input.GetKey ( KeyCode.W ) || Input.mousePosition.y >= Screen.height - ScreenPadding )
            {
                pos.y += PanSpeed * Time.deltaTime;
            }

            if ( Input.GetKey ( KeyCode.S ) || Input.mousePosition.y <= ScreenPadding )
            {
                pos.y -= PanSpeed * Time.deltaTime;
            }

            if ( Input.GetKey ( KeyCode.D ) || Input.mousePosition.x >= Screen.width - ScreenPadding )
            {
                pos.x += PanSpeed * Time.deltaTime;
            }

            if ( Input.GetKey ( KeyCode.A ) || Input.mousePosition.x <= ScreenPadding )
            {
                pos.x -= PanSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis ( "Mouse ScrollWheel" );
			cameraSize += scroll * ScrollSpeed * 100f * Time.deltaTime *(-1f);

            pos.x = Mathf.Clamp ( pos.x, MinMaxX [ 0 ], MinMaxX [ 1 ] );
            pos.y = Mathf.Clamp ( pos.y, MinMaxZ [ 0 ], MinMaxZ [ 1 ] );

            float ratio = ( ( float ) Screen.width / Screen.height );
            ratio *= 0.5f;

            //TODO camera orthographic size must be considered for movement
            camera.orthographicSize = Mathf.Clamp ( cameraSize, MinMaxY [ 0 ], MinMaxY [ 1 ] );
            //camera.orthographicSize = ratio;
            //camera.orthographicSize = cameraSize;

            transform.localPosition = pos;
        }

        CheckIfCameraIsWithinBounds();
    }

    private void CheckIfCameraIsWithinBounds()
    {
        Vector3 pos = transform.localPosition;

        leftExtent = pos.x - (camera.orthographicSize * camera.aspect);
        rightExtent = pos.x + (camera.orthographicSize * camera.aspect);
        topExtent = pos.y + camera.orthographicSize;
        bottomExtent = pos.y - camera.orthographicSize;

        pos.x = Mathf.Clamp(pos.x, MinMaxExtentsX.x + (camera.orthographicSize * camera.aspect), MinMaxExtentsX.y - (camera.orthographicSize * camera.aspect));
        pos.y = Mathf.Clamp(pos.y, MinMaxExtentsY.x + camera.orthographicSize, MinMaxExtentsY.y - camera.orthographicSize);

        transform.localPosition = pos;
    }
}
