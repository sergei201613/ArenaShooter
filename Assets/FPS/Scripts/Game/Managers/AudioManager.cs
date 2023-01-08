using Sgorey.ArenaShooter;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer[] AudioMixers;

    [SerializeField]
    private AudioClip _normalAmbient;

    [SerializeField]
    [Range(0f, 1f)]
    private float _normalAmbientVolume = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _battleAmbientVolume = 1f;

    [SerializeField]
    private AudioClip _battleAmbient;

    [SerializeField]
    private AudioSource _ambientSource;

    private BattleDetector _playerBattleDetector;

    private void Awake()
    {
        AudioUtility.SetMasterVolume(1f);

        _ambientSource.clip = _normalAmbient;
        _ambientSource.volume = _normalAmbientVolume;
        _ambientSource.Play();

        var pcc = FindObjectOfType<PlayerCharacterController>();

        if (pcc != null)
            _playerBattleDetector = pcc.GetComponent<BattleDetector>();
    }

    private void OnEnable()
    {
        if (_playerBattleDetector == null)
            return;

        _playerBattleDetector.BattleBegin += OnBattleBegin;
        _playerBattleDetector.BattleOver += OnBattleOver;
    }

    private void OnDisable()
    {
        if (_playerBattleDetector == null)
            return;

        _playerBattleDetector.BattleBegin -= OnBattleBegin;
        _playerBattleDetector.BattleOver -= OnBattleOver;
    }

    public AudioMixerGroup[] FindMatchingGroups(string subPath)
    {
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            AudioMixerGroup[] results = AudioMixers[i].FindMatchingGroups(subPath);
            if (results != null && results.Length != 0)
            {
                return results;
            }
        }

        return null;
    }

    public void SetFloat(string name, float value)
    {
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            if (AudioMixers[i] != null)
            {
                AudioMixers[i].SetFloat(name, value);
            }
        }
    }

    public void GetFloat(string name, out float value)
    {
        value = 0f;
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            if (AudioMixers[i] != null)
            {
                AudioMixers[i].GetFloat(name, out value);
                break;
            }
        }
    }

    private void OnBattleBegin()
    {
        _ambientSource.clip = _battleAmbient;
        _ambientSource.volume = _battleAmbientVolume;
        _ambientSource.Play();
    }

    private void OnBattleOver()
    {
        _ambientSource.clip = _normalAmbient;
        _ambientSource.volume = _normalAmbientVolume;
        _ambientSource.Play();
    }
}