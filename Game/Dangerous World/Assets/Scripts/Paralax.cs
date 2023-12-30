using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBase]
public class Paralax : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private List<ParalaxTarget> targets;
    [Header("To apply width click 'Update Width' in context menu")]
    [SerializeField] private float _width;

    private Vector2 _initializePosition;

    private void Start()
    {
        _initializePosition = follow.position;
        targets.ForEach((target) => target.Initialize());
    }

    private void LateUpdate()
    {
        var offset = (Vector2)follow.position - _initializePosition;
        targets.ForEach((target) => target.MoveTo(offset));
    }

    [Serializable]
    private class ParalaxTarget
    {
        [SerializeField] public Transform _transform;
        [SerializeField] public Vector2 _scale;
        private Vector2 _initializePosition;
        public void Initialize() => _initializePosition = _transform.position;
        public void MoveTo(Vector2 position) => _transform.position = _initializePosition + position * _scale;
    }

    private void OnValidate()
    {
        if (follow == null) { follow = Camera.main != null ? Camera.main.transform : null; }
        if (targets.Count == 0) {AutoFill();}
    }

    [ContextMenu("Auto fill")]
    private void AutoFill()
    {
        targets = GetComponentsInChildren<Transform>()
            .Skip(1)
            .Select(target => new ParalaxTarget() { _transform = target, _scale = Vector2.one})
            .ToList();
    }
    [ContextMenu("Align Targets")]
    private void AlignTargets()
    {
        targets.ForEach(target => 
            target._transform.position = Camera.main.transform.position + Vector3.down*Camera.main.orthographicSize);
    }

    [ContextMenu("Update Width")]
    private void UpdateWidth()
    {
        targets.ForEach(target => {
            var configurable = target._transform.GetComponentsInChildren<SpriteRenderer>()
                .Where(sprite => sprite.drawMode != SpriteDrawMode.Simple);
            foreach (var sprite in configurable) {
                sprite.size = new(_width, sprite.size.y);
            }   
        }); 
    }
}
