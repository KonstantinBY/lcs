using System.Collections.Generic;
using ToonPeople;
using UnityEngine;

public class UIButtonRandomizer : MonoBehaviour
{
    [SerializeField] public int showButtonsAmount = 3;

    private List<GameObject> allButtons = new();

    void Awake()
    {
        // Собираем кнопки
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            allButtons.Add(child);
            child.SetActive(false); // Скрыть все
        }
    }

    public void ShowRandomSet(EventEnum eventName)
    {
        GameObject required = allButtons.Find(b => b.name == eventName.ToString());
        if (required == null)
        {
            Debug.LogWarning($"Кнопка с именем {eventName} не найдена.");
            return;
        }

        ShowRandomSetInternal(required);
    }

    public void ShowRandomSet(int requiredIndex)
    {
        if (requiredIndex < 0 || requiredIndex >= allButtons.Count)
        {
            Debug.LogWarning($"Индекс {requiredIndex} вне диапазона.");
            return;
        }

        ShowRandomSetInternal(allButtons[requiredIndex]);
    }

    private void ShowRandomSetInternal(GameObject requiredButton)
    {
        // Скрываем все кнопки
        foreach (var btn in allButtons)
            btn.SetActive(false);

        // Собираем список кандидатов
        List<GameObject> pool = new List<GameObject>(allButtons);
        pool.Remove(requiredButton);

        // Выбираем случайные
        List<GameObject> result = new List<GameObject> { requiredButton };
        while (result.Count < showButtonsAmount && pool.Count > 0)
        {
            int rand = Random.Range(0, pool.Count);
            result.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        // Перемешиваем
        Shuffle(result);

        // Показываем и задаём порядок
        for (int i = 0; i < result.Count; i++)
        {
            result[i].SetActive(true);
            result[i].transform.SetSiblingIndex(i);
        }
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
    
    public void HideAllButtons()
    {
        foreach (var btn in allButtons)
        {
            btn.SetActive(false);
        }
    }
}
