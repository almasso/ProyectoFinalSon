using UnityEngine;

namespace AlterunaFPS
{
	public class GunAudio : MonoBehaviour
	{
		public RandomPitch ReloadSfx;

        [SerializeField]
        private FMODUnity.EventReference _shotEvent;

        public void PlayReloadSfx() => ReloadSfx.Play();
		public void PlayFireSfx() => FMODUnity.RuntimeManager.PlayOneShot(_shotEvent, gameObject.transform.position);
	}
}