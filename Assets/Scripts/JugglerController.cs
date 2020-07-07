using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugglerController : MonoBehaviour
{
    public GameObject soccerBall;
    public ParticleSystem hitEffects;

    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        capsuleCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (soccerBall.activeSelf)
                StartCoroutine(enableCollider());
        }
    }

    private IEnumerator enableCollider()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.0f));
        capsuleCollider.enabled = true;
        hitEffects.Play();

        yield return new WaitForSeconds(0.1f);

        capsuleCollider.enabled = false;
    }
}
