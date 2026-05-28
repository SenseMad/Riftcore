using System.Collections.Generic;
using Riftcore.Gameplay.Pickups.Core;
using UnityEngine;

namespace Riftcore.World.Grid
{
    public sealed class PickupGrid : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 10f;

        private readonly Dictionary<GridPosition, HashSet<BasePickup>> _cells = new();
        private readonly Dictionary<BasePickup, GridPosition> _pickupToCell = new();
        
        public void Register(BasePickup pickup)
        {
            var cell = WorldToCell(pickup.transform.position);
            if (!_cells.TryGetValue(cell, out var cells))
            {
                cells = new HashSet<BasePickup>();
                _cells[cell] = cells;
            }

            cells.Add(pickup);
            _pickupToCell[pickup] = cell;
        }

        public void Unregister(BasePickup pickup)
        {
            if (_pickupToCell.TryGetValue(pickup, out var cell))
            {
                if (_cells.TryGetValue(cell, out var cells))
                {
                    cells.Remove(pickup);
                    
                    if (cells.Count == 0)
                        _cells.Remove(cell);
                }

                _pickupToCell.Remove(pickup);
            }
        }
        
        public void UpdatePickupCell(BasePickup pickup)
        {
            var newCell = WorldToCell(pickup.transform.position);
            if (!_pickupToCell.TryGetValue(pickup, out var oldCell))
                return;
            
            if (oldCell.Equals(newCell))
                return;

            if (_cells.TryGetValue(oldCell, out var oldCells))
            {
                oldCells.Remove(pickup);
                
                if (oldCells.Count == 0)
                    _cells.Remove(oldCell);
            }

            if (!_cells.TryGetValue(newCell, out var newCells))
            {
                newCells = new HashSet<BasePickup>();
                _cells[newCell] = newCells;
            }

            newCells.Add(pickup);
            _pickupToCell[pickup] = newCell;
        }

        public void GetPickupsInRadius(Vector3 position, float radius, List<BasePickup> result)
        {
            result.Clear();
            
            int minX = Mathf.FloorToInt((position.x - radius) / _cellSize);
            int maxX = Mathf.FloorToInt((position.x + radius) / _cellSize);
            
            int minY = Mathf.FloorToInt((position.z - radius) / _cellSize);
            int maxY = Mathf.FloorToInt((position.z + radius) / _cellSize);
            
            float sqrRadius = radius * radius;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var cell = new GridPosition(x, y);
                    
                    if (!_cells.TryGetValue(cell, out var cells))
                        continue;

                    foreach (var pickup in cells)
                    {
                        if (pickup == null)
                            continue;
                        
                        var diff = pickup.transform.position - position;
                        if (diff.sqrMagnitude <= sqrRadius)
                            result.Add(pickup);
                    }
                }
            }
        }
        
        private GridPosition WorldToCell(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / _cellSize);
            int y = Mathf.FloorToInt(position.z / _cellSize);
            
            return new GridPosition(x, y);
        }
    }
}