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
                    diskMesh.ScheduleBuild(size, Game.Instance.NumberOfDisks);
                }
            }
        }

        public bool Visible
        {
            set => meshRenderer.enabled = value;
        }

        public bool Highlight
        {
            set => meshRenderer.material = value ? highlightMaterial : defaultMaterial;
        }

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            diskMesh = GetComponent<DiskMesh>();
        }

        void Start()
        {
            diskMesh.ScheduleBuild(size, Game.Instance.NumberOfDisks);
            meshRenderer.material = defaultMaterial;
        }
    }
}