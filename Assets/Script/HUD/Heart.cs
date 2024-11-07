using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    static List<GameObject> hearts = new List<GameObject>();
    static Stack<GameObject> createdHeart = new Stack<GameObject>();
    GameObject prefab;
    float spacing = 50f;
    string objName = "";
    public void Setup()
    {
        spacing = 50f;
        if (transform) objName = transform.name;
        var loaded = Resources.Load("prefabs/Heart", typeof(GameObject));
        prefab = loaded as GameObject;

        PlayerHealth.onInit += InitHeart;
    }
    private void OnDestroy()
    {
        PlayerHealth.onInit -= InitHeart;
    }
    void InitHeart()
    {
        hearts.Clear();
        createdHeart.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < PlayerHealth.GetHealth(); i += 10)
        {
            hearts.Add(prefab);
        }
        float tempt = 0f;
        var parent = GameObject.Find(objName).transform;
        foreach (GameObject item in hearts)
        {
            var created = Instantiate(item, parent);
            var rect = created.GetComponent<RectTransform>();
            tempt += rect.anchoredPosition.x + spacing;
            print(tempt);
            rect.anchoredPosition = new Vector3(tempt, rect.anchoredPosition.y);
            createdHeart.Push(created);
        }
        print("create heart");
    }
    public static void UpdateHeart(int damage)
    {
        for(int i = 0; i < damage; i+= 10)
        {
            if (createdHeart.Count > 0) createdHeart.Pop().SetActive(false);
        }
    }

}
