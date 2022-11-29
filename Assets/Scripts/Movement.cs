using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustPower = 1000;
    [SerializeField] float rotateSpeed = 100;
    [SerializeField] AudioClip thrustAudio;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    Rigidbody rigidBody;
    AudioSource audioSource;
    CollisionHandler collisionHandler;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        collisionHandler = GetComponent<CollisionHandler>();

        InitializeAudioSource();
    }

    void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(Vector3.forward);
            rightBooster.PlayWhenNotPlaying();
        }
        else
        {
            rightBooster.StopWhenPlaying();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Rotate(Vector3.back);
            leftBooster.PlayWhenNotPlaying();
        }
        else
        {
            leftBooster.StopWhenPlaying();
        }
    }

    void Rotate(Vector3 vector)
    {
        rigidBody.freezeRotation = true;
        var backwardRotate = vector * rotateSpeed * Time.deltaTime;
        transform.Rotate(backwardRotate);
        rigidBody.freezeRotation = false;
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var power = Time.deltaTime * thrustPower * Vector3.up;
            rigidBody.AddRelativeForce(power);
            PlayThrust();
        }
        else
        {
            StopThrust();
        }
    }

    void PlayThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustAudio);
        }

        mainBooster.PlayWhenNotPlaying();
    }

    void StopThrust()
    {
        mainBooster.StopWhenPlaying();

        if (audioSource.isPlaying && !collisionHandler.IsLandingSuccess)
        {
            audioSource.Stop();
        }
    }
}
