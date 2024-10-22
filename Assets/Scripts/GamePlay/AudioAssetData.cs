using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioAssetData", menuName = "SKMGamePlay/AudioAssetData", order = 1)]
public class AudioAssetData : ScriptableObject
{
    public AudioClip timeTickingClip;
    public AudioClip startingNewGameClip;
    public AudioClip wontDrawCardClip;
    public AudioClip willDrawCardClip;
    public AudioClip changingBankerClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip drawCardClip;
    public AudioClip distrubuteCardClip;
    public AudioClip playersAreBettingClip;
    public AudioClip yourTurnToBankClip;
}
