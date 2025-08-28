using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameChest : MonoBehaviour
{
    AudioSource _AudioSource;
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _AudioSource.Play();

            StartCoroutine(LoadSceneAfterSound());
        }
    }
    IEnumerator LoadSceneAfterSound()
    {
        yield return new WaitForSeconds(_AudioSource.clip.length);

        SceneManager.LoadScene("Win Panel");
    }
}
