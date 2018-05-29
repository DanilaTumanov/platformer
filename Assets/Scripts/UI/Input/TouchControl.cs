using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl: IInputController, IUIInteractable {


    const float deadlineX = 30f;
    const float deadlineY = 100f;
    const float pixelScale = 70f;
    const float accelerometerSensitivity = 10f;
    const float jumpSensitivity = 50f;
    const float accelerometerDeadline = 0.1f;


    private float horizontalAxis;
    private float verticalAxis;
    private bool jump;
    private bool shoot;
    private bool placeTrap;
    private bool interact;

    private Vector2[] touchStartPos = new Vector2[5];
    
    private int movementTouchIndex = -1;
    private int jumpTouchIndex = -1;
    private bool shootButtonPressed;
    private bool placeTrapButtonPressed;
    private bool interactionButtonPressed;
    private bool accselerometerSupported;

    public float HorizontalAxis
    {
        get
        {
            return horizontalAxis;
        }
    }

    public float VerticalAxis
    {
        get
        {
            return verticalAxis;
        }
    }

    public bool Jump
    {
        get
        {
            return jump;
        }
    }

    public bool Shoot
    {
        get
        {
            return shoot;
        }
    }

    public bool PlaceTrap
    {
        get
        {
            return placeTrap;
        }
    }

    public bool Interact
    {
        get
        {
            return interact;
        }
    }




    public TouchControl()
    {
        bool accselerometerSupported = SystemInfo.supportsAccelerometer;
    }




    public void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.touches[i];
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos[i] = touch.position;

                if (touch.position.x < (Screen.width / 2))
                    movementTouchIndex = i;
            }
            else
            {
                // Обработка движения
                if (i == movementTouchIndex)
                {
                    // Если отпустили палец, отвечающий за движение, то сбрасываем значение осей
                    if (touch.phase == TouchPhase.Ended)
                    {
                        horizontalAxis = 0;
                        verticalAxis = 0;
                        movementTouchIndex = -1;
                    }
                    // Иначе рассчитаем значение осей
                    else
                    {
                        var deltaX = touch.position.x - touchStartPos[i].x;     // Разница с первоначальным значением координаты
                        var deltaY = touch.position.y - touchStartPos[i].y;
                        
                        horizontalAxis = Mathf.Abs(deltaX) < deadlineX ? 0 : Mathf.Clamp((deltaX - Mathf.Sign(deltaX) * deadlineX) / pixelScale, -1, 1);
                        verticalAxis = Mathf.Abs(deltaY) < deadlineY ? 0 : Mathf.Clamp((deltaY - Mathf.Sign(deltaY) * deadlineY) / pixelScale, -1, 1);
                    }                    
                }

                // Обработка прыжка. Прыжок можно сделать любым пальцем, если резко потянуть вверх

                // Сбрасываем прыжок, если он был установлен в предыдущем кадре
                if (jump)
                    jump = false;

                if(touch.deltaPosition.y > jumpSensitivity && touch.phase != TouchPhase.Ended)
                {
                    jump = true;
                }
                
            }
        }


        ProcessGyro();

        ProcessShoot();
        ProcessPlaceTrap();
        ProcessUseInteraction();
    }





    private void ProcessGyro()
    {
        var x = Input.acceleration.x;

        if(movementTouchIndex < 0)
            horizontalAxis = Mathf.Abs(x) < accelerometerDeadline ? 0 : Mathf.Clamp((Input.acceleration.x - Mathf.Sign(x) * accelerometerDeadline) * accelerometerSensitivity, -1, 1);
    }

    
    private void ProcessShoot()
    {
        shoot = false;
        if (shootButtonPressed)
        {
            shoot = true;
            shootButtonPressed = false;
        }
    }


    private void ProcessPlaceTrap()
    {
        placeTrap = false;
        if (placeTrapButtonPressed)
        {
            placeTrap = true;
            placeTrapButtonPressed = false;
        }
    }


    private void ProcessUseInteraction()
    {
        interact = false;
        if (interactionButtonPressed)
        {
            interact = true;
            interactionButtonPressed = false;
        }
    }


    public void OnShootButtonPressed()
    {
        shootButtonPressed = true;
    }

    public void OnPlaceTrapButtonPressed()
    {
        placeTrapButtonPressed = true;
    }

    public void OnInteractionButtonPressed()
    {
        interactionButtonPressed = true;
    }

}
