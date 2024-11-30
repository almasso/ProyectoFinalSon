using FMODUnity;
using UnityEngine;

namespace AlterunaFPS
{
	public class GunAudio : MonoBehaviour
	{
		public RandomPitch ReloadSfx;

        [SerializeField]
        private FMODUnity.EventReference _shotEvent;

        StudioEventEmitter emitter;

        public void PlayReloadSfx() => ReloadSfx.Play();


        public void Start()
        {
            emitter = GetComponentInParent<StudioEventEmitter>();
        }

        public void StartReload()
		{
            if(emitter != null)
            {
                emitter.EventInstance.setParameterByName("reload", 1);
            }
		}

        public void EndReload()
        {
            if (emitter != null)
            {
                emitter.EventInstance.setParameterByName("reload", 0);
            }
        }

        public void PlayFireSfx() => FMODUnity.RuntimeManager.PlayOneShot(_shotEvent, gameObject.transform.position);
	}
}