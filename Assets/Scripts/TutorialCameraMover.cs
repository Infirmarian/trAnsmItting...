using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialCameraMover : MonoBehaviour {

    public AudioSource speech, tilt, weapon, enemy, firewall, begin;
    public Text tiltDescription, tapDescription;
    public GameObject background;
    public GameObject player;
    private Rigidbody2D rb;

    private PlayerController pcontroller;
    bool speechIsDone = false;
    bool alltalk = false;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
        pcontroller = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody2D>();
        tiltDescription.enabled = false;
        tapDescription.enabled = false;
        StartCoroutine(runTutorial());
	}

    private void LateUpdate()
    {
        if (speechIsDone)
            MoveUp(Time.deltaTime);
        else
            rb.velocity = Vector2.zero;

        if (alltalk)
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .5f);
    }

    IEnumerator runTutorial(){
        Debug.Log("Starting tutorial");
        speech.Play();
        yield return new WaitForSeconds(31);
        speechIsDone = true;
        tilt.Play();
        yield return new WaitForSeconds(3);
        tiltDescription.enabled = true;
        yield return new WaitForSeconds(6);
        tiltDescription.enabled = false;
        weapon.Play();
        yield return new WaitForSeconds(5);
        tapDescription.enabled = true;
        yield return new WaitForSeconds(9);
        tapDescription.enabled = false;

        enemy.Play();
        yield return new WaitForSeconds(8);

        firewall.Play();
        yield return new WaitForSeconds(8);

        begin.Play();
        yield return new WaitForSeconds(1);
        alltalk = true;

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("main");






    }

    void MoveUp(float dist){
        background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y - dist);
    }

    public void Skip(){
        SceneManager.LoadScene("main");
    }

}
