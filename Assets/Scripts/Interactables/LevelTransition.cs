using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Called when level is completed. Switches to next level with UI, etc.
    public void CompleteLevel()
    {
        GameManager.Instance.NextLevel();
    }
}
