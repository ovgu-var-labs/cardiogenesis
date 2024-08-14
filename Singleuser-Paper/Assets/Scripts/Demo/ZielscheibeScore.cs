using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZielscheibeScore : MonoBehaviour
{

    public ZielscheibeScore mainScript;
    public int value;

    public int mainScore;
    public GameObject prefabKugel;
    public GameObject positionKugel;
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kugel")
        {
            mainScript.UpdateScoreText(value);
            Destroy(other.gameObject);
            // Instantiate(prefabKugel, positionKugel.transform.position, positionKugel.transform.rotation);
        }
    }

    public void UpdateScoreText(int valueToAdd)
    {
        mainScore += valueToAdd;
        text.text = "Score\n" + mainScore;
    }

    public void ResetScore()
    {
        UpdateScoreText(-mainScore);
    }
}
