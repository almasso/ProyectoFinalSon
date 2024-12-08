using Alteruna;
using FMODUnity;
using UnityEngine;

namespace AlterunaFPS
{
	public class GunAudio : MonoBehaviour
	{
		public RandomPitch ReloadSfx;

        PlayerController pController;

        public bool shouldPlay = false;

        public void PlayMagazineSound()
        {
            SoundManager.Instance().SetReloadPhase(0, shouldPlay);
            SoundManager.Instance().PlayReloadSound(this.gameObject.transform.position);
        }

        public void SetReloadPhase(int phase)
        {
            SoundManager.Instance().SetReloadPhase(phase, shouldPlay);
            if (phase >= 2) shouldPlay = false;
        }

        public void PlayFireSfx() => SoundManager.Instance().PlayShotSound(gameObject.transform.position);
	}
}