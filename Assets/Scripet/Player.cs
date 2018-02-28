using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour {
    public int Force;           //小人受力
    public Transform Body;      //玩家身体
    public Transform Head;      //玩家头部
    public GameObject Cube;      //玩家立方体
    public int MaxDistance=3;   //生成盒子的最远距离
    public GameObject Scorestext;
    public GameObject Butons;

    private float _starttime;
    private Rigidbody _rigidbody;
    Vector3 _direction = new Vector3(1, 0.3f, 0);
    private Vector3 _body;
    private Vector3 _head;
    private GameObject _cube;
    private GameObject _cubenext;
    private Vector3 _cubetransform;
    private Vector3 _cameraRelativePosition;
    private int Scores;
    // Use this for initialization
    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _body = Body.transform.localScale;
        _head = Head.transform.localPosition;
        _cubetransform = Cube.transform.localScale;
        _cube = Cube;
        NewCube();
        ChangeCubeColor(_cube);
        _cameraRelativePosition= Camera.main.transform.position - transform.position;
        Time.timeScale = 0;
    }

    private void ChangeCubeColor(GameObject cube)
    {
        cube.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1), UnityEngine.Random.Range(0f, 1), UnityEngine.Random.Range(0f, 1));
    }

    private void NewCube()
    {
       var cube= Instantiate(Cube);
        cube.transform.position= _cube.transform.position +_direction * UnityEngine.Random.Range(1.1f, MaxDistance);
        ChangeCubeColor(cube);
        _cubenext = cube;
    }

    // Update is called once per frame
    void Update () {
        if (GetComponent<Transform>().position.y < 0||GetComponent<Transform>().position.y>15)
        {
            Gameover();
            for (int i = 0; i < 100; i++)
            {
                NewCube();
                GetComponent<Transform>().transform.localPosition = new Vector3(0, -30);

            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            _starttime = Time.time;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            var elapse = Time.time - _starttime;
            
            OnJump(elapse);
            Body.transform.DOScale(_body, 0.1f);
            Head.transform.DOLocalMoveY(_head.y, 0.1f);
            _cube.transform.DOScale(_cubetransform, 0.1f);
            //Vector3 pos = new Vector3(_direction.x*_cubenext.transform.localPosition.x, GetComponent<Transform>().transform.localPosition.y*Time.deltaTime, _direction.z*_cubenext.transform.localPosition.z);

            
            
        }
        if (Input.GetMouseButton(0)||Input.GetKey(KeyCode.Space))
        {
            //_Player.AddForce(new Vector3(0, Force, 0));
            if (Body.transform.localScale.y>0.05) {
                Head.transform.position += new Vector3(0, -1, 0) * Force * 0.05f * Time.deltaTime;
                Body.transform.localScale += new Vector3(1, -1, 1) * Force * 0.1f * Time.deltaTime;

                _cube.transform.localScale += new Vector3(0, -1, 0) * Force * 0.15f * Time.deltaTime;
               // Cube.transform.localPosition += new Vector3(0, -1, 0) * Force * 0.15f * Time.deltaTime;
            }
            
            //print(Force * Time.deltaTime);

        }
	}
    void OnCollisionStay(Collision collision)
    {
        //print(collision.gameObject.name);
        if (collision.gameObject.name == "Plane")
        {
            //print("Game over");
            Gameover();
            for(int i=0;i<100; i++)
            {
                NewCube();
                GetComponent<Transform>().transform.localPosition = new Vector3(0, -30);
               
            }
        }
        else if (collision.gameObject != _cube&&GetComponent<Transform>().transform.localPosition.y>0.4f)
        {
            _cube = collision.gameObject;
            Scores++;
            Scorestext.GetComponent<Text>().text = Scores.ToString();
            Randomdirection();
            NewCube();
            MoveCamera();
        }
    }

    private void Gameover()
    {
        Scorestext.SetActive(false);
        Butons.SetActive(true);
        //Butons.transform.Find("Text").SetActive(true);
        GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Text").GetComponent<Text>().text= "游戏结束，您的得分为\n" + Scores.ToString()+"分";
        Butons.transform.Find("Buttontext").GetComponent<Text>().text = "重新开始";
        //Butons
    }

    private void Randomdirection()
    {
        var direction = UnityEngine.Random.Range(0, 2);
//print(direction);
        _direction = direction ==0? new Vector3(1, 0.3f, 0): new Vector3(0, 0.3f, 1);
    }

    private void MoveCamera()
    {
        Camera.main.transform.DOMove(transform.position+ _cameraRelativePosition, 1);
    }

    void OnJump(float elapse)
    {
        _rigidbody.AddForce((new Vector3(0, 1.5f, 0) + _direction) * elapse * Force*5, ForceMode.Impulse);
        if (1 == _direction.x)
        {
            GetComponent<Transform>().transform.DOMoveZ(_cubenext.transform.position.z, 1f);
        }
        else
        {
            GetComponent<Transform>().transform.DOMoveX(_cubenext.transform.position.x, 1f);
        }
    }

}
