using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{

    /// <summary>
    /// �ﰢ�Լ� ������ ��ü�ϴ� Ŭ����
    /// </summary>
    public class Trigonometrics : MonoBehaviour
    {
        public const float PI = 3.1415927f;

        /// <summary>
        /// sin ���̺�
        /// </summary>
        static float[] sinLookUpTable = new float[360]
        {
        0f,
        0.0174f,
        0.0348f,
        0.0523f,
        0.0697f,
        0.0871f,
        0.1045f,
        0.1218f,
        0.1391f,
        0.1564f,
        0.1736f,
        0.1908f,
        0.2079f,
        0.2249f,
        0.2419f,
        0.2588f,
        0.2756f,
        0.2923f,
        0.309f,
        0.3255f,
        0.342f,
        0.3583f,
        0.3746f,
        0.3907f,
        0.4067f,
        0.4226f,
        0.4383f,
        0.4539f,
        0.4694f,
        0.4848f,
        0.5f,
        0.515f,
        0.5299f,
        0.5446f,
        0.5591f,
        0.5735f,
        0.5877f,
        0.6018f,
        0.6156f,
        0.6293f,
        0.6427f,
        0.656f,
        0.6691f,
        0.6819f,
        0.6946f,
        0.7071f,
        0.7193f,
        0.7313f,
        0.7431f,
        0.7547f,
        0.766f,
        0.7771f,
        0.788f,
        0.7986f,
        0.809f,
        0.8191f,
        0.829f,
        0.8386f,
        0.848f,
        0.8571f,
        0.866f,
        0.8746f,
        0.8829f,
        0.891f,
        0.8987f,
        0.9062f,
        0.9135f,
        0.9205f,
        0.9271f,
        0.9335f,
        0.9396f,
        0.9455f,
        0.951f,
        0.9563f,
        0.9612f,
        0.9659f,
        0.9702f,
        0.9743f,
        0.9781f,
        0.9816f,
        0.9848f,
        0.9876f,
        0.9902f,
        0.9924f,
        0.9945f,
        0.9960f,
        0.9975f,
        0.9985f,
        0.9993f,
        0.9998f,
        1f,
        0.9998f,
        0.9993f,
        0.9985f,
        0.9975f,
        0.9960f,
        0.9945f,
        0.9924f,
        0.9902f,
        0.9876f,
        0.9848f,
        0.9816f,
        0.9781f,
        0.9743f,
        0.9702f,
        0.9659f,
        0.9612f,
        0.9563f,
        0.951f,
        0.9455f,
        0.9396f,
        0.9335f,
        0.9271f,
        0.9205f,
        0.9135f,
        0.9062f,
        0.8987f,
        0.891f,
        0.8829f,
        0.8746f,
        0.866f,
        0.8571f,
        0.848f,
        0.8386f,
        0.829f,
        0.8191f,
        0.809f,
        0.7986f,
        0.788f,
        0.7771f,
        0.766f,
        0.7547f,
        0.7431f,
        0.7313f,
        0.7193f,
        0.7071f,
        0.6946f,
        0.6819f,
        0.6691f,
        0.656f,
        0.6427f,
        0.6293f,
        0.6156f,
        0.6018f,
        0.5877f,
        0.5735f,
        0.5591f,
        0.5446f,
        0.5299f,
        0.515f,
        0.5f,
        0.4848f,
        0.4694f,
        0.4539f,
        0.4383f,
        0.4226f,
        0.4067f,
        0.3907f,
        0.3746f,
        0.3583f,
        0.342f,
        0.3255f,
        0.309f,
        0.2923f,
        0.2756f,
        0.2588f,
        0.2419f,
        0.2249f,
        0.2079f,
        0.1908f,
        0.1736f,
        0.1564f,
        0.1391f,
        0.1218f,
        0.1045f,
        0.0871f,
        0.0697f,
        0.0523f,
        0.0348f,
        0.0174f,
        0f,
        -0.0174f,
        -0.0348f,
        -0.0523f,
        -0.0697f,
        -0.0871f,
        -0.1045f,
        -0.1218f,
        -0.1391f,
        -0.1564f,
        -0.1736f,
        -0.1908f,
        -0.2079f,
        -0.2249f,
        -0.2419f,
        -0.2588f,
        -0.2756f,
        -0.2923f,
        -0.309f,
        -0.3255f,
        -0.342f,
        -0.3583f,
        -0.3746f,
        -0.3907f,
        -0.4067f,
        -0.4226f,
        -0.4383f,
        -0.4539f,
        -0.4694f,
        -0.4848f,
        -0.4999f,
        -0.515f,
        -0.5299f,
        -0.5446f,
        -0.5591f,
        -0.5735f,
        -0.5877f,
        -0.6018f,
        -0.6156f,
        -0.6293f,
        -0.6427f,
        -0.656f,
        -0.6691f,
        -0.6819f,
        -0.6946f,
        -0.7071f,
        -0.7193f,
        -0.7313f,
        -0.7431f,
        -0.7547f,
        -0.766f,
        -0.7771f,
        -0.788f,
        -0.7986f,
        -0.809f,
        -0.8191f,
        -0.829f,
        -0.8386f,
        -0.848f,
        -0.8571f,
        -0.866f,
        -0.8746f,
        -0.8829f,
        -0.891f,
        -0.8987f,
        -0.9062f,
        -0.9135f,
        -0.9205f,
        -0.9271f,
        -0.9335f,
        -0.9396f,
        -0.9455f,
        -0.951f,
        -0.9563f,
        -0.9612f,
        -0.9659f,
        -0.9702f,
        -0.9743f,
        -0.9781f,
        -0.9816f,
        -0.9848f,
        -0.9876f,
        -0.9902f,
        -0.9924f,
        -0.9945f,
        -0.9960f,
        -0.9975f,
        -0.9985f,
        -0.9993f,
        -0.9998f,
        -1f,
        -0.9998f,
        -0.9993f,
        -0.9985f,
        -0.9975f,
        -0.9960f,
        -0.9945f,
        -0.9924f,
        -0.9902f,
        -0.9876f,
        -0.9848f,
        -0.9816f,
        -0.9781f,
        -0.9743f,
        -0.9702f,
        -0.9659f,
        -0.9612f,
        -0.9563f,
        -0.951f,
        -0.9455f,
        -0.9396f,
        -0.9335f,
        -0.9271f,
        -0.9205f,
        -0.9135f,
        -0.9062f,
        -0.8987f,
        -0.891f,
        -0.8829f,
        -0.8746f,
        -0.866f,
        -0.8571f,
        -0.848f,
        -0.8386f,
        -0.829f,
        -0.8191f,
        -0.809f,
        -0.7986f,
        -0.788f,
        -0.7771f,
        -0.766f,
        -0.7547f,
        -0.7431f,
        -0.7313f,
        -0.7193f,
        -0.7071f,
        -0.6946f,
        -0.6819f,
        -0.6691f,
        -0.656f,
        -0.6427f,
        -0.6293f,
        -0.6156f,
        -0.6018f,
        -0.5877f,
        -0.5735f,
        -0.5591f,
        -0.5446f,
        -0.5299f,
        -0.515f,
        -0.5f,
        -0.4848f,
        -0.4694f,
        -0.4539f,
        -0.4383f,
        -0.4226f,
        -0.4067f,
        -0.3907f,
        -0.3746f,
        -0.3583f,
        -0.342f,
        -0.3255f,
        -0.309f,
        -0.2923f,
        -0.2756f,
        -0.2588f,
        -0.2419f,
        -0.2249f,
        -0.2079f,
        -0.1908f,
        -0.1736f,
        -0.1564f,
        -0.1391f,
        -0.1218f,
        -0.1045f,
        -0.0871f,
        -0.0697f,
        -0.0523f,
        -0.0348f,
        -0.0174f
        };

        /// <summary>
        /// ������ ���޹޾� sin���� ��ȯ�ϴ� �޼ҵ�
        /// </summary>
        /// <param name="angle_p">degree ��</param>
        /// <returns>sin��</returns>
        public static float Sin(float angle_p)
        {
            float tmp0 = ((int)(angle_p * 0.002777777777778f));
            angle_p -= tmp0 * 360f;

            if (angle_p < 0)
            {
                angle_p += 360f;
            }

            int angleInt = (int)angle_p;
            float angleFloat = angle_p - angleInt;

            if (angleInt >= 359)
            {
                return sinLookUpTable[359] - sinLookUpTable[359] * angleFloat;
            }
            else
            {
                return sinLookUpTable[angleInt] + (sinLookUpTable[angleInt + 1] - sinLookUpTable[angleInt]) * angleFloat;
            }
        }

        /// <summary>
        /// ������ ���޹޾� cos���� ��ȯ�ϴ� �޼ҵ�
        /// </summary>
        /// <param name="angle_p">degree ��</param>
        /// <returns>cos��</returns>
        public static float Cos(float angle_p)
        {
            return Sin(angle_p + 90f);
        }
    }
}