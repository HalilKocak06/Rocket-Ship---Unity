using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust; // Inspector’da ayarlanabilir tek bir input aksiyonu. Mesela Space, W ya da gamepad tetik bağlayacaksın.
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] float rotationStrength = 100f;

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
            // Roketin yerel "up" ekseni (yerel Y+) yönünde itiş uygula.
            // AddRelativeForce: yerel eksende kuvvet uygular (yani roket hangi yöne dönükse o yöne iter).

            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
            
        }

    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            ApplyRotation(rotationStrength);
        }

        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationStrength);
        }

    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

}
