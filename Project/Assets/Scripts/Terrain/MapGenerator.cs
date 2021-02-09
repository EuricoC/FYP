using System;
using System.Collections.Generic;
using System.Threading;
using Terrain.TerrainData;
using UnityEngine;

namespace Terrain
{
    public class MapGenerator : MonoBehaviour
    {

        public TerrainData.TerrainData terrainData;
        public NoiseData noiseData;
        public TextureData textureData;
    
        public Material terrainMaterial;

        private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQ = new Queue<MapThreadInfo<MapData>>();
        private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQ = new Queue<MapThreadInfo<MeshData>>();

        public int mapChunkSize = 239;

        
        void Start() 
        {
            textureData.ApplyToMaterial (terrainMaterial);
        }
        
        public void RequestMapData(Vector2 centre, Action<MapData> callBack)
        {
            textureData.UpdateMeshHeights (terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
            
            ThreadStart threadStart = delegate
            {
                MapDataThread(centre, callBack);
            };
        
            new Thread(threadStart).Start();
        }

        void MapDataThread(Vector2 centre, Action<MapData> callBack)
        {
            MapData mapData = GenerateMapData(centre);
            lock (mapDataThreadInfoQ)
            {
                mapDataThreadInfoQ.Enqueue(new MapThreadInfo<MapData>(callBack, mapData));
            }
        }
    
        public void RequestMeshData(MapData mapData,int lod, Action<MeshData> callback)
        {
            ThreadStart threadStart = delegate
            {
                MeshDataThread(mapData, lod, callback);
            };
        
            new Thread(threadStart).Start();
        }
    
        void MeshDataThread(MapData mapData, int lod, Action<MeshData> callBack)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod);
            lock (meshDataThreadInfoQ)
            {
                meshDataThreadInfoQ.Enqueue(new MapThreadInfo<MeshData>(callBack, meshData));
            }
        }

        private void Update()
        {
            if(mapDataThreadInfoQ.Count > 0)
            {
                for (int i = 0; i < mapDataThreadInfoQ.Count; i++)
                {
                    MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQ.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
        
            if(meshDataThreadInfoQ.Count > 0)
            {
                for (int i = 0; i < meshDataThreadInfoQ.Count; i++)
                {
                    MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQ.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
        }

        MapData GenerateMapData(Vector2 centre)
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(noiseData.seed, mapChunkSize + 2, mapChunkSize + 2, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity,centre + noiseData.offset, noiseData.normalizeMode);

            return new MapData(noiseMap);
        }

        struct MapThreadInfo<T>
        {
            public readonly Action<T> callback;
            public readonly T parameter;

            public MapThreadInfo(Action<T> callback, T parameter)
            {
                this.callback = callback;
                this.parameter = parameter;
            }
        }
    }

    public struct MapData
    {
        public readonly float[,] heightMap;

        public MapData(float[,] heightMap)
        {
            this.heightMap = heightMap;
        }
    }
}