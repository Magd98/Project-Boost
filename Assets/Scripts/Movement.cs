using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Movement : MonoBehaviour
{
     
    //Caching Thrusting Speed
    [SerializeField] private float thrustingSpeed;
    //Caching Rotation Speed
    [SerializeField] private float rotationSpeed;
    //Setting up Multiple Audios
    [SerializeField] AudioClip thrust;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip winning;
    [SerializeField] AudioClip nextLevel;

    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winningParticles;

    Rigidbody rigidBody;
    AudioSource rocketSFX;
    //Setting Up different States for the interactions.. Stating at Alive State
    enum State { Alive, Dead, Transcending };
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
       rigidBody= GetComponent<Rigidbody>();
        rocketSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state==State.Alive)
        {
            RespondToThrustInput();
            RespondToRotationInput();
        }
    }

    private void RespondToRotationInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Rotate(-1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state!=State.Alive) { return; }
        switch (collision.gameObject.tag) 
        {
            case "Friendly":
                StartSuccesSequence();
                break;
            case "Start":
                  //Do Nothing
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dead;
        rocketSFX.Stop();
        rocketSFX.PlayOneShot(death);
        deathParticles.Play();
        Invoke("ReloadLevel", 2f);
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        rocketSFX.Stop();
        rocketSFX.PlayOneShot(winning);
        winningParticles.Play();
        Invoke("LoadLevel", 2f);
    }

    void ReloadLevel()
    {
        int currentLevel;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetActiveScene().buildIndex == currentLevel)
            rocketSFX.PlayOneShot(nextLevel);
        SceneManager.LoadScene(currentLevel);
    }

    void LoadLevel()
    {
        int currentLevel ;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetActiveScene().buildIndex==1)
        rocketSFX.PlayOneShot(nextLevel);
        SceneManager.LoadScene(currentLevel+1);
        
      }
    private void Rotate(float rotationDirection)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime * rotationDirection);
        rigidBody.freezeRotation = false;
       }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrusting();
           
        }
        else
        {
            rocketSFX.Stop();
            thrustParticles.Stop();

        }
    }

    private void ApplyThrusting()
    {
            rigidBody.AddRelativeForce(Vector3.up * thrustingSpeed * Time.deltaTime);
            if (!rocketSFX.isPlaying)
            {
               rocketSFX.PlayOneShot(thrust);
                thrustParticles.Play();

        }
         

        }
    }


