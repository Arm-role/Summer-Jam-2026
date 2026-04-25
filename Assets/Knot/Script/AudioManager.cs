using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
  [Header("---Sound---")]
  [SerializeField] AudioSource musicSource;
  [SerializeField] AudioSource sfxSource;

  [Header("---Audio Clip---")]
  public AudioClip background;
  public AudioClip shoot;
  public AudioClip death;
  public AudioClip click;

  public static AudioManager instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);

    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void Start()
  {
    musicSource.clip = background;
    musicSource.Play();
  }

  public void PlaySFX(AudioClip clip)
  {
    sfxSource.PlayOneShot(clip);

  }
}
