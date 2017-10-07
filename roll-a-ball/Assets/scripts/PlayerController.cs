using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public Text CountText;
    public Text WinText;

    private int count;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCount();
        WinText.text = "";
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 mouvment = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(mouvment*speed);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCount();
            if (count>=9)
            {
                WinText.text = "YOU WIN!!";
            }
        }

   }

    void SetCount()
    {
        CountText.text = "count:" + count.ToString();

    }
}
