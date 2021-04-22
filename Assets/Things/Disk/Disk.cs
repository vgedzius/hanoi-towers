using UnityEngine;

namespace HanoiTowers
{
    [RequireComponent(typeof(MeshRenderer), typeof(DiskMesh))]
    public class Disk : MonoBehaviour
    {
        [SerializeField] Material defaultMaterial;
        [SerializeField] Material highlightMaterial;
        
        MeshRenderer meshRenderer;
        DiskMesh diskMesh;
        bool highlight;
        bool visible;
        int size = 1;

        public float Height => diskMesh.Height;

        public int Size
        {
            get => size;
            set
            {
                int oldSize = size;
                size = value;

                if (oldSize != size)
                {
                    diskMesh.Build(size);
                }
            }
        }

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                meshRenderer.enabled = value;
            }
        }

        public bool Highlight
        {
            get => highlight;
            set
            {
                highlight = value;
                meshRenderer.material = value ? highlightMaterial : defaultMaterial;
            }
        }

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            diskMesh = GetComponent<DiskMesh>();
        }

        void Start()
        {
            diskMesh.Build(size);
            meshRenderer.material = defaultMaterial;
        }
    }
}