using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    //Groups
    public GameObject[] groups;

    public void spawnNext () {
        // random index
        int i = Random.Range (0, groups.Length);

        //spawn group at current position
        Instantiate (groups[i], transform.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start () {
        spawnNext ();
    }

    // Update is called once per frame
    void Update () {

    }
}