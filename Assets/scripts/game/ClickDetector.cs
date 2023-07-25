using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  private BuildManager buildManager;
  private float groundLevel;

  // Start is called before the first frame update
  void Start()
  {
    buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
    groundLevel = gameObject.transform.position.z - 0.5f;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnPointerDown(PointerEventData eventData)
  {
    // Voluntarily empty, but needed to receive the pointer up events
    // according to the doc.
    // https://docs.unity3d.com/2018.2/Documentation/ScriptReference/EventSystems.IPointerUpHandler.html
  }

  // https://forum.unity.com/threads/onmousedown-for-right-click.7131/
  public void OnPointerUp(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
      // https://discussions.unity.com/t/mouse-cursor-position-relative-to-the-object/144907
      var clickPos = eventData.pointerCurrentRaycast.worldPosition;
      var pos2d = VectorUtils.ConvertTo2dIntTile(clickPos);

      buildManager.SpawnBuildingRequest(pos2d, groundLevel);
    }
  }
}
