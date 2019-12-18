using UnityEngine;

namespace UniKid.Core.Model.Audio
{
    // WARN: ����� �� �������� �� ��, ��� ��������� ����� �������� �����������
    public class AudioSourceController : MonoBehaviour
    {
        private AudioSource _cachedAudioSource;

        /// <summary> ��� </summary>
        public bool IsMusic = false;

        /// <summary> ����������� ���� </summary>
        public bool Looped = false;

        /// <summary> ����������� �������� ����� �������� �������������� ����� </summary>
        public float MinPlayInterval = 0.2f;

        /// <summary> ����������� ��������� </summary>
        public float MaxVolume = 1.0f;

        /// <summary> ������� �������� �� ������������ </summary>
        private int _playCount = 0;

        /// <summary> ����� �� �������� ���� �� ����� ������������� �������� </summary>
        private float _playTimeThreshold;

        private void Awake()
        {
            _cachedAudioSource = audio;
            if (_cachedAudioSource == null)
            {
                Debug.LogError("Invalid audio source");
            }
            else
            {
                Volume = 1.0f;
                Mute = false;
            }

        }

        public float Volume
        {
            get { return Mathf.Clamp01(_cachedAudioSource.volume/MaxVolume); }
            set { _cachedAudioSource.volume = Mathf.Clamp01(value)*MaxVolume; }
        }

        public bool Mute
        {
            get { return _cachedAudioSource.mute; }
            set { _cachedAudioSource.mute = value; }
        }

        public bool IsPlaying
        {
            get { return _cachedAudioSource.isPlaying; }
        }

        public void Play()
        {
            if (Volume < 0.01f) return;

            if (Looped)
            {
                if (_playCount == 0)
                    _cachedAudioSource.Play();
                ++_playCount;
            }
            else
            {
                float time = Time.realtimeSinceStartup;
                if (time >= _playTimeThreshold)
                {
                    _playTimeThreshold = time + MinPlayInterval;
                    _cachedAudioSource.Play();
                }
            }
        }

        public void Stop()
        {
            if (Looped)
            {
                if (_playCount > 0)
                {
                    --_playCount;
                    if (_playCount == 0)
                        _cachedAudioSource.Stop();
                }
            }
            else
            {
                _cachedAudioSource.Stop();
            }
        }
    }
}
