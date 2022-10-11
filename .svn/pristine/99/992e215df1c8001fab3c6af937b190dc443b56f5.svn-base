using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PathNode
    {
        //是否可以通过
        public bool CanWalk;

        //节点空间位置
        public Vector3 WorldPos;

        //节点在数组的位置
        public int GridX;
        public int GridY;

        //开始节点到当前节点的距离估值
        public int _gCost;
        //当前节点到目标节点的距离估值
        public int _hCost;

        public int _fCost => _gCost + _hCost;
        
        //当前节点的父节点
        public PathNode _parent;

        public PathNode(bool canWalk, Vector3 position, int gridX, int gridY)
        {
            CanWalk = canWalk;
            WorldPos = position;
            GridX = gridX;
            GridY = gridY;
        }
    }

    public class APathFinder
    {
        PathNode[,] _Grid;
        Vector3 _StartPos;
        Vector3 _GridSize;
        int _GridCountX;
        int _GridCountY;
        float _nodeRadius;

        private List<PathNode> openList = new List<PathNode>();
        private HashSet<PathNode> closeSet = new HashSet<PathNode>();

        public void CreateGrid(Vector3 startPos, Vector3 endPos, float nodeRadius, LayerMask layer)
        {
            _GridSize = endPos - startPos;

            if (_GridSize.x < 0 || _GridSize.z < 0)
            {
                Debug.Log("endPos should be greater than startPos!");
                return;    
            }

            _StartPos = startPos;
            _nodeRadius = nodeRadius;

            _GridCountX = Mathf.RoundToInt(_GridSize.x / (nodeRadius * 2));
            _GridCountY = Mathf.RoundToInt(_GridSize.z / (nodeRadius * 2));
            _Grid = new PathNode[_GridCountX, _GridCountY];

            for (int i = 0; i < _GridCountX; i++)
            {
                for (int j = 0; j < _GridCountY; j++)
                {
                    Vector3 worldPos = startPos;
                    worldPos.x = worldPos.x + i * 2 * nodeRadius + nodeRadius;
                    worldPos.z = worldPos.z + j * 2 * nodeRadius + nodeRadius;
                    worldPos.y = 0.5f;
                    bool canWalk = !Physics.CheckSphere(worldPos, nodeRadius); //, layer);

                    // if (!canWalk) Debug.LogWarning("Grid[" + i + ", " + j + "]  can Not walk!");
                    _Grid[i, j] = new PathNode(canWalk, worldPos, i, j);
                }
            }
        }

        public void RefreshCanWalk(Vector3 start, Vector3 end, LayerMask layer)
        {
            PathNode from = GetFromPosition(start);
            PathNode to = GetFromPosition(end);

            for ( int i=from.GridX; i <= to.GridX; ++ i)
            {
                for ( int j = from.GridY; j <= to.GridY; ++j)
                {
                    bool canWalk = !Physics.CheckSphere(_Grid[i, j].WorldPos, _nodeRadius, layer);
                    _Grid[i, j].CanWalk = canWalk;
                }
            }
        }

        public PathNode GetFromPosition(Vector3 pos)
        {
            Vector3 offset = pos - _StartPos;

            float percentX = offset.x / _GridSize.x;
            float percentZ = offset.z / _GridSize.z;
            percentX = Mathf.Clamp01(percentX);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.RoundToInt((_GridCountX - 1) * percentX);
            int z = Mathf.RoundToInt((_GridCountY - 1) * percentZ);

            return _Grid[x, z];
        }

        public List<PathNode> GetNeighor(PathNode node)
        {
            List<PathNode> neighborList = new List<PathNode>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int tempX = node.GridX + i;
                    int tempY = node.GridY + j;
                    if (tempX < _GridCountX && tempX > 0 && tempY > 0 && tempY < _GridCountY)
                    {
                        neighborList.Add(_Grid[tempX, tempY]);
                    }
                }
            }
            return neighborList;
        }

        private int GetDistanceNodes(PathNode node1, PathNode node2)
        {
            int deltaX = Mathf.Abs(node1.GridX - node2.GridX);
            int deltaY = Mathf.Abs(node1.GridY - node2.GridY);
            if (deltaX > deltaY)
            {
                return deltaY * 14 + 10 * (deltaX - deltaY);
            }
            else
            {
                return deltaX * 14 + 10 * (deltaY - deltaX);
            }
        }

        public Stack<PathNode> FindingPath(Vector3 start, Vector3 end)
        {
            PathNode startNode = GetFromPosition(start);
            PathNode endNode = GetFromPosition(end);
            
            openList.Clear();
            closeSet.Clear();
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                // 寻找开启列表中的F最小的节点，如果F相同，选取H最小的
                PathNode currentNode = openList[0];
                for (int i = 0; i < openList.Count; i++)
                {
                    PathNode node = openList[i];
                    if (node._fCost < currentNode._fCost || node._fCost == currentNode._fCost && node._hCost < currentNode._hCost)
                    {
                        currentNode = node;
                    }
                }

                // 把当前节点从开启列表中移除，并加入到关闭列表中
                openList.Remove(currentNode);
                closeSet.Add(currentNode);

                // 如果是目的节点，返回
                if (currentNode == endNode)
                {
                    break;
                }
                // 搜索当前节点的所有相邻节点
                foreach (var node in GetNeighor(currentNode))
                {
                    // 如果节点不可通过或者已在关闭列表中，跳出
                    if (!node.CanWalk || closeSet.Contains(node))
                    {
                        continue;
                    }
                    int gCost = currentNode._gCost + GetDistanceNodes(currentNode, node);
                    // 如果新路径到相邻点的距离更短 或者不在开启列表中
                    if (gCost < node._gCost || !openList.Contains(node))
                    {
                        // 更新相邻点的F，G，H
                        node._gCost = gCost;
                        node._hCost = GetDistanceNodes(node, endNode);
                        // 设置相邻点的父节点为当前节点
                        node._parent = currentNode;
                        // 如果不在开启列表中，加入到开启列表中
                        if (!openList.Contains(node))
                        {
                            openList.Add(node);
                        }
                    }
                }
            }

            return GeneratePath(startNode, endNode);
        }

        private Stack<PathNode> GeneratePath(PathNode startNode, PathNode endNode)
        {
            Stack<PathNode> path = new Stack<PathNode>();
            PathNode node = endNode;
            while (node._parent != startNode && node._parent != null)
            {
                path.Push(node);
                node = node._parent;
            }

            return path;
        }
    }
}
