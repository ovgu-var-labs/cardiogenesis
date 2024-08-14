using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public List<Sprite> logos;
    public TiltFive.PlayerIndex playerIndex;
    public bool looping;


    private SpriteRenderer spriteRenderer;
    public int numberLogo = 0;
    private float elapsedTime = 0;
    private float bootUpTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = logos[numberLogo];
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedTime > 5f && elapsedTime > bootUpTime)
        {
            bootUpTime = 0f;
            numberLogo += 1;
            elapsedTime = 0;
            if (numberLogo < logos.Count || looping)
            {
                // Switch to next logo
                spriteRenderer.sprite = logos[numberLogo % logos.Count];
            }
            else
            {
                // Load Next Scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }


        elapsedTime += Time.deltaTime;
    }



    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            return TiltFive.PlayerIndex.One;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Two"))
        {
            return TiltFive.PlayerIndex.Two;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Three"))
        {
            return TiltFive.PlayerIndex.Three;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Four"))
        {
            return TiltFive.PlayerIndex.Four;
        }
        else
        {
            return TiltFive.PlayerIndex.One;
        }
    }
}
