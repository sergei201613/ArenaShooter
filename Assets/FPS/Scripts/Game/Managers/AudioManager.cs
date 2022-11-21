using Sgorey.ArenaShooter;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer[] AudioMixers;

    [SerializeField]
    private AudioClip _normalAmbient;

    [SerializeField]
    private AudioClip _battleAmbient;

    [SerializeField]
    private AudioSource _ambientSource;

    private BattleDetector _playerBattleDetector;

    private void Awake()
    {
        _ambientSource.clip = _normalAmbient;

        _playerBattleDetector = FindObjectOfType<PlayerCharacterController>()
            .GetComponent<BattleDetector>();
    }

    private void OnEnable()
    {
        _playerBattleDetector.BattleBegin += OnBattleBegin;
        _playerBattleDetector.BattleOver += OnBattleOver;
    }

    private void OnDisable()
    {
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
        _ambientSource.volume = .5f;
        _ambientSource.Play();
    }

    private void OnBattleOver()
    {
        _ambientSource.clip = _normalAmbient;
        _ambientSource.volume = .25f;
        _ambientSource.Play();
    }
}