using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableStreamer : MonoBehaviour
{
    [System.Serializable]
    public class ChunkInfo
    {
        public string addressableKey;
        public Vector3 worldPosition;


        [HideInInspector] public GameObject spawnedInstance;
        [HideInInspector] public bool isLoading = false;
    }

    [Header("Configuración")]
    public Transform playerTransform;
    public float loadDistance = 100f;
    public List<ChunkInfo> chunks = new List<ChunkInfo>();

    void Update()
    {
        if (playerTransform == null) return;

        foreach (var chunk in chunks)
        {
            float distance = Vector3.Distance(playerTransform.position, chunk.worldPosition);

            // Si el jugador está dentro del rango
            if (distance <= loadDistance)
            {
                // Si no está cargado ni cargándose, lo cargamos
                if (chunk.spawnedInstance == null && !chunk.isLoading)
                {
                    LoadChunk(chunk);
                }
            }
            // Si el jugador se alejó del rango
            else
            {
                // Si ya está cargado, lo descargamos
                if (chunk.spawnedInstance != null)
                {
                    UnloadChunk(chunk);
                }
            }
        }
    }

    void LoadChunk(ChunkInfo chunk)
    {
        chunk.isLoading = true;

        // Carga asíncrona del Addressable
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(chunk.addressableKey, chunk.worldPosition, Quaternion.identity);

        handle.Completed += operation =>
        {
            chunk.isLoading = false;
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                chunk.spawnedInstance = operation.Result;
            }
        };
    }

    void UnloadChunk(ChunkInfo chunk)
    {
        if (chunk.spawnedInstance != null)
        {
            // Libera la instancia del Addressable de la memoria
            Addressables.ReleaseInstance(chunk.spawnedInstance);
            chunk.spawnedInstance = null;
        }
    }
}