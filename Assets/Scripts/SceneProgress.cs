using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProgress : MonoBehaviour
{
    public GameObject jambi;
    private GameObject slowBot;
    private EnemyController slowController;
    private GameObject fastBot;
    private EnemyController fastController;
    private DialogTrigger dialogTrigger;

    // Start is called before the first frame update
    void Start()
    {
        dialogTrigger = GetComponent<DialogTrigger>();
        slowController = slowBot.GetComponent<EnemyController>();
        fastController = fastBot.GetComponent<EnemyController>();

    }

    // Update is called once per frame
    void Update()
    {
        //check that Bots have been fixed before continuing dialogue
        if (!(slowController.pubBroken || fastController.pubBroken))
        {
            
        }
    }
}
