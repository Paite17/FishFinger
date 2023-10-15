using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnKillScoreText : MonoBehaviour
{
    private float existTimer;

    [SerializeField] private float existTime;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);

        transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);

        transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.x, transform.rotation.w);

        existTimer += Time.deltaTime;

        if (existTimer > existTime)
        {
            Destroy(gameObject);
        }

    }
}
