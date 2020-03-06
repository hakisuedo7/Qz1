using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject Kegel;
    [SerializeField] private GameObject Ball;
    [SerializeField] private Material BallMaterial;
    [SerializeField] private float Edge;
    [SerializeField] ParticleSystem PS;

    Color[] colors = { Color.red, Color.blue, Color.white, Color.black, Color.cyan, Color.gray, Color.grey, Color.magenta };

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("restart");
            Init();
        }
    }

    // 初始化球瓶 && 球
    void Init() {
        // 先清除場上的東西
        foreach (GameObject kegel in GameObject.FindGameObjectsWithTag("Kegel")) {
            Destroy(kegel);
        }
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) {
            Destroy(ball);
        }

        // 創建球瓶
        SetKegel();
        // 創建球
        CreateBall();
    }

    void SetKegel() {
        for (int i = 0; i < 4; i++)
        {
            float x, z;
            x = -i * Edge * 1.732050f / 6;
            z = -i * Edge / 6;
            for (int j = 0; j <= i; j++)
            {
                Instantiate(Kegel, new Vector3(x - 20f, 0, z), Quaternion.identity);
                z += Edge / 3;
            }
        }
    }

    void CreateBall() {
        BallMaterial.SetColor("_Color", colors[Random.Range(0, 8)]);
        GameObject ball = Instantiate(Ball, new Vector3(47, 3.5f, 0), Quaternion.identity);
        ball.transform.localRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }

    public void CallParticle(Vector3 vector3) {
        PS.transform.position = vector3;
        PS.Play();
    }
}