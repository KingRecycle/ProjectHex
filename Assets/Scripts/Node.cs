using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CharlieMadeAThing.ProjectHex {
    public class Node {
        public OrbType CurrentOrb;
        public OrbType MatchingOrb;
        public Orb orb;
        public bool IsClickable;
        public Vector3Int Position;
        public GameObject CurrentOrbGameObject;
        public SpriteRenderer CurrentOrbSpriteRenderer;

        public List<Node> Neighbors = new();
        
        public Tile Tile;
        public BoardSetup board;


        public void ChangeOrbType( OrbType type ) {
            CurrentOrb = type;
        }

        public void DoTick() {
            if ( CurrentOrb is OrbType.None or OrbType.NonPlayable ) {
                if ( CurrentOrbSpriteRenderer ) {
                    CurrentOrbSpriteRenderer.color = new Color( 0f, 0f, 0f, 0f );
                }

                return;
            }

            if ( orb == null ) {
                orb = CurrentOrbGameObject.GetComponent<Orb>();
            }
            else {
                MatchingOrb = orb.orbData.MatchingTypes;
            }

            
            IsClickable = DoesHaveThreeConsecutiveEmptyNeighbors() && IsPrerequisiteAchieved();
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

        //Checks for prerequisites. Some orbs can't be clicked unless they are met even if 3 empty neighbors.
        bool IsPrerequisiteAchieved() {
            switch ( CurrentOrb ) {
                case OrbType.NetworkTwo when board.IsOrbOnBoard( OrbType.NetworkOne ):
                    return false;
                case OrbType.NetworkThree when board.IsOrbOnBoard( OrbType.NetworkTwo ):
                    return false;
                case OrbType.NetworkFour when board.IsOrbOnBoard( OrbType.NetworkThree ):
                    return false;
                case OrbType.NetworkFive when board.IsOrbOnBoard( OrbType.NetworkFour ):
                    return false;
                case OrbType.Encrypt when !board.IsEncryptTheLastOrbLeft():
                    return false;
                default:
                    return true;
            }
        }
        
    }
}