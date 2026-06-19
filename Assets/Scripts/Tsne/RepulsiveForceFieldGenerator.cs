using Models;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Tsne
{
    /**
     * Uses FieldKernelPixelJob to compute the repulsive field.
     */
    public static class RepulsiveForceFieldGenerator
    {
        public static Field RepulsiveForceField()
        {
            Settings settings = Manager.Settings;

            Field f = new(-settings.s_fieldSize, settings.s_fieldSize, settings.FieldResolution);
            Field kernel = FieldKernelGenerator.Generate(settings.KernelSize, settings.KernelResolution);

            using var fieldArray = new NativeArray<float3>(
                f.Shape.x * f.Shape.y,
                Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory
            );

            using var kernelArray = ToNativeArray(kernel, Allocator.TempJob);
            using var pointArray = PointArray(Allocator.TempJob);

            var job = new FieldKernelPixelJob
            {
                field = fieldArray,
                kernel = kernelArray,
                points = pointArray,

                fieldWidth = f.Shape.x,
                fieldHeight = f.Shape.y,

                kernelWidth = kernel.Shape.x,
                kernelHeight = kernel.Shape.y,

                fieldMin = f.min,
                fieldMax = f.max,

                kernelMin = kernel.min,
                kernelMax = kernel.max
            };

            JobHandle handle = job.Schedule(f.Shape.x * f.Shape.y, 64);
            handle.Complete();

            CopyBack(fieldArray, f);

            return f;
        }
        
        private static NativeArray<float3> ToNativeArray(Field field, Allocator allocator)
        {
            var array = new NativeArray<float3>(
                field.Value.Length,
                allocator,
                NativeArrayOptions.UninitializedMemory
            );

            for (int i = 0; i < field.Value.Length; i++)
            {
                Vector3 v = field.Value[i];
                array[i] = new float3(v.x, v.y, v.z);
            }

            return array;
        }

        private static NativeArray<float2> PointArray(Allocator allocator)
        {
            Settings settings = Manager.Settings;
            
            var array = new NativeArray<float2>(settings.N, allocator, NativeArrayOptions.UninitializedMemory);

            for (var i = 0; i < settings._points.Count; i++)
            {
                Vector3 pos = settings._points[i].transform.position;
                array[i] = new float2(pos.x, pos.z);
            }
            
            return array;
        }

        private static void CopyBack(NativeArray<float3> source, Field target)
        {
            for (int i = 0; i < source.Length; i++)
            {
                float3 v = source[i];
                target.Value[i] = new Vector3(v.x, v.y, v.z);
            }
        }
    }
}