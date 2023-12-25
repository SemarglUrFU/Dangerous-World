using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBase]
public class Paralax : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private List<ParalaxTarget> targets;

    private Vector2 _initializePosition;

    private void Start()
    {
        _initializePosition = follow.position;
        targets.ForEach((target) => target.Initialize());
    }

    private void LateUpdate()
    {
        var offset = (Vector2)follow.position - _initializePosition;
        targets.ForEach((target) => target.Move(offset));
    }

    [Serializable]
    private class ParalaxTarget
    {
        [SerializeField] public Transform _transform;
        [SerializeField] public Vector2 _scale;
        private Vector2 _initializePosition;
        public void Initialize() => _initializePosition = _transform.position;
        public void Move(Vector2 position) => _transform.position = _initializePosition + position * _scale;
    }

    private void OnValidate()
    {
        if (follow == null) { follow = Camera.main?.transform; }
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
    [ContextMenu("AlignTargets")]
    private void AlignTargets()
    {
        targets.ForEach(target => 
            target._transform.position = Camera.main.transform.position + Vector3.down*Camera.main.orthographicSize);
    }
}
