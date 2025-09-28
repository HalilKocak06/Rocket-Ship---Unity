using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    bool isControllable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void OnCollisionEnter(Collision other)
    {
        if (!isControllable) { return; }
        
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Everything is looking good");
                    break;

                case "Finish":
                    StartSuccessSequence();
                    Debug.Log("You are all done , welcome to our country");
                    break;

                case "Fuel":
                    Debug.Log("Pick me ");
                    break;

                default:
                    Debug.Log("You crashed");
                    StartCrashSequence();
                    break;
            
        }

    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }


        SceneManager.LoadScene(nextScene);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false; //movement scriptini durdurur.
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false; //movement scriptini durdurur.
        Invoke("LoadNextLevel", levelLoadDelay);
    }


    }