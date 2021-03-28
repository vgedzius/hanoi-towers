using System.Collections.Generic;
using UnityEngine;

namespace HanoiTowers
{
    public class Peg : MonoBehaviour
    {
        Stack<Disk> disks;

        float StackHeight => disks.Count > 0 ? disks.Count * disks.Peek().Height : 0f;

        void Awake()
        {
            disks = new Stack<Disk>();
        }

        public void AddDisk(Disk disk)
        {
            Transform diskTransform = disk.transform;
            diskTransform.parent = transform;
            diskTransform.localPosition = new Vector3(0f, StackHeight, 0f);
            disk.Peg = this;
            disks.Push(disk);
        }

        public void HighlightAllDisks(bool highlight = true)
        {
            foreach (Disk disk in disks)
            {
                disk.Highlight = highlight;
            }
        }
    }
}