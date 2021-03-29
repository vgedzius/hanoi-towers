using System;
using System.Collections;
using UnityEngine;

namespace HanoiTowers
{
    public class Game : MonoBehaviour
    {
        [SerializeField] int numberOfPegs = 3;
        [SerializeField] int numberOfDisks = 10;
        [SerializeField] float spawnInterval = 0.5f;
        [SerializeField] float spawnInitialDelay = 1f;
        [SerializeField] Disk diskPrefab;
        [SerializeField] Peg startingPeg;
        [SerializeField] Peg endPeg;
        [SerializeField] GameUI ui;

        Peg highlightedPeg;
        Peg selectedPeg;
        Camera mainCamera;
        bool selectionEnabled = false;

        public int Moves { get; private set; } = 0;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void Start()
        {
            StartCoroutine(SpawnDisks());
            ui.HideLosePanel();
            ui.HideVictoryPanel();
        }

        void Update()
        {
            HighlightPeg();
            HandleSelection();
            CheckForVictory();
        }

        void CheckForVictory()
        {
            if (endPeg.DiskCount == numberOfDisks)
            {
                ui.ShowVictoryPanel();
                selectionEnabled = false;
            }

            if (endPeg.DiskCount < numberOfDisks && Moves == OptimalNumberOfMoves(numberOfPegs, numberOfDisks))
            {
                ui.ShowLosePanel();
                selectionEnabled = false;
            }
        }

        int OptimalNumberOfMoves(int p, int r)
        {
            if (p != 3) Debug.LogError("Number of pegs other than 3 currently not supported");

            return (int)Math.Pow(2, r) - 1;
        }

        void HandleSelection()
        {
            if (!selectionEnabled || !Input.GetMouseButtonUp(0)) return;

            if (selectedPeg && selectedPeg != highlightedPeg)
            {
                if (highlightedPeg)
                {
                    bool added = highlightedPeg.TryAddingDisk(selectedPeg.SelectedDisk);
                    if (!added) return;

                    Moves++;
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
                return;
            }

            if (highlightedPeg && highlightedPeg == selectedPeg)
            {
                selectedPeg.Deselect();
                selectedPeg = null;
            }
        }

        void HighlightPeg()
        {
            if (highlightedPeg is { })
            {
                highlightedPeg.HighlightAllDisks(false);
                highlightedPeg = null;
            }

            if (!selectionEnabled || !Physics.Raycast(MouseToRay(), out RaycastHit hit)) return;

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

            for (int i = numberOfDisks; i > 0; i--)
            {
                Disk disk = Instantiate(diskPrefab);
                disk.MaxDisks = numberOfDisks;
                disk.Size = i;

                startingPeg.AddDisk(disk);

                yield return new WaitForSeconds(spawnInterval);
            }

            selectionEnabled = true;
            yield return null;
        }
    }
}