using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource _audioSource;
    [SerializeField]private AudioAssetData _audioData;

    public void Initialize()
    {
    }

    public void PlayBGMusic()
    {
        _audioSource.Stop();
        _audioSource.Play();
    }

    public void StopBGMusic()
    {
        _audioSource.Stop();
    }


    public void PlayTimeTickingClip()
    {
        _audioSource.PlayOneShot(_audioData.timeTickingClip);
    }

    public void PlayStartingNewGameClip()
    {
        _audioSource.PlayOneShot(_audioData.startingNewGameClip);
    }

    public void PlayWontDrawCardClip()
    {
        _audioSource.PlayOneShot(_audioData.wontDrawCardClip);
    }

    public void PlayWillDrawCardClip()
    {
        _audioSource.PlayOneShot(_audioData.willDrawCardClip);
    }

    public void PlayChangingBankerClip()
    {
        _audioSource.PlayOneShot(_audioData.changingBankerClip);
    }

    public void PlayWinClip()
    {
        _audioSource.PlayOneShot(_audioData.winClip);
    }

    public void PlayLoseClip()
    {
        _audioSource.PlayOneShot(_audioData.loseClip);
    }

    public void PlayDrawCardClip()
    {
        _audioSource.PlayOneShot(_audioData.drawCardClip);
    }

    public void PlayDistributeCardClip()
    {
        _audioSource.PlayOneShot(_audioData.distrubuteCardClip);
    }

    public void PlayPlayersAreBettingClip()
    {
        _audioSource.PlayOneShot(_audioData.playersAreBettingClip);
    }

    public void PlayYourTurnToBankClip()
    {
        _audioSource.PlayOneShot(_audioData.yourTurnToBankClip);
    }
    
    public void Play8DoClip()
    {
        _audioSource.PlayOneShot(_audioData._8doClip);
    }

    public void Play9DoClip()
    {
        _audioSource.PlayOneShot(_audioData._9doClip);
    }

    public void PlayFirstWarningClip()
    {
        _audioSource.PlayOneShot(_audioData.firstWarningClip);
    }
    public void PlaySecondWarningClip()
    {
        _audioSource.PlayOneShot(_audioData.secondWarningClip);
    }
    
    public void PlayThirdWarningClip()
    {
        _audioSource.PlayOneShot(_audioData.thirdWarningClip);
    }

    public void PlayWinWarningClip()
    {
        _audioSource.PlayOneShot(_audioData.winWarningClip);
    }
}
