using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {


    public IInputController InputController;


    public float HorizontalAxis {
        get
        {
            return InputController.HorizontalAxis;
        }
    }

    public float VerticalAxis
    {
        get
        {
            return InputController.VerticalAxis;
        }
    }

    public bool Jump
    {
        get
        {
            return InputController.Jump;
        }
    }

    public bool Shoot
    {
        get
        {
            return InputController.Shoot;
        }
    }

    public bool PlaceTrap
    {
        get
        {
            return InputController.PlaceTrap;
        }
    }

    public bool Execute
    {
        get
        {
            return InputController.Interact;
        }
    }



    // Use this for initialization
    void Start () {
#if UNITY_ANDROID
        InputController = new TouchControl();
#else
        InputController = new PCControl();
#endif
    }
	
	// Update is called once per frame
	void Update () {
        InputController.Update();
    }




    public void OnUIFireButtonPressed()
    {
        if(InputController is IUIInteractable)
        {
            (InputController as IUIInteractable).OnShootButtonPressed();
        }
    }


    public void OnUIPlaceTrapButtonPressed()
    {
        if (InputController is IUIInteractable)
        {
            (InputController as IUIInteractable).OnPlaceTrapButtonPressed();
        }
    }

}
