using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    [HideInInspector]
    public Transform bottom;


    private void Start()
    {
        bottom = transform.Find("ladderBottom");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<IClimbable>();

        if (controller != null)
        {
            controller.SetLadder(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<IClimbable>();

        if (controller != null)
        {
            controller.RemoveLadder();
        }
    }

}
