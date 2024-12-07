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
    [SerializeField]
    private string[] _backgroundMusicNames;
    private FMOD.System _coreSystem;
    private FMOD.ChannelGroup _channelGroup;
    private FMOD.Sound[] _backgroundSounds;
    #endregion


    public void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        _coreSystem = RuntimeManager.CoreSystem;
        _channelGroup = new FMOD.ChannelGroup();
        
        for(int i = 0; i < _backgroundMusicNames.Length; i++)
        {
            _coreSystem.createSound(_backgroundMusicNames[i], FMOD.MODE.DEFAULT, out _backgroundSounds[i]);
        }

        _stepsEventInstance = RuntimeManager.CreateInstance(_stepsEvent);
        _buttonEventInstance = RuntimeManager.CreateInstance(_buttonEvent);
        _reloadEventInstance = RuntimeManager.CreateInstance(_reloadEvent);
    }

    public void OnDestroy()
    {
        _stepsEventInstance.release();
        _buttonEventInstance.release();
        _reloadEventInstance.release();
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
        _shotEventInstance = RuntimeManager.CreateInstance(_shotEvent);
        _shotEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _shotEventInstance.start();
        _shotEventInstance.release();
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
