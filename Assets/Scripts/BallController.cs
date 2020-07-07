using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameController gameController;
    public AudioSource ballHitSound;

    private Rigidbody soccerBallRigidbody;

    private void Awake()
    {
        soccerBallRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Juggler")
        {
            soccerBallRigidbody.AddExplosionForce(OptionsController.kickForce, other.gameObject.transform.position - new Vector3(0, 1.5f, 0), OptionsController.kickForceRadius, 0.25f, ForceMode.Impulse);
            gameController.HitJuggler();
            ballHitSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LosingCollider")
        {
            gameController.EndGame();
        }

        if (other.gameObject.tag == "Target")
        {
            gameController.HitTarget();
        }
    }
}
