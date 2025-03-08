using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //all lists
    [SerializeField] private int[] beanFishBeanPasteList;
    [SerializeField] private int[] beanFishGoldList;
    [SerializeField] private int[] numGoldList;
    [SerializeField] private int[] clickGoldList;
    [SerializeField] private Transform[] PointList;
    [SerializeField] private Sprite[] beanSpriteArray;
    [SerializeField] private Sprite[] upgradeArray;
    [SerializeField] private string[] beanNameList;
    [SerializeField] private string[] beanDescriptionList;
    [SerializeField] private bool[] beanFishUnlockList;
    [SerializeField] private RuntimeAnimatorController[] LevelAc;

    [SerializeField] private Text beanPasteText;
    [SerializeField] private Text goldText;

    private List<GameObject> jellyList;

    List<SaveObject> entries = new List<SaveObject>();

    //saved var
    public bool IsSell;
    public int Jelatine { get; private set; }
    public int Gold { get; private set; }
    public bool isLoading { get; private set; }
    public int NumLevel { get; private set; }
    public int ClickLevel { get; private set; }
    public bool ClearGame { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Spawner.Instance.OnBeanFishSpawnedEvent += JellySpawner_OnJellySpawnedEvent;
        jellyList = new List<GameObject>();

        FileHandler.Init();

        StartGame();
    }

    private void JellySpawner_OnJellySpawnedEvent(object sender, Spawner.OnBeanFishSpawnedEventArgs e) {
        jellyList.Add(e.bean);
        if (!isLoading) { 
            SoundManager.Instance.ChangeSfxClip("Unlock");
            Save();
        }
    }


    IEnumerator AutoGetJelly()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            foreach(GameObject jelly in jellyList)
            {
                jelly.GetComponent<BeanFishMove>().GetBeanPaste();
            }
            yield return new WaitForSeconds(3f);
        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.E)) {
            //Jelatine += 100;
            //Gold += 100;
        }
        //JelatineText Format and update
        beanPasteText.text = string.Format("{0:n0}", Jelatine);

        //GoldText Format and Update
        goldText.text = string.Format("{0:n0}", Gold);
    }

    public void ChangeAc(Animator anim, int level)
    {
        //change the animation in runtime 
        anim.runtimeAnimatorController = LevelAc[level - 1];
    }

    public Vector3 GetPoint()
    {
        //Calculate new speed x and y
        Transform Point = PointList[Random.Range(0, PointList.Length)]; //Picks a random point from the list
        return Point.position;
    }
    public int GetBeanPasteFromArray(int id) {
        return beanFishBeanPasteList[id];
    }
    public bool GetUnlockFromArray(int id) {
        return beanFishUnlockList[id];
    }

    public bool[] GetUnlockArray() {
        return beanFishUnlockList;
    }
    public void SetUnlockInArray(int id) {
        beanFishUnlockList[id] = true;
    }
    public int GetGoldFromArray(int id) {
        return beanFishGoldList[id];
    }
    public string GetNameFromArray(int id) {
        return beanNameList[id];
    }
    public string GetDescriptionFromArray(int id) {
        return beanDescriptionList[id];
    }
    public Sprite GetSpriteFromArray(int id) {
        return beanSpriteArray[id];
    }
    public Sprite GetUpgradeFromArray(int id) {
        return upgradeArray[id];
    }
    public int GetClickGoldFromArray(int id) {
        return clickGoldList[id];
    }

    public int GetNumGoldFromArray(int id) {
        return numGoldList[id];
    }
    public void RemoveJelly(BeanFishMove jelly) {
        jellyList.Remove(jelly.gameObject);
        Destroy(jelly.gameObject);
        Save();
    }

    public List<GameObject> GetBeanList() {
        return jellyList;
    }

    public void SetNumLevel(int numLevel) {
        NumLevel = numLevel;
    }
    public void SetClickLevel(int clickLevel) {
        ClickLevel = clickLevel;
    }

    public void SetGold(int gold) {
        Gold = gold;
    }
    public void SetJelatine(int jelatine) {
        Jelatine = jelatine;
    }
 
    public void Save()
    {
        //Save
        //if loading prevent save so it doesnt save each time jelly is added
        if (isLoading)
            return;

        //create a class object to contain data only 
        int i = 0;
        int[] id = new int[jellyList.Count];
        int[] level =new int[jellyList.Count];
        float[] exp = new float[jellyList.Count];
        foreach (GameObject jelly in jellyList)
        {
            id[i] = jelly.GetComponent<BeanFishMove>().GetBeanFishId();
            level[i] = jelly.GetComponent<BeanFishMove>().GetBeanFishLevel();
            exp[i] = jelly.GetComponent<BeanFishMove>().GetBeanFishExp();
            i++;
        }

        //Create new instance of saveObject 
        SaveObject saveObject = new SaveObject
        {
            goldAmount = Gold,
            jelatineAmount = Jelatine,
            tempJellyList = jellyList,
            unlockList = beanFishUnlockList,
            idSave = id,
            levelSave = level,
            expSave = exp,
            numLevel = NumLevel,
            clickLevel = ClickLevel,
        };

        entries.Clear();
        entries.Add(saveObject);

        FileHandler.SaveToJson<SaveObject>(entries);

    }
    private void StartGame() {
        Load();
    }
    private void Load()
    {
        isLoading = true;

        entries = FileHandler.ReadFromJson<SaveObject>("save_");

        // Load Data to Card and Dice Manager
        if (entries.Count == 1) {
            SaveObject saveObject = entries[0];

            Gold = saveObject.goldAmount;
            Jelatine = saveObject.jelatineAmount;
            beanFishUnlockList = saveObject.unlockList;
            NumLevel = saveObject.numLevel;
            ClickLevel = saveObject.clickLevel;

            for(int i = 0; i < saveObject.tempJellyList.Count; i++) {
                Spawner.Instance.SpawnBeanFish(saveObject.idSave[i], saveObject.levelSave[i], saveObject.expSave[i]);
            }
          
        }
        else {
            // New Game
            NumLevel = 1;
            ClickLevel = 1;
            Gold = 100;
            Jelatine = 100;
            SoundManager.Instance.SetSFXVolume(0.5f);
            SoundManager.Instance.SetBGMVolume(0.5f);
        }
        isLoading = false;
        StartCoroutine(AutoGetJelly());
    }
}
