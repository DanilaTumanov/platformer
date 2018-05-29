using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float delay = 0f;        // Время в секундах, через которое начнется спаун
    public float minPeriod = 3f;    // Минимальное время до следующего спавна
    public float maxPeriod = 6f;    // Максимальное время до следующего спавна
    public Enemy[] enemies;

    private float camX;             // Расстояние от центра камеры, после которого противники могут спавниться
    private float period;
    private float lastSpawnTime;
    private Camera cam;

    // Use this for initialization
    void Start () {
        cam = Camera.main;

        // Определяем половину ширины камеры и добавляем множитель, чтобы противники не спаунились моментально после ухода камеры
        camX = cam.orthographicSize * cam.aspect * 1.5f;
        lastSpawnTime = Time.time;
        period = delay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        // Если прошло больше времени, чем последнее время спауна плюс период ожидания
        // И если спаунер находится не в поле зрения игрока, то спауним врага
        if ((Time.time > lastSpawnTime + period) 
            && (transform.position.x < cam.transform.position.x - camX || transform.position.x > cam.transform.position.x + camX))
               Spawn();

	}

    private void Spawn()
    {
        // Случайно выбираем врага
        int i = Random.Range(0, enemies.Length);

        // Генерируем врага в точке спауна
        Instantiate<Enemy>(enemies[i], transform.position, transform.rotation);

        // Генерируем время до следующего спауна
        period = Random.Range(minPeriod, maxPeriod + 1);
        
        // Запоминаем время спауна
        lastSpawnTime = Time.time;
    }
}
