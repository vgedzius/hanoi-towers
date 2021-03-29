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

        Peg highlightedPeg;
        Peg selectedPeg;
        Camera mainCamera;

        public int Moves { get; private set; } = 0;

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
            HighlightPeg();
            HandleSelection();
        }

        void HandleSelection()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            if (selectedPeg && selectedPeg != highlightedPeg)
            {
                if (highlightedPeg)
                {
                    bool added = highlightedPeg.TryAddingDisk(selectedPeg.SelectedDisk);
                    Moves++;
                    if (!added) return;
                    
                    selectedPeg.Clear();
                }

                selectedPeg.Deselect();
                selectedPeg = null;

                return;
            }

            if (highlightedPeg && selectedPeg != highlightedPeg)
            {
                selectedPeg = highlightedPeg;
                selectedPeg.Select();
            }
        }

        void HighlightPeg()
        {
            if (highlightedPeg is { })
            {
                highlightedPeg.HighlightAllDisks(false);
                highlightedPeg = null;
            }

            if (!Physics.Raycast(MouseToRay(), out RaycastHit hit)) return;

            Disk diskHit = hit.transform.GetComponent<Disk>();
            if (diskHit)
            {
                highlightedPeg = diskHit.Peg;
                highlightedPeg.HighlightAllDisks();
            }

            Peg peg = hit.transform.GetComponent<Peg>();
            if (peg)
            {
                highlightedPeg = peg;
                highlightedPeg.HighlightAllDisks();
            }
        }

        Ray MouseToRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
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