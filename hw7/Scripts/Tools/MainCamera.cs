using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public float xMargin = 1f;
    public float yMargin = 1f;
    public float xSmooth = 8f;
    public float ySmooth = 8f;
    public Vector2 maxXandY;
    public Vector2 minXandY;

    private Transform hero;
    private Vector3 camNextPosition;    // Поле для вычисления следующего положения камеры

	// Use this for initialization
	void Start () {
        hero = GameObject.FindGameObjectWithTag("Hero").transform;

        Camera cam = GetComponent<Camera>();
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        maxXandY.x -= camWidth;
        maxXandY.y -= camHeight;
        minXandY.x += camWidth;
        minXandY.y += camHeight;

        // Инициализируем следующее положение камеры текущими координатами
        camNextPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
	
	
	void LateUpdate () {

        if(hero != null)
        {
            if (ExceedXMargin())
                //camNextPosition.x = Mathf.Lerp(transform.position.x, hero.position.x, xSmooth * Time.fixedDeltaTime);
                camNextPosition.x = hero.position.x - Mathf.Sign(hero.position.x - transform.position.x) * xMargin;

            if (ExceedYMargin())
                //camNextPosition.y = Mathf.Lerp(transform.position.y, hero.position.y, xSmooth * Time.fixedDeltaTime);
                camNextPosition.y = hero.position.y - Mathf.Sign(hero.position.y - transform.position.y) * yMargin;

            camNextPosition.x = Mathf.Clamp(camNextPosition.x, minXandY.x, maxXandY.x);
            camNextPosition.y = Mathf.Clamp(camNextPosition.y, minXandY.y, maxXandY.y);

            transform.position = camNextPosition;
        }        
    }

    private bool ExceedXMargin()
    {
        return Mathf.Abs(hero.position.x - transform.position.x) > xMargin;
    }

    private bool ExceedYMargin()
    {
        return Mathf.Abs(hero.position.y - transform.position.y) > yMargin;
    }
}
