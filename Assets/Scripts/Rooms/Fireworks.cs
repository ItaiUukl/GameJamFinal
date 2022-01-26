using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    [SerializeField] private GameObject firework;
    // Start is called before the first frame update
    void Start()
    {
         for(int i = 0; i <= 10; i++){
            var position = new Vector3(Random.Range(-5f, 8f), Random.Range(-9f, 4f),0);
            Instantiate(firework, position, Quaternion.identity);}
    
    }

}
