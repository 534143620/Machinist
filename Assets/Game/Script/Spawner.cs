using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private List<Character> spawnCharactersList;
    private bool hasSpawned;
    public BoxCollider _collider;
    public UnityEvent onAllSpawnCharactersDead;

    private void Awake() {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointArray);
        spawnCharactersList = new List<Character>();
    }

    private void Update() {
        if(!hasSpawned || spawnCharactersList.Count == 0)
            return;
        bool allSpawnedCCDead = true;
        foreach (var item in spawnCharactersList)
        {
            if(item.currentState != Character.CharacterState.Dead)
            {
                allSpawnedCCDead = false;
                break;
            }
        }
        if(allSpawnedCCDead){
            if(onAllSpawnCharactersDead != null)
                onAllSpawnCharactersDead.Invoke();
            // EventHandler.CallOpenTheDoorEvent();
            // EventHandler.CallGameWinEvent();
            spawnCharactersList.Clear();
        }
    }

    public void SpawnCharacters()
    {
        if(hasSpawned)
            return;
        hasSpawned = true;

        foreach (SpawnPoint point in spawnPointList)
        {
            if(point.EnemyToSpawn != null)
            {
              GameObject spawnedGameObject = Instantiate(point.EnemyToSpawn,point.transform.position,Quaternion.identity);
              spawnCharactersList.Add(spawnedGameObject.GetComponent<Character>());
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            SpawnCharacters();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,_collider.bounds.size);
    }
}
