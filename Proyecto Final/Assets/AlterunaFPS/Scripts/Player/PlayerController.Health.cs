﻿using FMODUnity;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace AlterunaFPS
{
	public partial class PlayerController
	{
		[Header("Health")]
		public float MaxHealth = 20f;
		
		private Health _health;
		private int _lastSpawnIndex;

		RawImage damageIndicator;
        

        private void InitializeHealth()
		{
			_health = GetComponent<Health>();
			GameObject damageIndicatorGO = GameObject.FindGameObjectWithTag("DamageIndicator");
			if(damageIndicatorGO != null)
			{
				damageIndicator = damageIndicatorGO.GetComponent<RawImage>();
			}

            if (_isOwner)
			{
				_health.OnDeath.AddListener(OnDeath);
				_health.HealthPoints = MaxHealth;
				_health.MaxHealthPoints = MaxHealth;
			}
			UpdatedHealthValue();
        }
		public void UpdatedHealthValue()
		{
			if (damageIndicator)
			{
				Color c = Color.white; c.a = (1 - (_health.HealthPoints / MaxHealth));
				damageIndicator.color = c;

            }
			SoundManager.Instance().SetDanger(1 - (_health.HealthPoints / MaxHealth));
            /*if (emitter)
            {
                emitter.EventInstance.setParameterByName("danger", 1 - (_health.HealthPoints / MaxHealth));
            }*/
        }

		private void OnDeath(ushort senderID)
		{
			if (_possesed)
			{
				CinemachineVirtualCameraInstance.Instance.gameObject.SetActive(false);
				CinemachineVirtualCameraInstance.Instance.Follow(null);

					ScoreBoard.Instance.AddDeaths(Avatar.Possessor, 1);
					ScoreBoard.Instance.AddKills(senderID, 1);
				if (_isHost)
                {
                }
			}
			
			_health.HealthPoints = MaxHealth;
            UpdatedHealthValue();

            if (_offline)
			{
				transform.position = Vector3.zero;
			}
			else
			{
				int spawnIndex = 0;
				int spawnLocationsCount = Multiplayer.AvatarSpawnLocations.Count;

				// get random spawn location
				if (spawnLocationsCount > 1)
				{
					do
					{
						spawnIndex = Random.Range(0, spawnLocationsCount);
					}
					while (_lastSpawnIndex == spawnIndex);
				}
				else if (spawnLocationsCount <= 0)
				{
					throw new IndexOutOfRangeException("AvatarSpawnLocations must be greater than zero.");
				}

				Transform spawn = Multiplayer.AvatarSpawnLocations.Count > 0 ? 
					Multiplayer.AvatarSpawnLocations[spawnIndex] : 
					Multiplayer.AvatarSpawnLocation;
				
				_controller.enabled = false;
				transform.position = spawn.position;
				transform.rotation = spawn.rotation;
				_cinemachineTargetYaw = _bodyRotate = spawn.rotation.y;
				_controller.enabled = true;
			}
			RespawnController.Respawn(gameObject);
		}
	}
}