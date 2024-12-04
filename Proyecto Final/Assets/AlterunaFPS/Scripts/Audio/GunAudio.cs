using FMODUnity;
using UnityEngine;

namespace AlterunaFPS
{
	public class GunAudio : MonoBehaviour
	{
		public RandomPitch ReloadSfx;
        
        public void PlayMagazineSound()
        {
            SoundManager.Instance().SetReloadPhase(0);
            SoundManager.Instance().PlayReloadSound(this.gameObject.transform.position);
        }

        public void SetReloadPhase(int phase)
        {
            SoundManager.Instance().SetReloadPhase(phase);
        }

        public void PlayFireSfx() => SoundManager.Instance().PlayShotSound(gameObject.transform.position);
	}
}