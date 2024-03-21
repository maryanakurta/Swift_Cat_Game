using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] section;
    public float sectionLength = 48f;
    public int initialSectionsCount = 10;
    private List<GameObject> sections = new List<GameObject>();
    public Transform playerTransform;

    [SerializeField]
    private GameObject initialSectionPrefab;
    private GameObject initialSectionInstance;

    [SerializeField]
    private float sectionSpeed = 5f;

    [SerializeField]
    private int minSectionsAhead = 20;

    [SerializeField]
    private float sectionCreationInterval = 0.5f;

    [SerializeField]
    private int maxSections = 20;

    private bool canCreateSections = true;

    void Start()
    {
        for (int i = 0; i < initialSectionsCount; i++)
        {
            CreateSection();
        }

        if (initialSectionPrefab != null)
        {
            initialSectionInstance = Instantiate(initialSectionPrefab, Vector3.zero, Quaternion.identity);
        }

        StartCoroutine(CreateSectionPeriodically());
    }

    void Update()
    {
        MoveInitialSection();

        foreach (var section in sections)
        {
            if (section != null)
            {
                section.transform.Translate(Vector3.back * Time.deltaTime * sectionSpeed);
            }
        }

        if (sections.Count > 0 && sections[0].transform.position.z < playerTransform.position.z - sectionLength)
        {
            Destroy(sections[0]);
            sections.RemoveAt(0);
            CreateSection();
        }
    }

    IEnumerator CreateSectionPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(sectionCreationInterval);

            if (canCreateSections && sections.Count < maxSections)
            {
                CreateSection();
            }
        }
    }

    void CreateSection()
    {
        int secNum = Random.Range(0, section.Length);
        float lastSectionZPos = GetLastSectionPosition();
        float newZPos = lastSectionZPos + sectionLength;

        GameObject newSection = Instantiate(section[secNum], new Vector3(0, 0, newZPos), Quaternion.identity);
        sections.Add(newSection);
    }

    void MoveInitialSection()
    {
        if (initialSectionInstance != null)
        {
            initialSectionInstance.transform.Translate(Vector3.back * Time.deltaTime * sectionSpeed);

            if (initialSectionInstance.transform.position.z < playerTransform.position.z - sectionLength)
            {
                Destroy(initialSectionInstance);
                initialSectionInstance = null;
            }
        }
    }

    float GetLastSectionPosition()
    {
        if (sections.Count > 0)
        {
            return sections[sections.Count - 1].transform.position.z;
        }
        else
        {
            return transform.position.z;
        }
    }
}
