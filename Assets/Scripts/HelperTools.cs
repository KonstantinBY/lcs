using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class HelperTools
    {
        public static IEnumerator MoveToTarget(Transform obj, Vector3 target, float duration)
        {
            Vector3 start = obj.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                obj.position = Vector3.Lerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.position = target; // гарантируем точное попадание
        }
    }
}