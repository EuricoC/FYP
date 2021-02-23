using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Trees : MonoBehaviour
{
    private GameObject tree;
    public Transform player;
    private int selector;
    private Vector3 pos;
    private RaycastHit hit;

    private Vector2 viewerPosition;
    private Vector2 viewerPositionOld;


    private void Awake()
    {
        pos = player.position;
    }

    private void Start()
    {
        Populate();
    }

    private void Update()
    {
        Populate();
    }

    public void Populate()
    {
        
        for (var i = 0; i < 10; i++)
        {
            selector = Random.Range(0, 3);
            
            switch (selector)
            {
                case 0:
                    
                    CallInstantiate(0);
                    
                    break;
                case 1:
                    
                    CallInstantiate(1);
                    
                    break;
                case 2:
                    
                    CallInstantiate(2);
                    
                    break;
            }
        }

    }

    private void CallInstantiate(int x)
    {
        pos = player.position;
        pos.y += 100;
        pos.x += Random.Range(-200, 200);
        pos.z += Random.Range(-200, 200);
        
        tree = ObjectPool.sharedInstance.GetPooledObject(x);
        
        if (tree != null)
        {
            tree.transform.position = pos;
            tree.transform.rotation = Quaternion.identity;
            tree.SetActive(true);
            
            if (Physics.Raycast(tree.transform.position, transform.TransformDirection(Vector3.down), out hit, 120f))
            {
                
                pos.y -= hit.distance;
                tree.transform.position = pos;
                
            }
            else
            {
                Debug.DrawRay(tree.transform.position, transform.TransformDirection(Vector3.down) * 120f, Color.white);
                Debug.Log("raycast no hit");
                tree.SetActive(false);
            }
        }
    }

}
