using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public static Spawner Instance {get; private set;}

    [SerializeField]public GameObject beanPrefab;

    public event EventHandler<OnBeanFishSpawnedEventArgs> OnBeanFishSpawnedEvent;
    public class OnBeanFishSpawnedEventArgs : EventArgs {
        public GameObject bean;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    public void SpawnBeanFish(int page,int level, float exp)
    {
        //get a random point nad make an instance of the prefab
        GameObject instance = Instantiate(beanPrefab, transform);

        Vector3 point = GameManager.Instance.GetPoint();
        instance.GetComponent<BeanFishMove>().InstantiateBeanFish(page, level, exp,point);

        OnBeanFishSpawnedEvent?.Invoke(this, new OnBeanFishSpawnedEventArgs { bean = instance }) ;
    }

}
