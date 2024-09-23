using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public bool isTouch;
    public Vector2 pointA;
    public Vector2 pointB;
    public Vector2 movement;
    public GameObject player;
    public Transform circle;
    public Transform InnerCircle;

    private void Update()
    {
        if (!isTouch && Input.touchCount >=1)
        {
            pointA = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            isTouch = true;
            circle.GetComponent<Image>().enabled = true;
            circle.transform.position = pointA;
        }
    }

    private void FixedUpdate()
    {
        if (Input.touchCount >= 1)
        {

            pointB = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

            Vector2 offset = pointB - pointA;
            InnerCircle.GetComponent<Image>().enabled = true;
            InnerCircle.transform.localPosition = Vector3.ClampMagnitude(offset, 45);
            movement = Vector2.ClampMagnitude(offset, 1);
            player.transform.position += new Vector3(movement.x, movement.y, 0) * Time.deltaTime * 10;
        }

        if (Input.touchCount == 0)
        {
            isTouch = false;
            InnerCircle.GetComponent<Image>().enabled = false;
            circle.GetComponent<Image>().enabled = false;
        }
    }
}
