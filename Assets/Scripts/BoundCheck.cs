using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;
    
    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown; // Пересечена каждая граница 

    void Awake()
    {   // Растоняие от 0.0 до верхнего края экрана
        camHeight = Camera.main.orthographicSize;

        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        // Кадый иф определяет выход из каждой границы экрана
        if(pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camWidth - radius)
        {
            pos.y = camWidth - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camWidth + radius)
        {
            pos.y = -camWidth + radius;
            isOnScreen = false;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);

        if(keepOnScreen && !isOnScreen) // Принудительно остовляет объект в пределах экрана
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }

    }

    void OnDrawGizmos()
    {
        // Рисует большой куб (границы) на панель Scene
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawCube(Vector3.zero, boundSize);
    }
}
