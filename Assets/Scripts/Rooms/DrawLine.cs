using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;

  
    private void Start() {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.sortingLayerName  = "Room";
        Material _mat = Resources.Load("Shaders/Materials/WobbleMat_FullLine.mat", typeof(Material)) as Material;
        lr.material = _mat;
    }
}
