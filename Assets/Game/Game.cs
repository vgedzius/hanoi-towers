using System.Collections;
using UnityEngine;

namespace HanoiTowers
{
    public class Game : MonoBehaviour
    {
        [SerializeField] int disks = 10;
        [SerializeField] float spawnInterval = 0.5f;
        [SerializeField] float spawnInitialDelay = 1f;
        [SerializeField] Disk diskPrefab;
        [SerializeField] Peg[] pegs;
        [SerializeField] Peg startingPeg;

        Disk highlightedDisk;

        Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void Start()
        {
            StartCoroutine(SpawnDisks());
        }

        void Update()
        {
            if (highlightedDisk != null)
            {
                highlightedDisk.Peg.HighlightAllDisks(false);
                highlightedDisk = null;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Disk diskHit = hit.transform.GetComponent<Disk>();
                if (diskHit)
                {
                    highlightedDisk = diskHit;
                    highlightedDisk.Peg.HighlightAllDisks();
                }
            }
        }

        IEnumerator SpawnDisks()
        {
            yield return new WaitForSeconds(spawnInitialDelay);

            for (int i = disks; i > 0; i--)
            {
                Disk disk = Instantiate(diskPrefab);
                disk.MaxDisks = disks;
                disk.Size = i;

                startingPeg.AddDisk(disk);

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}