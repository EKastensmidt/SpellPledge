using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickName : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Vector3 offSet;
    Transform target;
    Camera camera;
    private void Awake()
    {
        camera = Camera.main;
    }
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
    public void SetName(string nick)
    {
        text.text = nick;
    }
    private void Update()
    {
        if (target != null)
        {
            var pos = camera.WorldToScreenPoint(target.position + offSet);
            transform.position = pos;
        }
    }
}
