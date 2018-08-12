using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixHarvestBeam : MonoBehaviour {

    [SerializeField] private GameObject repareBeamPrefab;
    [SerializeField] private List<Transform> sockets;

    [SerializeField] private float distance = 200;
    [SerializeField] private float duration = 5;

    [Header("UI")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Text MessageText;
    [SerializeField] private Slider ProgressBar;

    private float currentDistance;
    private float processTime = 0;
    private bool isInprocess = false;

    private LineRenderer lineRender;
    RaycastHit raycastHit;
    Coroutine fixHarvestProcess;
    GameObject beam;

    // Use this for initialization
    void Start () {
        panel = GameObject.Find("FixHarvestPanel").GetComponent<RectTransform>();
        MessageText = GameObject.Find("MessageText").GetComponent<Text>();
        ProgressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
        panel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isInprocess)
        {
            currentDistance = Vector3.Distance(sockets[0].position, raycastHit.point);
            if (currentDistance > distance)
            {
                Interrupt();
            }

            panel.gameObject.SetActive(true);
            ProgressBar.gameObject.SetActive(true);
            processTime += Time.deltaTime;
            var percent = processTime / duration;
            ProgressBar.value = percent;
            MessageText.text = "Harvesting";
            MessageText.color = Color.green;
        }
    }

    public void Fire()
    {
        if (!isInprocess)
        {
            fixHarvestProcess = StartCoroutine(FixHarvestProcess());
        }
    }

    IEnumerator FixHarvestProcess()
    {
        if (Physics.Raycast(sockets[0].position, sockets[0].forward, out raycastHit, distance) && raycastHit.rigidbody.tag == "Asteroid")
        {
            processTime = 0;
            isInprocess = true;
            beam = Instantiate(repareBeamPrefab, sockets[0].position, sockets[0].rotation);
            lineRender = beam.GetComponent<LineRenderer>();
            FixLaser laser = beam.GetComponent<FixLaser>();
            laser.startPosition = sockets[0];
            laser.targetPosition = raycastHit.point;

            yield return new WaitForSeconds(duration);
            var resource = raycastHit.rigidbody.GetComponentInParent<Resource>();

            if (resource != null)
            {
               var givenRes = resource.GiveResource();
                isInprocess = false;
                Destroy(beam);
                raycastHit = new RaycastHit();
                MessageText.text = string.Format("+{0} {1}", givenRes.ammount, givenRes.resource);
                MessageText.color = Color.green;
            }
            yield return new WaitForSeconds(2);
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(true);
            ProgressBar.gameObject.SetActive(false);
            MessageText.text = "No target";
            MessageText.color = Color.red;
            yield return new WaitForSeconds(2);
            panel.gameObject.SetActive(false);
        }
    }

    public void Interrupt()
    {
        if (fixHarvestProcess != null)
        {
            StopCoroutine(fixHarvestProcess);
            isInprocess = false;
            fixHarvestProcess = null;
            Destroy(beam);
            MessageText.text = "Interrupted";
            MessageText.color = Color.red;
            ProgressBar.gameObject.SetActive(false);
        }
    }
}
