using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
  private ITooltipData _data;

  public void SetData(ITooltipData data) => _data = data;

  public void OnPointerEnter(PointerEventData _)
  {
    if (_data == null) return;
    TooltipCanvas.ShowTooltip_Static(_data.BuildTooltip);
  }

  public void OnPointerExit(PointerEventData _)
  {
    TooltipCanvas.HideTooltip_Static();
  }

  private void OnDisable()
  {
    TooltipCanvas.HideTooltip_Static();
  }
}