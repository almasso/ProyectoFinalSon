using System;
using FMODUnity;
using UnityEngine;
using UnityEditor;
using FMOD;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;

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
    private FMOD.Channel _channel;
    private FMOD.Sound[] _backgroundSounds;
    private FMOD.DSP _lowPassDSP;
    private int _speedMultiplier = 2;
    private float _speed = 1.0f;
    #endregion


    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(_instance);
    }

    public void Start()
    {
        _coreSystem = FMODUnity.RuntimeManager.CoreSystem;
        _channelGroup = new FMOD.ChannelGroup();
        _backgroundSounds = new FMOD.Sound[_backgroundMusicNames.Length];
        for(int i = 0; i < _backgroundMusicNames.Length; i++)
        {
            _coreSystem.createSound(Application.dataPath + "/Songs/" + _backgroundMusicNames[i], FMOD.MODE.DEFAULT, out _backgroundSounds[i]);
        }

        _coreSystem.playSound(_backgroundSounds[0], _channelGroup, false, out _channel);

        //Filtro de paso bajo (Digital Signal Processor)
        _coreSystem.createDSPByType(FMOD.DSP_TYPE.LOWPASS, out _lowPassDSP);
        _lowPassDSP.setParameterFloat((int)FMOD.DSP_LOWPASS.CUTOFF, 500.0f);
        _lowPassDSP.setParameterFloat((int)FMOD.DSP_LOWPASS.RESONANCE, 1.0f);

        _stepsEventInstance = RuntimeManager.CreateInstance(_stepsEvent);
        _buttonEventInstance = RuntimeManager.CreateInstance(_buttonEvent);
        _reloadEventInstance = RuntimeManager.CreateInstance(_reloadEvent);
    }

    public void OnDestroy()
    {
        _stepsEventInstance.release();
        _buttonEventInstance.release();
        _reloadEventInstance.release();
        foreach (var sound in _backgroundSounds)
        {
            if (sound.hasHandle())
            {
                sound.release();
            }
        }
        if (_lowPassDSP.hasHandle())
        {
            _lowPassDSP.release();
        }
    }

    private void Update()
    {
        Sound sound;
        _channel.getCurrentSound(out sound);
        sound.setMusicSpeed(_speed);
    }

    //Ajusta el par�metro de la velocidad, el parametro recibido es un par�metro entre 0 y 1
    public void SetSpeedParameter(float speed)
    {
        //Dividimos entre 2 para que no se acelere demasiado
        //Y empezar en 0.8 como m�nimo para que al caminar no suene muy acelerado tampoco
        _speed = 0.8f + (speed/2);
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
        if(phase == 0)
        {
            _channel.addDSP(0, _lowPassDSP);
        }
        else if(phase >= 2) {
            _channel.removeDSP(_lowPassDSP);
        }
    }
    
    public void PlayReloadSound(Vector3 position)
    {
        _reloadEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _reloadEventInstance.start();
        
    }

    public void SetDanger(float danger)
    {
        _channel.stop();
        int index = Math.Clamp((int)(danger / 0.33f), 0, _backgroundSounds.Length -1);
        _coreSystem.playSound(_backgroundSounds[index], _channelGroup, false, out _channel);
    }
}
