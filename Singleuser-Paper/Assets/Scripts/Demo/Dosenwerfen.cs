using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dosenwerfen : MonoBehaviour
{

    public int publicScore = 0;
    private int score = 0;
    public TextMeshPro text;
    public GameObject prefabKugel;
    public GameObject prefabDosen;
    public GameObject spawnPosition;
    public GameObject spawnDosen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();

        if(score == 10)
        {
            score = 0;
            Instantiate(prefabDosen, spawnDosen.transform.position, spawnDosen.transform.rotation, spawnDosen.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dose")
        {
            score += 1;
            publicScore += 1;
            Destroy(other.gameObject);
        }
        else if(other.tag == "Kugel")
        {
            Destroy(other.gameObject);
            // Instantiate(prefabKugel, spawnPosition.transform.position, spawnPosition.transform.rotation);
        }
    }

    private void UpdateText()
    {
        text.text = "Score\n" + publicScore;
    }

    public void ResetDosenwerfen()
    {
        score = 0;
        publicScore = 0;
        var dosen = GameObject.FindGameObjectsWithTag("Dose");
        foreach(GameObject dose in dosen)
        {
            Destroy(dose);
        }
        Instantiate(prefabDosen, spawnDosen.transform.position, spawnDosen.transform.rotation, spawnDosen.transform);
        // Instantiate(prefabKugel, spawnPosition.transform.position, spawnPosition.transform.rotation, spawnPosition.transform);
    }
}
