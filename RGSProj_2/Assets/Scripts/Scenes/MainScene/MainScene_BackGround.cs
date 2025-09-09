using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class MainScene_BackGround : MonoBehaviour
{
    [SerializeField] private GameObject backGroundDummy;
    [SerializeField]private List<Color> colors = new List<Color>();
    [SerializeField]private Transform[] spawns = new Transform[0];
    [SerializeField] private float instantiateCool;
    private void Start()
    {
        StartCoroutine(InstantiateDummy());
    }
    private IEnumerator InstantiateDummy()
    {
        while (true) {
            yield return new WaitForSeconds(instantiateCool);
            for(int i = 0; i < Random.Range(1, 4); i++)
            {
                GameObject GO = Instantiate(backGroundDummy, spawns[Random.Range(0, spawns.Length)].position, Quaternion.identity);
                int aa = Random.Range(0, colors.Count);
                GO.GetComponentInChildren<SpriteRenderer>().color = colors[aa];
                var PS = GO.transform.GetChild(1).GetComponent<ParticleSystem>().main;
                PS.startColor = colors[aa];
                GO.GetComponent<Rigidbody2D>().linearVelocity = Random.Range(0.18f, 0.3f) * UtilClass.RotateVector2(-GO.transform.position, Random.Range(-30.0f, 30.0f));
                Destroy(GO, 15f);
            }

        }
    }
}
