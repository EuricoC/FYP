
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu()]
public class trees : ScriptableObject
{
    public GameObject treeprefab;
    private Vector3[] position;

    
    public void GetV(Vector3[] v)
    {
        position = v;
    }
   
    public void populate()
    {
        foreach (var i in position) //for(int i = 0; i<=1000; i++)
        {
            //Debug.Log(i);
            var pos = i; //position[i];

            if (pos.y > 0 && pos.y < 1)
            {
                Instantiate(treeprefab, pos, Quaternion.identity );
            }
        }
    }
   
}
