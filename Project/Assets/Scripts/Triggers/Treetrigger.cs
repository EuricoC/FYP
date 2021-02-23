using UnityEngine;

public class Treetrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mesh"))
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().enabled = true;

        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.transform.localPosition.y > 5f || gameObject.transform.localPosition.y < -4f)
        {
            gameObject.SetActive(false);
            //Debug.Log(gameObject.transform.position.y);
            
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
        }
        
        if (other.gameObject.CompareTag("TreeTag"))
        {
            gameObject.SetActive(false);

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
        }
        
    }
}
