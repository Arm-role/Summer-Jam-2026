using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObject : MonoBehaviour
{
  public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
  {
    Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

    PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
    placedObject.placedObjectTypeSO = placedObjectTypeSO;
    placedObject.origin = origin;
    placedObject.dir = dir;

    placedObject.Setup();

    return placedObject;
  }

  public static PlacedObject CreateCanvas(Transform parent, Vector2 anchoredPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
  {
    Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, parent);
    placedObjectTransform.rotation = Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
    placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

    PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
    placedObject.placedObjectTypeSO = placedObjectTypeSO;
    placedObject.origin = origin;
    placedObject.dir = dir;

    placedObject.Setup();

    return placedObject;
  }

  private PlacedObjectTypeSO placedObjectTypeSO;
  private Vector2Int origin;
  private PlacedObjectTypeSO.Dir dir;

  protected virtual void Setup()
  {
    //Debug.Log("PlacedObject.Setup() " + transform);
  }

  public virtual void GridSetupDone()
  {
    //Debug.Log("PlacedObject.GridSetupDone() " + transform);
  }

  public Vector2Int GetGridPosition()
  {
    return origin;
  }

  public void SetOrigin(Vector2Int origin)
  {
    this.origin = origin;
  }

  public List<Vector2Int> GetGridPositionList()
  {
    return placedObjectTypeSO.GetGridPositionList(origin, dir);
  }

  public PlacedObjectTypeSO.Dir GetDir()
  {
    return dir;
  }

  public virtual void DestroySelf()
  {
    Destroy(gameObject);
  }

  public override string ToString()
  {
    return placedObjectTypeSO.nameString;
  }

  [SerializeField] private Image cooldownImage;

  public void SetCooldownVisual(float progress)
  {
    cooldownImage.fillAmount = progress;
  }

  public void FixCooldownRotation()
  {
    switch (dir)
    {
      case PlacedObjectTypeSO.Dir.Down:
        cooldownImage.fillMethod = Image.FillMethod.Vertical;
        cooldownImage.fillOrigin = (int)Image.OriginVertical.Bottom;
        break;

      case PlacedObjectTypeSO.Dir.Up:
        cooldownImage.fillMethod = Image.FillMethod.Vertical;
        cooldownImage.fillOrigin = (int)Image.OriginVertical.Top;
        break;

      case PlacedObjectTypeSO.Dir.Left:
        cooldownImage.fillMethod = Image.FillMethod.Horizontal;
        cooldownImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        break;

      case PlacedObjectTypeSO.Dir.Right:
        cooldownImage.fillMethod = Image.FillMethod.Horizontal;
        cooldownImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        break;
    }
  }


  public PlacedObjectTypeSO GetPlacedObjectTypeSO()
  {
    return placedObjectTypeSO;
  }

  [System.Serializable]
  public class SaveObject
  {

    public string placedObjectTypeSOName;
    public Vector2Int origin;
    public PlacedObjectTypeSO.Dir dir;
    public string floorPlacedObjectSave;

  }
}
