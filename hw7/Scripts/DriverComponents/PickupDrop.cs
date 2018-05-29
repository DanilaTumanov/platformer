using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : MonoBehaviour {

    public PickupDropSettings[] DropablePickups;

    protected PickupDropDriver PickupDropDriver;

    // Use this for initialization
    void Start () {
        PickupDropDriver = new PickupDropDriver(DropablePickups);
    }
	
	public void DropPickup()
    {
        PickupDropDriver.DropPickup(transform.position);
    }
}
