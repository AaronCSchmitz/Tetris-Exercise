﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    float lastFall = 0;
    int keyBuffer = 0;

    bool isValidGridPos () {
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2 (child.position);

            //not inside border?
            if (!Playfield.insideBorder (v)) {
                return false;
            }

            //block in grid cell, and not part of same group
            if (Playfield.grid[(int) v.x, (int) v.y] != null &&
                Playfield.grid[(int) v.x, (int) v.y].parent != transform) {
                return false;
            }
        }
        return true;
    }

    void updateGrid () {
        // Remove old children from grid
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2 (child.position);
            Playfield.grid[(int) v.x, (int) v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start () {
        //if the top grid is full, game over
        if (!isValidGridPos ()) {
            Debug.Log ("GAME OVER");
            Destroy (gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey (KeyCode.DownArrow)) {
            keyBuffer++;
        } else {
            keyBuffer = 0;
        }

        // Move Left
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            // Modify position
            transform.position += new Vector3 (-1, 0, 0);

            // See if valid
            if (isValidGridPos ())
                // It's valid. Update grid.
                updateGrid ();
            else
                // It's not valid. revert.
                transform.position += new Vector3 (1, 0, 0);
        }

        // Move Right
        else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            // Modify position
            transform.position += new Vector3 (1, 0, 0);

            // See if valid
            if (isValidGridPos ())
                // It's valid. Update grid.
                updateGrid ();
            else
                // It's not valid. revert.
                transform.position += new Vector3 (-1, 0, 0);
        }

        // Rotate
        else if (Input.GetKeyDown (KeyCode.UpArrow)) {
            transform.Rotate (0, 0, -90);

            // See if valid
            if (isValidGridPos ())
                // It's valid. Update grid.
                updateGrid ();
            else
                // It's not valid. revert.
                transform.Rotate (0, 0, 90);
        }

        // Move Downwards and Fall
        else if (Input.GetKeyDown (KeyCode.DownArrow) || keyBuffer == 10 ||
            Time.time - lastFall >= 1) {
            // Modify position
            transform.position += new Vector3 (0, -1, 0);
            keyBuffer = 0;

            // See if valid
            if (isValidGridPos ()) {
                // It's valid. Update grid.
                updateGrid ();
            } else {
                // It's not valid. revert.
                transform.position += new Vector3 (0, 1, 0);

                // Clear filled horizontal lines
                Playfield.deleteFullRows ();

                // Spawn next Group
                FindObjectOfType<Spawner> ().spawnNext ();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }

}