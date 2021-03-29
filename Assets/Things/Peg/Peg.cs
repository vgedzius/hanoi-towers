using System.Collections.Generic;
using UnityEngine;

namespace HanoiTowers
{
    public class Peg : MonoBehaviour
    {
        [SerializeField] float selectedHeight = 0.5f;
        [SerializeField] Material defaultMaterial;
        [SerializeField] Material highlightMaterial;
        [SerializeField] MeshRenderer meshRenderer;
        
        Stack<Disk> disks;

        public Disk SelectedDisk { get; private set; }
        public int DiskCount => disks.Count;

        float StackHeight => disks.Count > 0 ? disks.Count * disks.Peek().Height : 0f;

        void Awake()
        {
            disks = new Stack<Disk>();
        }

        void Start()
        {
            meshRenderer.material = defaultMaterial;
        }

        public void AddDisk(Disk disk)
        {
            Transform diskTransform = disk.transform;
            diskTransform.parent = transform;
            diskTransform.localPosition = new Vector3(0f, StackHeight, 0f);
            disks.Push(disk);
        }

        public void Highlight(bool highlight = true)
        {
            meshRenderer.material = highlight ? highlightMaterial : defaultMaterial;

            foreach (Disk disk in disks)
            {
                disk.Highlight = highlight;
            }

            if (SelectedDisk)
            {
                SelectedDisk.Highlight = highlight;
            }
        }

        public void Select()
        {
            SelectedDisk = disks.Pop();
            SelectedDisk.transform.localPosition += new Vector3(0f, selectedHeight, 0f);
        }

        public void Deselect()
        {
            if (!SelectedDisk) return;

            SelectedDisk.transform.localPosition -= new Vector3(0f, selectedHeight, 0f);
            disks.Push(SelectedDisk);
            SelectedDisk = null;
        }

        public void Clear()
        {
            SelectedDisk = null;
        }

        public bool TryAddingDisk(Disk disk)
        {
            if (!CanPlace(disk.Size)) return false;

            AddDisk(disk);
            return true;
        }

        public void Placeholder(Transform placeholder)
        {
            placeholder.parent = transform;
            placeholder.localPosition = new Vector3(0f, StackHeight, 0f);
        }

        public bool CanPlace(int size)
        {
            if (disks.Count == 0) return true;
            
            return size < disks.Peek().Size;
        }
    }
}