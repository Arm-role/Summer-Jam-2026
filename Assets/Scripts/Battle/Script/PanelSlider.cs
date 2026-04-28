using System;
using UnityEngine;

public class PanelSlider : MonoBehaviour
{
  public event Action<int> OnPageChanged;
  public event Action OnReachFirstPage;
  public event Action OnReachLastPage;

  [SerializeField] int maxPage;
  [SerializeField] Vector3 pageStep;
  [SerializeField] RectTransform levelPagesRect;
  [SerializeField] float tweenTime;
  [SerializeField] LeanTweenType tweenType;

  int currentPage;
  Vector3 targetPos;

  void Start()
  {
    currentPage = 1;
    targetPos = levelPagesRect.localPosition;
  }

  public void Next()
  {
    if (currentPage >= maxPage)
    {
      Debug.Log($"Next page: {currentPage} == {maxPage}");
      OnReachLastPage?.Invoke();
      return;
    }

    currentPage++;

    targetPos += pageStep;

    MovePage();
  }

  public void Previous()
  {
    if (currentPage <= 1)
      return;

    currentPage--;

    targetPos -= pageStep;

    MovePage();
  }

  void MovePage()
  {
    levelPagesRect
      .LeanMoveLocal(targetPos, tweenTime)
      .setEase(tweenType)
      .setOnComplete(OnMoveComplete);
  }

  void OnMoveComplete()
  {
    OnPageChanged?.Invoke(currentPage);

    if (currentPage == 1)
      OnReachFirstPage?.Invoke();

    if (currentPage == maxPage)
      OnReachLastPage?.Invoke();
  }
}
