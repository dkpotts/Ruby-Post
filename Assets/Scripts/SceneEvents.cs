using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject slowBot;
    private GameObject fastBot;

    private void Start()
    {
        
    }

    void Update()
    {
        if (!slowBot.GetComponent<EnemyController>().pubBroken)
        {
            GameEvents.current.RobotsFixed();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.RobotsFixed();
    }*/


}
