using System;
using FMODUnity;
using UnityEngine;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance() => _instance;
    
    #region playerSounds
    [SerializeField] 
    private EventReference _stepsEvent;
    private FMOD.Studio.EventInstance _stepsEventInstance;
    #endregion
    
    #region uiSounds
    [SerializeField] 
    private EventReference _buttonEvent;
    private FMOD.Studio.EventInstance _buttonEventInstance;
    #endregion
    
    #region gunSounds
    [SerializeField]
    private EventReference _shotEvent;
    private FMOD.Studio.EventInstance _shotEventInstance;

    [SerializeField] 
    private EventReference _reloadEvent;
    private FMOD.Studio.EventInstance _reloadEventInstance;
    #endregion

    #region backgroundMusic
    // Esto es lo de FMOD CORE
    #endregion


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _stepsEventInstance = RuntimeManager.CreateInstance(_stepsEvent);
        _buttonEventInstance = RuntimeManager.CreateInstance(_buttonEvent);
        _shotEventInstance = RuntimeManager.CreateInstance(_shotEvent);
        _reloadEventInstance = RuntimeManager.CreateInstance(_reloadEvent);
    }

    private void OnDestroy()
    {
        _stepsEventInstance.release();
        _buttonEventInstance.release();
        _shotEventInstance.release();
    }

    public void PlayFootstepSound(string floorMaterial, Vector3 position)
    {
        _stepsEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _stepsEventInstance.setParameterByNameWithLabel("Floor Material", floorMaterial);
        _stepsEventInstance.start();
    }

    public void PlayUISound()
    {
        _buttonEventInstance.start();
    }

    public void PlayShotSound(Vector3 position)
    {
        _shotEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _shotEventInstance.start();
    }

    public void SetReloadPhase(int phase)
    {
        _reloadEventInstance.setParameterByName("ReloadPhase", phase);
    }
    
    public void PlayReloadSound(Vector3 position)
    {
        _reloadEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _reloadEventInstance.start();
    }
}
