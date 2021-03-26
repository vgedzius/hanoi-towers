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

        void Start()
        {
            StartCoroutine(SpawnDisks());
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