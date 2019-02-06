using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// Generate random map.
    /// </summary>
    public class RandomMapGenerator : MonoBehaviour
    {
        #region Fields
        /// <summary>seed of X axis</summary>
        float m_seedX;
        /// <summary>seed of Z axis</summary>
        float m_seedZ;

        /// <summary>Height range of the map.</summary>
        [Header("Can't be changed during execution.")]
        [SerializeField] float m_width = 50f;
        /// <summary>Depth range of the map.</summary>
        [SerializeField] float m_depth = 50f;
        /// <summary>Whether need to attach collider.</summary>
        [SerializeField] bool m_needToCollider = false;

        /// <summary>Maximum height of map.</summary>
        [Header("Can be changed during execution.")]
        [SerializeField] float m_maxHeight = 10;
        /// <summary>Whether it is a map using Perlin noise.</summary>
        [SerializeField] bool m_isPerlinNoiseMap = true;
        /// <summary>Intensity of nosie</summary>
        [SerializeField] float m_intensity = 15f;
        /// <summary>Whether to smooth the Y axis.</summary>
        [SerializeField] bool m_isSmoothness = false;
        /// <summary>mapの大きさ</summary>
        [SerializeField] float m_mapSize = 1f;
        #endregion

        #region Methods
        void Awake()
        {
            // Setting map size.
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);

            // Generate seed so that it does not become the same map.
            m_seedX = Random.value * 100f;
            m_seedZ = Random.value * 100f;

            // Generate Cube object.
            for (int x = 0; x < m_width; x++)
            {
                for (int z = 0; z < m_depth; z++)
                {
                    // Create a new cube, put it on the flat.
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localPosition = new Vector3(x, 0, z);
                    cube.transform.SetParent(transform);

                    // If you don't need a collider, destroy it.
                    if (!m_needToCollider)
                    {
                        Destroy(cube.GetComponent<BoxCollider>());
                    }

                    // Setting Height.
                    SetYaxis(cube);
                }
            }
        }

        void OnValidate()
        {
            // If the application is playing Processing end.
            if (!Application.isPlaying)
            {
                return;
            }

            // Set the size of the map. 
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);

            // Change the Y coordinate of each cube.
            foreach (Transform child in transform)
            {
                SetYaxis(child.gameObject);
            }
        }

        /// <summary>
        /// Set the Y coordinate of cube.
        /// </summary>
        /// <param name="cube"></param>
        void SetYaxis(GameObject cube)
        {
            float yAxis = 0f;

            // When using Perlin noise to determine the height.
            if (m_isPerlinNoiseMap)
            {
                float xSample = (cube.transform.localPosition.x + m_seedX) / m_intensity;
                float zSample = (cube.transform.localPosition.z + m_seedZ) / m_intensity;

                float noise = Mathf.PerlinNoise(xSample, zSample);

                yAxis = m_maxHeight * noise;
            }
            else // When deciding the height at completely random.
            {
                yAxis = Random.Range(0, m_maxHeight);
            }

            // If it does not change smoothly, "y" is rouded of.
            if (!m_isSmoothness)
            {
                yAxis = Mathf.Round(yAxis);
            }

            // Set the position.
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, yAxis, cube.transform.localPosition.z);

            // Change color gradually according to height.
            Color color = Color.black;

            if (yAxis > m_maxHeight * 0.3f)
            {
                ColorUtility.TryParseHtmlString("#019540FF", out color); // Color that looks grass.
            }
            else if (yAxis > m_maxHeight * 0.2f)
            {
                ColorUtility.TryParseHtmlString("#2432ADFF", out color); // Color that looks water.
            }
            else if (yAxis > m_maxHeight * 0.1f)
            {
                ColorUtility.TryParseHtmlString("#D4500EFF", out color); // Color that looks lava.
            }

            cube.GetComponent<MeshRenderer>().material.color = color;
        }
        #endregion
    }
}
