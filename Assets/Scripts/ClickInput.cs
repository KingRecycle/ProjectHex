using System;
using UnityEngine;

namespace CharlieMadeAThing.ProjectHex {
    public class ClickInput : MonoBehaviour {
        [SerializeField] BoardSetup board;
        Node firstClickedNode;
        void Update() {
            if ( Input.GetMouseButtonDown( 0 ) ) {
                var mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                var cellPosition = board.boardTilemap.WorldToCell( new Vector3(mousePosition.x, mousePosition.y, 0) );
                var tempNode = board.GetNode( cellPosition );
                if ( tempNode == null ) {
                    Debug.Log( $"[ClickInput]: No node found at clicked position {cellPosition}" );
                    return;
                }
                if ( !tempNode.IsClickable ) return;
                
                if ( firstClickedNode == null ) {
                    firstClickedNode = tempNode;
                    Debug.Log($"[ClickInput]: Orb Selected: {firstClickedNode.CurrentOrb}");
                    if ( firstClickedNode.CurrentOrb == OrbType.Encrypt ) {
                        board.RemoveOrb( firstClickedNode.Position );
                        firstClickedNode = null;
                    }
                } else if ( firstClickedNode != null && firstClickedNode == tempNode ) {
                    Debug.Log($"[ClickInput]: Current selected orb {firstClickedNode} was selected again.");
                    firstClickedNode = null;
                } else if ( firstClickedNode != null && !firstClickedNode.MatchingOrb.HasFlag( tempNode.CurrentOrb ) ) {
                    Debug.Log($"[ClickInput]: Current Selected Orb: {firstClickedNode} || Second Orb Selected: {tempNode} == None Matching Orbs!");
                    firstClickedNode = null;
                }
                else {
                    //Match!
                    Debug.Log($"[ClickInput]: Current Selected Orb: {firstClickedNode} || Second Orb Selected: {tempNode} == MATCHING ORBS!!");
                    board.RemoveOrb( firstClickedNode.Position );
                    board.RemoveOrb( tempNode.Position );
                    firstClickedNode = null;
                }
            }
        }
    }
}