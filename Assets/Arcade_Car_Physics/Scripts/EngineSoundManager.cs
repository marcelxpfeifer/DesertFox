/*
 * This code is part of Arcade Car Physics for Unity by Saarg (2018)
 * 
 * This is distributed under the MIT Licence (see LICENSE.md for details)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleBehaviour {
    [RequireComponent(typeof(WheelVehicle))]
    [RequireComponent(typeof(AudioSource))]

    public class EngineSoundManager : MonoBehaviour {

        [Header("AudioClips")]
        public AudioClip starting;
        public AudioClip rolling;
        public AudioClip stopping;

        [Header("pitch parameter")]
        public float flatoutSpeed = 20.0f;
        [Range(0.0f, 3.0f)]
        public float minPitch = 0.7f;
        [Range(0.0f, 0.1f)]
        public float pitchSpeed = 0.05f;

        private AudioSource _source;
        private WheelVehicle _vehicle;
        
        void Start () {
            _source = GetComponent<AudioSource>();
            _vehicle = GetComponent<WheelVehicle>();
        }
        
        void Update ()
        {
            var state = _source.clip;
            var engineOff = _vehicle.Handbrake || !_vehicle.IsPlayer;
            
            
            if (state != stopping && engineOff)
            {
                _source.clip = stopping;
                _source.pitch = 1;
                _source.PlayOneShot(stopping);
            }

            if ((state == stopping || state == null) && !_vehicle.Handbrake && _vehicle.IsPlayer)
            {
                _source.clip = starting;
                _source.PlayOneShot(starting);

                _source.pitch = 1;
            }

            if (!engineOff && !_source.isPlaying)
            {
                _source.clip = rolling;
                _source.Play();
            }

            if (_source.clip == rolling)
            {
                _source.pitch = Mathf.Lerp(_source.pitch, minPitch + Mathf.Abs(_vehicle.Speed) / flatoutSpeed, pitchSpeed);
            }
        }
    }
}
