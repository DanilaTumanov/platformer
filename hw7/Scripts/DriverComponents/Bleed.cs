using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : MonoBehaviour, IBleedable {

    private GameObject blood;
    private ParticleSystem bloodPS;

	// Use this for initialization
	void Start () {
        blood = transform.Find("Blood").gameObject;
        bloodPS = blood.GetComponent<ParticleSystem>();
    }

    public void StartBleed()
    {
        bloodPS.Play();
    }

    public void StopBleed()
    {
        bloodPS.Stop();
    }

    public GameObject GetBloodGO()
    {
        return blood;
    }

    public ParticleSystem GetBloodPS()
    {
        return bloodPS;
    }
}
