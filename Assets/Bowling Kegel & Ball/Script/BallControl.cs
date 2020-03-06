using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    enum GameStatus {
        Wait,
        Move,
        Rotate,
        SetStrength,
        Start,
        End
    }
    GameStatus status;
    GameObject camera;
    GameObject arrow;

    Vector2 moveRange = new Vector2(-5, 5);
    Vector2 rotateRange = new Vector2(-15, 15);

    float Angle;
    float Strength;

    TrailRenderer TR;
    GameController GC;

    // Start is called before the first frame update
    void Start()
    {
        status = GameStatus.Wait;
        camera = GameObject.Find("Camera");
        camera.transform.position = new Vector3(50, 7, 0);
        arrow = FindInActiveObjectByName("Arrow");
        arrow.transform.position = new Vector3(45, 0.1f, 0);
        arrow.transform.rotation = Quaternion.identity;
        Angle = 0;
        Strength = 25;

        TR = gameObject.GetComponent<TrailRenderer> ();
        TR.enabled = false;

        GC = GameObject.Find("Stage").GetComponent<GameController> ();
    }

    void Update()
    {
        if (status == GameStatus.Wait) {
            StartCoroutine(Waiting(1f));
            status = GameStatus.Move;
        }
        else if (status == GameStatus.Move) {
            Vector3 newPos = transform.position;
            if (Input.GetKey(KeyCode.Space)) {
                status = GameStatus.SetStrength;
            }
            else if(Input.GetKey(KeyCode.DownArrow)) {
                status = GameStatus.Rotate;
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                newPos.z -= 0.1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                newPos.z += 0.1f;
            }

            newPos.z = Mathf.Clamp(newPos.z, -5.5f, 5.5f);
            transform.position = newPos;
            camera.transform.position = new Vector3(50, 7, newPos.z);
            arrow.transform.position = new Vector3(45, 0.1f, newPos.z);
        }
        else if(status == GameStatus.Rotate) {
            float ang = Angle;
            if (Input.GetKey(KeyCode.Space)) {
                status = GameStatus.SetStrength;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow)) {
                status = GameStatus.Move;
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                ang -= 0.15f;
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                ang += 0.15f;
            }

            Angle = Mathf.Clamp(ang, -10f, 10f);
            arrow.transform.rotation = Quaternion.Euler(0, Angle, 0);
        }
        else if (status == GameStatus.SetStrength) {
            arrow.SetActive(false);

            // 放開射出
            if (Input.GetKeyUp(KeyCode.Space)) {
                status = GameStatus.Start;
            }
        }
        else if (status == GameStatus.Start) {
            TR.enabled = true;
            Vector3 currentAng = Vector3.left;
            this.gameObject.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, Angle, 0) * currentAng * 55;   //  Strength range(35 ~ 55)
            status = GameStatus.End;
        }
        else if (status == GameStatus.End) {
            Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1f) {
                GC.CallParticle(gameObject.transform.position);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Waiting(float x) {
        yield return new WaitForSeconds(x);
        if(status == GameStatus.Move || status == GameStatus.Rotate)
            arrow.SetActive(true);
    }

    GameObject FindInActiveObjectByName(string name) {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].hideFlags == HideFlags.None) {
                if (objs[i].name == name) {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
