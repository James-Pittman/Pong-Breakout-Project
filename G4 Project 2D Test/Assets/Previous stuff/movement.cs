using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public Transform obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject.GetComponent<Transform> ();
    }

    // Update is called once per frame
    void Update()
    {
	    float h = Input.GetAxis("horip1");
        Vector3 tempVect = new Vector3(h, 0, 0);
	    tempVect = tempVect.normalized * speed * Time.deltaTime;
        obj.transform.position += tempVect;
        if (obj.transform.position.x < -15.0){
            tempVect = new Vector3((float)15.0, 0, 0);
            tempVect = tempVect.normalized * speed * Time.deltaTime;
            obj.transform.position += tempVect;
        }
        else if (obj.transform.position.x > 15.0){
            tempVect = new Vector3((float)-15.0, 0, 0);
            tempVect = tempVect.normalized * speed * Time.deltaTime;
            obj.transform.position += tempVect;
        }
    }
}
