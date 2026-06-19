using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace _Project.Scripts
{
    [BurstCompile]
    public struct FieldKernelPixelJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> kernel;
        [ReadOnly] public NativeArray<float2> points;

        [WriteOnly] public NativeArray<float3> field;

        public int fieldWidth;
        public int fieldHeight;

        public int kernelWidth;
        public int kernelHeight;

        public float2 fieldMin;
        public float2 fieldMax;

        public float2 kernelMin;
        public float2 kernelMax;

        public void Execute(int i)
        {
            int x = i % fieldWidth;
            int y = i / fieldWidth;

            float2 worldPos = FieldIndexToWorld(x, y);

            float3 sum = 0;

            for (int p = 0; p < points.Length; p++)
            {
                float2 kernelPos = worldPos - points[p];

                if (kernelPos.x < kernelMin.x || kernelPos.x > kernelMax.x ||
                    kernelPos.y < kernelMin.y || kernelPos.y > kernelMax.y)
                    continue;

                sum += SampleKernel(kernelPos);
            }

            field[i] = sum;
        }

        private float2 FieldIndexToWorld(int x, int y)
        {
            float tx = (float)x / (fieldWidth - 1);
            float ty = (float)y / (fieldHeight - 1);

            return math.lerp(fieldMin, fieldMax, new float2(tx, ty));
        }

        private float3 SampleKernel(float2 pos)
        {
            float tx = math.unlerp(kernelMin.x, kernelMax.x, pos.x) * (kernelWidth - 1);
            float ty = math.unlerp(kernelMin.y, kernelMax.y, pos.y) * (kernelHeight - 1);

            tx = math.clamp(tx, 0f, kernelWidth - 1);
            ty = math.clamp(ty, 0f, kernelHeight - 1);

            int x0 = (int)math.floor(tx);
            int y0 = (int)math.floor(ty);

            int x1 = math.min(x0 + 1, kernelWidth - 1);
            int y1 = math.min(y0 + 1, kernelHeight - 1);

            float fx = tx - x0;
            float fy = ty - y0;

            float3 v00 = kernel[y0 * kernelWidth + x0];
            float3 v10 = kernel[y0 * kernelWidth + x1];
            float3 v01 = kernel[y1 * kernelWidth + x0];
            float3 v11 = kernel[y1 * kernelWidth + x1];

            return math.lerp(
                math.lerp(v00, v10, fx),
                math.lerp(v01, v11, fx),
                fy
            );
        }
    }
}