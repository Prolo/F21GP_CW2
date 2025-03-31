using UnityEngine;

public class playBGM : MonoBehaviour
{
    [SerializeField] private AudioSource BGM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGM = GetComponent<AudioSource>();
        BGM.Play();
    }

}
