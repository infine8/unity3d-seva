using UnityEngine;
using System.Collections.Generic;

namespace UniKid.Core.Model.Audio
{
    public sealed class AudioLoader : MonoBehaviour
    {
        public AudioSourceController[] AudioOriginals;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            AudioSourceController instance;
            var audios = new List<AudioSourceController>();

            foreach (AudioSourceController original in AudioOriginals)
            {
                instance = Instantiate(original) as AudioSourceController;
                instance.name = original.name;
                instance.transform.parent = transform;

                audios.Add(instance);
            }

            CoreContext.AudioController.Initialize(audios);
        }

    }
}
