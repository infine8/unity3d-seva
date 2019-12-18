using System.Collections.Generic;
using System.Xml.Serialization;

namespace UniKid.Core.Model.Audio
{
    [XmlType("audio-controller-snapshot")]
    public sealed class AudioControllerSnapshot
    {
        [XmlAttribute("sound-mute")] public bool SoundMute = false;
        [XmlAttribute("music-mute")] public bool MusicMute = false;
        [XmlAttribute("sound-volume")] public float SoundVolume = 1.0f;
        [XmlAttribute("music-volume")] public float MusicVolume = 1.0f;
    }

    public sealed class AudioController
    {
        public static readonly string SETTINGS_PLAY_MUSIC_KEY = "settings_play_music";
        public static readonly string SETTINGS_PLAY_SOUND_KEY = "settings_play_effects";


        private Dictionary<string, AudioSourceController> _audio = new Dictionary<string, AudioSourceController>();
        private List<AudioSourceController> _soundGroup = new List<AudioSourceController>();
        private List<AudioSourceController> _musicGroup = new List<AudioSourceController>();
        private bool _soundMute = false;
        private bool _musicMute = false;
        private float _soundVolume = 1.0f;
        private float _musicVolume = 1.0f;

        private bool _initialized = false;

        public void Initialize(IEnumerable<AudioSourceController> sounds)
        {
            if (!_initialized)
            {
                _audio.Clear();

                foreach (AudioSourceController s in sounds)
                    if (!_audio.ContainsKey(s.name))
                    {
                        _audio.Add(s.name, s);
                        if (s.IsMusic)
                            _musicGroup.Add(s);
                        else
                            _soundGroup.Add(s);
                    }

                SoundVolume = _soundVolume;
                MusicVolume = _musicVolume;
                SoundMute = _soundMute;
                MusicMute = _musicMute;

                _initialized = true;
            }
        }

        public void CheckSoundMute()
        {
            //if (PlayerPrefs.HasKey(SETTINGS_PLAY_MUSIC_KEY))
            //    _musicMute = PlayerPrefs.GetInt(SETTINGS_PLAY_MUSIC_KEY) == 0;
            //else
            //    _musicMute = false;

            //if (PlayerPrefs.HasKey(SETTINGS_PLAY_SOUND_KEY))
            //    _soundMute = PlayerPrefs.GetInt(SETTINGS_PLAY_SOUND_KEY) == 0;
            //else
            //    _soundMute = false;

            SoundMute = _soundMute;
            MusicMute = _musicMute;
        }

        public bool LoadSnapshot(AudioControllerSnapshot snapshot)
        {
            CheckSoundMute();

            if (snapshot == null) return false;

            _soundMute = snapshot.SoundMute;
            _musicMute = snapshot.MusicMute;

            _soundVolume = snapshot.SoundVolume;
            _musicVolume = snapshot.MusicVolume;

            return true;
        }

        public AudioControllerSnapshot CreateSnapshot()
        {
            var snapshot = new AudioControllerSnapshot();
            snapshot.SoundMute = SoundMute;
            snapshot.MusicMute = MusicMute;

            //PlayerPrefs.SetInt(SETTINGS_PLAY_MUSIC_KEY, _musicMute ? 0 : 1);
            //PlayerPrefs.SetInt(SETTINGS_PLAY_SOUND_KEY, _soundMute ? 0 : 1);

            snapshot.SoundVolume = SoundVolume;
            snapshot.MusicVolume = MusicVolume;
            return snapshot;
        }

        public void Play(string soundName)
        {
            AudioSourceController s;
            if (_audio.TryGetValue(soundName, out s)) s.Play();
        }

        //public void Play(Const.Sound soundName)
        //{
        //    Play(soundName.ToString());
        //}

        public void Stop(string soundName)
        {
            AudioSourceController s;
            if (_audio.TryGetValue(soundName, out s))
                s.Stop();
        }

        public bool IsSoundPlaying(string soundName)
        {
            AudioSourceController s;
            if (_audio.TryGetValue(soundName, out s)) return s.IsPlaying;

            return false;
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                _soundGroup.ForEach(x => x.Volume = _soundVolume);
            }
        }

        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                _musicGroup.ForEach(x => x.Volume = _musicVolume);
            }
        }

        public bool SoundMute
        {
            get { return _soundMute; }
            set
            {
                _soundMute = value;
                _soundGroup.ForEach(x => x.Mute = _soundMute);
            }
        }

        public bool MusicMute
        {
            get { return _musicMute; }
            set
            {
                _musicMute = value;
                _musicGroup.ForEach(x => x.Mute = _musicMute);
            }
        }
    }
}