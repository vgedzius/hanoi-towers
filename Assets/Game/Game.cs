using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanoiTowers
{
    public class Game : MonoBehaviour
    {
        [SerializeField, Range(3, 10)] int numberOfPegs = 3;
        [SerializeField, Range(1, 30)] int numberOfDisks = 10;
        [SerializeField] float spawnInterval = 0.5f;
        [SerializeField] float spawnInitialDelay = 1f;
        [SerializeField] Disk diskPrefab;
        [SerializeField] Disk placeholderPrefab;
        [SerializeField] Peg startingPeg;
        [SerializeField] Peg endPeg;
        [SerializeField] GameUI ui;

        Peg highlightedPeg;
        Peg selectedPeg;
        Camera mainCamera;
        Disk placeholder;
        int optimalMoves;

        public int Moves { get; private set; } = 0;
        public bool SelectionEnabled { get; set; } = false;
        public int NumberOfDisks => numberOfDisks;
        public static Game Instance { get; private set; }

        void Awake()
        {
            mainCamera = Camera.main;
            Instance = this;
        }

        void Start()
        {
            StartCoroutine(SpawnDisks());
            ui.HideVictoryPanel();

            placeholder = Instantiate(placeholderPrefab);
            placeholder.Visible = false;

            optimalMoves = FrameStewart(numberOfDisks, numberOfPegs);
            endPeg.ShowArrow();
        }

        void Update()
        {
            HighlightPeg();
            HandleSelection();
            CheckForVictory();
        }

        public void Quit()
        {
            Application.Quit();
        }

        void CheckForVictory()
        {
            if (endPeg.DiskCount != numberOfDisks) return;

            ui.ShowVictoryPanel(Moves == optimalMoves, Moves);
            SelectionEnabled = false;
        }

        static int FrameStewart(int n, int r)
        {
            if (n == 0) return 0;

            if (n == 1 && r > 1) return 1;

            if (r == 3) return (int) Math.Pow(2, n) - 1;

            if (r > 3 && n > 0)
            {
                List<int> possibleSolutions = new List<int>();
                for (int k = 1; k < n; k++)
                {
                    possibleSolutions.Add(
                        2 * FrameStewart(k, r) + FrameStewart(n - k, r - 1)
                    );
                }

                return Min(possibleSolutions);
            }

            return int.MaxValue;
        }

        static int Min(List<int> possibleSolutions)
        {
            int min = possibleSolutions[0];

            foreach (int value in possibleSolutions)
            {
                if (value < min)
                {
                    min = value;
                }
            }

            return min;
        }

        void HandleSelection()
        {
            if (!SelectionEnabled || !Input.GetMouseButtonUp(0)) return;

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
            if (highlightedPeg)
            {
                highlightedPeg.Highlight(false);
                highlightedPeg = null;
                placeholder.Visible = false;
            }

            if (!SelectionEnabled || !Physics.Raycast(MouseToRay(), out RaycastHit hit)) return;

            Peg peg = hit.transform.GetComponent<Peg>();
            if (!peg || (peg.DiskCount < 1 && !selectedPeg)) return;

            highlightedPeg = peg;
            highlightedPeg.Highlight();

            if (
                !selectedPeg ||
                highlightedPeg == selectedPeg ||
                !highlightedPeg.CanPlace(selectedPeg.SelectedDisk)
            ) return;

            placeholder.Size = selectedPeg.SelectedDisk.Size;
            placeholder.Visible = true;

            highlightedPeg.Placeholder(placeholder.transform);
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
                disk.Size = i;
                disk.Visible = true;

                startingPeg.AddDisk(disk);

                yield return new WaitForSeconds(spawnInterval);
            }

            SelectionEnabled = true;
            yield return null;
        }
    }
}