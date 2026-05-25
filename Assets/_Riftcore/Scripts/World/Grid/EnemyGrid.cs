using System.Collections.Generic;
using Riftcore.Gameplay.Enemies;
using Riftcore.Gameplay.Enemies.Core;
using UnityEngine;

namespace Riftcore.World.Grid
{
    public sealed class EnemyGrid : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 10f;

        private readonly Dictionary<GridPosition, HashSet<Enemy>> _cells = new();
        private readonly Dictionary<Enemy, GridPosition> _enemyToCell = new();

        public void Register(Enemy enemy)
        {
            var cell = WorldToCell(enemy.transform.position);
            if (!_cells.TryGetValue(cell, out var cells))
            {
                cells = new HashSet<Enemy>();
                _cells[cell] = cells;
            }

            cells.Add(enemy);
            _enemyToCell[enemy] = cell;
        }

        public void Unregister(Enemy enemy)
        {
            if (_enemyToCell.TryGetValue(enemy, out var cell))
            {
                if (_cells.TryGetValue(cell, out var cells))
                {
                    cells.Remove(enemy);
                    
                    if (cells.Count == 0)
                        _cells.Remove(cell);
                }

                _enemyToCell.Remove(enemy);
            }
        }

        public void UpdateEnemyCell(Enemy enemy)
        {
            var newCell = WorldToCell(enemy.transform.position);
            if (!_enemyToCell.TryGetValue(enemy, out var oldCell))
                return;
            
            if (oldCell.Equals(newCell))
                return;

            if (_cells.TryGetValue(oldCell, out var oldCells))
            {
                oldCells.Remove(enemy);
                
                if (oldCells.Count == 0)
                    _cells.Remove(oldCell);
            }

            if (!_cells.TryGetValue(newCell, out var newCells))
            {
                newCells = new HashSet<Enemy>();
                _cells[newCell] = newCells;
            }

            newCells.Add(enemy);
            _enemyToCell[enemy] = newCell;
        }

        public void GetEnemiesInRadius(Vector3 position, float radius, List<Enemy> result)
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

                    foreach (var enemy in cells)
                    {
                        if (enemy == null)
                            continue;
                        
                        var diff = enemy.transform.position - position;
                        if (diff.sqrMagnitude <= sqrRadius)
                            result.Add(enemy);
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