using UnityEngine;

namespace Flatformer.GameData
{
    public class StartLoopMusic : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Play();
        }
    }
}