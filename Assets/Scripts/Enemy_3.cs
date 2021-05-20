using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector")]
    public float lifeTime = 5;

    [Header("Set Dynamically")]
    public Vector3[] points;
    public float birthTime;

    private void Start()
    {
        points = new Vector3[3];
        points[0] = Pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius; 
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        // Выбор точки для интерполяции
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;
        // Выбор точки для интерполяции
        v = Vector3.zero;
        v.y = Pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
    }
    protected override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 p01, p12;
        // Очереденая интерполяция 
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        Pos = (1 - u) * p01 + u * p12;
    }
}
