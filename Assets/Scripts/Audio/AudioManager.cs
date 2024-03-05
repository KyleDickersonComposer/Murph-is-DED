using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Volume")]
        [Range(0, 1)]
        public float masterVolume = 0.5f;

        [Header("Volume")]
        [Range(0, 1)]
        public float sfxVolume = 0.5f;

        [Header("Volume")]
        [Range(0, 1)]
        public float musicVolume = 0.5f;

        [Header("Volume")]
        [Range(0, 1)]
        public float ambienceVolume = 0.5f;

        private Bus masterBus;
        private Bus sfxBus;
        private Bus musicBus;
        private Bus ambienceBus;

        private List<EventInstance> eventInstances;
        private List<StudioEventEmitter> eventEmitters;

        public EventInstance musicEventInstance;
        public EventInstance ambienceInstance;

        public static AudioManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else Instance = this;

            masterBus = RuntimeManager.GetBus("bus:/");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
            musicBus = RuntimeManager.GetBus("bus:/MUSIC");
            ambienceBus = RuntimeManager.GetBus("bus:/AMBIENCE");
        }
        void Start()
        {
            eventInstances = new List<EventInstance>();
            eventEmitters = new List<StudioEventEmitter>();



            InitializeMusic(FmodEvents.Instance.Music);
            InitalizeAmbience(FmodEvents.Instance.Ambience);
        }

        void Update()
        {
            masterBus.setVolume(masterVolume);
            sfxBus.setVolume(sfxVolume);
            musicBus.setVolume(musicVolume);
            ambienceBus.setVolume(ambienceVolume);
        }

        public void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.start();
        }

        public void InitalizeAmbience(EventReference ambienceEventReference)
        {
            ambienceInstance = CreateInstance(ambienceEventReference);
            ambienceInstance.start();
        }

        public void PlayOneShot(EventReference sound, Vector3 pos)
        {
            RuntimeManager.PlayOneShot(sound, pos);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }

        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            eventEmitters.Add(emitter);
            return emitter;
        }

        private void CleanUp()
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }

            foreach (StudioEventEmitter emitter in eventEmitters)
            {
                emitter.Stop();
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
