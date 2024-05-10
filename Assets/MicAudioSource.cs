using UnityEngine;
using System.Linq;

class MicAudioSource : MonoBehaviour {
    private const int SAMPLE_RATE = 44100;
    private const int RESOLUTION = 1024;
    private AudioSource m_MicAudioSource;
    private GameObject[] m_Cubes = new GameObject[RESOLUTION / 2]; // Cubeの配列
    [SerializeField, Range(1, 300)] private float m_AmpGain = 300;

    public GameObject prefabCube;
    
    private void Awake() {
        m_MicAudioSource = GetComponent<AudioSource>();
    }

    void Start() {

        foreach (var device in Microphone.devices) {
            Debug.Log($"Device Name: {device}");
        }

        // 最初に見つかったデバイスを取得
        string firstDevice = Microphone.devices.FirstOrDefault() ?? "";

        // デバイス名をログに出力
        Debug.Log($"Selected Device: {firstDevice}");

        // マイクを開始
        MicStart(firstDevice);
        
        // Cubeの初期化と配置
        for (int i = 0; i < RESOLUTION / 2; i++) {
            m_Cubes[i] = Instantiate(prefabCube, new Vector3(i * 0.1f, 0, 0), Quaternion.identity);
        }
    }

    void Update() {
        DrawSpectrum();
    }

    private void DrawSpectrum() {
        if (!m_MicAudioSource.isPlaying) return;
        
        float[] spectrum = new float[RESOLUTION];
        m_MicAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < RESOLUTION / 2; i++) {
            Vector3 newPosition = m_Cubes[i].transform.position;
            newPosition.y = spectrum[i] * m_AmpGain;
            m_Cubes[i].transform.position = newPosition;
        }
    }
    
    private void MicStart(string device) {
        if (string.IsNullOrEmpty(device)) {
            Debug.LogError("No microphone device available.");
            return;
        }

        Debug.Log($"Attempting to start microphone on device: {device}");
        m_MicAudioSource.clip = Microphone.Start(device, true, 1, SAMPLE_RATE);
        if (Microphone.IsRecording(device)) {
            while (Microphone.GetPosition(device) <= 0) { }
            m_MicAudioSource.Play();
            Debug.Log("Microphone started successfully.");
        } else {
            Debug.LogError("Failed to start microphone.");
        }
    }
}