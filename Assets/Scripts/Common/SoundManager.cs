using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class SoundManager : UnitySingleton<SoundManager>
    {
        private const float CROSSFADE_TIME = 1.0f;

        public List<AudioSource> _efxSource = null;
        private AudioSource _preMusicSource = null;
        public AudioSource _musicSource = null;
        public float lowPitchRange = 0.9f;
        public float highPitchRange = 1.1f;


        private float _volumeBGM = 0.5f;
        private float _volumeFX = 0.5f;
        public float VolumeBGM 
        {
            get { return _volumeBGM; }
            private set
            {
                if (_musicSource != null)
                {
                    _musicSource.volume = value;
                }
                _volumeBGM = value;
            }
        }
        public float VolumeFX
        {
            get { return _volumeFX; }
            private set
            {
                if (_efxSource != null)
                {
                    foreach (AudioSource fx in _efxSource)
                    {
                        fx.volume = value;
                    }
                }

                _volumeFX = value;
            }
        }

        public void SetBGMVolume(float val)
        {
            VolumeBGM = val;
        }

        public void SetFXVolume(float val)
        {
            VolumeFX = val;
        }

        protected override void Awake()
        {
            base.Awake();
            MakeSoundComponenet();
        }

        public void Update()
        {
            for (int i = _efxSource.Count - 1; i >= 0; i--)
            {
                if (!_efxSource[i].isPlaying)
                {
                    Destroy(_efxSource[i]);
                    _efxSource.RemoveAt(i);
                }
            }
        }

        private void MakeSoundComponenet()
        {
            _efxSource = new List<AudioSource>();

            _musicSource = gameObject.AddComponent<AudioSource>();
            _preMusicSource = gameObject.AddComponent<AudioSource>();

            gameObject.AddComponent<AudioListener>();
        }

        public void PlayFx(string clipPath)
        {
            PlayFx(Resources.Load<AudioClip>("Sounds/FX/" + clipPath));
        }

        public void PlayFx(AudioClip clip, float pitch = 0)
        {
            if (clip == null) return;

            AudioSource aus = GetPlayingAudioSource(clip);

            if (aus != null) return;

            aus = gameObject.AddComponent<AudioSource>();
            aus.loop = false;
            aus.clip = clip;
            aus.volume = _volumeFX;

            if (pitch > 0)
            {
                aus.pitch = pitch;
            }

            aus.Play();

            _efxSource.Add(aus);
        }

        public bool IsPlayingFx(string clipPath)
        {
            return IsPlayingFx(Resources.Load<AudioClip>("Sounds/FX/" + clipPath));
        }

        public bool IsPlayingFx(AudioClip clip)
        {
            if (clip == null) return false;

            AudioSource audioS = GetPlayingAudioSource(clip);

            if (audioS == null) return false;

            return audioS.isPlaying;
        }

        public void StopFx(string clipPath)
        {
            if (_volumeFX < 0.1f) return;

            StopFx(Resources.Load<AudioClip>("Sounds/FX/" + clipPath));
        }

        public void StopFx(AudioClip clip)
        {
            if (clip == null || _volumeFX < 0.1f) return;

            AudioSource audioS = GetPlayingAudioSource(clip);

            if (audioS == null) return;

            audioS.Stop();
        }

        private AudioSource GetPlayingAudioSource(AudioClip clip)
        {
            for (int i = 0; i < _efxSource.Count; i++)
            {
                if (_efxSource[i].clip.Equals(clip))
                {
                    return _efxSource[i];
                }
            }

            return null;
        }

        public void PlayBgm(string clipPath, bool loop = true)
        {
            PlayBgm(Resources.Load<AudioClip>("Sounds/BGM/" + clipPath), loop);
        }
        public void PlayBgm(AudioClip clip, bool loop = true)
        {
            if (clip == null) return;

            if (_musicSource.isPlaying)
            {
                if (_musicSource.clip == clip) return;

                AudioSource temp = _preMusicSource;

                _preMusicSource = _musicSource;
                _musicSource = temp;
            }


            _musicSource.loop = loop;
            _musicSource.clip = clip;
            _musicSource.volume = VolumeBGM;

            StartCoroutine("CrossFadeBgm");

        }

        IEnumerator CrossFadeBgm()
        {
            float elapsedTime = 0.0f;
            float pastTime = 0.0f;
            float deltaTime = 0.0f;

            _musicSource.volume = 0.0f;
            _musicSource.Play();

            while (elapsedTime < CROSSFADE_TIME)
            {
                _preMusicSource.volume = ((CROSSFADE_TIME - elapsedTime) / CROSSFADE_TIME) * VolumeBGM;
                _musicSource.volume = (elapsedTime / CROSSFADE_TIME) * VolumeBGM;
                pastTime = Time.realtimeSinceStartup;

                yield return null;

                deltaTime = (Time.realtimeSinceStartup - pastTime);
                elapsedTime += deltaTime;
            }

            _musicSource.volume = VolumeBGM;
            _preMusicSource.Stop();
        }

        public void StopBgm()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
                _musicSource.clip = null;
            }
        }

        public void PauseBgm()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Pause();
            }
        }

        public void ResumeBgm()
        {
            _musicSource.UnPause();
        }

        public void RandomizeFx(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);

            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            PlayFx(clips[randomIndex], randomPitch);
        }
    }
}
