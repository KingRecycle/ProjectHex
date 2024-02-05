using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CharlieMadeAThing.ProjectHex {
    public class Node {
        public OrbType CurrentOrb;
        public bool IsClickable;
        public Vector3Int Position;
        public GameObject CurrentOrbGameObject;
        public SpriteRenderer CurrentOrbSpriteRenderer;

        public List<Node> Neighbors = new();

        //public Node[] OrderedNeighbors = new Node[6];
        public Tile Tile;


        public void ChangeOrbType( OrbType type ) {
            CurrentOrb = type;
        }

        public void DoTick() {
            if ( CurrentOrb is OrbType.None or OrbType.NonPlayable ) {
                return;
            }

            IsClickable = DoesHaveThreeConsecutiveEmptyNeighbors();
            if ( CurrentOrbSpriteRenderer ) {
                CurrentOrbSpriteRenderer.color =
                    IsClickable ? new Color( 1f, 1f, 1f, 1f ) : new Color( 1f, 1f, 1f, 0.3f );
            }
        }

        bool DoesHaveThreeConsecutiveEmptyNeighbors() {
            for ( var i = 0; i < Neighbors.Count; i++ ) {
                var currentNeighbor = Neighbors[i];
                
                if ( currentNeighbor.CurrentOrb is not (OrbType.None or OrbType.NonPlayable) ) {
                    continue;
                }
                //Debug.Log($"{i % Neighbors.Count + 1} == {i % Neighbors.Count + 2}");
                var nextNeighbor = Neighbors[(i + 1) % Neighbors.Count];
                var nextNextNeighbor = Neighbors[(i + 2) % Neighbors.Count];
                
//                Debug.Log($"{currentNeighbor.CurrentOrb} == {nextNeighbor.CurrentOrb} == {nextNextNeighbor.CurrentOrb}");
                
                if ( (nextNeighbor.CurrentOrb is (OrbType.None or OrbType.NonPlayable)) &&
                     (nextNextNeighbor.CurrentOrb is (OrbType.None or OrbType.NonPlayable ) ) ) {
                    return true;
                }
            }

            return false;
        }
    }
}