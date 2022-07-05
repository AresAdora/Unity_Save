using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll : MonoBehaviour
{
    private int key;
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;
    private float moveSpeed = 120.0f;
    private float rotationSpeed = 100.0f;
    private Transform playerTr;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");
        //Debug.Log("H:" + h.ToString() + ",V:" + v.ToString());
        playerTr.Translate(new Vector3(h, 0, v)*moveSpeed*Time.deltaTime);
        playerTr.Rotate(new Vector3(0, r*20, 0) * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="KEY")
        {
            Destroy(collision.gameObject);
            key += 1; // È¹µæÇÑ Å°ÀÇ °¹¼ö 1Ãß°¡
            Debug.Log("key : " + key.ToString());
        }

        if(collision.gameObject.tag=="BOX")
        {
            if(key <= 3)
            {
                Debug.Log("¿­¼è4°³¸¦ Ã£¾Æ¿Í¶ó!");
            } else
            {
                Debug.Log("Å»Ãâ!!!");
            }
        }
    }
}
