using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffects : MonoBehaviour {

    public GameObject effectGO;


    private GameObject[] Effects = new GameObject[5];       // Эффекты для каждого прикосновения по индексу

    // Use this for initialization
    void Start () {

        

        // Создаем пул объектов эффектов
		for(var i = 0; i < Effects.Length; i++)
        {
            Effects[i] = Instantiate(effectGO, Vector3.zero, Quaternion.identity);
            Effects[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < Input.touchCount; i++)
        {
            ProcessTouchEffect(i);
        }
	}


    void ProcessTouchEffect(int i)
    {
        Touch touch = Input.touches[i];
        
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            Effects[i].SetActive(false);
        }
        else if(touch.phase == TouchPhase.Began)
        {
            TrailRenderer TR = Effects[i].GetComponent<TrailRenderer>();

            if (TR != null)
                TR.Clear();

            Effects[i].SetActive(true);
        }
        else if(touch.phase == TouchPhase.Moved)
        {
            var newPos = Camera.main.ScreenToWorldPoint(touch.position);
            newPos.z = 0;

            Effects[i].transform.position = newPos;
        }
    }
}
