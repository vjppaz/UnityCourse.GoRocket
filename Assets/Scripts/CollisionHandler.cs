using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 3;
    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip fuelAudio;

    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem explosionParticle;

    public bool IsLandingSuccess = false;

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool CollisionIsDisabled = false;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        ProcessCheat();        
    }
    
    void ProcessCheat()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CompleteLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionIsDisabled = !CollisionIsDisabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This object is friendly.");
                break;
            case "Finish":
                CompleteLevel();
                break;
            case "Fuel":
                GetFuel(other.gameObject);
                break;
            default :
                RocketCrashed();
                break;
        }
    }

    void GetFuel(GameObject fuelObject)
    {
        rigidBody.freezeRotation = true;
        var fuelComponent = fuelObject.GetComponent<Fuel>();
        var meshRenderer = fuelObject.GetComponent<MeshRenderer>();
        var audioSource = fuelObject.AddComponent<AudioSource>();
        var collider = fuelObject.GetComponent<CapsuleCollider>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = fuelAudio;
        audioSource.Play();

        meshRenderer.enabled = false;
        collider.enabled = false;

        Debug.Log($"Fuel {fuelComponent.fuelAmount} acquired!");
        rigidBody.freezeRotation = false;
    }

    void RocketCrashed()
    {
        if (CollisionIsDisabled)
        {
            return;
        }
        
        audioSource.Stop();
        audioSource.PlayOneShot(explosionAudio);
        explosionParticle.Play();

        var movement = gameObject.GetComponent<Movement>();
        movement.enabled = false;

        Debug.Log("You bumped into an obstacle!");
        Invoke(nameof(ReloadScene), levelLoadDelay);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(CurrentSceneIndex());
    }

    void LoadNextScene()
    {
        var expectedNextLevel = CurrentSceneIndex() + 1;
        var nextLevel = SceneManager.sceneCountInBuildSettings == expectedNextLevel ? 0 : expectedNextLevel;
        SceneManager.LoadScene(nextLevel);
    }

    int CurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    void CompleteLevel()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        successParticle.Play();

        IsLandingSuccess = true;
        rigidBody.freezeRotation = true;

        Debug.Log("Success!");

        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }
}
