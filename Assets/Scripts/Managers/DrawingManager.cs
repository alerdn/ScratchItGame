using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingManager : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public List<LineRenderer> LineInstances = new List<LineRenderer>();

    [SerializeField] private GameObject LineRenderPrefab;
    private LineRenderer _currentLineRender;
    private List<Vector3> _positions = new List<Vector3>();

    [ContextMenu("Clear Instances")]
    private void ClearInstance()
    {
        foreach (var item in LineInstances)
        {
            Destroy(item.gameObject);
        }

        _positions.Clear();
        LineInstances.Clear();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentLineRender = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Draw(eventData);
    }

    public void Draw(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.isValid == true)
        {
            if (_currentLineRender == null) CreateLineWithSamePoint(eventData);
            else UpdateLinePosition(eventData);

            _positions.Add(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    private void CreateLineWithSamePoint(PointerEventData eventData)
    {
        _currentLineRender = Instantiate(LineRenderPrefab).GetComponent<LineRenderer>();
        _currentLineRender.SetPosition(0, new Vector3(eventData.pointerCurrentRaycast.worldPosition.x, eventData.pointerCurrentRaycast.worldPosition.y, 0));
        _currentLineRender.SetPosition(1, new Vector3(eventData.pointerCurrentRaycast.worldPosition.x, eventData.pointerCurrentRaycast.worldPosition.y, 0));
        LineInstances.Add(_currentLineRender);
    }

    private void UpdateLinePosition(PointerEventData eventData)
    {
        _currentLineRender.positionCount = _currentLineRender.positionCount + 1;
        _currentLineRender.SetPosition(_currentLineRender.positionCount - 1, new Vector3(eventData.pointerCurrentRaycast.worldPosition.x, eventData.pointerCurrentRaycast.worldPosition.y, 0));
        CreateLineWithSamePoint(eventData);
    }
}
