using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust; // Inspector’da ayarlanabilir tek bir input aksiyonu. Mesela Space, W ya da gamepad tetik bağlayacaksın.
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;

    AudioSource audioSource;


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Aynı GameObject’teki Rigidbody bileşenini bulur ve rb’ye atar.
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() // Bileşen aktif olduğunda (play’e basınca ya da enable edilince) çağrılır. Start’tan ÖNCE de çalışabilir.
    {
        thrust.Enable(); // InputAction’ı aktif ederiz; yoksa input okumaz. (Eşleniği OnDisable’da Disable etmektir.)
        rotation.Enable();

    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed()) // Aksiyon butonu basılı mı? (Basılı tutulduğu sürece true döner.)
        {
            
            StartThrusting();
        }


        else
        {
            StopThrusting();
        }



    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void StartThrusting()
    {
        // Roketin yerel "up" ekseni (yerel Y+) yönünde itiş uygula.
            // AddRelativeForce: yerel eksende kuvvet uygular (yani roket hangi yöne dönükse o yöne iter).
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotateRight();
        }

        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }

    }
  private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Play();
        }
    }
   private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrustParticles.isPlaying)
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

 
    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

}
